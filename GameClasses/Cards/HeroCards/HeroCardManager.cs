using BoardGameBackend.Helpers;
using BoardGameBackend.Managers;
using BoardGameBackend.Mappers;

namespace BoardGameBackend.Models
{
    public class HeroCardManager
    {
        private List<HeroCard> _heroCards;
        private List<HeroCardCombined> _heroCardCombinedList;
        private List<HeroCardCombined> _currentHeroCards;
        private int _currentPosition = 0;
        private Dictionary<int, PlayerInGame?> _takenCards;
        private GameContext _gameContext;
        private bool _moreCardsPerRound = false;

        private static Random _random = new Random();

        public HeroCardManager(GameContext gameContext, bool lessCards, bool moreCardsPerRound, HeroPoolTypes heroPool)
        {
            _gameContext = gameContext;
            _moreCardsPerRound = moreCardsPerRound;

            // Generate and filter hero cards based on players and lessCards flag
            _heroCardCombinedList = GenerateAndCombineHeroCards(gameContext.PlayerManager.Players.Count, lessCards, heroPool);
            _takenCards = new Dictionary<int, PlayerInGame?>();

            ShuffleHeroCardCombinedList();

            foreach (var card in _heroCardCombinedList)
            {
                _takenCards[card.Id] = null;
            }

            _gameContext.EventManager.Subscribe("StartTurn", () =>
            {
                StartOfRoundSetup();
            }, priority: 5);
        }

        private List<HeroCardCombined> GenerateAndCombineHeroCards(int playerCount, bool lessCards, HeroPoolTypes heroPool)
        {
            // Retrieve the combined card data from JSON
            var combinedFromJsonList = HeroesCardsFromJson.HeroesCombinedFromJsonList;

            if (lessCards)
            {
                combinedFromJsonList = combinedFromJsonList
                    .Where(card => card.NumPlayers <= playerCount)
                    .ToList();
            }

            var combinedHeroCards = combinedFromJsonList.Select(jsonCard => new HeroCardCombined
            {
                Id = jsonCard.Id,
                LeftSide = GetHeroCardById(jsonCard.LeftSide, heroPool),
                RightSide = GetHeroCardById(jsonCard.RightSide, heroPool)
            }).ToList();

            return combinedHeroCards;
        }

        private HeroCard GetHeroCardById(int id, HeroPoolTypes heroPool)
        {
            var heroCardsJson = HeroesFromJson.HeroesFromJsonList;
            if(heroPool == HeroPoolTypes.PRE_03_2025)
                heroCardsJson = HeroesFromJson.HeroesFromJsonListPre032025;

            // Assuming _gameContext or some data source has the list of HeroCards to fetch by ID
            return GameMapper.Instance.Map<HeroCard>(heroCardsJson.FirstOrDefault(h => h.Id == id));
        }

        private void ShuffleHeroCardCombinedList()
        {
            _heroCardCombinedList = _heroCardCombinedList.OrderBy(x => _random.Next()).ToList();
        }

        public List<HeroCardCombined> TakeTopNHeroCards(int n)
        {

            var takenCards = new List<HeroCardCombined>();

            for (int i = 0; i < n; i++)
            {
                _currentPosition += 1;
                if (_heroCardCombinedList[_currentPosition] != null)
                {
                    takenCards.Add(_heroCardCombinedList[_currentPosition]);
                }
                else
                {
                    break;
                }
            }

            _currentHeroCards = takenCards;

            return takenCards;
        }

        public List<HeroCardCombined> GetCurrentHeroCards()
        {
            return _currentHeroCards;
        }

        public bool? BuffHeroCard(PlayerInGame player, int heroCardId)
        {
            HeroCard? heroCard = player.PlayerHeroCardManager.HeroCardsLeft.FirstOrDefault(h => h.Id == heroCardId);
            if (heroCard == null)
            {
                heroCard = player.PlayerHeroCardManager.HeroCardsRight.FirstOrDefault(h => h.Id == heroCardId);
            }
            if (heroCard == null) return false;

            var siege = heroCard.Siege;
            var magic = heroCard.Magic;
            var army = heroCard.Army;

            player.ResourceHeroManager.AddResource(ResourceHeroType.Siege, siege);
            player.ResourceHeroManager.AddResource(ResourceHeroType.Magic, magic);
            player.ResourceHeroManager.AddResource(ResourceHeroType.Army, army);

            heroCard.Siege = siege * 2;
            heroCard.Magic = magic * 2;
            heroCard.Army = army * 2;

            Dictionary<ResourceHeroType, int> heroResources = new Dictionary<ResourceHeroType, int> {
                { ResourceHeroType.Army, heroCard.Army },
                { ResourceHeroType.Siege, heroCard.Siege },
                { ResourceHeroType.Magic, heroCard.Magic },
            };

            BuffHeroData eventArgs = new BuffHeroData { PlayerId = player.Id, HeroId = heroCardId, HeroResourcesNew = heroResources };
            _gameContext.EventManager.Broadcast("HeroCardBuffed", ref eventArgs);

            return true;
        }

