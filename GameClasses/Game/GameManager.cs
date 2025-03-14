using System.Collections.Concurrent;
using BoardGameBackend.Managers.EventListeners;
using BoardGameBackend.Mappers;
using BoardGameBackend.Models;
using BoardGameBackend.Providers;

namespace BoardGameBackend.Managers
{
    public static class GameManager
    {
        private static readonly ConcurrentDictionary<string, GameContext> ActiveGames = new ConcurrentDictionary<string, GameContext>();
        private static readonly ConcurrentDictionary<string, EventListenerContainer> EventListenerContainers = new();
        private static IHubContextProvider? _hubContextProvider;

        public static void Initialize(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }
        public static GameContext StartGameFromLobby(Lobby lobby, StartGameModel startGameModel)
        {
            var gameId = Guid.NewGuid().ToString();
            var players = lobby.Players.Select(p => new Player { Id = p.Id, Name = p.Name }).ToList();

            var gameContext = new GameContext(gameId, players, startGameModel);

            var eventListenerContainer = new EventListenerContainer(_hubContextProvider);
            eventListenerContainer.SubscribeAll(gameContext);
            
            ActiveGames.TryAdd(gameId, gameContext);
            EventListenerContainers.TryAdd(gameId, eventListenerContainer);

            gameContext.EventManager.Subscribe<EndOfGame>("EndOfGame", endOfGame =>
            {             
                EndGame(gameId);                     
            }, priority: 0);
            
            return gameContext;
        }

        public static GameContext? GetGameById(string gameId)
        {
            if (ActiveGames.TryGetValue(gameId, out var gameContext))
            {
                return gameContext;
            }

            return null;
        }

        public static FullGameData? GetGameData(string gameId, Guid playerId)
        {
            var gameContext = GetGameById(gameId);

            if(gameContext == null) return null;

            List<FullPlayerData> playerData =  new List<FullPlayerData> {}; 
            FillPlayerData(playerData, gameContext); 

            List<Player> playerBasedOnMorales = gameContext.PlayerManager.PlayersBasedOnMorale.Select(p=>GameMapper.Instance.Map<Player>(p)).ToList();
            var artifactInfo = gameContext.ArtifactManager.GetArtifactLeftInfo();
            if(gameContext.TurnManager.CurrentPlayer?.Id != playerId){
                artifactInfo.ArtifactToPickFrom = new List<Artifact>();
            }

            var fullGameData = new FullGameData {
                GameId = gameId,
                MercenaryData = gameContext.MercenaryManager.GetMercenaryData(),
                RoyalCards = gameContext.RolayCardManager.GetRolayCards(),
                Turn = gameContext.TurnManager.CurrentTurn,
                Round = gameContext.TurnManager.CurrentRound,
                HeroCards = gameContext.HeroCardManager.GetCurrentHeroCards(),
                CurrentPhase = gameContext.PhaseManager.CurrentPhase.Name,
                CurrentMiniPhase = gameContext.MiniPhaseManager.CurrentMiniPhase?.Name,
                TokenSetup = gameContext.GameTiles.GetTokenInfo(),
                ArtifactInfo = gameContext.ArtifactManager.GetArtifactLeftInfo(),
                DragonData = gameContext.DragonManager.GetFullDragonData(),
                PlayersData = playerData,
                PlayerBasedOnMorales = playerBasedOnMorales,
                PawnTilePosition = gameContext.PawnManager._currentTile.Id,
                CurrentPlayerId = gameContext.TurnManager.CurrentPlayer!.Id
            };

            return fullGameData;
        }

        public static void FillPlayerData(List<FullPlayerData> playersData, GameContext gameContext){
            gameContext.PlayerManager.Players.ForEach(p => {
                FullPlayerData playerData = new FullPlayerData {
                    Player = new Player {Name = p.Name, Id = p.Id,},
                    Auras = p.AurasTypes,
                    Resources = p.ResourceManager.GetResources(),
                    ResourceHero = p.ResourceHeroManager.GetResources(),
                    Mercenaries = p.PlayerMercenaryManager.Mercenaries,
                    Dragons = p.PlayerDragonManager.Dragons,
                    RoyalCardsData = p.PlayerRolayCardManager.GetData(),
                    Heroes = p.PlayerHeroCardManager.GetPlayerHeroData(),
                    Tokens = p.Tokens,
                    Artifacts = p.ArtifactPlayerData(),
                    Morale = p.Morale,
                    GoldIncome = p.ResourceManager.GoldIncome,
                    AlreadyPlayedTurn = p.AlreadyPlayedCurrentPhase,
                    BoolStorage = p.BoolAdditionalStorage.Keys.ToList(),
                };

                playersData.Add(playerData);
            });
        }

        public static void EndGame(string gameId)
        {
            ActiveGames.TryRemove(gameId, out _);
        }

    }
}