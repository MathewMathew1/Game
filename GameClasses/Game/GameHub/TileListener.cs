using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;
using BoardGameBackend.Hubs;
using BoardGameBackend.Mappers;

namespace BoardGameBackend.Managers.EventListeners
{
    public class TileEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public TileEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

            gameContext.EventManager.Subscribe<MoveOnTile>("MoveOnTile", moveOnTileData =>
            {
                BroadcastMoveToTile(moveOnTileData, gameId);
            }, priority: 1);

             gameContext.EventManager.Subscribe("MoveOnTileOnEvent", (MoveOnTileOnEvent data) =>
            {
                BroadcastMoveOnTileOnEvent(gameId, data);
            }, priority: 0);

            gameContext.EventManager.Subscribe("TileReward", (MoveOnTileOnEvent data) =>
            {
                BroadcastMoveOnTileOnEvent(gameId, data);
            }, priority: 0);

            gameContext.EventManager.Subscribe<GetCurrentTileReward>("GetCurrentTileReward",  getCurrentTileReward =>
            {
                BroadcastTileReward(getCurrentTileReward, gameId);
            }, priority: 5);

            gameContext.EventManager.Subscribe<BlockedTileData>("BlockedTileEvent", data =>
            {            
                BroadcastBlockTileEvent(gameId, data);
            }, priority: 5);   

            gameContext.EventManager.Subscribe<GoldIntoMovementEventData>("GoldIntoMovementEvent", data =>
            {            
                BroadcastGoldIntoMovementEvent(gameId, data);
            }, priority: 5);  

            gameContext.EventManager.Subscribe<FullMovementIntoEmptyEventData>("FullMovementIntoEmptyEvent", data =>
            {            
                BroadcastFullMovementIntoEmptyEvent(gameId, data);
            }, priority: 5);

            gameContext.EventManager.Subscribe<ResourceReceivedEventData>("ResourceReceivedEvent", data =>
            {            
                BroadcastReceivedResourcesEvent(gameId, data);
            }, priority: 5);

            gameContext.EventManager.Subscribe<SwapTokensDataEventData>("SwapTokensDataEvent", data =>
            {             
                BroadcastSwapTokensEvent(gameId, data);                    
            }, priority: 5);

            gameContext.EventManager.Subscribe<RotateTileEventData>("RotateTileEvent", data =>
            {             
                BroadcastRotatePawnEvent(gameId, data);                    
            }, priority: 5);

            gameContext.EventManager.Subscribe<RotateTileEventData>("BlinkTileEvent", data =>
            {             
                BroadcastBlinkPawnEvent(gameId, data);                    
            }, priority: 5);
        }

        public void BroadcastMoveToTile(MoveOnTile moveOnTile, string gameId)
        {
           
            IHubContext<LobbyHub> hubContext = _hubContextProvider!.LobbyHubContext;
            string connectionId = LobbyHub.ConnectionMappings
                .FirstOrDefault(kvp => kvp.Value.Id == moveOnTile.Player.Id).Key;
            
            var lobbyId = LobbyManager.GetLobbyByGameId(gameId)!.Id;

            hubContext.Clients.Client(connectionId)
                .SendAsync("MoveToTile", moveOnTile);

            var dataForOtherUsers = GameMapper.Instance.Map<MoveOnTileForOtherUsers>(moveOnTile);

            hubContext.Clients.GroupExcept(lobbyId, connectionId)
                .SendAsync("MoveToTile", dataForOtherUsers);
 
        }

        public void BroadcastTileReward(GetCurrentTileReward getCurrentTileReward, string gameId)
        {
           
            IHubContext<LobbyHub> hubContext = _hubContextProvider!.LobbyHubContext;
            string connectionId = LobbyHub.ConnectionMappings
                .FirstOrDefault(kvp => kvp.Value.Id == getCurrentTileReward.Player.Id).Key;
            
            var lobbyId = LobbyManager.GetLobbyByGameId(gameId)!.Id;

            hubContext.Clients.Client(connectionId)
                .SendAsync("TileReward", getCurrentTileReward);

            var dataForOtherUsers = GameMapper.Instance.Map<GetCurrentTileRewardForOtherUsers>(getCurrentTileReward);

            hubContext.Clients.GroupExcept(lobbyId, connectionId)
                .SendAsync("TileReward", dataForOtherUsers);
 
        }

        public void BroadcastBlockTileEvent(string gameId, BlockedTileData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BlockedTileEvent", data);
        }  

        public void BroadcastMoveOnTileOnEvent(string gameId, MoveOnTileOnEvent moveOnTileOnEvent)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("MoveOnTileOnEvent", moveOnTileOnEvent);
        }

        public void BroadcastGoldIntoMovementEvent(string gameId, GoldIntoMovementEventData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("GoldIntoMovementEvent", data);
        }

        public void BroadcastFullMovementIntoEmptyEvent(string gameId, FullMovementIntoEmptyEventData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("FullMovementIntoEmptyEvent", data);
        }

        public void BroadcastReceivedResourcesEvent(string gameId, ResourceReceivedEventData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ResourceReceivedEvent", data);
        }

        public void BroadcastSwapTokensEvent(string gameId, SwapTokensDataEventData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("SwapTokensDataEvent", data);
        }

        public void BroadcastRotatePawnEvent(string gameId, RotateTileEventData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RotateTileDataEvent", data);
        }

        public void BroadcastBlinkPawnEvent(string gameId, RotateTileEventData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BlinkTileDataEvent", data);
        }
    }
}