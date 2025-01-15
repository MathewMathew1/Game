namespace BoardGameBackend.Models
{
    public class StartGameModel
    {
        public TurnTypes TurnType { get; set; }  = TurnTypes.FULL_TURN;
        public bool LessCards {get; set;} = true;
        public bool MoreHeroCards {get; set;} = false;
        public bool RemovePropheciesAtLastRound {get; set;} = true;
        public bool SameAmountOfMercenariesEachRound {get; set;} = true;
        public bool SignetsTwoFiveNine {get; set;} = false;
        public bool DLCDragons {get; set;} = false;
    }

    public enum TurnTypes{
        PHASE_BY_PHASE,
        FULL_TURN
    }
}