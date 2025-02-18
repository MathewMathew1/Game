using BoardGameBackend.Helpers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public abstract class BaseTurnManager : ITurnManager
    {
        protected readonly GameContext _gameContext;
        public bool BlockNextPlayerTurn { get; set; } = false;
        protected PlayerInGame? _currentPlayer;
        protected int _currentRound = 1;
        protected int _currentTurn = 1;
        protected PlayerInGame? realCurrentPlayer;
        public readonly int MAX_ROUNDS = 5;

        public BaseTurnManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _currentPlayer = _gameContext.PlayerManager.Players.FirstOrDefault();
            _currentPlayer.AlreadyPlayedCurrentPhase = true;
            SubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
            _gameContext.EventManager.Subscribe<HeroCardPicked>("HeroCardPicked", args => EndTurn(), priority: 5);
            _gameContext.EventManager.Subscribe<MercenaryPicked>("MercenaryPicked", args => EndTurn(), priority: 1);
            _gameContext.EventManager.Subscribe<HeroTurnEnded>("HeroTurnEnded", args => EndTurn(), priority: 0);
            _gameContext.EventManager.Subscribe("ArtifactsTaken", (ArtifactsTaken data) =>
            {
                EndTurn();
            }, priority: 0);
            _gameContext.EventManager.Subscribe("ArtifactRerolled", (ArtifactRerolledData data) => {
                var player = data.Player;
                if(player.AurasTypes.Any(a => a.Aura == AurasType.EXTRA_ARTIFACT_REROLL) && !player.BoolAdditionalStorage.ContainsKey(BoolHelper.EXTRA_REROLL_PLAYED)){
                    player.BoolAdditionalStorage.Add(BoolHelper.EXTRA_REROLL_PLAYED, true);
                }else{
                    EndTurn();
                }       
            }, priority: 0);
            _gameContext.EventManager.Subscribe("ArtifactPlayed", (ArtifactPlayed data) => EndTurn(), priority: 0);
     
            _gameContext.EventManager.Subscribe<TeleportationData>("TeleportationEvent", rerollMercenaryData =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
                {
                    EndTurn();
                }
            }, priority: 5);
            _gameContext.EventManager.Subscribe<LockMercenaryData>("LockMercenary", data =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() == typeof(MercenaryPhase))
                {
                    EndTurn();
                }
            }, priority: 0);

            _gameContext.EventManager.Subscribe<MercenaryRerolled>("MercenaryRerolled", data =>
            {
                if (_gameContext.PhaseManager.CurrentPhase.GetType() != typeof(BoardPhase))
                {
                    EndTurn();
                }
            }, priority: 0);
            _gameContext.EventManager.Subscribe("BlockTileMiniPhaseEnded", (MiniPhaseDataWithDifferentPlayer data) =>
            {
                AfterEndPlayerTurn();
            }, priority: 0);

            _gameContext.EventManager.Subscribe<RoyalCardPlayed>("RolayCardPlayed", data =>
            {             
                EndTurn();                    
            }, priority: 1);
            _gameContext.EventManager.Subscribe<ArtifactPlayed>("ArtifactRePlayed", data =>
            {
                EndTurn();    
            }, priority: 0);

            _gameContext.EventManager.Subscribe<ReplaceNextHeroEventData>("ReplaceNextHeroEvent", data =>
            {             
                EndTurn();                     
            }, priority: 1);

            _gameContext.EventManager.Subscribe<ArtifactPhaseSkipped>("PlayerSkippedArtifactPhase", data =>
            {             
                EndTurn();                     
            }, priority: 1);

            _gameContext.EventManager.Subscribe<BanishRoyalCardEventData>("BanishRoyalCardEvent", data =>
            {             
                EndTurn();                     
            }, priority: 1);

            _gameContext.EventManager.Subscribe<SwapTokensDataEventData>("SwapTokensDataEvent", data =>
            {             
                EndTurn();                    
            }, priority: 1);

            _gameContext.EventManager.Subscribe<RotateTileEventData>("RotateTileEvent", data =>
            {             
                EndTurn();                    
            }, priority: 1);

            _gameContext.EventManager.Subscribe<RotateTileEventData>("BlinkTileEvent", data =>
            {             
                EndTurn();                    
            }, priority: 1);
            
            _gameContext.EventManager.Subscribe<ArtifactDiscardData>("DiscardArtifactForFullMovementEvent", data =>
            {             
                EndTurn();                    
            }, priority: 1);

            _gameContext.EventManager.Subscribe<DragonSummonEventData>("SummonDragonEvent", data =>
            {             
                IfDragonNotBlockingEndTurn();                    
            }, priority: 1);
        }

        public abstract void EndTurn(); 
        public abstract void AfterEndPlayerTurn();

        public void ResetCurrentPlayer()
        {
            if(realCurrentPlayer == null)
                return;
                
            _currentPlayer = realCurrentPlayer;
            BlockNextPlayerTurn = false;
        }

        public void TemporarySetCurrentPlayer(PlayerInGame player)
        {
            BlockNextPlayerTurn = true;
            realCurrentPlayer = _currentPlayer;
            _currentPlayer = player;

            var eventArgs = new MiniPhaseDataWithDifferentPlayer
            {
                PlayerId = player.Id
            };

            
            _gameContext.EventManager.Broadcast("ChangedPlayerInPhase", ref eventArgs);
        }
        public void IfDragonNotBlockingEndTurn()
        {
            if(!_gameContext.DragonManager.IsExtraSpawnOn())
            {
                EndTurn();
            }
        }

        public PlayerInGame? CurrentPlayer => _currentPlayer;
        public int CurrentRound => _currentRound;
        public int CurrentTurn => _currentTurn;
    }
}