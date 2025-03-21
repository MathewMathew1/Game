using BoardGameBackend.Helpers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class MiniPhaseManager
    {
        private List<MiniPhase> _miniPhases;
        private GameContext _gameContext;
        public MiniPhase? CurrentMiniPhase = null;

        public MiniPhaseManager(GameContext gameContext)
        {
            _miniPhases = new List<MiniPhase>
            {
                new RerollMercenaryMiniPhase(_gameContext),
                new TeleportMiniPhase(_gameContext),
                new ArtifactPickMiniPhase(_gameContext)
            };
            _gameContext = gameContext;

            gameContext.EventManager.Subscribe<MoveOnTile>("MoveOnTile", moveOnTileData =>
            {
                HandleMoveOnTile(moveOnTileData.TileReward);
            }, priority: 5);

            _gameContext.EventManager.Subscribe<GetCurrentTileReward>("GetCurrentTileReward", getCurrentTileReward =>
            {
                HandleMoveOnTile(getCurrentTileReward.TileReward);
            }, priority: 5);


            _gameContext.EventManager.Subscribe<MercenaryRerolled>("MercenaryRerolled", rerollMercenaryData =>
           {
               if (CurrentMiniPhase is RerollMercenaryMiniPhase)
               {
                   EndCurrentMiniPhase();
               }

           }, priority: 20);
            gameContext.EventManager.Subscribe("ArtifactsTaken", (ArtifactsTaken data) =>
            {
                if (CurrentMiniPhase?.GetType() == typeof(ArtifactPickMiniPhase))
                {
                    EndCurrentMiniPhase();
                }

            }, priority: 5);

            gameContext.EventManager.Subscribe<FulfillProphecy>("FulfillProphecy", data =>
            {
                if (CurrentMiniPhase is FulfillProphecyMiniPhase)
                {
                    EndCurrentMiniPhase();
                }

            }, priority: 5);

            gameContext.EventManager.Subscribe<BuffHeroData>("HeroCardBuffed", data =>
            {
                if (CurrentMiniPhase is BuffHeroMiniPhase)
                {
                    EndCurrentMiniPhase();
                }

            }, priority: 5);

            gameContext.EventManager.Subscribe<ArtifactPlayed>("ArtifactRePlayed", data =>
            {
                if (CurrentMiniPhase is ArtifactReplayMiniPhase)
                {
                    EndCurrentMiniPhase();
                }

            }, priority: 1);

            gameContext.EventManager.Subscribe<LockMercenaryData>("LockMercenary", data =>
            {
                if (CurrentMiniPhase is LockCardMiniPhase)
                {
                    EndCurrentMiniPhase();
                }
            }, priority: 5);

            gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", mercenaryPicked =>
            {
                if (mercenaryPicked.Card.LockedByPlayerInfo != null)
                {
                    StartLockCardMiniPhase();
                };
            }, priority: 15);

            gameContext.EventManager.Subscribe<BlockedTileData>("BlockedTileEvent", data =>
            {
                if (CurrentMiniPhase is BlockTileMiniPhase)
                {
                    EndCurrentMiniPhase();
                };
            }, priority: 0);

            gameContext.EventManager.Subscribe<RoyalCardPlayed>("RolayCardPlayed", data =>
            {             
                EndCurrentMiniPhase();                     
            }, priority: 2);

            gameContext.EventManager.Subscribe<ReplaceNextHeroEventData>("ReplaceNextHeroEvent", data =>
            {             
                EndCurrentMiniPhase();                     
            }, priority: 2);

            _gameContext.EventManager.Subscribe<BanishRoyalCardEventData>("BanishRoyalCardEvent", data =>
            {             
                EndCurrentMiniPhase();                    
            }, priority: 2);

            _gameContext.EventManager.Subscribe<SwapTokensDataEventData>("SwapTokensDataEvent", data =>
            {             
                EndCurrentMiniPhase();                    
            }, priority: 2);

            gameContext.EventManager.Subscribe<RotateTileEventData>("RotateTileEvent", data =>
            {             
                EndCurrentMiniPhase();                    
            }, priority: 2);

            gameContext.EventManager.Subscribe<RotateTileEventData>("BlinkTileEvent", data =>
            {             
                EndCurrentMiniPhase();                    
            }, priority: 2);

            gameContext.EventManager.Subscribe<DragonSummonEventData>("SummonDragonEvent", data =>
            {             
                EndCurrentMiniPhase();                    
            }, priority: 2);
       
            gameContext.EventManager.Subscribe<ArtifactDiscardData>("DiscardArtifactForFullMovementEvent", data =>
            {             
                EndCurrentMiniPhase();                   
            }, priority: 2);

            gameContext.EventManager.Subscribe("BuffHeroMiniPhaseEnded", () =>
            {
                _gameContext.PhaseManager.EndCurrentPhaseIfPhaseType(PhaseType.HeroCardPickingPhase, false);
            }, priority: 0);

            gameContext.EventManager.Subscribe("FulfillProphecyMiniPhaseEnded", () =>
            {
                _gameContext.PhaseManager.EndCurrentPhaseIfPhaseType(PhaseType.HeroCardPickingPhase, false);
            }, priority: 0);
        }

        private void StartCurrentMiniPhase(MiniPhase miniPhase)
        {
            CurrentMiniPhase = miniPhase;
            CurrentMiniPhase.StartMiniPhase();
        }

        public void EndCurrentMiniPhase()
        {
            if (CurrentMiniPhase != null)
            {
                CurrentMiniPhase.EndMiniPhase();
            }
            CurrentMiniPhase = null;
        }

        public void HandleMoveOnTile(TileReward tileReward)
        {

            if (tileReward.RerollMercenaryAction != null && tileReward.RerollMercenaryAction == true)
            {
                var miniPhaseClass = new RerollMercenaryMiniPhase(_gameContext);

                StartCurrentMiniPhase(miniPhaseClass);
            }
        }

        public void StartTeleportMiniPhase()
        {
            var miniPhaseClass = new TeleportMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StartArtifactPickMiniPhase()
        {
            var miniPhaseClass = new ArtifactPickMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StartMercenaryFulfillMiniPhase()
        {
            var miniPhaseClass = new FulfillProphecyMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StartLockCardMiniPhase()
        {
            var miniPhaseClass = new LockCardMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StartBuffHeroMiniPhase()
        {
            var miniPhaseClass = new BuffHeroMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StartBlockTileMiniPhase()
        {
            var miniPhaseClass = new BlockTileMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StarRoyalCardPickMiniPhase()
        {
            var miniPhaseClass = new RoyalCardPickMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StarRerollMercenaryMiniPhase()
        {
            var miniPhaseClass = new RerollMercenaryMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StarRotatePawnMiniPhase()
        {
            if(_gameContext.PawnManager._currentTile.RotateID == 14) return;

            bool bAnyValidTile = false;
            foreach(var tile in _gameContext.GameTiles.Tiles)
            {
                if(tile.RotateID == _gameContext.PawnManager._currentTile.RotateID)
                {
                    if(tile.Id != _gameContext.PawnManager._currentTile.Id)
                    {
                        if(tile.Token == null || !tile.Token.IsDragon)
                        {
                            bAnyValidTile = true;
                            break;
                        }
                    }
                }
            }

            if(!bAnyValidTile)
                return;

            var miniPhaseClass = new RotatePawnMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StartBlinkPawnMiniPhase()
        {
            bool bAnyValidTile = false;
            foreach(Connection connetion in _gameContext.PawnManager._currentTile.Connections)
            {
                var tile = _gameContext.GameTiles.GetTileById(connetion.ToId);
                if(tile.Token == null || !tile.Token.IsDragon)
                {
                    bAnyValidTile = true;
                    break;
                }
            }

            if(!bAnyValidTile)
                return;

            var miniBlinkClass = new BlinkPawnMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniBlinkClass);
        }

        public void StartSummonDragonMiniPhase()
        {
            if(_gameContext.DragonManager.PreparedNextDragonForSummon())
            {
                var miniPhaseClass = new SummonDragonMiniPhase(_gameContext);
                StartCurrentMiniPhase(miniPhaseClass);
            }
        }

        public void StartPickingDragonsForMiniPhase()
        {
            List<Dragon> dragons = _gameContext.DragonManager.PrepareDragonsToPickFrom(2);
            if(dragons.Count() == 0)
            {
                _gameContext.DragonManager.DragonsToPickFrom = null;
                return;
            }
            else if(dragons.Count() == 1)
            {
                _gameContext.DragonManager.SetNextDragonToSummon(dragons[0]);
                _gameContext.DragonManager.DragonsToPickFrom = null;
                var miniPhaseClass = new SummonDragonMiniPhase(_gameContext);
                StartCurrentMiniPhase(miniPhaseClass);
            }
            else
            {
                var miniPhaseClass = new PickDragonToSummonMiniPhase(_gameContext);
                StartCurrentMiniPhase(miniPhaseClass);
            }
        }

        public void StarReplaceHeroMiniPhase()
        {
            var miniPhaseClass = new ReplaceNextHeroMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StarBanishRoyalCardMiniPhase()
        {
            if(_gameContext.RolayCardManager.GetAvailableCardsToPick().Count <=0){
                return;
            }
            var miniPhaseClass = new BanishCarMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
            
        }

        public void StartSwapTokensMiniPhase()
        {
            var tokenInfo = _gameContext.GameTiles.GetTokenInfo();
            var tokensAmount = tokenInfo.Count(token => token.Token.InStartingPool == true);

            if(tokensAmount < 2){
                return;
            }
            var miniPhaseClass = new SwapTokenMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void ReplayArtifactMiniPhase(PlayerInGame player)
        {
            var instantArtifacts = player.ArtifactsPlayed.Where(a => a.EffectType == EffectHelper.InstantEffect).ToList();

            if(instantArtifacts.Count() == 0) return;

            
            if(instantArtifacts.Count() == 1 ){
                if(instantArtifacts[0]!.Effect2 == -1){
                    _gameContext.ArtifactManager.HandleArtifactPlay(instantArtifacts[0].InGameIndex, player, true, true);
                    return;
                }
                if(instantArtifacts[0]!.SecondEffectSuperior){
                    _gameContext.ArtifactManager.HandleArtifactPlay(instantArtifacts[0].InGameIndex, player, false, true);
                    return;
                }
            }

            var miniPhaseClass = new ArtifactReplayMiniPhase(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }

        public void StarDiscardArtifactForFullMovementMiniPhase(PlayerInGame player)
        {   
            if(player.Artifacts.Count == 0)
                return;

            var miniPhaseClass = new DiscardArtifactForFullMovement(_gameContext);
            StartCurrentMiniPhase(miniPhaseClass);
        }
    }
}