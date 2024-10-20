using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public interface ITurnManager
    {
        PlayerInGame? CurrentPlayer { get; }
        int CurrentRound { get; }
        int CurrentTurn { get; }
        bool BlockNextPlayerTurn { get; set; }

        void EndTurn();
        void ResetCurrentPlayer();
        void TemporarySetCurrentPlayer(PlayerInGame player);
    }
}