using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PlayerHeroCardManager
    {
        public List<HeroCard> HeroCardsLeft { get; set; } = new List<HeroCard>();
        public List<HeroCard> HeroCardsRight { get; set; } = new List<HeroCard>();

        public CurrentHeroCard? CurrentHeroCard { get; set; }

        public void AddHeroCardLeft(HeroCard heroCard)
        {
            HeroCardsLeft.Add(heroCard);

        }

        public void ResetCurrentHeroCard()
        {
            if (CurrentHeroCard == null) return;

            if (CurrentHeroCard.LeftSide)
            {
                HeroCardsLeft.Add(CurrentHeroCard.HeroCard);
            }
            else
            {
                HeroCardsRight.Add(CurrentHeroCard.HeroCard);
            }

            CurrentHeroCard = null;
        }

        public void AddHeroCardRight(HeroCard heroCard)
        {
            HeroCardsRight.Add(heroCard);
        }

        public int AmountOfHeroesOfFaction(int factionId)
        {
            var amountOfHerosOfFractionOnTheLeft = HeroCardsLeft.Count(card => card.Faction.Id == factionId);
            var amountOfHerosOfFractionOnTheRight = HeroCardsRight.Count(card => card.Faction.Id == factionId);
            var amountOfHerosOfFractionOnTheCenter = CurrentHeroCard?.HeroCard.Faction.Id == factionId ? 1 : 0;

            var amountHeroesOfThatFactions = amountOfHerosOfFractionOnTheCenter + amountOfHerosOfFractionOnTheLeft + amountOfHerosOfFractionOnTheRight;
            
            return amountHeroesOfThatFactions;
        }


    }


}