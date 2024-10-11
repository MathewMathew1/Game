
namespace BoardGameBackend.Models
{
    public class MoveToTileDto
    {
        public required int TileId { get; set; }   
        public required bool FullMovement {get; set;}     
        public int? TeleportationPlace {get; set;}
    }

    public class TeleportToTile
    {
        public required int TileId { get; set; }   
    }
}