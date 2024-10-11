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

      
    }
}