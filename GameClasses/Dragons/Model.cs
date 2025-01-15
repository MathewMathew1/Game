namespace BoardGameBackend.Models
{
    public class DragonFromJson
    {
        public int? Req {get; set;}
        public required int Id { get; set; }
        public required int TileId { get; set; }
        public required string NameEng { get; set; }
        public required int Siege { get; set; }
        public required int Magic { get; set; }
        public required int Army { get; set; }
        public required int Morale { get; set; }
        public required string BackgroundAtlas { get; set; }
        public required int BackgroundIndex { get; set; }
        public required int ScorePoints { get; set; }
        public string? EffectIconAtlas { get; set; }
        public int? EffectIconIndex { get; set; }
        public int? EffectId { get; set; }
        public string? ToolTipText {get; set;}
        public int? EffectType { get; set; }
        public required int TokenId { get; set; }
        public required List<int> Reqs { get; set; }
    }

    public class Dragon
    {
        public required int Id { get; set; }
        public required int TileId { get; set; }
        public required string NameEng { get; set; }
        public required int Siege { get; set; }
        public required int Magic { get; set; }
        public required int Army { get; set; }
        public required int Morale { get; set; }
        public int InGameIndex { get; set; }
        public required string BackgroundAtlas { get; set; }
        public required int BackgroundIndex { get; set; }
        public required int ScorePoints { get; set; }
        public string? EffectIconAtlas { get; set; }
        public int? EffectIconIndex { get; set; }
        public int? EffectId { get; set; }
        public int? EffectType { get; set; }
        public string? ToolTipText {get; set;}
        public required int TokenId { get; set; }
        public required List<int> Reqs { get; set; }
    }

    public class DragonData
    {
        public required List<Dragon> DeckDragons { get; set; }
        public int RemainingDragonsAmount { get; set; }
    }

    public class DragonFulfillProphecyReturnData
    {
        public int? DragonId {get; set;}
        public AurasType? aurasType {get; set;}
    }
}