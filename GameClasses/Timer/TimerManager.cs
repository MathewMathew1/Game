using System.Diagnostics;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class TimerManager
    {
        private Stopwatch _gameStopwatch;                          
        private Dictionary<Guid, TimeSpan> _playerTimes;  
        private PlayerInGame? _currentPlayer;                       
        private Stopwatch _playerStopwatch;   
        private GameContext _gameContext;                     

        public TimerManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _gameStopwatch = new Stopwatch();
            _playerTimes = new Dictionary<Guid, TimeSpan>();
            _playerStopwatch = new Stopwatch();

            gameContext.EventManager.Subscribe<PlayerInGame>("New player turn", OnPlayerTurnStart, priority: 2);
            gameContext.EventManager.Subscribe<StartOfGame>("GameStarted", (data) => OnPlayerTurnStart(_gameContext.TurnManager.CurrentPlayer!), priority: 0);

            _gameStopwatch.Start();
        }

        public void EndTimer()
        {
            if (_currentPlayer != null)
            {
                UpdatePlayerTime(_currentPlayer, _playerStopwatch.Elapsed);
            }

            _currentPlayer = null;
            _playerStopwatch.Reset();
        }
        private void OnPlayerTurnStart(PlayerInGame player)
        {
            if (_currentPlayer != null)
            {
                _playerStopwatch.Stop();
                UpdatePlayerTime(_currentPlayer, _playerStopwatch.Elapsed);
            }
            Console.WriteLine("OnPlayerTurnStart");
            _currentPlayer = player;
            _playerStopwatch.Reset();
            _playerStopwatch.Start();
        }

        private void UpdatePlayerTime(PlayerInGame player, TimeSpan elapsedTime)
        {
            if (!_playerTimes.ContainsKey(player.Id))
            {
                _playerTimes[player.Id] = elapsedTime;
            }

            _playerTimes[player.Id] = _playerTimes[player.Id].Add(elapsedTime);
        }

        public TimeSpan GetPlayerTime(PlayerInGame player)
        {
            return _playerTimes.ContainsKey(player.Id) ? _playerTimes[player.Id] : TimeSpan.Zero;
        }

        public Dictionary<Guid, TimeSpan> GetPlayerTimes()
        {
            return _playerTimes;
        }

        public TimeSpan GetTotalGameTime()
        {
            return _gameStopwatch.Elapsed;
        }
    }
}