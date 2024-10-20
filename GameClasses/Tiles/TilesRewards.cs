using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{
    public class Resource
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }

        public Resource(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }

    public class TileReward
    {
        public List<Resource> Resources { get; set; }
        public TokenReward? TokenReward {get; set;}
        public int? TeleportedTileId { get; set; }
        public int? ExperiencePoints { get; set; }
        public bool? RerollMercenaryAction {get; set;}
        public bool? GetRandomArtifact  { get; set; }
        public Artifact? Artifact { get; set; }
        public bool EmptyReward { get; set; } = false;
        public bool EmptyMovement { get; set; } = false;
        public bool TempSignet { get; set; } = false;

        public TileReward()
        {
            Resources = new List<Resource>();
        }
    }

    public interface ITileAction
    {
        TileReward OnEnterReward();
    }

    public class MoneyGreaterTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 3) },
            };
        }
    }

    public class MoneyTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 2) },
            };
        }
    }

    public class MoneySmallerTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 1) },
            };
        }
    }

    public class WoodTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Wood, 1) },
            };
        }
    }

    public class NitreTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Niter, 1) },
            };
        }
    }

    public class SteelTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Iron, 1) },
            };
        }
    }

    public class GemsTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gems, 1) },
            };
        }
    }

    public class MysticFogTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.MysticFog, 1) },
            };
        }
    }

    public class TeleportationTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> {  },
            };
        }
    }

    public class StartTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                EmptyReward = true
            };
        }
    }

    public class RerollMercenaryTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                RerollMercenaryAction = true,
                Resources = new List<Resource> {  },
            };
        }
    }

    public class GetRandomMercenaryTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                GetRandomArtifact  = true,
                Resources = new List<Resource> {  },
            };
        }
    }

    public class CastleTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 3) },
            };
        }
    }

    public class EmptyMovementTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                EmptyMovement = true,
            };
        }
    }

     public class SignetTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                TempSignet = true
            };
        }
    }

    public class DefaultTileAction : ITileAction
    {
        public TileReward OnEnterReward()
        {
            return new TileReward
            {
                Resources = new List<Resource> {  },
            };
        }
    }

    public static class TileActionFactory
    {
        public static ITileAction GetTileAction(int id)
        {
            return id switch
            {
                1 => new MoneyGreaterTileAction(),
                2 => new MoneyTileAction(),
                3 => new MoneySmallerTileAction(),
                4 => new StartTileAction(),
                5 => new WoodTileAction(),
                6 => new NitreTileAction(),
                7 => new SteelTileAction(),
                8 => new GemsTileAction(),
                9 => new MysticFogTileAction(),
                11 => new GetRandomMercenaryTileAction(),
                12 => new RerollMercenaryTileAction(),
                13 => new CastleTileAction(),
                14 => new SignetTileAction(),
                15 => new EmptyMovementTileAction(),
                _ => new DefaultTileAction() // Default if no specific ID is matched
            };
        }
    }
}