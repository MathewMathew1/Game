using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public abstract class BaseTurnManager : ITurnManager
    {
        protected readonly GameContext _gameContext;
        protected PlayerInGame? _currentPlayer;
        protected int _currentRound = 1;
        protected int _currentTurn = 1;
        protected PlayerInGame? realCurrentPlayer;
        public readonly int MAX_ROUNDS = 5;

        public BaseTurnManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _currentPlayer = _gameContext.PlayerManager.Players.FirstOrDefault();
            SubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
            _gameContext.EventManager.Subscribe<HeroCardPicked>("HeroCardPicked", args => EndTurn(), priority: 5);
            _gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", args => EndTurn(), priority: 1);
            _gameContext.EventManager.Subscribe<HeroTurnEnded>("HeroTurnEnded", args => EndTurn(), priority: 0);
            _gameContext.EventManager.Subscribe("ArtifactsTaken", (ArtifactsTaken data) =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
                {
                    EndTurn();
                }
            }, priority: 0);
            _gameContext.EventManager.Subscribe("ArtifactRerolled", (ArtifactRerolledData data) => EndTurn(), priority: 0);
            _gameContext.EventManager.Subscribe("ArtifactPlayed", (ArtifactPlayed data) => EndTurn(), priority: 0);
            _gameContext.EventManager.Subscribe<MercenaryRerolled>("MercenaryRerolled", rerollMercenaryData =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
                {
                    EndTurn();
                }
            }, priority: 5);
            _gameContext.EventManager.Subscribe<TeleportationData>("TeleportationEvent", rerollMercenaryData =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
                {
                    EndTurn();
                }
            }, priority: 5);
            _gameContext.EventManager.Subscribe<LockMercenaryData>("LockMercenary", data =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(MercenaryPhase))
                {
                    EndTurn();
                }
            }, priority: 0);
        }

        public abstract void EndTurn();  // To be implemented by specific turn managers

        public void ResetCurrentPlayer()
        {
            _currentPlayer = realCurrentPlayer;
        }

        public void TemporarySetCurrentPlayer(PlayerInGame player)
        {
            realCurrentPlayer = _currentPlayer;
            _currentPlayer = player;
        }

        public PlayerInGame? CurrentPlayer => _currentPlayer;
        public int CurrentRound => _currentRound;
        public int CurrentTurn => _currentTurn;
    }
}