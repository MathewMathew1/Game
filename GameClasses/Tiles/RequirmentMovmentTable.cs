using System.Text.Json;

namespace BoardGameBackend.Models
{
    public class Req
    {
        public required int Id { get; set; }
        public required int Value1 { get; set; }
        public required int Value2 { get; set; }
    }

    public interface IRequirementMovement
    {
        bool CheckRequirements(PlayerInGame player);
    }

    public class RequirementMovementFirst : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementMovementFirst(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            return player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege) >= _value1;
        }
    }

    public class RequirementMovementSecond : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementMovementSecond(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            var currentHeroCard = player.PlayerHeroCardManager.CurrentHeroCard;
            return currentHeroCard?.HeroCard.Faction.Id != _value1 || currentHeroCard.NoFractionMovement;
        }
    }

    public class RequirementMovementThird : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementMovementThird(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            var currentHeroCard = player.PlayerHeroCardManager.CurrentHeroCard;
            return currentHeroCard?.HeroCard.Faction.Id != _value1 || currentHeroCard.NoFractionMovement;
        }
    }

    public class RequirementMovementForth : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementMovementForth(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            var currentHeroCard = player.PlayerHeroCardManager.CurrentHeroCard;
            return currentHeroCard?.HeroCard.Faction.Id != _value1 || currentHeroCard.NoFractionMovement;
        }
    }

    public class RequirementMovementFifth : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementMovementFifth(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            var currentHeroCard = player.PlayerHeroCardManager.CurrentHeroCard;
            return currentHeroCard?.HeroCard.Faction.Id != _value1 || currentHeroCard.NoFractionMovement;
        }
    }

    public class RequirementMovementSix : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementMovementSix(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            return player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= _value1;
        }
    }

    public class RequirementMovementSeven
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementMovementSeven(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            return player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army) >= _value1;
        }
    }

    public class RequirementMovementEight : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementMovementEight(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {

            var hasAtLeastOneHeroOfThatFactions = player.PlayerHeroCardManager.AmountOfHeroesOfFaction(_value1);

            return hasAtLeastOneHeroOfThatFactions > 0;
        }
    }



    public static class RequirementMovementStore
    {
        private static readonly Dictionary<int, IRequirementMovement> _requirements;

        static RequirementMovementStore()
        {
            string filePath = "Data/Reqs.json";
            List<Req> Reqs = new List<Req>();

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                Reqs = JsonSerializer.Deserialize<List<Req>>(jsonData) ?? new List<Req>();
            }

            _requirements = new Dictionary<int, IRequirementMovement>();

            foreach (var requirementData in Reqs)
            {
                switch (requirementData.Id)
                {
                    case 1:
                        _requirements.Add(requirementData.Id, new RequirementMovementFirst(requirementData.Value1, requirementData.Value2));
                        break;
                    case 2:
                        _requirements.Add(requirementData.Id, new RequirementMovementSecond(requirementData.Value1, requirementData.Value2));
                        break;
                    case 3:
                        _requirements.Add(requirementData.Id, new RequirementMovementThird(requirementData.Value1, requirementData.Value2));
                        break;
                    case 4:
                        _requirements.Add(requirementData.Id, new RequirementMovementForth(requirementData.Value1, requirementData.Value2));
                        break;
                    case 5:
                        _requirements.Add(requirementData.Id, new RequirementMovementFifth(requirementData.Value1, requirementData.Value2));
                        break;
                    case 6:
                        _requirements.Add(requirementData.Id, new RequirementMovementSix(requirementData.Value1, requirementData.Value2));
                        break;
                    case 7:
                        _requirements.Add(requirementData.Id, new RequirementMovementSecond(requirementData.Value1, requirementData.Value2));
                        break;
                    case 8:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 12:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 13:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 14:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;

                }
            }
        }

        public static IRequirementMovement GetRequirementById(int id)
        {
            _requirements.TryGetValue(id, out var requirement);
            return requirement!;
        }

        public static IEnumerable<IRequirementMovement> GetAllRequirements()
        {
            return _requirements.Values;
        }
    }



}