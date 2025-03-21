
using System.Text.Json;
using BoardGameBackend.Helpers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Models
{
    public class ReqProphecies
    {
        public required int Id { get; set; }
        public required int IntValue1 { get; set; }
        public required int IntValue2 { get; set; }
    }

    public interface IProphecyPoints
    {
        int GetProphecyPoints(PlayerInGame player, Mercenary mercenary);
    }

    public abstract class BaseProphecyPoints : IProphecyPoints
    {
        protected int Value1 { get; }
        protected int Value2 { get; }

        protected BaseProphecyPoints(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }


        public int GetProphecyPoints(PlayerInGame player, Mercenary mercenary)
        {
            if (mercenary.IsAlwaysFulfilled) return Value1;


            return CalculatePoints(player, mercenary);
        }


        protected abstract int CalculatePoints(PlayerInGame player, Mercenary mercenary);
    }

    public class ProphecyTwenty : BaseProphecyPoints
    {
        public ProphecyTwenty(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var moreOrEqualMagicThanArmy = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            var moreOrEqualArmyThanSiege = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            if (moreOrEqualArmyThanSiege && moreOrEqualMagicThanArmy) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyOne : BaseProphecyPoints
    {
        public ProphecyTwentyOne(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var moreOrEqualArmyThanMagic = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            var moreOrEqualMagicThanSiege = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            if (moreOrEqualArmyThanMagic && moreOrEqualMagicThanSiege) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyTwo : BaseProphecyPoints
    {
        public ProphecyTwentyTwo(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var moreOrEqualArmyThanSiege = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            var moreOrEqualSiegeThanMagic = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            if (moreOrEqualArmyThanSiege && moreOrEqualSiegeThanMagic) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyThree : BaseProphecyPoints
    {
        public ProphecyTwentyThree(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var moreOrEqualMagicThanSiege = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            var moreOrEqualSiegeThanArmy = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            if (moreOrEqualMagicThanSiege && moreOrEqualSiegeThanArmy) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyFour : BaseProphecyPoints
    {
        public ProphecyTwentyFour(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var moreOrEqualSiegeThanMagic = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            var moreOrEqualMagicThanArmy = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            if (moreOrEqualSiegeThanMagic && moreOrEqualMagicThanArmy) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyFive : BaseProphecyPoints
    {
        public ProphecyTwentyFive(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var moreOrEqualSiegeThanArmy = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            var moreOrEqualArmyThanMagic = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            if (moreOrEqualSiegeThanArmy && moreOrEqualArmyThanMagic) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentySix : BaseProphecyPoints
    {
        public ProphecyTwentySix(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var amountOfConstructions = player.PlayerMercenaryManager.Mercenaries.Count(mercenary => mercenary.TypeCard == MercenaryHelper.BuildingCardType);
            if (amountOfConstructions >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentySeven : BaseProphecyPoints
    {
        public ProphecyTwentySeven(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var amountOfMercenaries = player.PlayerMercenaryManager.Mercenaries.Count(mercenary => mercenary.TypeCard == MercenaryHelper.MercenaryCardType);
            if (amountOfMercenaries >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyEight : BaseProphecyPoints
    {
        public ProphecyTwentyEight(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var allFactionsHaveAtLeastTwoHeroes = true;
            for (var i = 1; i < 5; i++)
            {
                var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i);
                var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == i);

                if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight < Value2)
                {
                    allFactionsHaveAtLeastTwoHeroes = false;
                    break;
                }
            }

            if (allFactionsHaveAtLeastTwoHeroes) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentyNine : BaseProphecyPoints
    {
        public ProphecyTwentyNine(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight == 0) return Value1;

            return 0;
        }
    }

    public class ProphecyThirty : BaseProphecyPoints
    {
        public ProphecyThirty(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight == 0) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtyOne : BaseProphecyPoints
    {
        public ProphecyThirtyOne(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight == 0) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtyTwo : BaseProphecyPoints
    {
        public ProphecyThirtyTwo(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight == 0) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtyThree : BaseProphecyPoints
    {
        public ProphecyThirtyThree(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight >= 5) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtyFour : BaseProphecyPoints
    {
        public ProphecyThirtyFour(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight >= 5) return Value1;

            return 0;
        }
    }
    
    public class ProphecyOneHundredFifty : BaseProphecyPoints
    {
        public ProphecyOneHundredFifty(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);
            int iTotal = amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight;
            if(iTotal == 0)
                return 0;

            for(int i = 1; i <= 4; i++)
            {
                if(i != Value2)
                {
                    if(iTotal <= player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i) + player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i))
                        return 0;
                }
            }

            return Value1;
        }
    }

    public class ProphecyOneHundredFiftyOne : BaseProphecyPoints
    {
        public ProphecyOneHundredFiftyOne(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);
            int iTotal = amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight;
            if(iTotal == 0)
                return 0;

            for(int i = 1; i <= 4; i++)
            {
                if(i != Value2)
                {
                    if(iTotal <= player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i) + player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i))
                        return 0;
                }
            }

            return Value1;
        }
    }
    
    public class ProphecyOneHundredFiftyTwo : BaseProphecyPoints
    {
        public ProphecyOneHundredFiftyTwo(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);
            int iTotal = amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight;
            if(iTotal == 0)
                return 0;

            for(int i = 1; i <= 4; i++)
            {
                if(i != Value2)
                {
                    if(iTotal <= player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i) + player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i))
                        return 0;
                }
            }

            return Value1;
        }
    }
    
    public class ProphecyOneHundredFiftyThree : BaseProphecyPoints
    {
        public ProphecyOneHundredFiftyThree(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);
            int iTotal = amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight;
            if(iTotal == 0)
                return 0;

            for(int i = 1; i <= 4; i++)
            {
                if(i != Value2)
                {
                    if(iTotal <= player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i) + player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i))
                        return 0;
                }
            }

            return Value1;
        }
    }
    public class ProphecyOneHundredFiftyFour : BaseProphecyPoints
    {
        public ProphecyOneHundredFiftyFour(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            for(int i = 1; i <= 4; i++)
            {
                if((player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i) > 0) && (player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == i) > 0))
                        return 0;
            }

            return Value1;
        }
    }

    public class ProphecyThirtyFive : BaseProphecyPoints
    {
        public ProphecyThirtyFive(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight >= 5) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtySix : BaseProphecyPoints
    {
        public ProphecyThirtySix(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count(hero => hero.Faction.Id == Value2);
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count(hero => hero.Faction.Id == Value2);

            if (amountOfHerosWithFactionOnLeft + amountOfHerosWithFactionOnRight >= 5) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtySeven : BaseProphecyPoints
    {
        public ProphecyThirtySeven(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count();
            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count();

            if (amountOfHerosWithFactionOnLeft >= Value2 && amountOfHerosWithFactionOnRight >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtyEight : BaseProphecyPoints
    {
        public ProphecyThirtyEight(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnRight = player.PlayerHeroCardManager.HeroCardsRight.Count();

            if (amountOfHerosWithFactionOnRight >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyThirtyNine : BaseProphecyPoints
    {
        public ProphecyThirtyNine(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfHerosWithFactionOnLeft = player.PlayerHeroCardManager.HeroCardsLeft.Count();

            if (amountOfHerosWithFactionOnLeft >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyNinetySeven : BaseProphecyPoints
    {
        public ProphecyNinetySeven(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var moreOrEqualSiegeThanMagic = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            var moreOrEqualSiegeThanArmy = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            if (moreOrEqualSiegeThanMagic && moreOrEqualSiegeThanArmy) return Value1;

            return 0;
        }
    }

    public class ProphecyNinetyNine : BaseProphecyPoints
    {
        public ProphecyNinetyNine(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var moreOrEqualArmyThanMagic = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            var moreOrEqualArmyThanSiege = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            if (moreOrEqualArmyThanMagic && moreOrEqualArmyThanSiege) return Value1;

            return 0;
        }
    }

    public class ProphecyNinetyEight : BaseProphecyPoints
    {
        public ProphecyNinetyEight(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var moreOrEqualMagicThanArmy = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            var moreOrEqualMagicThanSiege = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            if (moreOrEqualMagicThanArmy && moreOrEqualMagicThanSiege) return Value1;

            return 0;
        }
    }

    public class ProphecyOneHundred : BaseProphecyPoints
    {
        public ProphecyOneHundred(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {

            var amountOfRoyalCards = player.PlayerRolayCardManager.RolayCards.Count();
            if (amountOfRoyalCards >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyOneHundredOne : BaseProphecyPoints
    {
        public ProphecyOneHundredOne(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            if (player.Morale >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyOneHundredTwo : BaseProphecyPoints
    {
        public ProphecyOneHundredTwo(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            if (player.ArtifactsPlayed.Count() >= Value2) return Value1;

            return 0;
        }
    }
    public class ProphecyOneHundredThirtyEight : BaseProphecyPoints
    {
        public ProphecyOneHundredThirtyEight(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            if (player.PlayerDragonManager.Dragons.Count() >= Value2) return Value1;

            return 0;
        }
    }
    public class ProphecyOneHundredThirtyNine : BaseProphecyPoints
    {
        public ProphecyOneHundredThirtyNine(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            List<bool> bools = new List<bool>{};
            foreach(var h in Fractions.GetAllFractions())
            {
                bools.Add(false);
            }
            foreach(var mercenaryCard in player.PlayerMercenaryManager.Mercenaries)
            {
                if(mercenaryCard.Faction != null)
                {
                    bools[mercenaryCard.Faction.Id - 1] = true;
                }
            }
            foreach(var b in bools)
            {
                if(!b)
                    return 0;
            }
            return Value1;
        }
    }



    public static class ProphecyRequirementStore
    {
        private static readonly Dictionary<int, IProphecyPoints> _requirements;

        static ProphecyRequirementStore()
        {
            string filePath = "Data/SpecialEffect.json";
            List<ReqProphecies> Reqs = new List<ReqProphecies>();

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                Reqs = JsonSerializer.Deserialize<List<ReqProphecies>>(jsonData) ?? new List<ReqProphecies>();
            }

            _requirements = new Dictionary<int, IProphecyPoints>();

            foreach (var requirementData in Reqs)
            {
                switch (requirementData.Id)
                {
                    case 20:
                        _requirements.Add(requirementData.Id, new ProphecyTwenty(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 21:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyOne(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 22:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyTwo(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 23:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyThree(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 24:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyFour(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 25:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyFive(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 26:
                        _requirements.Add(requirementData.Id, new ProphecyTwentySix(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 27:
                        _requirements.Add(requirementData.Id, new ProphecyTwentySeven(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 28:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyEight(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 29:
                        _requirements.Add(requirementData.Id, new ProphecyTwentyNine(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 30:
                        _requirements.Add(requirementData.Id, new ProphecyThirty(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 31:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyOne(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 32:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyTwo(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 33:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyThree(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 34:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyFour(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 35:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyFive(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 36:
                        _requirements.Add(requirementData.Id, new ProphecyThirtySix(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 37:
                        _requirements.Add(requirementData.Id, new ProphecyThirtySeven(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 38:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyEight(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 39:
                        _requirements.Add(requirementData.Id, new ProphecyThirtyNine(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 97:
                        _requirements.Add(requirementData.Id, new ProphecyNinetySeven(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 98:
                        _requirements.Add(requirementData.Id, new ProphecyNinetyEight(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 99:
                        _requirements.Add(requirementData.Id, new ProphecyNinetyNine(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 100:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundred(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 101:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredOne(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 102:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredTwo(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 138:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredThirtyEight(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 139:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredThirtyNine(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 150:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredFifty(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 151:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredFiftyOne(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 152:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredFiftyTwo(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 153:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredFiftyThree(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    case 154:
                        _requirements.Add(requirementData.Id, new ProphecyOneHundredFiftyFour(requirementData.IntValue1, requirementData.IntValue2));
                        break;
                    default:
                        break;
                }

            }
        }

        public static IProphecyPoints? GetRequirementById(int id)
        {
            if (_requirements.TryGetValue(id, out var requirement))
            {
                return requirement;
            }

            return null;
        }

        public static IEnumerable<IProphecyPoints> GetAllRequirements()
        {
            return _requirements.Values;
        }
    }



}