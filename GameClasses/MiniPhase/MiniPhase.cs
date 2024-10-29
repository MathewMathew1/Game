using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{
    public abstract class MiniPhase
    {
        public abstract MiniPhaseType Name { get; }
        public abstract void StartMiniPhase();
        public abstract void EndMiniPhase();
        protected readonly GameContext _gameContext;

        protected MiniPhase(GameContext gameContext)
        {
            _gameContext = gameContext;
        }
    }

    public class RerollMercenaryMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.MercenaryRerollPhase;

        public RerollMercenaryMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("RerollMercenaryMiniPhaseStarted");
            Console.WriteLine("Mini Phase RerollMercenary Started");
        }

        public override void EndMiniPhase()
        {
            Console.WriteLine("Reroll Mercenary Mini Phase ended.");
        }
    }

    public class TeleportMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.TeleportationPhase;

        public TeleportMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("TeleportationMiniPhaseStarted");
        }

        public override void EndMiniPhase()
        {
            Console.WriteLine("Reroll Mercenary Mini Phase ended.");
        }
    }

    public class ArtifactPickMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.ArtifactPickPhase;

        public ArtifactPickMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("ArtifactPickMiniPhase");
            
            Console.WriteLine("ArtifactPick Phase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("ArtifactPickMiniPhaseEnded");
            Console.WriteLine("ArtifactPick Phase ended.");
        }
    }

    public class FulfillProphecyMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.FulfilProphecyPhase;

        public FulfillProphecyMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("FulfillProphecyMiniPhaseStarted");
            Console.WriteLine("FulfillProphecyMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("FulfillProphecyMiniPhaseEnded");
            Console.WriteLine("ArtifactPick Phase ended.");
        }
    }

    public class LockCardMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.LockMercenaryPhase;

        public LockCardMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("LockCardMiniPhaseStarted");
            Console.WriteLine("LockCardMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("LockCardMiniPhaseEnded");
            Console.WriteLine("LockCardMiniPhase ended.");
        }
    }

    public class BanishCarMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.BanishRoyalCard;

        public BanishCarMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("BanishCarMiniPhaseStarted");
            Console.WriteLine("BanishCarMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("BanishCarMiniPhaseEnded");
            Console.WriteLine("BanishCarMiniPhase ended.");
        }
    }

    public class SwapTokenMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.SwapTokens;

        public SwapTokenMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("SwapTokenMiniPhaseStarted");
            Console.WriteLine("SwapTokenMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("SwapTokenMiniPhaseEnded");
            Console.WriteLine("SwapTokenMiniPhase ended.");
        }
    }

    public class BuffHeroMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.BuffHeroPhase;

        public BuffHeroMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("BuffHeroPhaseStarted");
            Console.WriteLine("BuffHeroMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("BuffHeroMiniPhaseEnded");
            Console.WriteLine("BuffHeroMiniPhase ended.");
        }
    }

    public class BlockTileMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.BlockTilePhase;

        public BlockTileMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {

            var eventArgs = new MiniPhaseDataWithDifferentPlayer
            {
                PlayerId =_gameContext.TurnManager.CurrentPlayer.Id
            };

            
            _gameContext.EventManager.Broadcast("BlockTilePhaseStarted", ref eventArgs);
            Console.WriteLine("BlockTileMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.TurnManager.ResetCurrentPlayer();
            var eventArgs = new MiniPhaseDataWithDifferentPlayer
            {
                PlayerId =_gameContext.TurnManager.CurrentPlayer.Id
            };
            _gameContext.EventManager.Broadcast("BlockTileMiniPhaseEnded", ref eventArgs);
            Console.WriteLine("BuffHeroMiniPhase ended.");
        }
    }

    public class RoyalCardPickMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.RoyalCardPickMiniPhase;

        public RoyalCardPickMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("RoyalMiniPhaseStarted");
            Console.WriteLine("BlockTileMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("RoyalMiniPhaseEnded");
            Console.WriteLine("RoyalMiniPhaseEnded.");
        }
    }

    public class ArtifactReplayMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.ReplayArtifactMiniPhase;

        public ArtifactReplayMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("ReplayArtifactPhaseStarted");
            Console.WriteLine("ReplayArtifactPhaseStarted started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("ReplayArtifactPhaseEnded");
            Console.WriteLine("ReplayArtifactPhaseEnded.");
        }
    }

    public class ReplaceNextHeroMiniPhase : MiniPhase
    {
        public override MiniPhaseType Name => MiniPhaseType.ReplaceHeroMiniPhase;

        public ReplaceNextHeroMiniPhase(GameContext gameContext) : base(gameContext)
        {
            
        }

        public override void StartMiniPhase()
        {
            _gameContext.EventManager.Broadcast("ReplaceHeroMiniPhaseStarted");
            Console.WriteLine("ReplaceHeroMiniPhase started.");
        }

        public override void EndMiniPhase()
        {
            _gameContext.EventManager.Broadcast("ReplaceHeroMiniPhaseEnded");
            Console.WriteLine("ReplaceHeroMiniPhaseEnded.");
        }
    }

    public class RerollMercenaryMiniPhaseStarted
    {
        public required Player CurrentPlayer { get; set; }
        public required List<Mercenary> MercenariesAvailable { get; set; }
    }
}