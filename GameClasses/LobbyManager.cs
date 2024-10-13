using BoardGameBackend.Models;

namespace BoardGameBackend.Managers;


public static class LobbyManager
{
    private static readonly List<Lobby> Lobbies = new List<Lobby>();
    public static readonly int MAX_AMOUNT_OF_USERS_IN_LOBBY = 4;
    private static readonly object _lock = new object();

    public static Lobby CreateLobby(UserModel user, CreateLobbyDto createLobbyDto)
    {
        lock (_lock)
        {
            var player = new Player { Id = user.Id, Name = user.Username };
            var lobby = new Lobby { HostId = user.Id, LobbyName = createLobbyDto.LobbyName };
            lobby.Players.Add(player);
            Lobbies.Add(lobby);
            return lobby;
        }
    }

    public static void SetGameIdForLobby(string lobbyId, string gameId)
    {
        var lobby = GetLobbyById(lobbyId);
        if (lobby != null)
        {
            lobby.GameId = gameId;
        }
    }


    public static LobbyJoinResult JoinLobby(string lobbyId, UserModel user)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.Id == lobbyId);
        if (lobby == null)
        {
            return new LobbyJoinResult { ErrorMessage = "Lobby not found." };
        }
        if (lobby.Players.Any(p => p.Id == user.Id))
        {
            return new LobbyJoinResult { ErrorMessage = "User is already in the lobby." };
        }
        if (lobby.Players.Count >= MAX_AMOUNT_OF_USERS_IN_LOBBY)
        {
            return new LobbyJoinResult { ErrorMessage = "Lobby is full." };
        }
        if (lobby.GameId != null)
        {
            return new LobbyJoinResult { ErrorMessage = "Lobby is already started." };
        }

        

        var player = new Player { Id = user.Id, Name = user.Username };
        lobby.Players.Add(player);

        return new LobbyJoinResult { Lobby = lobby };
    }

    public static Lobby? LeaveLobby(string lobbyId, UserModel user)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.Id == lobbyId);
        if (lobby != null)
        {
            var player = lobby.Players.FirstOrDefault(p => p.Id == user.Id);
            if (player != null)
            {
                lobby.Players.Remove(player);

                if (lobby.HostId == player.Id)
                {
                    Lobbies.Remove(lobby);
                    return null;
                }
            }
        }
        return lobby;
    }

    public static bool CanDestroyLobby(string lobbyId, UserModel user)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.Id == lobbyId);

        if (lobby != null && lobby.HostId == user.Id)
        {
            return true;
        }
        return false;
    }

    public static void DestroyLobby(string lobbyId)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.Id == lobbyId);
        if (lobby != null)
        {
            Lobbies.Remove(lobby);
        }

    }

    public static Lobby? GetLobbyById(string lobbyId)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.Id == lobbyId);

        return lobby;
    }

    public static Lobby? GetLobbyByGameId(string gameId)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.GameId == gameId);

        return lobby;
    }

    public static GameContext? StartGame(string lobbyId, UserModel user,StartGameModel startGameModel)
    {
        var lobby = Lobbies.FirstOrDefault(l => l.Id == lobbyId);
        if (lobby != null && lobby.HostId == user.Id)
        {
            var gameContext = GameManager.StartGameFromLobby(lobby, startGameModel);
            SetGameIdForLobby(lobbyId, gameContext.GameId);
            return gameContext;
        }
        return null;
    }

    public static List<Lobby> GetAllLobbies()
    {
        return Lobbies; // Assuming Lobbies is a dictionary or similar collection
    }

}