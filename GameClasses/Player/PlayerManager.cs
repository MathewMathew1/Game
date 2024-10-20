using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PlayersManager
    {
        public List<PlayerInGame> Players { get; private set; }
        public List<PlayerInGame> PlayersBasedOnMorale { get; private set; }
        private readonly GameContext _gameContext;

        public PlayersManager(List<Player> players, GameContext gameContext)
        {
            _gameContext = gameContext;
            var random = new Random();
            Players = players
                .OrderBy(p => random.Next())
                .Select(p => new PlayerInGame(p))
                .ToList();
            PlayersBasedOnMorale = Players;
            UpdatePlayersBasedOnMorale();

            _gameContext.EventManager.Subscribe("EndOfPlayerTurn", (EndOfPlayerTurn data) =>
            {
                EndTurn(data.Player);
            }, priority: 5);

            _gameContext.EventManager.Subscribe<HeroTurnEnded>("HeroTurnEnded", moveOnTileData =>
            {
                CheckAfterMovementEvents();
                CheckOnSignets();
            }, priority: 1);

            _gameContext.EventManager.Subscribe("ArtifactsTaken", (ArtifactsTaken data) =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
                {
                    CheckOnSignets();
                }
            }, priority: 1);
            _gameContext.EventManager.Subscribe("ArtifactRerolled", (ArtifactRerolledData data) => CheckOnSignets(), priority: 1);
            _gameContext.EventManager.Subscribe("ArtifactPlayed", (ArtifactPlayed data) => CheckOnSignets(), priority: 1);

            gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", mercenaryPicked =>
            {
                CheckForAurasOnMercenaryPicked(mercenaryPicked);
            }, priority: 1);

            _gameContext.EventManager.Subscribe("MoveOnTile", (MoveOnTile data) => CheckOnMovementEvents(data), priority: 1);
            
        }

        public void CheckForAurasOnMercenaryPicked(MercenaryPicked mercenaryPicked)
        {
            var player = GetPlayerById(mercenaryPicked.Player.Id);
            if (player == null) return;

            if (mercenaryPicked.Card.TypeCard != 3) return;

            var auraIndex = player.AurasTypes.FindIndex(a => a.Aura == AurasType.FULFILL_PROPHECY);

            if (auraIndex == -1) return;

            player.PlayerMercenaryManager.SetMercenaryProphecyFulfill(mercenaryPicked.Card.InGameIndex);

            var eventArgs = new FulfillProphecy
            {
                MercenaryId = mercenaryPicked.Card.InGameIndex,
                PlayerId = player.Id
            };
            _gameContext.EventManager.Broadcast("FulfillProphecy", ref eventArgs);

            player.AurasTypes.RemoveAt(auraIndex);
        }

        public PlayerInGame? GetPlayerById(Guid playerId)
        {
            return Players.FirstOrDefault(p => p.Id == playerId);
        }

        public void CheckOnSignets(){
            var player = Players.FirstOrDefault(p => p.Id == _gameContext.TurnManager.CurrentPlayer?.Id);

            if(player == null) return;

            var newSignet = player.PlayerRolayCardManager.IsNewRolayCardToPick(player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Signet));
            if(newSignet == true){
                _gameContext.MiniPhaseManager.StarRoyalCardPickMiniPhase();
            }
        }

        public void CheckOnMovementEvents(MoveOnTile data)
        {
            var player = _gameContext.TurnManager.CurrentPlayer;

            if (player == null) return;

            var amountOfAuras = player.AurasTypes.Count(a => a.Aura == AurasType.GOLD_ON_TILES_WITHOUT_GOLD);

            if (amountOfAuras > 0)
            {
                var noGoldReward = (data.TileReward.EmptyReward == false && data.TileReward.Resources.FindIndex(r => r.Type == ResourceType.Gold) == -1) ||
                (data.TileReward.TokenReward != null && data.TileReward.TokenReward.Reward.EmptyReward == false && data.TileReward.TokenReward.Reward.Resources.FindIndex(r => r.Type == ResourceType.Gold) == -1);
                if(noGoldReward){
                    data.TileReward.Resources.Add(new Resource(ResourceType.Gold, amountOfAuras ));
                }
            }
        }

        public void CheckAfterMovementEvents()
        {
            var player = _gameContext.TurnManager.CurrentPlayer;

            if (player == null) return;

            var removedCount = player.RemoveAurasOfTypeAndReturnAmountCount(AurasType.RETURN_TO_CENTER_ON_MOVEMENT);

            if (removedCount > 0)
            {
                _gameContext.PawnManager.MoveToCenter(player, AurasType.RETURN_TO_CENTER_ON_MOVEMENT);
            }
        }

        public void AddMoraleToPlayer(PlayerInGame player, int addedMorale)
        {
            if (addedMorale == 0) return;
            player.Morale += addedMorale;
            UpdatePlayersBasedOnMorale(player);
        }

        public void EndTurn(PlayerInGame player)
        {
            if (player.AurasTypes.Any(a => a.Aura == AurasType.BLOCK_TILE))
            {
                _gameContext.TurnManager.TemporarySetCurrentPlayer(player);
                _gameContext.MiniPhaseManager.StartBlockTileMiniPhase();
            }
            player.ResetAura();
            player.ResourceManager.EndOfTurnIncome();
        }

        private void UpdatePlayersBasedOnMorale(PlayerInGame? updatedPlayer = null)
        {
            if (updatedPlayer != null)
            {
                PlayersBasedOnMorale = PlayersBasedOnMorale
                    .OrderByDescending(p => p.Morale)
                    .ThenBy(p => p == updatedPlayer ? -1 : 0)
                    .ToList();
            }
            else
            {
                PlayersBasedOnMorale = PlayersBasedOnMorale
                    .OrderByDescending(p => p.Morale)
                    .ToList();
            }
        }

        public Dictionary<Guid, Dictionary<ResourceType, int>> GetPlayersResources()
        {
            var playerResources = new Dictionary<Guid, Dictionary<ResourceType, int>>();

            foreach (var player in Players)
            {
                var resources = player.ResourceManager.GetResources();
                playerResources[player.Id] = resources;
            }

            return playerResources;
        }

        public PlayerInGame? GetNextPlayerForPhase()
        {
            var nextPlayer = PlayersBasedOnMorale.FirstOrDefault(p => !p.AlreadyPlayedCurrentPhase);

            return nextPlayer;
        }

        public void ResetAllPlayersPlayedTurn()
        {
            PlayersBasedOnMorale.ForEach(p => p.AlreadyPlayedCurrentPhase = false);
        }
    }


}
