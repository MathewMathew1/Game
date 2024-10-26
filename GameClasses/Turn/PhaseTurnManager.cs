using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PhaseByPhaseTurnManager : BaseTurnManager
    {
        public PhaseByPhaseTurnManager(GameContext gameContext) : base(gameContext) { }

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
                if (BlockNextPlayerTurn == true) return;
            }

            AfterEndPlayerTurn();
        }

        public override void AfterEndPlayerTurn()
        {
            var nextPlayer = _gameContext.PlayerManager.GetNextPlayerForPhase();

            if (nextPlayer == null)
            {
                _gameContext.PlayerManager.ResetAllPlayersPlayedTurn();
                nextPlayer = _gameContext.PlayerManager.GetNextPlayerForPhase();

                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(MercenaryPhase))
                {
                    _currentTurn++;

                    var eventArgs = new EndOfTurnEventData
                    {
                        TurnCount = (_currentTurn & 2)
                    };

                    _gameContext.EventManager.Broadcast("EndOfTurn", ref eventArgs);

                    if (_currentTurn > 2)
                    {
                        if (CurrentRound == MAX_ROUNDS)
                        {
                            _gameContext.EventManager.Broadcast("EndOfGamePreData");
                            return;
                        }
                        EndRound();
                    }


                }

                _gameContext.PhaseManager.EndCurrentPhase(true);
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

        private void EndRound()
        {

            _currentRound++;
            _currentTurn = 1;
            EndOfRoundMercenaryData endOfRoundMercenaryData = new EndOfRoundMercenaryData
            {
                MercenariesLeftData = new MercenariesLeftData { TossedMercenariesAmount = 0, MercenariesAmount = 0 },
                Mercenaries = new List<Mercenary> { }
            };

            EndOfRoundData data = new EndOfRoundData { EndOfRoundMercenaryData = endOfRoundMercenaryData };
            _gameContext.EventManager.Broadcast("EndOfRound", ref data);
        }
    }

}