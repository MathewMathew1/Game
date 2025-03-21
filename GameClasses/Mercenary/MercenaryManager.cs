using BoardGameBackend.Helpers;
using BoardGameBackend.Mappers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class MercenaryManager
    {
        private List<Mercenary> _mercenaries;
        public List<Mercenary> BuyableMercenaries { get; private set; } = new List<Mercenary>();
        public List<Mercenary> TossedAwayMercenaries { get; private set; } = new List<Mercenary>();
        private int BuyableMercenariesCount = 3;
        private int _nextInGameIndex = 1;
        private GameContext _gameContext;
        private bool _removePropheciesAtLastRound = false;
        private bool _sameAmountOfMercenariesEachRound = false;

        public MercenaryManager(GameContext gameContext, bool removePropheciesAtLastRound, bool sameAmountOfMercenariesEachRound)
        {
            _sameAmountOfMercenariesEachRound = sameAmountOfMercenariesEachRound;
            if(_sameAmountOfMercenariesEachRound){
                BuyableMercenariesCount = 5;
            }
            _removePropheciesAtLastRound = removePropheciesAtLastRound;
            _gameContext = gameContext;
            _mercenaries = new List<Mercenary>();

            bool bIsDragonDLCOn = gameContext.IsDLCDragonsOn();
            bool bNoBuildingsAllows = gameContext.NoBuildingsInPool();

            foreach (var mercenaryFromJson in MercenariesFactory.MercenariesFromJsonList)
            {
                if(bIsDragonDLCOn || !mercenaryFromJson.DragonDLC)
                {
                    if(!bNoBuildingsAllows || mercenaryFromJson.TypeCard != MercenaryHelper.BuildingCardType)
                    {
                        for (int i = 0; i < mercenaryFromJson.ShuffleX; i++)
                        {
                            var mercenary = GameMapper.Instance.Map<Mercenary>(mercenaryFromJson);
                            mercenary.InGameIndex = _nextInGameIndex++;
                            _mercenaries.Add(mercenary);
                        }
                    }
                }
            }

            ShuffleMercenaries();

            for (int i = 0; i < BuyableMercenariesCount; i++)
            {
                if (_mercenaries.Count > 0)
                {
                    BuyableMercenaries.Add(_mercenaries[0]);
                    _mercenaries.RemoveAt(0);
                }
            }

            _gameContext.EventManager.Subscribe("EndOfRound", (EndOfRoundData data) =>
            {
                EndOfRound();
                data.EndOfRoundMercenaryData.MercenariesLeftData = new MercenariesLeftData
                {
                    MercenariesAmount = _mercenaries.Count,
                    TossedMercenariesAmount = TossedAwayMercenaries.Count
                };
                data.EndOfRoundMercenaryData.Mercenaries = BuyableMercenaries;

            }, priority: 5);

            gameContext.EventManager.Subscribe("RerollMercenaryMiniPhaseStarted", () =>
            {
                AddRerolledMercenary();
            }, priority: 0);


        }

        private void ShuffleMercenaries()
        {
            Random rng = new Random();
            _mercenaries = _mercenaries.OrderBy(m => rng.Next()).ToList();
        }

        public bool BuyMercenary(int mercenaryInGameIndex, PlayerInGame player)
        {
            var boughtMercenaryIndex = BuyableMercenaries.FindIndex(mercenary => mercenary.InGameIndex == mercenaryInGameIndex);
            if (boughtMercenaryIndex == -1)
            {
                return false;
            }

            var boughtMercenary = BuyableMercenaries[boughtMercenaryIndex];
            List<ResourceInfo> ResourcesToSpend = new List<ResourceInfo>(boughtMercenary.ResourcesNeeded);

            ReduceRequiredResourcesByAura(ResourcesToSpend, player, boughtMercenary);

            if (boughtMercenary.Req != null)
            {
                var requirement = RequirementMovementStore.GetRequirementById(boughtMercenary.Req.Value);
                if (requirement != null)
                {
                    var fulifiedRequirement = requirement.CheckRequirements(player);
                    if (!fulifiedRequirement) return false;
                }
            }

            if (boughtMercenary.LockedByPlayerInfo != null && boughtMercenary.LockedByPlayerInfo.PlayerId != player.Id) return false;


            var canBuyMercenary = false;

            if (player.AurasTypes.FindIndex(aura => aura.Aura == AurasType.BUY_CARDS_BY_ANY_RESOURCE) == -1)
            {
                canBuyMercenary = player.ResourceManager.CheckForResourceAndRemoveThem(ResourcesToSpend, _gameContext.EventManager);
            }
            else
            {
                canBuyMercenary = player.ResourceManager.CheckForResourceAndRemoveThemWithSubstitue(ResourcesToSpend, _gameContext.EventManager);
            }


            if (!canBuyMercenary)
            {
                return false;
            }

            BuyableMercenaries.RemoveAt(boughtMercenaryIndex);

            Mercenary? newMercenary = AddNewMercenary();

            player.AddMercenary(boughtMercenary);
            if (boughtMercenary.Morale != 0)
            {
                _gameContext.PlayerManager.AddMoraleToPlayer(player, boughtMercenary.Morale);
            }

            Reward? reward = null;
            if (boughtMercenary.EffectId != null && boughtMercenary.TypeCard != MercenaryHelper.ProphecyCardType)
            {
                var mercenaryRewardClass = RewardFactory.GetRewardById(boughtMercenary.EffectId.Value);
                reward = mercenaryRewardClass.OnReward();
                _gameContext.RewardHandlerManager.HandleReward(player, reward);
            }

            var eventArgs = new MercenaryPicked
            {
                Reward = reward,
                Card = boughtMercenary,
                Player = player,
                ResourcesSpend = ResourcesToSpend,
                MercenaryReplacement = newMercenary,
                MercenariesLeftData = new MercenariesLeftData { MercenariesAmount = _mercenaries.Count, TossedMercenariesAmount = TossedAwayMercenaries.Count }
            };
            _gameContext.EventManager.Broadcast("MercenaryPicked", ref eventArgs);

            return true;
        }

        public List<Mercenary> GetShuffledMercenaries()
        {
            return _mercenaries;
        }

        public Mercenary? AddNewMercenary()
        {
            Mercenary? newMercenary = null;
            if (_mercenaries.Count <= 0)
            {
                _mercenaries = TossedAwayMercenaries;
                TossedAwayMercenaries.Clear();
            }

             if (_mercenaries.Count <= 0)
            {
                return newMercenary;
            }

            newMercenary = _mercenaries[0];
            BuyableMercenaries.Add(newMercenary);
            _mercenaries.RemoveAt(0);

            return newMercenary;
        }

        public void EndOfRound()
        {
            if (_removePropheciesAtLastRound && _gameContext.TurnManager.CurrentRound == 5) RemoveProphecyMercenaries();

            if(!_sameAmountOfMercenariesEachRound){
                BuyableMercenariesCount += 1;
            }
            
            var mercenariesToRemove = new List<Mercenary>();

            if(!_gameContext.NoEndRoundDiscount())
            {
                BuyableMercenaries.ForEach(mercenary =>
                {
                    if (mercenary.LockedByPlayerInfo != null) return;
                    mercenary.GoldDecrease += 1;
                    var goldNeeded = mercenary.ResourcesNeeded.Find(x => x.Name == ResourceType.Gold);

                    if (goldNeeded!.Amount - mercenary.GoldDecrease <= 0)
                    {
                        TossedAwayMercenaries.Add(mercenary);
                        mercenariesToRemove.Add(mercenary);
                    }
                });
            }

            foreach (var mercenary in mercenariesToRemove)
            {
                BuyableMercenaries.Remove(mercenary);
                AddNewMercenary();
            }

            var MercenariesToAdd = BuyableMercenariesCount - BuyableMercenaries.Count;
            for (var i = 0; i < MercenariesToAdd; i++)
            {
                AddNewMercenary();
            }
        }

        public void RemoveProphecyMercenaries()
        {
            BuyableMercenaries = BuyableMercenaries.Where(m => m.TypeCard != MercenaryHelper.ProphecyCardType || m.LockedByPlayerInfo != null).ToList();
            TossedAwayMercenaries = TossedAwayMercenaries.Where(m => m.TypeCard != MercenaryHelper.ProphecyCardType).ToList();
            _mercenaries = _mercenaries.Where(m => m.TypeCard != MercenaryHelper.ProphecyCardType).ToList();
        }

        public void AddRerolledMercenary(){
            Mercenary? newMercenary = AddNewMercenary();

            var eventArgs = new PreMercenaryRerolled
            {
                MercenaryReplacement = newMercenary,
                MercenariesLeftData = new MercenariesLeftData { MercenariesAmount = _mercenaries.Count, TossedMercenariesAmount = TossedAwayMercenaries.Count }
            };

            _gameContext.EventManager.Broadcast("PreMercenaryRerolled", ref eventArgs);

        }

        public bool RerollMercenary(int mercenaryInGameIndex, PlayerInGame player)
        {
            var boughtMercenaryIndex = BuyableMercenaries.FindIndex(mercenary => mercenary.InGameIndex == mercenaryInGameIndex);
            if (boughtMercenaryIndex == -1)
            {
                return false;
            }

            var replacedMercenary = BuyableMercenaries[boughtMercenaryIndex];

            if (replacedMercenary.LockedByPlayerInfo != null) return false;

            TossedAwayMercenaries.Add(replacedMercenary);
            BuyableMercenaries.RemoveAt(boughtMercenaryIndex);

            var eventArgs = new MercenaryRerolled
            {
                Card = replacedMercenary,
                MercenariesLeftData = new MercenariesLeftData { MercenariesAmount = _mercenaries.Count, TossedMercenariesAmount = TossedAwayMercenaries.Count }
            };
            _gameContext.EventManager.Broadcast("MercenaryRerolled", ref eventArgs);

            return true;
        }

        public void RefreshBuyableMercenaries()
        {
            int iToAdd = 0;
            foreach(var oldMerc in BuyableMercenaries)
            {
                if(oldMerc.LockedByPlayerInfo == null)
                {
                    iToAdd++;
                    TossedAwayMercenaries.Add(oldMerc);
                }
            }
            BuyableMercenaries = BuyableMercenaries.FindAll(m => m.LockedByPlayerInfo != null);

            for (int i = 0; i < iToAdd; i++)
            {
                if (_mercenaries.Count > 0)
                {
                    AddNewMercenary();
                }
            }

            var eventArgs = new BuyableMercenariesRefreshed
            {
                NewBuyableMercenaries = BuyableMercenaries,
                MercenariesLeftData = new MercenariesLeftData { MercenariesAmount = _mercenaries.Count, TossedMercenariesAmount = TossedAwayMercenaries.Count }
            };
            _gameContext.EventManager.Broadcast("BuyableMercenariesRefreshed", ref eventArgs);
        }

        public int GetRemainingMercenariesCount()
        {
            return _mercenaries.Count;
        }

        public MercenaryData GetMercenaryData()
        {
            return new MercenaryData
            {
                TossedMercenariesAmount = TossedAwayMercenaries.Count,
                BuyableMercenaries = BuyableMercenaries,
                RemainingMercenariesAmount = _mercenaries.Count
            };
        }

        public bool FulfillProphecy(PlayerInGame player, int mercenaryId)
        {
            player.PlayerMercenaryManager.SetMercenaryProphecyFulfill(mercenaryId);

            var eventArgs = new FulfillProphecy
            {
                MercenaryId = mercenaryId,
                PlayerId = player.Id
            };
            _gameContext.EventManager.Broadcast("FulfillProphecy", ref eventArgs);

            return true;
        }

        public bool LockMercenary(PlayerInGame player, int mercenaryId)
        {
            var lockedMercenary = BuyableMercenaries.Find(mercenary => mercenary.InGameIndex == mercenaryId);
            if (lockedMercenary == null)
            {
                return false;
            }

            var lockedByPlayerInfo = new LockedByPlayerInfo { PlayerId = player.Id, PlayerName = player.Name };
            lockedMercenary.LockedByPlayerInfo = lockedByPlayerInfo;

            var eventArgs = new LockMercenaryData
            {
                LockMercenary = lockedByPlayerInfo,
                MercenaryId = mercenaryId
            };

            _gameContext.EventManager.Broadcast("LockMercenary", ref eventArgs);
            return true;
        }

        public void ReduceRequiredResourcesByAura(List<ResourceInfo> resources, PlayerInGame player, Mercenary mercenary)
        {
            resources.ForEach(r =>
            {
                if (r.Name == ResourceType.Gold)
                {
                    r.Amount = Math.Max(r.Amount - mercenary.GoldDecrease, 0);
                }
            });

            player.AurasTypes.ForEach(a =>
            {
                if (a.Aura == AurasType.CHEAPER_BUILDINGS && mercenary.TypeCard == MercenaryHelper.BuildingCardType)
                {
                    resources.ForEach(r =>
                    {
                        if (r.Name == ResourceType.Gold && r.Amount > 0)
                        {
                            r.Amount = Math.Max(r.Amount - 2, 0);
                        }
                    });
                }
                if (a.Value1 != null && a.Aura == AurasType.MAKE_CHEAPER_MERCENARIES && a.Value1 == mercenary.Faction?.Id)
                {
                    resources.ForEach(r =>
                    {
                        if (r.Name == ResourceType.Gold && r.Amount > 0)
                        {
                            r.Amount -= 1;
                        }
                    });
                }
            });
        }
    }
}