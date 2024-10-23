using BoardGameBackend.Models;

namespace BoardGameBackend.Helpers
{
    public static class TileDistanceHelper{
        
        public static bool IsInRange(int range, Tile tile, int factionId){
            for(var i =0; i< tile.CastleRange.Count; i++){
                if(tile.CastleRange[i][0]==factionId && tile.CastleRange[i][1] <= range){
                    return true;
                }
            }

            return false;
        }
    }
}