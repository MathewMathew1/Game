using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace BoardGameBackend.Managers.EventListeners
{
    public class MercenaryEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public MercenaryEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

            gameContext.EventManager.Subscribe<BuyableMercenariesRefreshed>("BuyableMercenariesRefreshed", buyableMercenariesRefreshed =>
            {
                BroadcastRefreshBuyableMercenaries(gameId, buyableMercenariesRefreshed);
            }, priority: 1);
            gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", mercenaryPicked =>
            {
                BroadcastMercenaryPicked(mercenaryPicked, gameId);
            }, priority: 10);
            gameContext.EventManager.Subscribe<MercenaryRerolled>("MercenaryRerolled", mercenaryRerolled =>
            {
                BroadcastMercenaryRerolled(gameId, mercenaryRerolled);
            }, priority: 10);
            gameContext.EventManager.Subscribe<FulfillProphecy>("FulfillProphecy", data =>
            {
                BroadcastFulfillMercenaryProphecy(gameId, data);
            }, priority: 10);
            gameContext.EventManager.Subscribe<LockMercenaryData>("LockMercenary", data =>
            {
                BroadcastLockMercenary(gameId, data);
            }, priority: 10);   

            gameContext.EventManager.Subscribe<PreMercenaryRerolled>("PreMercenaryRerolled", data =>
            {
                BroadcastPreMercenaryReroll(gameId, data);
            }, priority: 10);     

        }

        public void BroadcastMercenaryPicked(MercenaryPicked mercenaryPicked, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("MercenaryPicked", mercenaryPicked);
        }

        public void BroadcastMercenaryRerolled(string gameId, MercenaryRerolled mercenaryRerolled)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("MercenaryRerolled", mercenaryRerolled);
        }

        
        public void BroadcastRefreshBuyableMercenaries(string gameId, BuyableMercenariesRefreshed buyableMercenariesRefreshed)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BuyableMercenariesRefreshed", buyableMercenariesRefreshed);
        }

        public void BroadcastFulfillMercenaryProphecy(string gameId, FulfillProphecy data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("FulfillProphecy", data);
        }

        public void BroadcastLockMercenary(string gameId, LockMercenaryData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("LockMercenary", data);
        }

        public void BroadcastPreMercenaryReroll(string gameId, PreMercenaryRerolled data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("PreMercenaryRerolled", data);
        }
    }
}