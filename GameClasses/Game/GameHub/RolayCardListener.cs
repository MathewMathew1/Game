using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace BoardGameBackend.Managers.EventListeners
{
    public class RoyalCardListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public RoyalCardListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

               
            gameContext.EventManager.Subscribe<RoyalCardPlayed>("RolayCardPlayed", data =>
            {             
                BroadcastRoyalCardPlayed(gameId, data);                     
            }, priority: 5);

        }

        public void BroadcastRoyalCardPlayed(string gameId, RoyalCardPlayed teleportationData)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RoyalCardPlayed", teleportationData);
        }



    }
}
