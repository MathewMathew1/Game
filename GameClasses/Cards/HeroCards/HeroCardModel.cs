namespace BoardGameBackend.Models
{
    public class HeroCardFromJson
    {
        public required int Id {get; set;}
        public required string HeroName { get; set; }
        public required int Army {get; set;}
        public required int Magic {get; set;}
        public required int Siege {get; set;}
        public required int Morale {get; set;}
        public required int Faction { get; set; }
        public required int MovementFull {get; set;}
        public required int MovementEmpty {get; set;}
        public required int ScorePoints {get; set;}
        public required int ImageIndex {get; set;}
        public required string ImageAtlas { get; set; }
        public required int RoyalSignet {get; set;}
        public string? EffectImageAtlas { get; set; }
        public int? EffectImageIndex {get; set;}
        public string? EffectToolTip { get; set; }
        public int? EffectTypeId {get; set;}
        public int? EffectId {get; set;}
    }

    public class HeroCard
    {
        public required int Id {get; set;}
        public required string HeroName { get; set; }
        public required int Army {get; set;}
        public required int Magic {get; set;}
        public required int Siege {get; set;}
        public required int Morale {get; set;}
        public required Fraction Faction { get; set; }
        public required int MovementFull {get; set;}
        public required int MovementEmpty {get; set;}
        public required int ScorePoints {get; set;}
        public required int ImageIndex {get; set;}
        public required string ImageAtlas { get; set; }
        public required int RoyalSignet {get; set;}
        public string? EffectImageAtlas { get; set; }
        public int? EffectImageIndex {get; set;}
        public string? EffectToolTip { get; set; }
        public int? EffectTypeId {get; set;}
        public int? EffectId {get; set;}
    }

    public class CurrentHeroCard{
        public required bool LeftSide { get; set; }
        public required HeroCard HeroCard {get; set;}
        public required HeroCard UnUsedHeroCard {get; set;}
        public ReplacedHero? ReplacedHeroCard {get; set;}
        public required int MovementFullLeft { get; set; }
        public required int MovementUnFullLeft { get; set; }
        public bool NoFractionMovement {get; set;} = false;
        public List<int> VisitedPlaces {get; set;} = new List<int>();
    }

    public class HeroFullInfo{
        public required HeroCard HeroCard{get; set;}
        public required bool LeftSide{get; set;}
    }

    public class HeroCardCombined
    {
        public required int Id {get; set;}
        public required HeroCard LeftSide {get; set;}
        public required HeroCard RightSide {get; set;}
        public PlayerViewModel? PlayerWhoPickedCard {get; set;}
    }

    public class HeroCardCombinedFromJson
    {
        public required int Id {get; set;}
        public required int LeftSide {get; set;}
        public required int RightSide {get; set;}
        public required int NumPlayers {get; set;}
    }

    public class Fraction
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }

    public class ReplacedHero
    {
        public required HeroCard HeroCard { get; set;}
        public required bool WasOnLeftSide { get; set;}
    }

    public class PlayerHeroData{
        public required List<HeroCard> LeftHeroCards {get; set;}
        public required List<HeroCard> RightHeroCards {get; set;}
        public required CurrentHeroCard? CurrentHeroCard {get; set;}
    }

    public static class Fractions
    {
        public static readonly Fraction Green = new Fraction { Id = 1, Name = "Green" };
        public static readonly Fraction Red = new Fraction { Id = 2, Name = "Red" };
        public static readonly Fraction Yellow = new Fraction { Id = 3, Name = "Yellow" };
        public static readonly Fraction Blue = new Fraction { Id = 4, Name = "Blue" };

        public static List<Fraction> GetAllFractions()
        {
            return new List<Fraction> { Red, Blue, Yellow, Green };
        }

        public static Fraction? GetFractionById(int id)
        {
            return GetAllFractions().FirstOrDefault(f => f.Id == id);
        }
    }
}