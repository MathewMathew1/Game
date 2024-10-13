using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PlayerFullTurnManager : BaseTurnManager
    {
        public PlayerFullTurnManager(GameContext gameContext) : base(gameContext) { }

        public override void EndTurn()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.AlreadyPlayedCurrentPhase = true;
            }

            if (_gameContext.MiniPhaseManager.CurrentMiniPhase != null) return;

            if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(MercenaryPhase))
            {
                EndOfPlayerTurn data = new EndOfPlayerTurn { Player = CurrentPlayer! };
                _gameContext.EventManager.Broadcast("EndOfPlayerTurn", ref data);
                var nextPlayer = _gameContext.PlayerManager.GetNextPlayerForPhase();
                if (nextPlayer == null)
                {
                    _gameContext.PlayerManager.ResetAllPlayersPlayedTurn();
                    nextPlayer = _gameContext.PlayerManager.GetNextPlayerForPhase();
                    _currentPlayer = nextPlayer;

                    _currentTurn++;
                    _gameContext.EventManager.Broadcast("EndOfTurn");

                    if (_currentTurn > 2)
                    {
                        if (CurrentRound == MAX_ROUNDS)
                        {
                            _gameContext.EventManager.Broadcast("EndOfGamePreData");
                            return;
                        }
                        EndRound();
                    }
                    _gameContext.PhaseManager.EndCurrentPhase(true);
                    _gameContext.EventManager.Broadcast("New player turn", ref _currentPlayer);
                }
                else
                {
                    _gameContext.PhaseManager.EndCurrentPhase(false);
                    _currentPlayer = nextPlayer;
                    if (_currentPlayer != null)
                    {
                        _gameContext.EventManager.Broadcast("New player turn", ref _currentPlayer);
                    }
                }
            }
            else
            {
                _gameContext.PhaseManager.EndCurrentPhase(false);
            }


            if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(BoardPhase) && _currentPlayer != null)
            {
                _currentPlayer.PlayerHeroCardManager.CurrentHeroCard!.VisitedPlaces.Add(_gameContext.PawnManager._currentTile.Id);
            }


        }

        private void EndRound()
        {
            EndOfRoundMercenaryData endOfRoundMercenaryData = new EndOfRoundMercenaryData
            {
                MercenariesLeftData = new MercenariesLeftData { TossedMercenariesAmount = 0, MercenariesAmount = 0 },
                Mercenaries = new List<Mercenary> { }
            };

            EndOfRoundData data = new EndOfRoundData { EndOfRoundMercenaryData = endOfRoundMercenaryData };
            _gameContext.EventManager.Broadcast("EndOfRound", ref data);

            _currentRound++;
            _currentTurn = 1;
        }
    }
}