        public bool SetReplacementForNextHero(PlayerInGame player, int heroCardId)
        {
            HeroFullInfo? heroCardInfo = player.PlayerHeroCardManager.GetHeroCardById(heroCardId);

            if (heroCardInfo == null) return false;

            HeroCard? heroCard = heroCardInfo.HeroCard;

            AuraTypeWithLongevity replacementHeroAura = new AuraTypeWithLongevity { Aura = AurasType.REPLACE_NEXT_HERO, Value1 = heroCardId, Permanent = false };

            player.AurasTypes.Add(replacementHeroAura);
            ReplaceNextHeroEventData replaceNextHeroData = new ReplaceNextHeroEventData
            {
                Hero = heroCard,
                PlayerId = player.Id,
                ReplacementHeroAura = replacementHeroAura,
            };

            _gameContext.EventManager.Broadcast("ReplaceNextHeroEvent", ref replaceNextHeroData);

            return true;
        }

        public HeroCard? TakeHeroCard(PlayerInGame player, int heroCardId)
        {
            HeroCard? heroCard = null;
            HeroCard? unusedHeroCard = null;
            bool leftSide = false;
            for (var i = 0; i < _heroCardCombinedList.Count; i++)
            {
                var id = _heroCardCombinedList[i].Id;

                var leftCard = _heroCardCombinedList[i].LeftSide;
                if (leftCard.Id == heroCardId)
                {
                    if (_takenCards[id] != null)
                    {
                        break;
                    }

                    heroCard = leftCard;
                    unusedHeroCard = _heroCardCombinedList[i].RightSide;

                    var playerViewModel = GameMapper.Instance.Map<PlayerViewModel>(player);
                    _heroCardCombinedList[i].PlayerWhoPickedCard = playerViewModel;

                    leftSide = true;
                    _takenCards[id] = player;
                    break;
                }

                var rightCard = _heroCardCombinedList[i].RightSide;
                if (rightCard.Id == heroCardId)
                {
                    if (_takenCards[id] != null)
                    {
                        break;
                    }

                    unusedHeroCard = _heroCardCombinedList[i].LeftSide;
                    heroCard = rightCard;

                    var playerViewModel = GameMapper.Instance.Map<PlayerViewModel>(player);
                    _heroCardCombinedList[i].PlayerWhoPickedCard = playerViewModel;

                    _takenCards[id] = player;
                    break;
                }

            }
            if (heroCard != null && unusedHeroCard != null)
            {
                var preEventArgs = new PreHeroCardPickedEventData { PlayerId = player.Id, HeroCard = heroCard, WasOnLeftSide = leftSide };
                _gameContext.EventManager.Broadcast("PreHeroCardPicked", ref preEventArgs);
                var replacedHero = preEventArgs.ReplacedHero;
                heroCard = preEventArgs.HeroCard;
                leftSide = preEventArgs.WasOnLeftSide;

                var currentHeroCard = player.SetCurrentHeroCard(heroCard, leftSide, _gameContext, unusedHeroCard, replacedHero);

                Reward? reward = null;
                var effectId = heroCard.EffectId;
                if (effectId != null)
                {
                    var heroRewardClass = RewardFactory.GetRewardById(effectId.Value);
                    if (heroRewardClass != null)
                    {
                        reward = heroRewardClass.OnReward();
                        _gameContext.RewardHandlerManager.HandleReward(player, reward);
                    }
                    else
                    {
                        Console.WriteLine("ERROR LACKING REWARD");
                    }
                }
                else
                {
                    // Tutaj można testować nowe umiejętności.
              //      Dragon? topdeckdragon = _gameContext.DragonManager.GetDragonFromTopDeck();
              //      if(topdeckdragon != null)
              //         _gameContext.DragonManager.AcquireDragonCard(topdeckdragon, player);
              //     reward = RewardFactory.GetRewardById(141).OnReward();
              //     _gameContext.RewardHandlerManager.HandleReward(player, reward);
                }

                var eventArgs = new HeroCardPicked(heroCard, player, currentHeroCard, reward);
                _gameContext.EventManager.Broadcast("HeroCardPicked", ref eventArgs);
            }
            return heroCard;
        }

        public void StartOfRoundSetup(){
            if (_gameContext.TurnManager.CurrentTurn % 2 == 0) return;

            SetUpNewCards();
            SetupTokens();
        }

        public void SetUpNewCards()
        {
            List<HeroCardCombined>? newCards;
            

            var amount = _gameContext.PlayerManager.Players.Count * 2;
            if (_moreCardsPerRound == true)
            {
                amount = amount + 1;
            }
            newCards = _gameContext.HeroCardManager.TakeTopNHeroCards(amount);

            var eventArgs = new NewCardsSetupData
            {
                Player = _gameContext.TurnManager.CurrentPlayer!,
                TurnCount = _gameContext.TurnManager.CurrentTurn,
                NewCards = newCards,
                RoundCount = _gameContext.TurnManager.CurrentRound
            };
            
            _gameContext.EventManager.Broadcast("NewCardsSetup", ref eventArgs);

        }

        public void SetupTokens(){
            List<TokenTileInfo> newTokens = new List<TokenTileInfo> {};

            var token = _gameContext.TokenManager.GetTokenById(TileHelper.MarketTokenId);

            if(token == null) return;

            _gameContext.GameTiles.Tiles.ForEach(tile => {
                if(tile.TileTypeId == TileHelper.MarketTileId && tile.Token == null){
                    tile.Token = token;
                    newTokens.Add(new TokenTileInfo { TileId = tile.Id, Token = token});
                }
            });

            NewTokensSetupEventData eventArgs = new NewTokensSetupEventData {
                NewTokens = newTokens,
            };

            _gameContext.EventManager.Broadcast("NewTokensSetup", ref eventArgs);
        }
    }
}