namespace BoardGameBackend.Models
{
    public class Lobby
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required Guid HostId { get; set; }
        public required string LobbyName { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public string? GameId { get; set; }
    }

    public class LobbyInfo
    {
        public List<Guid> Players { get; set; } = new List<Guid>();
        public Guid HostId { get; set; }
        public bool GameHasStarted { get; set; }
    }

    public class CreateLobbyDto
    {
        public required string LobbyName { get; set; } // User who is subscribing    
    }

    public class JoinLobbyDto
    {
        public required string PlayerName { get; set; } // User who is subscribing    
        public required string LobbyId { get; set; } // User who is subscribing       
    }

    public class LeaveLobbyDto
    {
        public required string PlayerName { get; set; } // User who is subscribing    
        public required string LobbyId { get; set; } // User who is subscribing       
    }

    public class DestroyLobbyDto
    {
        public required string PlayerName { get; set; } // User who is subscribing    
        public required string LobbyId { get; set; } // User who is subscribing       
    }

    public class SendLobbyMessageDto
    {// User who is subscribing       
        public required string Message { get; set; }
    }

        public class LobbyJoinResult
        {
            public Lobby? Lobby { get; set; }
            public string? ErrorMessage { get; set; }
            public bool Success => Lobby != null && string.IsNullOrEmpty(ErrorMessage);
        }


    
}