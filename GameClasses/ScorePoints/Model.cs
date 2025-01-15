

namespace BoardGameBackend.Models
{

    public class PointsWithPower{
        public int Points { get; set; } = 0;
        public int Power { get; set; } = 0;
    }

    public class ScorePointsTable{
        public PointsWithPower MoralePoints { get; set; } = new PointsWithPower();
        public PointsWithPower SiegePoints { get; set; } = new PointsWithPower();
        public PointsWithPower ArmyPoints { get; set; }  = new PointsWithPower();
        public PointsWithPower MagicPoints { get; set;}  = new PointsWithPower();
        public int MercenaryPoints { get; set;} = 0;
        public int OraclePoints { get; set;} = 0;
        public int HeroPoints { get; set;} = 0;
        public int RoyalCardPoints { get; set;} = 0;
        public int DragonCardPoints {get; set;} = 0;
        public int ArtefactPoints { get; set;} = 0;
        public int TokenPoints { get; set;} = 0;
        public int PointsOverall {get; set;} = 0;
    }


}