using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace BoardGameBackend.Managers.EventListeners
{
    public class DragonEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public DragonEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;
            gameContext.EventManager.Subscribe<DragonAcquired>("DragonAcquired", dragonacquired =>
            {
                BroadcastDragonPicked(dragonacquired, gameId);
            }, priority: 10);  

            gameContext.EventManager.Subscribe<DragonSummonData>("DragonPreSummon", data =>
            {
                BroadcastDragonReadyToSummon(data, gameId);
            }, priority: 10);  


            gameContext.EventManager.Subscribe<DragonSummonEventData>("SummonDragonEvent", data =>
            {             
                BroadcastSummonDragonEvent(data, gameId);                    
            }, priority: 5);

            gameContext.EventManager.Subscribe<MiniPhaseDataWithDifferentPlayer>("ChangedPlayerInPhase", data =>
            {             
                BroadcastChangePlayerEvent(data, gameId);                    
            }, priority: 5);


        }

        public void BroadcastDragonPicked(DragonAcquired dragonacquired, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("DragonAcquired", dragonacquired);
        }
        
        public void BroadcastDragonReadyToSummon(DragonSummonData data, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("DragonPreSummon", data);
        }

        public void BroadcastSummonDragonEvent(DragonSummonEventData data, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("SummonDragonEvent", data);
        }

        public void BroadcastChangePlayerEvent(MiniPhaseDataWithDifferentPlayer data, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ChangedPlayerInPhase", data);
        }
    }
}