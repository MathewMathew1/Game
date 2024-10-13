using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public interface ITurnManager
    {
        PlayerInGame? CurrentPlayer { get; }
        int CurrentRound { get; }
        int CurrentTurn { get; }

        void EndTurn();
        void ResetCurrentPlayer();
        void TemporarySetCurrentPlayer(PlayerInGame player);
    }
}