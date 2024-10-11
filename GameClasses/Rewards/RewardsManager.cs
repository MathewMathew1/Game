using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{


    public class Reward
    {
        public List<Resource> Resources { get; set; }
        public List<HeroResource> HeroResources { get; set; }
        public List<AuraTypeWithLongevity> AurasTypes { get; set; }
        public List<EndGameAuraType> EndGameAura { get; set; }
        public List<EffectType> Effects { get; set; }
        public int? Morale {get; set;}


        public Reward()
        {
            Resources = new List<Resource>();
            AurasTypes = new List<AuraTypeWithLongevity>();
            HeroResources = new List<HeroResource>();
            Effects = new List<EffectType>();
            EndGameAura = new List<EndGameAuraType>();
        }
    }

    public interface IGetReward
    {
        Reward OnReward();
    }

    public class ThreeGold : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 3) },
            };
        }
    }

    public class FiveGold : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 5) },
            };
        }
    }

    public class SevenGold : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 7) },
            };
        }
    }

    public class DefaultRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 5) },
            };
        }
    }

    public class FullMovementRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { },
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.ONE_FULL_MOVEMENT, Permanent = true}}
            };
        }
    }

    public class EmptyMovementRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { },
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.ONE_EMPTY_MOVEMENT, Permanent = false}} 
            };
        }
    }

    public class AllBasicsResourceReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { 
                new Resource(ResourceType.Niter, 1), 
                new Resource(ResourceType.Wood, 1), 
                new Resource(ResourceType.Iron, 1), 
                new Resource(ResourceType.Gems, 1) },
            };
        }
    }

    public class EmptyMovesIntoFullRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { },
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.EMPTY_MOVES_INTO_FULL, Permanent = false}}
            };
        }
    }

    public class SiegeRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Siege, 1) },
            };
        }
    }

    public class MagicRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Magic, 1) },
            };
        }
    }

    public class ArmyRewardReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Army, 1) },
            };
        }
    }

    public class ReturnMidAfterMovementReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.RETURN_TO_CENTER_ON_MOVEMENT, Permanent = false}} 
            };
        }
    }

    public class NoFractionMovementReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.NO_FRACTION_MOVEMENT, Permanent = true}}   
            };
        }
    }

    public class ReturnMidReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.RETURN_TO_CENTER}
            };
        }
    }

    public class RefreshMercenariesReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.REFRESH_MERCENARIES}
            };
        }
    }

    public class CummulativePointsReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                EndGameAura = new List<EndGameAuraType>() { EndGameAuraType.CUMMULATIVE_POINTS}
            };
        }
    }

    public class GetRewardFromCurrentTileReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.GET_REWARD_FROM_TILE}
            };
        }
    }

    public class TeleportToPortalReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.START_TELEPORT_MINI_PHASE}
            };
        }
    }

    public class TakeThreeArtifactsReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.TAKE_THREE_ARTIFACTS}
            };
        }
    }

    public class StartArtifactPickMiniPhaseReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.START_PICK_ARTIFACT_MINI_PHASE}
            };
        }
    }

    public class MoraleReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Morale = 1
            };
        }
    }

    public class SignetsIntoPointsReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                EndGameAura = new List<EndGameAuraType>() { EndGameAuraType.SIGNETS_INTO_POINTS}
            };
        }
    }

    public class MoraleAndSignetReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Morale = 2,
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Signet, 1) },
            };
        }
    }

    public class FulfillProphecyReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.FULFILL_PROPHECY}
            };
        }
    }

    public class LockMercenaryReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.LOCK_CARD}
            };
        }
    }

    public class BuffHeroReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.BUFF_HERO}
            };
        }
    }    

    public class TeleportationRewardOneFreeMovementReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.TELEPORTATION_REWARD_ONE_FREE_MOVEMENT, Permanent = true}}   
            };
        }
    }

    public class BuyCardsByAnyResourceReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.BLOCK_TILE, Permanent = true}}   
            };
        }
    }

    public class BlockTileReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity{Aura = AurasType.BLOCK_TILE, Permanent = true}}   
            };
        }
    }

    public class NoReward : IGetReward
    {
        public Reward OnReward()
        {
            return new Reward
            {
                
            };
        }
    }

    
    public static class RewardFactory
    {
        public static IGetReward GetReward(int id)
        {
            return id switch
            {
                1 => new ReturnMidReward(),
                2 => new ReturnMidAfterMovementReward(),
                3 => new EmptyMovesIntoFullRewardReward(),
                4 => new AllBasicsResourceReward(),
                5 => new EmptyMovementRewardReward(),
                6 => new FullMovementRewardReward(),
                7 => new ThreeGold(),
                8 => new FiveGold(),
                9 => new TeleportToPortalReward(),
                10 => new NoFractionMovementReward(),
                11 => new GetRewardFromCurrentTileReward(),
                12 => new CummulativePointsReward(),
                13 => new RefreshMercenariesReward(),
                14 => new MoraleReward(),
                15 => new SiegeRewardReward(),
                16 => new ArmyRewardReward(),
                17 => new MagicRewardReward(),
                18 => new StartArtifactPickMiniPhaseReward(),
                19 => new TakeThreeArtifactsReward(),
                41 => new MoraleAndSignetReward(),
                42 => new BlockTileReward(),
                43 => new BuffHeroReward(),
                44 => new BuyCardsByAnyResourceReward(),
                45 => new TeleportationRewardOneFreeMovementReward(),
                46 => new SignetsIntoPointsReward(),
                47 => new SevenGold(),
                48 => new FulfillProphecyReward(),
                49 => new LockMercenaryReward(),
                85 => new NoReward(),
                _ => new StartArtifactPickMiniPhaseReward()
            };
        }
    }
}