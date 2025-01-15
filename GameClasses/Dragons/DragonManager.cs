using BoardGameBackend.Helpers;
using BoardGameBackend.Mappers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class DragonManager
    {
        private List<Dragon> _dragons;
        public List<Dragon> DragonsOnBoard { get; private set; } = new List<Dragon>();
        public List<Dragon> DragonsToPickFrom;
        private int _nextInGameIndex = 1;
        private GameContext _gameContext;
        private bool _dragonDLCon = false;
        public Dragon? nextDragonToSummon;
        private const int iSpawnOnRoundStart = 3;
        public int iSpawnOnRoundHelpIndex = -1;

        public DragonManager(GameContext gameContext, bool dragonDLCon)
        {
            _gameContext = gameContext;
            _dragons = new List<Dragon>();
            _dragonDLCon = dragonDLCon;
            if(!_dragonDLCon)
                return;
                
            foreach (var dragonFromJson in DragonsFactory.DragonsFromJsonList)
            {
                var dragon = GameMapper.Instance.Map<Dragon>(dragonFromJson);
                dragon.InGameIndex = _nextInGameIndex++;
                _dragons.Add(dragon);
            }

            ShuffleDragons();
            
            _gameContext.EventManager.Subscribe("StartTurn", () =>
            {
                StartOfRoundSetup();
            }, priority: 1); 

            gameContext.EventManager.Subscribe<PlayerInGame>("New player turn", player =>
            {
                CheckIfExtraSummonIsNeeded();
            }, priority: 1);

            gameContext.EventManager.Subscribe<DragonSummonEventData>("SummonDragonEvent", data =>
            {             
                CheckIfExtraSummonIsNeeded();                
            }, priority: 0); 
        }

        private void ShuffleDragons()
        {
            Random rng = new Random();
            _dragons = _dragons.OrderBy(m => rng.Next()).ToList();
        }

        public bool AcquireDragonCard(Dragon dragonCard, PlayerInGame player, TileReward? tileReward)
        {
            bool bUsedToken = false;
            var auraIndex = player.AurasTypes.FindIndex(a => a.Aura == AurasType.NEXT_DRAGON_REQ_IGNORE);
            if (auraIndex != -1 && !CanPlayerDefeatDragon(dragonCard, player, true))
            {
                player.AurasTypes.RemoveAt(auraIndex);
                bUsedToken = true;
            }

            player.AddDragon(dragonCard);
            if (dragonCard.Morale != 0)
            {
                _gameContext.PlayerManager.AddMoraleToPlayer(player, dragonCard.Morale);
            }

            Reward? reward = null;
            if (dragonCard.EffectId != null)
            {
                var rewardClass = RewardFactory.GetRewardById(dragonCard.EffectId.Value);
                reward = rewardClass.OnReward();
                _gameContext.RewardHandlerManager.HandleReward(player, reward);

                if(tileReward != null && reward.Resources != null)
                {
                    if(reward.Resources.FindIndex(r => r.Type == ResourceType.Gold) != -1)
                        tileReward.GoldFromDragon = true; // it was for an ability +1 gold from taking rewards that are not gold
                }
            }

            int iMovementFullLeft = -1;
            if (player.PlayerHeroCardManager.CurrentHeroCard != null)
            {
                var amountOfAuras = player.AurasTypes.Count(a => a.Aura == AurasType.FULL_MOVE_ON_DRAGON_DEFEAT);
                if (amountOfAuras > 0)
                {
                    player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft += amountOfAuras;
                }
                iMovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft;
            }

            var eventArgs = new DragonAcquired
            {
                Reward = reward,
                Card = dragonCard,
                Player = player,
                MovementFullLeft = iMovementFullLeft,
                UsedToken = bUsedToken
            };
            _gameContext.EventManager.Broadcast("DragonAcquired", ref eventArgs);

            return true;
        }

        public List<Dragon> GetDragonsDeck()
        {
            return _dragons;
        }

        public DragonFullData GetFullDragonData()
        {
            DragonFullData dfd = new DragonFullData();
            dfd.CurrentlySummonedDragon = nextDragonToSummon;
            dfd.DragonsToPick = DragonsToPickFrom;
            return dfd;
        }

        public Dragon? GetDragonFromTopDeck()
        {
            Dragon? newDragon = null;
             if (_dragons.Count <= 0)
            {
                return newDragon;
            }

            newDragon = _dragons[0];
            _dragons.RemoveAt(0);

            return newDragon;
        }

        public int GetRemainingDragonsCount()
        {
            return _dragons.Count;
        }

        public List<Dragon> PrepareDragonsToPickFrom(int iHowMany)
        {
            DragonsToPickFrom = new List<Dragon>();
            while(DragonsToPickFrom.Count() < iHowMany)
            {
                Dragon? nextDragon = GetDragonFromTopDeck();
                if(nextDragon == null)
                    break;

                if(IsDragonValidToSummonAnyTile(nextDragon))
                    DragonsToPickFrom.Add(nextDragon);
            }
            return DragonsToPickFrom;
        }

        public bool PreparedNextDragonForSummon()
        {
            if(nextDragonToSummon != null)
                return true;
                
            bool bContinue = true;
            while(bContinue)
            {
                Dragon? nextDragon = GetDragonFromTopDeck();
                if(nextDragon == null)
                    return false;

                if(IsDragonValidToSummonAnyTile(nextDragon))
                {
                    SetNextDragonToSummon(nextDragon);
                    bContinue = false;
                }
              
            }
            return true;
        }

        public void SetNextDragonToSummon(Dragon dragon)
        {
            nextDragonToSummon = dragon;
            var eventArgs = new DragonSummonData
            {
                Card = nextDragonToSummon
            };
            _gameContext.EventManager.Broadcast("DragonPreSummon", ref eventArgs);
        }

        public bool IsDragonValidToSummonAnyTile(Dragon dragon)
        {
            foreach(var tile in _gameContext.GameTiles.Tiles)
            {
                if(tile.TileTypeId == dragon.TileId)
                {
                    if(tile.Token == null)
                    {
                        if(_gameContext.PawnManager._currentTile.Id != tile.Id)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsTileValidToSummonCurrentDragon(TileWithType tile)
        {
            if(nextDragonToSummon == null)
                return false;

            if(nextDragonToSummon.TileId != tile.TileTypeId)
                return false;

            if(tile.Token != null)
                return false;
                
            if(_gameContext.PawnManager._currentTile.Id == tile.Id)
                return false;

            bool m_bIsAdjacent = false;
            foreach(var adjtile in _gameContext.PawnManager._currentTile.Connections)
            {
                if(adjtile.ToId == tile.Id)
                {
                    m_bIsAdjacent = true;
                    break;
                }
            }   
            if(m_bIsAdjacent)
            {
                // there cannot be any non-adjacent valid tiles
                foreach(var other in _gameContext.GameTiles.Tiles)
                {   
                    if(other.TileTypeId == nextDragonToSummon.TileId && other.Id != tile.Id)
                    {
                        if(other.Token == null)
                        {
                            if(_gameContext.PawnManager._currentTile.Id != other.Id)
                            {
                                bool bAdjacent = false;
                                foreach(var adjtile in _gameContext.PawnManager._currentTile.Connections)
                                {
                                    if(adjtile.ToId == other.Id)
                                    {
                                        bAdjacent = true;
                                        break;
                                    }
                                }  

                                if(!bAdjacent)
                                    return false;
                            }
                        }
                    }
                }
            }


            return true;
        }
        public bool SummonDragon(PlayerInGame player, int TileId)
        {
            if(nextDragonToSummon == null)
                return false;

            var tile = _gameContext.GameTiles.GetTileById(TileId);

            if(!IsTileValidToSummonCurrentDragon(tile))
                return false;


            // SPAWN DRAGON TOKEN
            List<TokenTileInfo> newTokens = new List<TokenTileInfo> {};

            TokenFromJson? token = _gameContext.TokenManager.GetTokenById(nextDragonToSummon.TokenId);
            if(token == null)
                return false;

            token.DragonLink = nextDragonToSummon;
            tile.Token = token;

            nextDragonToSummon = null;
            var eventArgs = new DragonSummonEventData
            {
                TileId = tile.Id,
                PlayerId = player.Id,
                Token = token,
            };
            _gameContext.EventManager.Broadcast("SummonDragonEvent", ref eventArgs);
            return true;
        }
        
        public bool ConfirmPickDragon(int iDragonId)
        {
            if(DragonsToPickFrom == null)
                return false;

            Dragon newDragonToSummon = null;
            foreach(var dragon in DragonsToPickFrom)
            {
                if(dragon.InGameIndex == iDragonId)
                {
                    newDragonToSummon = dragon;
                    break;
                }
            }
            if(newDragonToSummon != null)
            {
                SetNextDragonToSummon(newDragonToSummon);
                DragonsToPickFrom.Clear();

                _gameContext.MiniPhaseManager.EndCurrentMiniPhase();
                _gameContext.MiniPhaseManager.StartSummonDragonMiniPhase();
                return true;
            }
            return false;
        }
        public void StartOfRoundSetup()
        {
            if(_gameContext.TurnManager.CurrentRound != iSpawnOnRoundStart) return;

            if(_gameContext.TurnManager.CurrentTurn % 2 != 1) return;

            iSpawnOnRoundHelpIndex = 0;
        }
        public void DoStartExtraSpawn()
        {
            if(iSpawnOnRoundHelpIndex == -1)
                return;

            iSpawnOnRoundHelpIndex++;
            if(iSpawnOnRoundHelpIndex == 5)
            {
                DoEndExtraSpawnNow();
                return;
            }
            if(_dragons.Count() > 2)
            {
                _gameContext.TurnManager.TemporarySetCurrentPlayer(_gameContext.PlayerManager.GetPlayerByMoraleOrder(iSpawnOnRoundHelpIndex));
                _gameContext.MiniPhaseManager.StartSummonDragonMiniPhase();
            }
            else
            {
                DoEndExtraSpawnNow();
            }
        }
        public void CheckIfExtraSummonIsNeeded()
        {
            DoStartExtraSpawn();
        }
        public bool IsExtraSpawnOn()
        {
            return iSpawnOnRoundHelpIndex != -1;
        }
        public void DoEndExtraSpawnNow()
        {
            iSpawnOnRoundHelpIndex = -1;
        }

        public bool CanPlayerDefeatDragon(Dragon dragon, PlayerInGame player, bool bIgnoreTokenAura = false)
        {
            if(!bIgnoreTokenAura)
            {
                if(player.AurasTypes.Count(a => a.Aura == AurasType.NEXT_DRAGON_REQ_IGNORE) > 0)
                    return true;
            }
            foreach (var req in dragon.Reqs)
            {
                var requirement = RequirementMovementStore.GetRequirementById(req);
                if(!requirement.CheckRequirements(player))
                    return false;
            }

            return true;
        }
    }
}