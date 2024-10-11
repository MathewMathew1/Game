using System.Collections.Concurrent;
using BoardGameBackend.Managers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;
using BoardGameBackend.Repositories;
using AutoMapper;

namespace BoardGameBackend.Hubs
{
    public class LobbyHub : Hub
    {
        public static readonly ConcurrentDictionary<string, Player> ConnectionMappings = new ConcurrentDictionary<string, Player>();
        private static readonly ConcurrentDictionary<string, LobbyInfo> Lobbies = new ConcurrentDictionary<string, LobbyInfo>();
        private IAuthService _userService {get;set;}
        private readonly IMapper _mapper;

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public LobbyHub(IAuthService userService, IMapper mapper){
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(Context.ConnectionId);
            // Find out which player is associated with this connection
            if (ConnectionMappings.TryRemove(Context.ConnectionId, out var disconnectedPlayer))
            {
                var lobbyId = GetLobbyIdForPlayer(disconnectedPlayer.Id);
                if (lobbyId != null && Lobbies.TryGetValue(lobbyId, out var lobbyInfo))
                {
                    // Handle lobby logic for disconnection
                    if (disconnectedPlayer.Id == lobbyInfo.HostId && !lobbyInfo.GameHasStarted)
                    {
                        // Host has disconnected and game has not started, destroy the lobby
                        LobbyManager.DestroyLobby(lobbyId);
                        await DestroyLobby(lobbyId, disconnectedPlayer);
                    }
                    else
                    {
                        // Player has disconnected, remove from lobby
                        lobbyInfo.Players.Remove(disconnectedPlayer.Id);
                        await Clients.Group(lobbyId).SendAsync("PlayerLeft", disconnectedPlayer);
                    }
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
                }
            }
            
            
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinLobby(string lobbyId)
        {
            var userIdClaim = Context.User?.FindFirst("id");
            var user = await _userService.GetUserById(Guid.Parse(userIdClaim!.Value));
            Player player = _mapper.Map<Player>(user);

            var lobbyInfo = Lobbies.GetOrAdd(lobbyId, _ => new LobbyInfo
            {
                Players = new List<Guid>(),
                HostId = player.Id,
                GameHasStarted = false
            });

            lobbyInfo.Players.Add(player.Id);
            ConnectionMappings[Context.ConnectionId] = player; 
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
            await Clients.Group(lobbyId).SendAsync("PlayerJoined", new { player });
        }

        private string? GetLobbyIdForPlayer(Guid playerId)
        {
            foreach (var kvp in Lobbies)
            {
                if (kvp.Value.Players.Contains(playerId))
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        public async Task DestroyLobby(string lobbyId, Player player)
        {
            if (Lobbies.TryRemove(lobbyId, out var lobbyInfo))
            {
                await Clients.Group(lobbyId).SendAsync("DestroyLobby", player);
            }
        }

    }
}