
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public  class PlayerRolayCardManager
    {
        private readonly int SIGNETS_FOR_EACH_CARD = 3;
        public readonly List<RolayCard> RolayCards = new List<RolayCard>();
        public int SignetsNeededForNextCard;
        private bool m_bSpecialThreshold;
        private int m_iNextThresholdGrowth;

        public PlayerRolayCardManager(bool m_bSignets25914)
        {
            m_bSpecialThreshold = m_bSignets25914;
            if(m_bSpecialThreshold)
            {
                SignetsNeededForNextCard = 2;
                m_iNextThresholdGrowth = 3;
            }
            else
            {
                SignetsNeededForNextCard = SIGNETS_FOR_EACH_CARD;
                m_iNextThresholdGrowth = SIGNETS_FOR_EACH_CARD;
            }
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

            SignetsNeededForNextCard += m_iNextThresholdGrowth;
            if(m_bSpecialThreshold)
                m_iNextThresholdGrowth++;

            return true;
        } 
    
    }
}