using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;
using BoardGameBackend.Hubs;
using BoardGameBackend.Mappers;

namespace BoardGameBackend.Managers.EventListeners
{
    public class ArtifactEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public ArtifactEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

            gameContext.EventManager.Subscribe("ArtifactsToPick", (ArtifactToPickFromData data) =>
            {
                BroadcastArtifactsToPickFrom(gameId, data, "ArtifactsToPickFrom", "ArtifactSummary");

            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactsGivenToPlayer", (ArtifactToPickFromData data) =>
            {
                BroadcastArtifactsToPickFrom(gameId, data, "ArtifactsGiven", "ArtifactsGivenToOtherPlayer");
            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactsTaken", (ArtifactsTaken data) =>
            {
                BroadcastArtifactsTaken(gameId, data);
            }, priority: 10);
            gameContext.EventManager.Subscribe("ArtifactPlayed", (ArtifactPlayed data) =>
            {
                BroadcastArtifactPlayed(gameId, data);
            }, priority: 0);
            gameContext.EventManager.Subscribe("ArtifactRePlayed", (ArtifactPlayed data) =>
            {
                BroadcastArtifactRePlayed(gameId, data);
            }, priority: 2);
            gameContext.EventManager.Subscribe("ArtifactRerolled", (ArtifactRerolledData data) =>
            {
                BroadcastArtifactRerolled(gameId, data);
            }, priority: 10);
            
            gameContext.EventManager.Subscribe<ArtifactPhaseSkipped>("PlayerSkippedArtifactPhase", data =>
            {             
                BroadcastArtifactPhaseSkipped(gameId, data);                     
            }, priority: 5);
            
        }

        public void BroadcastArtifactsToPickFrom(string gameId, ArtifactToPickFromData artifactToPickFromData,
        string EventForUserThatGetsArtifact, string EventForOtherUsers)
        {
            IHubContext<LobbyHub> hubContext = _hubContextProvider!.LobbyHubContext;
            string connectionId = LobbyHub.ConnectionMappings
                .FirstOrDefault(kvp => kvp.Value.Id == artifactToPickFromData.Player.Id).Key;

            var lobbyId = LobbyManager.GetLobbyByGameId(gameId)!.Id;

            hubContext.Clients.Client(connectionId)
                .SendAsync(EventForUserThatGetsArtifact, artifactToPickFromData);

            var dataForOtherUsers = GameMapper.Instance.Map<ArtifactToPickFromDataForOtherUsers>(artifactToPickFromData);

            hubContext.Clients.GroupExcept(lobbyId, connectionId)
                .SendAsync(EventForOtherUsers, dataForOtherUsers);
        }

        public void BroadcastArtifactsTaken(string gameId, ArtifactsTaken artifactToPickFromData)
        {
            IHubContext<LobbyHub> hubContext = _hubContextProvider!.LobbyHubContext;
            string connectionId = LobbyHub.ConnectionMappings
                .FirstOrDefault(kvp => kvp.Value.Id == artifactToPickFromData.Player.Id).Key;

            var lobbyId = LobbyManager.GetLobbyByGameId(gameId)!.Id;

            hubContext.Clients.Client(connectionId)
                .SendAsync("ArtifactsTaken", artifactToPickFromData);

            var dataForOtherUsers = GameMapper.Instance.Map<ArtifactsTakenDataForOtherUsers>(artifactToPickFromData);

            hubContext.Clients.GroupExcept(lobbyId, connectionId)
                .SendAsync("ArtifactsTakenOtherData", dataForOtherUsers);
        }

        public void BroadcastArtifactRerolled(string gameId, ArtifactRerolledData artifactRerolledData)
        {
            IHubContext<LobbyHub> hubContext = _hubContextProvider!.LobbyHubContext;
            string connectionId = LobbyHub.ConnectionMappings
                .FirstOrDefault(kvp => kvp.Value.Id == artifactRerolledData.Player.Id).Key;

            var lobbyId = LobbyManager.GetLobbyByGameId(gameId)!.Id;

            hubContext.Clients.Client(connectionId)
                .SendAsync("ArtifactRerolled", artifactRerolledData);

            var dataForOtherUsers = GameMapper.Instance.Map<ArtifactRerolledDataForOtherUsers>(artifactRerolledData);

            hubContext.Clients.GroupExcept(lobbyId, connectionId)
                .SendAsync("ArtifactRerolledOtherData", dataForOtherUsers);
        }

        public void BroadcastArtifactPlayed(string gameId, ArtifactPlayed ArtifactPlayed)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ArtifactPlayed", ArtifactPlayed);
        }

        public void BroadcastArtifactRePlayed(string gameId, ArtifactPlayed ArtifactPlayed)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("ArtifactRePlayed", ArtifactPlayed);
        }

        public void BroadcastArtifactPhaseSkipped(string gameId, ArtifactPhaseSkipped data)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("PlayerSkippedArtifactPhase", data);
        }
    }
}