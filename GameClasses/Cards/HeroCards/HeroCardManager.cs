using BoardGameBackend.Managers;
using BoardGameBackend.Mappers;

namespace BoardGameBackend.Models
{
    public class HeroCardManager
    {
        private List<HeroCard> _heroCards;
        private List<HeroCardCombined> _heroCardCombinedList;
        private int _currentPosition = 0;
        private Dictionary<int, PlayerInGame?> _takenCards;
        private GameContext _gameContext;
        private bool _moreCardsPerRound = false;

        private static Random _random = new Random();

        public HeroCardManager(GameContext gameContext, bool lessCards, bool moreCardsPerRound)
        {
            _gameContext = gameContext;
            _moreCardsPerRound = moreCardsPerRound;

            // Generate and filter hero cards based on players and lessCards flag
            _heroCardCombinedList = GenerateAndCombineHeroCards(gameContext.PlayerManager.Players.Count, lessCards);
            _takenCards = new Dictionary<int, PlayerInGame?>();

            ShuffleHeroCardCombinedList();

            foreach (var card in _heroCardCombinedList)
            {
                _takenCards[card.Id] = null;
            }

            _gameContext.EventManager.Subscribe("StartTurn", () =>
            {
                SetUpNewCards();
            }, priority: 5);
        }

        private List<HeroCardCombined> GenerateAndCombineHeroCards(int playerCount, bool lessCards)
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
                LeftSide = GetHeroCardById(jsonCard.LeftSide),
                RightSide = GetHeroCardById(jsonCard.RightSide)
            }).ToList();

            return combinedHeroCards;
        }

        private HeroCard GetHeroCardById(int id)
        {
            // Assuming _gameContext or some data source has the list of HeroCards to fetch by ID
            var heroCardsJson = HeroesFromJson.HeroesFromJsonList;
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

            return takenCards;
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

        public HeroCard? TakeHeroCard(PlayerInGame player, int heroCardId)
        {
            HeroCard? heroCard = null;
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

                    heroCard = rightCard;
                    _takenCards[id] = player;
                    break;
                }

            }
            if (heroCard != null)
            {
                player.SetCurrentHeroCard(heroCard, leftSide);
                _gameContext.PlayerManager.AddMoraleToPlayer(player, heroCard.Morale);

                Reward? reward = null;
                if(heroCard.EffectId != null){
                    var heroRewardClass = RewardFactory.GetRewardById(heroCard.EffectId.Value);
                    reward = heroRewardClass.OnReward();
                    _gameContext.RewardHandlerManager.HandleReward(player, reward);
                }
            
                var eventArgs = new HeroCardPicked(heroCard, player, reward);
                _gameContext.EventManager.Broadcast("HeroCardPicked", ref eventArgs);
            }
            return heroCard;


        }

        public void SetUpNewCards()
        {
            List<HeroCardCombined>? newCards;
            if (_gameContext.TurnManager.CurrentTurn % 2 != 0)
            {
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

        }
    }
}