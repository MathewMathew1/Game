using AutoMapper;
using BoardGameBackend.Mappers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class GameContext
    {
        public string GameId { get; private set; }
        public PlayersManager PlayerManager { get; private set; }
        public ITurnManager TurnManager { get; private set; }
        public HeroCardManager HeroCardManager { get; set; }
        public PhaseManager PhaseManager { get; set; }
        public BoardManager BoardManager { get; set; }
        public EventManager EventManager { get; }
        public PawnManager PawnManager { get; private set; }
        public GameTiles GameTiles { get; private set; }
        public MercenaryManager MercenaryManager { get; private set; }
        public MiniPhaseManager MiniPhaseManager { get; private set; }
        public ArtifactManager ArtifactManager { get; private set; }
        public EffectManager EffectManager { get; private set; }
        public ScorePointsManager ScorePointsManager { get; private set; }
        public EndGameEffectManager EndGameEffectManager { get; private set; }
        public TokenManager TokenManager { get; private set; }
        public RewardHandlerManager RewardHandlerManager { get; private set; }
        public RolayCardManager RolayCardManager { get; private set; }

        public GameContext(string gameId, List<Player> players, StartGameModel startGameModel)
        {
            EventManager = new EventManager();
            GameId = gameId;
            PlayerManager = new PlayersManager(players, this);
            TurnManager = startGameModel.TurnType == TurnTypes.FULL_TURN ? new PlayerFullTurnManager(this) : new PhaseByPhaseTurnManager(this);
            HeroCardManager = new HeroCardManager(this, startGameModel.LessCards, startGameModel.MoreHeroCards);
            PhaseManager = new PhaseManager(this);
            BoardManager = new BoardManager(this);
            GameTiles = new GameTiles(this);
            PawnManager = new PawnManager(GameTiles.GetTileById(27), this);
            MercenaryManager = new MercenaryManager(this, startGameModel.RemovePropheciesAtLastRound);
            MiniPhaseManager = new MiniPhaseManager(this);
            ArtifactManager = new ArtifactManager(this);
            EffectManager = new EffectManager(this);
            ScorePointsManager = new ScorePointsManager(this);
            EndGameEffectManager = new EndGameEffectManager(this);
            RewardHandlerManager = new RewardHandlerManager(this);
            TokenManager = new TokenManager();
            RolayCardManager = new RolayCardManager(this);

            EventManager.Subscribe("EndOfGamePreData", () =>
            {
                EndGame();
            }, priority: 5);
        }

        public void StartGame()
        {
            var tokenSetup = GameTiles.SetupTokens();

            var playerViewModels = PlayerManager.Players
            .Select(GameMapper.Instance.Map<PlayerViewModel>)
            .ToList();

            var mercenaryData = MercenaryManager.GetMercenaryData();

            var rolayCards = RolayCardManager.GetRolayCards();

            StartOfGame data = new StartOfGame
            {
                MercenaryData = mercenaryData,
                TokenSetup = tokenSetup,
                Players = playerViewModels,
                GameId = GameId,
                RolayCards = rolayCards
            };

            EventManager.Broadcast("GameStarted", ref data);

            EventManager.Broadcast("StartTurn");

            PhaseManager.StartCurrentPhase();

            ArtifactManager.SetUpNewArtifacts(TurnManager.CurrentPlayer!);
        }

        public void EndGame()
        {
            var ScorePoints = ScorePointsManager.FinalScore();

            EndOfGame data = new EndOfGame { PlayerScores = ScorePoints };

            EventManager.Broadcast("EndOfGame", ref data);
        }
    }
}