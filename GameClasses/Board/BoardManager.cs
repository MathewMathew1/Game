using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class BoardManager
    {
        private GameContext _gameContext;
        
        public BoardManager(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void EndTurn()
        {
            var player = _gameContext.TurnManager.CurrentPlayer;
            player!.ResetCurrentHeroCard(_gameContext.EventManager);
            var eventArgs = new HeroTurnEnded{Player=player};
            _gameContext.EventManager.Broadcast("HeroTurnEnded", ref eventArgs);
        }
    }
}
