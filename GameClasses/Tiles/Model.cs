using System.Text.Json;
using AutoMapper;

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
        public int RotateID {get; set;}
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

    public class TileWithType
    {
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int RotateID {get; set;}
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
        public required TileType TileType { get; set; }
        
    }

    public class TileType
    {
        public required int Id { get; set; }
        public int? Req {get; set;}
        public required string Name { get; set; }
        public required int ImageIndex { get; set; }
        public required int OffsetX { get; set; }
        public required int OffsetY { get; set; }
        public required string IconAtlas { get; set; }
        public string ImagePathString { get; set; } = "";
    }

    public class TileTypeResolver : IValueResolver<Tile, TileWithType, TileType>
    {
        private readonly IEnumerable<TileType> _tileTypes;

        public TileTypeResolver(IEnumerable<TileType> tileTypes)
        {
            _tileTypes = tileTypes;
        }

        public TileType Resolve(Tile source, TileWithType destination, TileType destMember, ResolutionContext context)
        {
            return _tileTypes.FirstOrDefault(t => t.Id == source.TileTypeId);
        }
    }

    public static class TileTypes
    {

        public static readonly List<TileType> Tiles;

        static TileTypes()
        {
            string filePath = "Data/TileTypes.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                Tiles = JsonSerializer.Deserialize<List<TileType>>(jsonData) ?? new List<TileType>();;
            }
            else
            {
                Tiles = new List<TileType>();
            }
        }
    }


    
}