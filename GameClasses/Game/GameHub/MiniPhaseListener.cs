using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace BoardGameBackend.Managers.EventListeners
{
    public class MiniPhaseEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public MiniPhaseEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

            gameContext.EventManager.Subscribe("TeleportationMiniPhaseStarted", () =>
            {
                BroadcastTeleportationMiniPhaseStart(gameId);
            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactPickMiniPhaseEnded", () =>
            {
                BroadcastArtifactPickMiniPhaseEnded(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("ArtifactPickMiniPhase", () =>
            {
                BroadcastArtifactPickMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("FulfillProphecyMiniPhaseStarted", () =>
            {
                BroadcastStartFulfillProphecyMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("FulfillProphecyMiniPhaseEnded", () =>
            {
                BroadcastEndFulfillProphecyMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("LockCardMiniPhaseStarted", () =>
            {
                BroadcastStartLockCardMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("LockCardMiniPhaseEnded", () =>
            {
                BroadcastEndLockCardMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("BuffHeroPhaseStarted", () =>
            {
                BroadcastStartBuffHeroMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("BuffHeroMiniPhaseEnded", () =>
            {
                BroadcastEndBuffHeroMiniPhase(gameId);
            }, priority: 0);

            gameContext.EventManager.Subscribe("BlockTilePhaseStarted", (MiniPhaseDataWithDifferentPlayer data) =>
            {
                BroadcastStartBlockTileMiniPhase(gameId, data);
            }, priority: 0);

            gameContext.EventManager.Subscribe("BlockTileMiniPhaseEnded", (MiniPhaseDataWithDifferentPlayer data) =>
            {
                BroadcastEndBlockTileMiniPhase(gameId, data);
            }, priority: 1);

            gameContext.EventManager.Subscribe("RoyalMiniPhaseStarted", () =>
            {
                BroadcastRoyalCardPickMiniPhaseStart(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("RoyalMiniPhaseEnded", () =>
            {
                BroadcastRoyalCardPickMiniPhaseEnded(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("RerollMercenaryMiniPhaseStarted", () =>
            {
                BroadcastRerollMercenaryMiniPhase(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("ReplayArtifactPhaseStarted", () =>
            {
                BroadcastStartReplayArtifactMiniPhase(gameId);
            }, priority: 1);    

            gameContext.EventManager.Subscribe("ReplayArtifactPhaseEnded", () =>
            {
                BroadcastEndReplayArtifactMiniPhase(gameId);
            }, priority: 1);    

            gameContext.EventManager.Subscribe("ReplaceHeroMiniPhaseStarted", () =>
            {
                BroadcastStartReplaceHeroMiniPhase(gameId);
            }, priority: 1);  

            gameContext.EventManager.Subscribe("ReplaceHeroMiniPhaseEnded", () =>
            {
                BroadcastEndReplaceHeroMiniPhase(gameId);
            }, priority: 1);  
            
        }

        public void BroadcastTeleportationMiniPhaseStart(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("TeleportationMiniPhaseStarted");
        }

        public void BroadcastRoyalCardPickMiniPhaseStart(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RoyalMiniPhaseStarted");
        }

        public void BroadcastRoyalCardPickMiniPhaseEnded(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RoyalMiniPhaseEnded");
        }

        public void BroadcastArtifactPickMiniPhaseEnded(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ArtifactPickMiniPhaseEnded");
        }

        public void BroadcastArtifactPickMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ArtifactPickMiniPhase");
        }

        public void BroadcastStartFulfillProphecyMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("FulfillProphecyMiniPhaseStarted");
        }

        public void BroadcastEndFulfillProphecyMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("FulfillProphecyMiniPhaseEnded");
        }

        public void BroadcastStartLockCardMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("LockCardMiniPhaseStarted");
        }

        public void BroadcastEndLockCardMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("LockCardMiniPhaseEnded");
        }

        public void BroadcastStartBuffHeroMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BuffHeroPhaseStarted");
        }

        public void BroadcastEndBuffHeroMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BuffHeroMiniPhaseEnded");
        }

        public void BroadcastRerollMercenaryMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RerollMercenaryMiniPhaseStarted");
        }

        public void BroadcastStartBlockTileMiniPhase(string gameId, MiniPhaseDataWithDifferentPlayer data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BlockTileMiniPhaseStarted", data);
        }

        public void BroadcastEndBlockTileMiniPhase(string gameId, MiniPhaseDataWithDifferentPlayer data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BlockTileMiniEnded", data);
        }

        public void BroadcastStartReplayArtifactMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ReplayArtifactPhaseStarted");
        }

        public void BroadcastEndReplayArtifactMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ReplayArtifactPhaseEnded");
        }

        public void BroadcastStartReplaceHeroMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ReplaceHeroMiniPhaseStarted");
        }

        public void BroadcastEndReplaceHeroMiniPhase(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ReplaceHeroMiniPhaseEnded");
        }
    }
}