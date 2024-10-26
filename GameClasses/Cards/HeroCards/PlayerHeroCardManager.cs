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

        public HeroFullInfo? GetHeroCardById(int heroCardId){
            var onLeft = true;
            var hero = HeroCardsLeft.FirstOrDefault(x => x.Id == heroCardId);
            if(hero == null){
                onLeft = false;
                hero = HeroCardsRight.FirstOrDefault(x => x.Id == heroCardId);
            }

            if(hero == null) return null;

            return new HeroFullInfo {HeroCard = hero, LeftSide = onLeft};
        }

        public void RemoveHeroCardById(int heroCardId){
            HeroCardsLeft.RemoveAll(x => x.Id == heroCardId);
            HeroCardsRight.RemoveAll(x => x.Id == heroCardId);
        }

        public PlayerHeroData GetPlayerHeroData(){
            return new PlayerHeroData{
                LeftHeroCards = HeroCardsLeft,
                RightHeroCards = HeroCardsRight,
                CurrentHeroCard = CurrentHeroCard,
            };
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

            if(CurrentHeroCard.ReplacedHeroCard != null){
                if (CurrentHeroCard.ReplacedHeroCard.WasOnLeftSide)
                {
                    HeroCardsLeft.Add(CurrentHeroCard.ReplacedHeroCard.HeroCard);
                }
                else
                {
                    HeroCardsRight.Add(CurrentHeroCard.ReplacedHeroCard.HeroCard);
                }
            }
            

            CurrentHeroCard = null;
        }

        public void ResetCurrentHeroCardReverse()
        {
            if (CurrentHeroCard == null) return;

            if (CurrentHeroCard.LeftSide)
            {
                HeroCardsRight.Add(CurrentHeroCard.UnUsedHeroCard);
            }
            else
            {
                HeroCardsLeft.Add(CurrentHeroCard.UnUsedHeroCard);
            }

             if(CurrentHeroCard.ReplacedHeroCard != null){
                if (CurrentHeroCard.ReplacedHeroCard.WasOnLeftSide)
                {
                    HeroCardsLeft.Add(CurrentHeroCard.ReplacedHeroCard.HeroCard);
                }
                else
                {
                    HeroCardsRight.Add(CurrentHeroCard.ReplacedHeroCard.HeroCard);
                }
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