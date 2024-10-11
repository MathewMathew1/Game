using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PhaseManager
    {
        private List<Phase> _phases;
        private int _currentPhaseIndex = 0;

        public Phase CurrentPhase => _phases[_currentPhaseIndex];

        public PhaseManager(GameContext _gameContext)
        {
            _phases = new List<Phase>
            {
                new ArtifactPhase(_gameContext),
                new HeroCardPickingPhase(_gameContext),
                new BoardPhase(_gameContext),
                new MercenaryPhase(_gameContext)
            };
        }

        public void StartCurrentPhase()
        {
            CurrentPhase.StartPhase();
        }

        public void EndCurrentPhase()
        {
            CurrentPhase.EndPhase();
            MoveToNextPhase();
        }

        private void MoveToNextPhase()
        {
            _currentPhaseIndex++;
            if (_currentPhaseIndex >= _phases.Count)
            {
                _currentPhaseIndex = 0; 
            }
            CurrentPhase.StartPhase();
        }
    }
}