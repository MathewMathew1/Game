namespace BoardGameBackend.Models
{
    
    public class RolayCardFromJson
    {
        public required string Type { get; set; }
        public required int Id { get; set; }
        public required int FactionId { get; set; }
        public required int ScorePoints { get; set; }
        public required int Army { get; set; }
        public required int Siege { get; set; }
        public required int Magic { get; set; }
        public required int Morale { get; set; }
        public string? ImageAtlas { get; set; }
        public int? ImageIndex { get; set; }
        public int? EffectId { get; set; }
        public int? EffectImageIndex { get; set; }
        public string? EffectImageAtlas { get; set; }
        public int? EffectTypeId {get; set;}
        public string? EffectToolTip {get; set;}
    }

    public class RolayCard
    {
        public required string Type { get; set; }
        public required int Id { get; set; }
        public required Fraction Faction { get; set; }
        public required int ScorePoints { get; set; }
        public required int Army { get; set; }
        public required int Siege { get; set; }
        public required int Magic { get; set; }
        public required int Morale { get; set; }
        public string? ImageAtlas { get; set; }
        public int? ImageIndex { get; set; }
        public int? EffectId { get; set; }
        public int? EffectImageIndex { get; set; }
        public string? EffectImageAtlas { get; set; }
        public int? EffectTypeId {get; set;}
        public string? EffectToolTip {get; set;}
        public PlayerViewModel? PickedByPlayer {get; set;}
    }


}
