
using System.Text.Json;
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
            var amountOfConstructions = player.PlayerMercenaryManager.Mercenaries.Count(mercenary => mercenary.TypeCard == 2);
            if (amountOfConstructions >= Value2) return Value1;

            return 0;
        }
    }

    public class ProphecyTwentySeven : BaseProphecyPoints
    {
        public ProphecyTwentySeven(int value1, int value2) : base(value1, value2) { }

        protected override int CalculatePoints(PlayerInGame player, Mercenary mercenary)
        {
            var amountOfMercenaries = player.PlayerMercenaryManager.Mercenaries.Count(mercenary => mercenary.TypeCard == 1);
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