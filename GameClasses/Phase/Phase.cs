using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{
    // Abstract class representing a general phase
    public abstract class Phase
    {
        public abstract string Name { get; }
        public abstract void StartPhase();
        public abstract void EndPhase();
        protected readonly GameContext _gameContext;

        protected Phase(GameContext gameContext)
        {
            _gameContext = gameContext;
        }
    }

    // HeroCardPickingPhase class
    public class HeroCardPickingPhase : Phase
    {
        public override string Name => "HeroCardPickingPhase";

        public HeroCardPickingPhase(GameContext gameContext) : base(gameContext)
        {
        }

        public override void StartPhase()
        {
            var eventArgs = new DummyPhaseStarted
            {
                Player = _gameContext.TurnManager.CurrentPlayer!,
            };
            _gameContext.EventManager.Broadcast("HeroCardPickingPhaseStarted", ref eventArgs);
            Console.WriteLine("Hero Card Picking Phase started.");
        }

        public override void EndPhase()
        {
            Console.WriteLine("Hero Card Picking Phase ended.");
        }
    }

    public class MercenaryPhase : Phase
    {
        public override string Name => "MercenaryPhase";

        public MercenaryPhase(GameContext gameContext) : base(gameContext)
        {
        }

        public override void StartPhase()
        {
            var eventArgs = new DummyPhaseStarted
            {
                Player = _gameContext.TurnManager.CurrentPlayer!,
            };
            _gameContext.EventManager.Broadcast("MercenaryPhaseStarted", ref eventArgs);
            Console.WriteLine("Mercenary started.");
        }

        public override void EndPhase()
        {
            Console.WriteLine("Mercenary Phase ended.");
        }
    }

    public class DummyPhase : Phase
    {
        public override string Name => "DummyPhase";

        public DummyPhase(GameContext gameContext) : base(gameContext)
        {
        }

        public override void StartPhase()
        {
            var eventArgs = new DummyPhaseStarted
            {
                Player = _gameContext.TurnManager.CurrentPlayer!,
            };
            _gameContext.EventManager.Broadcast("DummyPhasePickingPhaseStarted", ref eventArgs);
            Console.WriteLine("Dummy Phase started.");
        }

        public override void EndPhase()
        {
            // Logic to end the Dummy phase
            Console.WriteLine("Dummy Phase ended.");
        }
    }

    public class BoardPhase : Phase
    {
        public override string Name => "BoardPhase";

        public BoardPhase(GameContext gameContext) : base(gameContext)
        {
        }

        public override void StartPhase()
        {
            Console.WriteLine("Board Phase Started.");
            var eventArgs = new DummyPhaseStarted
            {
                Player = _gameContext.TurnManager.CurrentPlayer!,
            };
            _gameContext.EventManager.Broadcast("BoardPhasePickingPhaseStarted", ref eventArgs);
        }

        public override void EndPhase()
        {
            Console.WriteLine("Dummy Phase ended.");
        }
    }

    public class ArtifactPhase : Phase
    {
        public override string Name => "ArtifactPhase";

        public ArtifactPhase(GameContext gameContext) : base(gameContext)
        {
        }

        public override void StartPhase()
        {

            var eventArgs = new DummyPhaseStarted
            {
                Player = _gameContext.TurnManager.CurrentPlayer!,
            };

            _gameContext.EventManager.Broadcast("ArtifactPhaseStarted", ref eventArgs);
        }

        public override void EndPhase()
        {
            Console.WriteLine("Artifact Phase ended.");
        }
    }
}