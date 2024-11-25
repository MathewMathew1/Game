using Newtonsoft.Json;
using BoardGameBackend.Models;
using BoardGameBackend.Helpers;
using BoardGameBackend.Mappers;

namespace BoardGameBackend.Managers
{
    public class GameTiles
    {
        public List<TileWithType> Tiles { get; set; } = new List<TileWithType>();
        public GameContext _gameContext;
        
        public GameTiles(GameContext gameContext)
        {
            _gameContext = gameContext;
            Tiles = LoadTileMapFromFile("Data/TilesData.json");   
        }

        public TileWithType GetTileById(int id){
            return Tiles.FirstOrDefault(tile => tile.Id == id)!;
        }

        public List<TileWithType> LoadTileMapFromFile(string filePath)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found at path: {fullPath}");
            }

            var jsonData = File.ReadAllText(fullPath);
            var tileList = JsonConvert.DeserializeObject<List<Tile>>(jsonData);
            var tilesWithTypeList = tileList!.Select(data => GameMapper.Instance.Map<TileWithType>(data)).ToList();

            return tilesWithTypeList;
        }

        public List<TokenTileInfo> SetupTokens(){
            List<TokenTileInfo> tokenTileInfos = new List<TokenTileInfo>();

            Tiles.ForEach(tile => {
                if(tile.TileTypeId == TileHelper.CastleTileId){
                    var token = _gameContext.TokenManager.DrawToken();
                    tile.Token = token;

                    TokenTileInfo tokenTileInfo =  new TokenTileInfo{Token = token, TileId = tile.Id};

                    tokenTileInfos.Add(tokenTileInfo);
                }
            });

            return tokenTileInfos;
        }

        public List<TokenTileInfo> GetTokenInfo(){
            List<TokenTileInfo> tokenTileInfos = new List<TokenTileInfo>();

            Tiles.ForEach(tile => {
                if(tile.Token != null){
                    TokenTileInfo tokenTileInfo =  new TokenTileInfo{Token = tile.Token, TileId = tile.Id};

                    tokenTileInfos.Add(tokenTileInfo);
                }
            });

            return tokenTileInfos;
        }
    }
}