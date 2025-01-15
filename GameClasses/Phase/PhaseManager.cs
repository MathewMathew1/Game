using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PhaseManager
    {
        private List<Phase> _phases;
        private int _currentPhaseIndex = 0;
        private GameContext _gameContext;
        public Phase CurrentPhase => _phases[_currentPhaseIndex];

        public PhaseManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _phases = new List<Phase>
            {
                new ArtifactPhase(gameContext),
                new HeroCardPickingPhase(gameContext),
                new BoardPhase(gameContext),
                new MercenaryPhase(gameContext)
            };
        }

        public void StartCurrentPhase()
        {
            CurrentPhase.StartPhase();
        }

        public void EndCurrentPhase(bool startNextTurn)
        {
            CurrentPhase.EndPhase();
            MoveToNextPhase(startNextTurn);
        }

        private void MoveToNextPhase(bool startNextTurn)
        {
            _currentPhaseIndex++;
            if (_currentPhaseIndex >= _phases.Count)
            {
                if(startNextTurn){
                   _gameContext.EventManager.Broadcast("StartTurn"); 
                }
                _currentPhaseIndex = 0; 
            }
            CurrentPhase.StartPhase();
        }

        public void EndCurrentPhaseIfPhaseType(PhaseType iPhaseType, bool startNextTurn)
        {
            if(iPhaseType == CurrentPhase.Name)
                EndCurrentPhase(startNextTurn);
        }
    }
}