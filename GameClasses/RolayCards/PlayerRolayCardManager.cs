
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public  class PlayerRolayCardManager
    {
        private readonly int SIGNETS_FOR_EACH_CARD = 3;
        public readonly List<RolayCard> RolayCards = new List<RolayCard>();
        public int SignetsNeededForNextCard;

        public PlayerRolayCardManager()
        {
            SignetsNeededForNextCard = SIGNETS_FOR_EACH_CARD;
        }

        public void AddRolayCard(RolayCard card){
            RolayCards.Add(card);
        }

        public RoyalCardsPlayerData GetData(){
            return new RoyalCardsPlayerData{
                RoyalCards = RolayCards,
                SignetsNeededForNextCard = SignetsNeededForNextCard
            };
        }

        public bool IsNewRolayCardToPick(int amountOfSignets){
            if(amountOfSignets<SignetsNeededForNextCard) return false;

            SignetsNeededForNextCard += SIGNETS_FOR_EACH_CARD;
            return true;
        } 
    
    }
}