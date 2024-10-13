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

        public MercenaryManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _mercenaries = new List<Mercenary>();

            foreach (var mercenaryFromJson in MercenariesFactory.MercenariesFromJsonList)
            {
                for (int i = 0; i < mercenaryFromJson.ShuffleX; i++)
                {
                    var mercenary = GameMapper.Instance.Map<Mercenary>(mercenaryFromJson);
                    mercenary.InGameIndex = _nextInGameIndex++;
                    _mercenaries.Add(mercenary);
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
            var ResourcesToSpend = boughtMercenary.ResourcesNeeded;

            if(boughtMercenary.Req != null){
                var requirement = RequirementMovementStore.GetRequirementById(boughtMercenary.Req.Value);
                if(requirement != null){
                    var fulifiedRequirement = requirement.CheckRequirements(player);
                    if(!fulifiedRequirement) return false;
                }            
            }

            if(boughtMercenary.LockedByPlayerInfo != null && boughtMercenary.LockedByPlayerInfo.PlayerId != player.Id ) return false;
            

            var canBuyMercenary = false;

            if(player.AurasTypes.FindIndex(aura => aura.Aura == AurasType.BUY_CARDS_BY_ANY_RESOURCE) == -1){
                canBuyMercenary = player.ResourceManager.CheckForResourceAndRemoveThem(ResourcesToSpend);
            }else{
                canBuyMercenary = player.ResourceManager.CheckForResourceAndRemoveThemWithSubstitue(ResourcesToSpend);
            }
            

            if (!canBuyMercenary)
            {
                return false;
            }

            BuyableMercenaries.RemoveAt(boughtMercenaryIndex);

            Mercenary newMercenary = AddNewMercenary();

            player.AddMercenary(boughtMercenary);
            if (boughtMercenary.Morale != 0)
            {
                _gameContext.PlayerManager.AddMoraleToPlayer(player, boughtMercenary.Morale);
            }
            
            _gameContext.PlayerManager.AddMoraleToPlayer(player, boughtMercenary.Morale);
            var eventArgs = new MercenaryPicked
            {
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

        public Mercenary AddNewMercenary()
        {
            Mercenary newMercenary;
            if (_mercenaries.Count <= 0)
            {
                _mercenaries = TossedAwayMercenaries;
                TossedAwayMercenaries.Clear();
            }

            newMercenary = _mercenaries[0];
            BuyableMercenaries.Add(newMercenary);
            _mercenaries.RemoveAt(0);

            return newMercenary;
        }

        public void EndOfRound()
        {
            BuyableMercenariesCount += 1;
            var mercenariesToRemove = new List<Mercenary>();

            BuyableMercenaries.ForEach(mercenary =>
            {
                if(mercenary.LockedByPlayerInfo != null) return;
                mercenary.GoldDecrease += 1;
                var goldNeeded = mercenary.ResourcesNeeded.Find(x => x.Name == ResourceType.Gold);

                if (goldNeeded!.Amount - mercenary.GoldDecrease <= 0)
                {
                    TossedAwayMercenaries.Add(mercenary);
                    mercenariesToRemove.Add(mercenary);
                }
            });

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

        public bool RerollMercenary(int mercenaryInGameIndex, PlayerInGame player)
        {
            var boughtMercenaryIndex = BuyableMercenaries.FindIndex(mercenary => mercenary.InGameIndex == mercenaryInGameIndex);
            if (boughtMercenaryIndex == -1)
            {
                return false;
            }     

            var replacedMercenary = BuyableMercenaries[boughtMercenaryIndex];

            if(replacedMercenary.LockedByPlayerInfo != null && replacedMercenary.LockedByPlayerInfo.PlayerId != player.Id ) return false;
            
            TossedAwayMercenaries.Add(replacedMercenary);
            BuyableMercenaries.RemoveAt(boughtMercenaryIndex);

            Mercenary newMercenary = AddNewMercenary();

            var eventArgs = new MercenaryRerolled
            {
                Card = replacedMercenary,
                MercenaryReplacement = newMercenary,
                MercenariesLeftData = new MercenariesLeftData { MercenariesAmount = _mercenaries.Count, TossedMercenariesAmount = TossedAwayMercenaries.Count }
            };
            _gameContext.EventManager.Broadcast("MercenaryRerolled", ref eventArgs);

            return true;
        }

        public void RefreshBuyableMercenaries()
        {
            TossedAwayMercenaries.AddRange(BuyableMercenaries);

            BuyableMercenaries.Clear();

            for (int i = 0; i < BuyableMercenariesCount; i++)
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
                BuyableMercenaries = BuyableMercenaries,
                RemainingMercenariesAmount = _mercenaries.Count
            };
        }

        public bool FulfillProphecy(PlayerInGame player, int mercenaryId){
            player.PlayerMercenaryManager.SetMercenaryProphecyFulfill(mercenaryId);

            var eventArgs = new FulfillProphecy
            {
                MercenaryId = mercenaryId,
                PlayerId = player.Id
            };
            _gameContext.EventManager.Broadcast("FulfillProphecy", ref eventArgs);

            return true;
        }

        public bool LockMercenary(PlayerInGame player, int mercenaryId){
            var lockedMercenary = BuyableMercenaries.Find(mercenary => mercenary.InGameIndex == mercenaryId);
            if (lockedMercenary == null)
            {
                return false;
            }

            var lockedByPlayerInfo = new LockedByPlayerInfo {PlayerId = player.Id, PlayerName = player.Name};
            lockedMercenary.LockedByPlayerInfo = lockedByPlayerInfo;
            
            var eventArgs = new LockMercenaryData
            {
                LockMercenary = lockedByPlayerInfo,
                MercenaryId = mercenaryId
            };
            
            _gameContext.EventManager.Broadcast("LockMercenary", ref eventArgs);
            return true;
        }
    }
}