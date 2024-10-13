using System.Collections.Concurrent;
using BoardGameBackend.Managers.EventListeners;
using BoardGameBackend.Models;
using BoardGameBackend.Providers;

namespace BoardGameBackend.Managers
{
    public static class GameManager
    {
        private static readonly ConcurrentDictionary<string, GameContext> ActiveGames = new ConcurrentDictionary<string, GameContext>();
        private static readonly ConcurrentDictionary<string, EventListenerContainer> EventListenerContainers = new();
        private static IHubContextProvider? _hubContextProvider;

        public static void Initialize(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }


        public static GameContext StartGameFromLobby(Lobby lobby, StartGameModel startGameModel)
        {
            var gameId = Guid.NewGuid().ToString();
            var players = lobby.Players.Select(p => new Player { Id = p.Id, Name = p.Name }).ToList();

            var gameContext = new GameContext(gameId, players, startGameModel);

            var eventListenerContainer = new EventListenerContainer(_hubContextProvider);
            eventListenerContainer.SubscribeAll(gameContext);
            
            ActiveGames.TryAdd(gameId, gameContext);
            EventListenerContainers.TryAdd(gameId, eventListenerContainer);

            gameContext.EventManager.Subscribe<EndOfGame>("EndOfGame", endOfGame =>
            {             
                EndGame(gameId);                     
            }, priority: 0);
            
            return gameContext;
        }

        public static GameContext? GetGameById(string gameId)
        {
            if (ActiveGames.TryGetValue(gameId, out var gameContext))
            {
                return gameContext;
            }

            return null;
        }

        public static void EndGame(string gameId)
        {
            ActiveGames.TryRemove(gameId, out _);
        }

    }
}