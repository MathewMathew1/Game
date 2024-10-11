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
        public required bool Dummy {get; set;}
    }

    public class TokenTileInfo
    {
        public required TokenFromJson Token { get; set; }
        public required int TileId { get; set; }
    }
}