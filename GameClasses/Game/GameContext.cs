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
        public TimerManager TimerManager { get; private set; }
        public GameTiles GameTiles { get; private set; }
        public MercenaryManager MercenaryManager { get; private set; }
        public DragonManager DragonManager { get; private set; }
        public MiniPhaseManager MiniPhaseManager { get; private set; }
        public ArtifactManager ArtifactManager { get; private set; }
        public EffectManager EffectManager { get; private set; }
        public ScorePointsManager ScorePointsManager { get; private set; }
        public EndGameEffectManager EndGameEffectManager { get; private set; }
        public TokenManager TokenManager { get; private set; }
        public RewardHandlerManager RewardHandlerManager { get; private set; }
        public RolayCardManager RolayCardManager { get; private set; }

        public bool m_bDLCDragons {get; private set; }
        public bool m_bSignets25914 {get; private set; }
        public bool m_bNoEndRoundDiscount {get; private set; }
        public bool m_bNoBuildingsInPool {get; private set; }

        public bool AreSignetsSpecialThreshold()
        {
            return m_bSignets25914;
        }
        public bool IsDLCDragonsOn()
        {
            return m_bDLCDragons;
        }
        public bool NoEndRoundDiscount()
        {
            return m_bNoEndRoundDiscount;
        }
        public bool NoBuildingsInPool()
        {
            return m_bNoBuildingsInPool;
        }

        public GameContext(string gameId, List<Player> players, StartGameModel startGameModel)
        {
            EventManager = new EventManager();
            GameId = gameId;
            m_bDLCDragons = startGameModel.DLCDragons;
            m_bSignets25914 = startGameModel.SignetsTwoFiveNine;
            m_bNoEndRoundDiscount = startGameModel.NoEndRoundDiscount;
            m_bNoBuildingsInPool = startGameModel.NoBuildingsInPool;
            PlayerManager = new PlayersManager(players, this, startGameModel.SignetsTwoFiveNine);
            TurnManager = startGameModel.TurnType == TurnTypes.FULL_TURN ? new PlayerFullTurnManager(this) : new PhaseByPhaseTurnManager(this);
            HeroCardManager = new HeroCardManager(this, startGameModel.LessCards, startGameModel.MoreHeroCards, startGameModel.HeroPoolType);
            PhaseManager = new PhaseManager(this);
            BoardManager = new BoardManager(this);
            GameTiles = new GameTiles(this);
            PawnManager = new PawnManager(GameTiles.GetTileById(27), this);
            MercenaryManager = new MercenaryManager(this, startGameModel.RemovePropheciesAtLastRound, startGameModel.SameAmountOfMercenariesEachRound);
            DragonManager = new DragonManager(this, m_bDLCDragons);
            MiniPhaseManager = new MiniPhaseManager(this);
            ArtifactManager = new ArtifactManager(this);
            EffectManager = new EffectManager(this);
            TimerManager = new TimerManager(this);
            ScorePointsManager = new ScorePointsManager(this);
            EndGameEffectManager = new EndGameEffectManager(this);
            RewardHandlerManager = new RewardHandlerManager(this);
            TokenManager = new TokenManager(m_bDLCDragons);
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
                RolayCards = rolayCards,
                Signets25914 = m_bSignets25914
            };

            EventManager.Broadcast("GameStarted", ref data);

            EventManager.Broadcast("StartTurn");

            PhaseManager.StartCurrentPhase();

            ArtifactManager.SetUpNewArtifacts(TurnManager.CurrentPlayer!);
        }

        public void EndGame()
        {
            var ScorePoints = ScorePointsManager.FinalScore();
            
            TimerManager.EndTimer();
            var gameTimeSpan = TimerManager.GetTotalGameTime();
            var playerTimeSpan = TimerManager.GetPlayerTimes();

            EndOfGame data = new EndOfGame { PlayerScores = ScorePoints, GameTimeSpan = gameTimeSpan, PlayerTimeSpan = playerTimeSpan };

            EventManager.Broadcast("EndOfGame", ref data);
        }
    }
}