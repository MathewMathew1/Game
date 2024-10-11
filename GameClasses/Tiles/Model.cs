namespace BoardGameBackend.Models
{
    public class Connection
    {
        public int ToId { get; set; }
        public required List<int> Reqs { get; set; }
    }

    public class Tile
    {
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Id { get; set; }
        public int TileTypeId { get; set; }
        public TokenFromJson? Token { get; set; }
        public required List<Connection> Connections { get; set; }
        public required List<List<int>> CastleRange { get; set; }

        public bool IsInRangeOfCastle(Fraction fraction, int distance = 3){
            var isInRange = false;
            CastleRange.ForEach(castle => {
                if (castle[0]==fraction.Id && castle[1] <= distance){
                    isInRange = true;
                    return;
                }
            });

            return isInRange;
        }
    }

    public class TokenReward{
        public required Reward Reward {get; set;}
    }

    
}