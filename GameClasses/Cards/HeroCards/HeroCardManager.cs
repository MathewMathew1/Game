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

        private static Random _random = new Random();

        public HeroCardManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _heroCards = GenerateRandomHeroCards();
            _heroCardCombinedList = CombineHeroCards(_heroCards);
            _takenCards = new Dictionary<int, PlayerInGame?>();
            ShuffleHeroCardCombinedList();

            foreach (var card in _heroCardCombinedList)
            {
                _takenCards[card.Id] = null;
            }
        }

        private List<HeroCard> GenerateRandomHeroCards()
        {
            var heroCardsJson = HeroesFromJson.HeroesFromJsonList;

            List<HeroCard> heroCards = heroCardsJson
                .Select(heroCardJson => GameMapper.Instance.Map<HeroCard>(heroCardJson))
                .ToList();

            return heroCards;
        }

        private List<HeroCardCombined> CombineHeroCards(List<HeroCard> heroCards)
        {
            var combinedList = new List<HeroCardCombined>();

            for (int i = 0; i < heroCards.Count - 1; i += 2)
            {
                var combinedCard = new HeroCardCombined
                {
                    Id = i + 1,
                    LeftSide = heroCards[i],
                    RightSide = heroCards[i + 1]
                };
                combinedList.Add(combinedCard);
            }

            return combinedList;
        }

        private void ShuffleHeroCardCombinedList()
        {
            _heroCardCombinedList = _heroCardCombinedList.OrderBy(x => _random.Next()).ToList();
        }

        public List<HeroCardCombined> TakeTopNHeroCards(int n)
        {
            if (n <= 0 || n > _heroCardCombinedList.Count)
                throw new ArgumentException("Invalid number of cards requested.");

            var takenCards = new List<HeroCardCombined>();

            for (int i = 0; i < n; i++)
            {
                int index = (_currentPosition + i) % _heroCardCombinedList.Count;
                takenCards.Add(_heroCardCombinedList[index]);
            }

            _currentPosition = (_currentPosition + n) % _heroCardCombinedList.Count;

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

            BuffHeroData eventArgs = new BuffHeroData { PlayerId = player.Id, HeroId = heroCardId, HeroResourcesNew = heroResources};
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
                var eventArgs = new HeroCardPicked(heroCard, player);
                _gameContext.EventManager.Broadcast("HeroCardPicked", ref eventArgs);
            }
            return heroCard;


        }
    }
}