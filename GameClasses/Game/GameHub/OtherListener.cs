using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace BoardGameBackend.Managers.EventListeners
{
    public class OtherEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public OtherEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

               
            gameContext.EventManager.Subscribe<TeleportationData>("TeleportationEvent", teleportationData =>
            {             
                BroadcastTeleportation(gameId, teleportationData);                     
            }, priority: 5);

            gameContext.EventManager.Subscribe<EndOfGame>("EndOfGame", endOfGame =>
            {             
                BroadcastEndOfGame(gameId, endOfGame);                     
            }, priority: 1);

            gameContext.EventManager.Subscribe<AddAura>("AddAura", data =>
            {             
                BroadcastAddAura(gameId, data);                     
            }, priority: 1);

        }

        public void BroadcastTeleportation(string gameId, TeleportationData teleportationData)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("TeleportationEvent", teleportationData);
        }

        public void BroadcastEndOfGame(string gameId, EndOfGame endOfGame)
        {
            Console.WriteLine("Halo");
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("EndOfGame", endOfGame);
        }

        public void BroadcastAddAura(string gameId, AddAura data)
        {
            Console.WriteLine("Halo");
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("AddAura", data);
        }


    }
}
