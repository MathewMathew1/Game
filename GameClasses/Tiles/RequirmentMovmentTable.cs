using System.Text.Json;
using BoardGameBackend.Helpers;

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
    public class RequirementDragonMightMagic : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementDragonMightMagic(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            int iTotalStrength = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic);
            iTotalStrength += player.AurasTypes.Count(a => a.Aura == AurasType.DRAGON_REQ_MIGHTS_MINUS_ONE);
            return iTotalStrength >= _value1;
        }
    }
    public class RequirementDragonMightSiege : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementDragonMightSiege(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            int iTotalStrength = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Siege);
            iTotalStrength += player.AurasTypes.Count(a => a.Aura == AurasType.DRAGON_REQ_MIGHTS_MINUS_ONE);
            return iTotalStrength >= _value1;
        }
    }
    public class RequirementDragonMightArmy : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public RequirementDragonMightArmy(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            int iTotalStrength = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Army);
            iTotalStrength += player.AurasTypes.Count(a => a.Aura == AurasType.DRAGON_REQ_MIGHTS_MINUS_ONE);
            return iTotalStrength >= _value1;
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
            if(player.PlayerHeroCardManager.CurrentHeroCard != null)
            {
                if(player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.Faction.Id == _value1)
                    hasAtLeastOneHeroOfThatFactions++;
            }

            return hasAtLeastOneHeroOfThatFactions >= _value2;
        }
    }
    public class RequirementHasProphecy : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementHasProphecy(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            var amountOfCards = player.PlayerMercenaryManager.Mercenaries.Count(Mercenary => Mercenary.TypeCard == MercenaryHelper.ProphecyCardType);
            return amountOfCards >= 0;
        }
    }
    public class RequirementHasDragon : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementHasDragon(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            return player.PlayerDragonManager.Dragons.Count > 0;
        }
    }
    public class RequirementTwentyFour : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementTwentyFour(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            if(player.PlayerHeroCardManager.CurrentHeroCard == null)
                return false;

            if(player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null)
                return player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard.RoyalSignet > 0;
            else
                return player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.RoyalSignet > 0;
        }
    }
    public class RequirementTwentyFive : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementTwentyFive(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            if(player.PlayerHeroCardManager.CurrentHeroCard == null)
                return false;

            if(player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null)
                return player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard.Morale > 0;
            else
                return player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.Morale > 0;
        }
    }
    public class RequirementTwentySix : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementTwentySix(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            if(player.PlayerHeroCardManager.CurrentHeroCard == null)
                return false;

            if(player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null)
                return player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard.ScorePoints > 0;
            else
                return player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.ScorePoints > 0;
        }
    }
    public class RequirementThirtyFour : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementThirtyFour(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            if(player.PlayerHeroCardManager.CurrentHeroCard == null)
                return false;

            if(player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null)
                return player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard.Army > 0;
            else
                return player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.Army > 0;
        }
    }
    
    public class RequirementThirtyFive : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementThirtyFive(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            if(player.PlayerHeroCardManager.CurrentHeroCard == null)
                return false;

            if(player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null)
                return player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard.Magic > 0;
            else
                return player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.Magic > 0;
        }
    }
    public class RequirementThirtySix : IRequirementMovement
    {
        private readonly int _value1;
        private readonly int _value2;

        public int Value1 => _value1;
        public int Value2 => _value2;

        public RequirementThirtySix(int value1, int value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public bool CheckRequirements(PlayerInGame player)
        {
            if(player.PlayerHeroCardManager.CurrentHeroCard == null)
                return false;

            if(player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null)
                return player.PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard.Siege > 0;
            else
                return player.PlayerHeroCardManager.CurrentHeroCard.HeroCard.Siege > 0;
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
                    case 9:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 10:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 11:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 15:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightMagic(requirementData.Value1, requirementData.Value2));
                        break;
                    case 16:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightMagic(requirementData.Value1, requirementData.Value2));
                        break;
                    case 17:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightMagic(requirementData.Value1, requirementData.Value2));
                        break;
                    case 18:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightSiege(requirementData.Value1, requirementData.Value2));
                        break;
                    case 19:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightSiege(requirementData.Value1, requirementData.Value2));
                        break;
                    case 20:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightSiege(requirementData.Value1, requirementData.Value2));
                        break;
                    case 21:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightArmy(requirementData.Value1, requirementData.Value2));
                        break;
                    case 22:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightArmy(requirementData.Value1, requirementData.Value2));
                        break;
                    case 23:
                        _requirements.Add(requirementData.Id, new RequirementDragonMightArmy(requirementData.Value1, requirementData.Value2));
                        break;
                    case 24:
                        _requirements.Add(requirementData.Id, new RequirementTwentyFour(requirementData.Value1, requirementData.Value2));
                        break;
                    case 25:
                        _requirements.Add(requirementData.Id, new RequirementTwentyFive(requirementData.Value1, requirementData.Value2));
                        break;
                    case 26:
                        _requirements.Add(requirementData.Id, new RequirementTwentySix(requirementData.Value1, requirementData.Value2));
                        break;
                    case 28:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 29:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 30:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 31:
                        _requirements.Add(requirementData.Id, new RequirementMovementEight(requirementData.Value1, requirementData.Value2));
                        break;
                    case 32:
                        _requirements.Add(requirementData.Id, new RequirementHasProphecy(requirementData.Value1, requirementData.Value2));
                        break;
                    case 33:
                        _requirements.Add(requirementData.Id, new RequirementHasDragon(requirementData.Value1, requirementData.Value2));
                        break;
                    case 34:
                        _requirements.Add(requirementData.Id, new RequirementThirtyFour(requirementData.Value1, requirementData.Value2));
                        break;
                    case 35:
                        _requirements.Add(requirementData.Id, new RequirementThirtyFive(requirementData.Value1, requirementData.Value2));
                        break;
                    case 36:
                        _requirements.Add(requirementData.Id, new RequirementThirtySix(requirementData.Value1, requirementData.Value2));
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