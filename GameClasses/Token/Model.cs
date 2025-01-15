namespace BoardGameBackend.Models
{
    public class TokenFromJson
    {
        public required int Id { get; set; }
        public required string Type { get; set; }
        public required string ImageAtlas { get; set; }
        public required int ImageIndex { get; set; }
        public required int EffectType { get; set; }
        public required int EffectID { get; set; }
        public required string ToolTipText {get; set;}
        public required bool Collectable {get; set;}
        public required bool InStartingPool {get; set;}
        public required bool DragonDLC {get; set;}
        public required bool IsDragon {get; set;}
        public Dragon? DragonLink {get; set; }
    }

    public class TokenTileInfo
    {
        public required TokenFromJson Token { get; set; }
        public required int TileId { get; set; }
    }
}