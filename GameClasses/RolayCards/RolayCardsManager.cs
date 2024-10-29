using BoardGameBackend.Managers;
using BoardGameBackend.Mappers;

namespace BoardGameBackend.Models
{
    public class RolayCardManager
    {
        private List<RolayCard> _cards = new List<RolayCard>();
        private GameContext _gameContext;

        public RolayCardManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            List<RolayCardFromJson> cardsFromJson = new List<RolayCardFromJson>(RolayCardsFactory.RolayCardsFromJsonList);
            foreach (var cardFromJson in cardsFromJson)
            {
                
                var rolayCard = GameMapper.Instance.Map<RolayCard>(cardFromJson);
                _cards.Add(rolayCard);

            }
        }

        public List<RolayCard> GetAvailableCardsToPick(){
            return _cards.Where(card => card.BanishedCard == false && card.PickedByPlayer == null).ToList();
        }

        public RolayCard? BanishCard(PlayerInGame player, int royalCardId)
        {
            var pickedCard = _cards.FirstOrDefault(c => c.Id == royalCardId && c.PickedByPlayer == null);

            if (pickedCard == null)
            {
                return null;
            }

            if(pickedCard.BanishedCard == true || pickedCard.PickedByPlayer != null) return null;

            pickedCard.BanishedCard = true;

            var eventArgs = new BanishRoyalCardEventData
            {
                RoyalCard = pickedCard,
                PlayerId = player.Id
            };

            _gameContext.EventManager.Broadcast("BanishRoyalCardEvent", ref eventArgs);

            return pickedCard;
        }

        public RolayCard? PickCardForPlayer(PlayerInGame player, int rolayCardId)
        {
            var pickedCard = _cards.FirstOrDefault(c => c.Id == rolayCardId && c.PickedByPlayer == null);

            if (pickedCard == null)
            {
                return null;
            }

            if(pickedCard.BanishedCard == true || pickedCard.PickedByPlayer != null) return null;

            var playerViewModel = GameMapper.Instance.Map<PlayerViewModel>(player);
            pickedCard.PickedByPlayer = playerViewModel;

            Reward? royalCardRewards = null;
            if(pickedCard.EffectId != null){
                var royalCardReward = RewardFactory.GetRewardById(pickedCard.EffectId.Value);
                royalCardRewards = royalCardReward.OnReward();

                _gameContext.RewardHandlerManager.HandleReward(player, royalCardRewards);
            }   
            

            player.AddRoyalCard(pickedCard);
            if (pickedCard.Morale > 0)
            {
                _gameContext.PlayerManager.AddMoraleToPlayer(player, pickedCard.Morale);
            }

            var eventArgs = new RoyalCardPlayed
            {
                RoyalCard = pickedCard,
                PlayerId = player.Id,
                Reward = royalCardRewards,
                AmountOfSignetsForNextRoyalCard = player.PlayerRolayCardManager.SignetsNeededForNextCard
            };
            _gameContext.EventManager.Broadcast("RolayCardPlayed", ref eventArgs);

            return pickedCard;
        }

        public List<RolayCard> GetRolayCards()
        {
            return _cards;
        }



    }
}
