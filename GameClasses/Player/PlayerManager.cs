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

            _gameContext.EventManager.Subscribe("EndOfTurn ", () =>
            {
                EndTurn();
            }, priority: 5);



            _gameContext.EventManager.Subscribe<MoveOnTile>("MoveOnTile", moveOnTileData =>
            {
                CheckOnMovementEvents();
            }, priority: 0);

            gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", mercenaryPicked =>
            {
                CheckForAurasOnMercenaryPicked(mercenaryPicked);
            }, priority: 1);     
        }

        public void CheckForAurasOnMercenaryPicked(MercenaryPicked mercenaryPicked){
            var player = GetPlayerById(mercenaryPicked.Player.Id);
            if(player == null) return;

            if(mercenaryPicked.Card.TypeCard != 3) return;

            var auraIndex = player.AurasTypes.FindIndex(a => a.Aura == AurasType.FULFILL_PROPHECY);

            if(auraIndex==-1) return;

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

        public void CheckOnMovementEvents(){
            var player = _gameContext.TurnManager.CurrentPlayer;

            if(player == null) return;

            var removedCount = player.RemoveAurasOfTypeAndReturnAmountCount(AurasType.RETURN_TO_CENTER_ON_MOVEMENT);

            if(removedCount > 0){
                _gameContext.PawnManager.MoveToCenter(player, AurasType.RETURN_TO_CENTER_ON_MOVEMENT);
            }
        }

        public void AddMoraleToPlayer(PlayerInGame player, int addedMorale)
        {
            player.Morale += addedMorale;
            UpdatePlayersBasedOnMorale(player);
        }

        public void EndTurn(){
            Players.ForEach(p => {
                if(p.AurasTypes.Any(a=>a.Aura==AurasType.BLOCK_TILE)){
                    _gameContext.TurnManager.TemporarySetCurrentPlayer(p);
                    _gameContext.MiniPhaseManager.StartBlockTileMiniPhase();
                }
                p.ResetAura();
                p.ResourceManager.EndOfTurnIncome();
            });     
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

        public void ResetAllPlayersPlayedTurn(){
            PlayersBasedOnMorale.ForEach(p=>p.AlreadyPlayedCurrentPhase = false);
        }
    }


}
