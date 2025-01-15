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

            gameContext.EventManager.Subscribe("BanishCarMiniPhaseStarted", () =>
            {
                BroadcastStartBanishRoyalCard(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("BanishCarMiniPhaseEnded", () =>
            {
                BroadcastEndBanishRoyalCard(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("SwapTokenMiniPhaseStarted", () =>
            {
                BroadcastStartSwapTokens(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("SwapTokenMiniPhaseEnded", () =>
            {
                BroadcastEndSwapTokens(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("RotatePawnMiniPhaseStarted", () =>
            {
                BroadcastStartRotatePawn(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("BlinkPawnMiniPhaseStarted", () =>
            {
                BroadcastStartBlinkPawn(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("SummonDragonMiniPhaseStarted", () =>
            {
                BroadcastStartSummonDragon(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("PickDragonMiniPhaseStarted", (DragonPickData data) =>
            {
                BroadcastStartPickDragon(gameId, data);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("DiscardArtifactForFullMovementPhaseStarted", () =>
            {
                BroadcastStartOptionalDiscardArtifactForFullMove(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("RotatePawnMiniPhaseEnded", () =>
            {
                BroadcastEndRotatePawn(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("BlinkPawnMiniPhaseEnded", () =>
            {
                BroadcastEndBlinkPawn(gameId);
            }, priority: 1);

            gameContext.EventManager.Subscribe("SummonDragonMiniPhaseEnded", (MiniPhaseDataWithDifferentPlayer data) =>
            {
                BroadcastEndSummonDragon(gameId, data);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("ReplaceHeroToBuyMiniPhaseStarted", () =>
            {
                BroadcastEndRotatePawn(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("ReplaceHeroToBuyMiniPhaseEnded", () =>
            {
                BroadcastEndRotatePawn(gameId);
            }, priority: 1); 

            gameContext.EventManager.Subscribe("DiscardArtifactForFullMovementPhaseEnded", () =>
            {
                BroadcastDiscardArtifactForFullMovement(gameId);
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

        public void BroadcastEndBanishRoyalCard(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BanishCarMiniPhaseEnded");
        }

        public void BroadcastStartBanishRoyalCard(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BanishCarMiniPhaseStarted");
        }

        public void BroadcastEndSwapTokens(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("SwapTokenMiniPhaseEnded");
        }

        public void BroadcastStartSwapTokens(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("SwapTokenMiniPhaseStarted");
        }
        public void BroadcastEndRotatePawn(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RotatePawnMiniPhaseEnded");
        }
        public void BroadcastEndBlinkPawn(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BlinkPawnMiniPhaseEnded");
        }
        
        public void BroadcastEndSummonDragon(string gameId, MiniPhaseDataWithDifferentPlayer data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("SummonDragonMiniPhaseEnded", data);
        }

        public void BroadcastDiscardArtifactForFullMovement(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("DiscardArtifactForFullMovementPhaseEnded");
        }

        public void BroadcastStartRotatePawn(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("RotatePawnMiniPhaseStarted");
        }
        public void BroadcastStartBlinkPawn(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("BlinkPawnMiniPhaseStarted");
        }
        public void BroadcastStartSummonDragon(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("SummonDragonMiniPhaseStarted");
        }
        public void BroadcastStartPickDragon(string gameId, DragonPickData data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("PickDragonMiniPhaseStarted", data);
        }
        public void BroadcastStartOptionalDiscardArtifactForFullMove(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("DiscardArtifactForFullMovementPhaseStarted");
        }

        public void BroadcastReplaceHeroToBuy(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ReplaceHeroToBuyMiniPhaseStarted");
        }


        public void BroadcastReplaceHeroToBuyEnded(string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ReplaceHeroToBuyMiniPhaseEnded");
        }



    }
}