using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class TurnManager
    {
        private readonly GameContext _gameContext;
        public PlayerInGame? realCurrentPlayer;
        private PlayerInGame? _currentPlayer;
        public int _currentRound = 1;
        private int _currentTurn = 1;
        public readonly int MAX_ROUNDS = 5;

        public TurnManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _currentPlayer = _gameContext.PlayerManager.Players.FirstOrDefault();

            _gameContext.EventManager.Subscribe<HeroCardPicked>("HeroCardPicked", args =>
            {
                EndTurn();
            }, priority: 5);
            _gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", args =>
            {
                EndTurn();
            }, priority: 1);
            _gameContext.EventManager.Subscribe<HeroTurnEnded>("HeroTurnEnded", args =>
            {
                EndTurn();
            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactsTaken", (ArtifactsTaken data) =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
                {
                    EndTurn();
                }

            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactRerolled", (ArtifactRerolledData data) =>
            {
                EndTurn();
            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactPlayed", (ArtifactPlayed data) =>
            {
                EndTurn();
            }, priority: 0);
            _gameContext.EventManager.Subscribe<MercenaryRerolled>("MercenaryRerolled", rerollMercenaryData =>
            {
                if(_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase)){
                    EndTurn();
                }        
            }, priority: 5);

            _gameContext.EventManager.Subscribe<TeleportationData>("TeleportationEvent", rerollMercenaryData =>
            {
                if(_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase)){
                    EndTurn();
                }        
            }, priority: 5);

            gameContext.EventManager.Subscribe<LockMercenaryData>("LockMercenary", data =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(MercenaryPhase))
                {
                    EndTurn();
                }
            }, priority: 0); 
            
        }

        public void EndTurn()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.AlreadyPlayedCurrentPhase = true;
            }

            if(_gameContext.MiniPhaseManager.CurrentMiniPhase != null) return;

            var nextPlayer = _gameContext.PlayerManager.GetNextPlayerForPhase();

            if (nextPlayer == null)
            {
                _gameContext.PlayerManager.ResetAllPlayersPlayedTurn();
                nextPlayer = _gameContext.PlayerManager.GetNextPlayerForPhase();

                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(MercenaryPhase))
                {
                    _currentTurn++;
                    _gameContext.PlayerManager.EndTurn();
                    _gameContext.EventManager.Broadcast("EndOfTurn");
                    if (_currentTurn > 2)
                    {
                        EndOfRoundMercenaryData endOfRoundMercenaryData = new EndOfRoundMercenaryData
                        {
                            MercenariesLeftData = new MercenariesLeftData { TossedMercenariesAmount = 0, MercenariesAmount = 0 },
                            Mercenaries = new List<Mercenary> { }
                        };

                        EndOfRoundData data = new EndOfRoundData { EndOfRoundMercenaryData = endOfRoundMercenaryData };

                        _gameContext.EventManager.Broadcast("EndOfRound", ref data);
                        

                        if(CurrentRound == MAX_ROUNDS){
                            _gameContext.EventManager.Broadcast("EndOfGamePreData");
                            return;
                        }

                        _currentRound++;
                        _currentTurn = 1;
                    }

                }
                _gameContext.PhaseManager.EndCurrentPhase();

            }
            _currentPlayer = nextPlayer;

            if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(BoardPhase) && _currentPlayer != null)
            {
                _currentPlayer.PlayerHeroCardManager.CurrentHeroCard!.VisitedPlaces.Add(_gameContext.PawnManager._currentTile.Id);
            }

            if (_currentPlayer != null)
            {
                _gameContext.EventManager.Broadcast("New player turn", ref _currentPlayer);
            }
        }

        public void ResetCurrentPlayer(){
            _currentPlayer = realCurrentPlayer;
        }

        public void TemporarySetCurrentPlayer(PlayerInGame player){
            realCurrentPlayer = _currentPlayer;
            _currentPlayer = player;
        }

        public PlayerInGame? CurrentPlayer => _currentPlayer;
        public int CurrentRound => _currentRound;
        public int CurrentTurn => _currentTurn;
    }
}
