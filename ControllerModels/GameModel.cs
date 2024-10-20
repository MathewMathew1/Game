namespace BoardGameBackend.Models
{
    public class StartGameModel
    {
        public TurnTypes TurnType { get; set; }  = TurnTypes.FULL_TURN;
        public bool LessCards {get; set;} = false;
        public bool MoreHeroCards {get; set;} = false;
        public bool RemovePropheciesAtLastRound {get; set;} = false;
    }

    public enum TurnTypes{
        PHASE_BY_PHASE,
        FULL_TURN
    }
}