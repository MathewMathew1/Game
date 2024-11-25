using System.Text.Json;
using BoardGameBackend.Managers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Models
{


    public class Reward
    {
        public List<Resource> Resources { get; set; }
        public List<HeroResource> HeroResources { get; set; }
        public List<AuraTypeWithLongevity> AurasTypes { get; set; }
        public List<EndGameAura> EndGameAura { get; set; }
        public List<EffectType> Effects { get; set; }
        public bool EmptyReward { get; set; } = false;

        public int? Morale { get; set; }


        public Reward()
        {
            Resources = new List<Resource>();
            AurasTypes = new List<AuraTypeWithLongevity>();
            HeroResources = new List<HeroResource>();
            Effects = new List<EffectType>();
            EndGameAura = new List<EndGameAura>();
        }
    }

    public interface IGetReward
    {
        Reward OnReward();
    }

    public abstract class BaseReward : IGetReward
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }

        public BaseReward(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public abstract Reward OnReward();
    }

    public class ThreeGold : BaseReward
    {
        public ThreeGold(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, Value1) },
            };
        }
    }

    public class MarketGold : BaseReward
    {
        public MarketGold(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, Value1) },
            };
        }
    }

    public class FiveGold : BaseReward
    {
        public FiveGold(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, Value1) },
            };
        }
    }

    public class SevenGold : BaseReward
    {
        public SevenGold(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, Value1) },
            };
        }
    }

    public class DefaultRewardReward : BaseReward
    {
        public DefaultRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 5) },
            };
        }
    }

    public class FullMovementRewardReward : BaseReward
    {
        public FullMovementRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { },
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.ONE_FULL_MOVEMENT, Permanent = false } }
            };
        }
    }

    public class EmptyAndFullMovementReward : BaseReward
    {
        public EmptyAndFullMovementReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.ONE_FULL_MOVEMENT, Permanent = false },
                    new AuraTypeWithLongevity { Aura = AurasType.ONE_EMPTY_MOVEMENT, Permanent = false }
                }
            };
        }
    }

    public class GoldWhenNoGoldReward : BaseReward
    {
        public GoldWhenNoGoldReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { },
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.GOLD_WHEN_NO_GOLD, Permanent = true } }
            };
        }
    }

    public class EmptyMovementRewardReward : BaseReward
    {
        public EmptyMovementRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.ONE_EMPTY_MOVEMENT, Permanent = false } }
            };
        }
    }

    public class AdjacentTileAuraReward : BaseReward
    {
        public AdjacentTileAuraReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.ADJACENT_TILE_REWARD, Permanent = false } }
            };
        }
    }

    public class EmptyMovementOnHeroWithSignetReward : BaseReward
    {
        public EmptyMovementOnHeroWithSignetReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVEMENT_WHEN_HERO_HAS_SIGNET, Permanent = true } }
            };
        }
    }

    public class EmptyMovementOnHeroWithMoraleReward : BaseReward
    {
        public EmptyMovementOnHeroWithMoraleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVEMENT_WHEN_HERO_HAS_MORALE, Permanent = true } }
            };
        }
    }


    public class EmptyMovementOnHeroWithNoEmptyMovementReward : BaseReward
    {
        public EmptyMovementOnHeroWithNoEmptyMovementReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVEMENT_WHEN_HERO_HAS_EMPTY_MOVEMENT, Permanent = true } }
            };
        }
    }

    public class GoldWhenCloseToCastleReward : BaseReward
    {
        public GoldWhenCloseToCastleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.GOLD_WHEN_CLOSE_TO_CASTLE, Permanent = true } }
            };
        }
    }

    public class AllBasicsResourceReward : BaseReward
    {
        public AllBasicsResourceReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
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

    public class EmptyMovesIntoFullRewardReward : BaseReward
    {
        public EmptyMovesIntoFullRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> { },
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVES_INTO_FULL, Permanent = false } }
            };
        }
    }

    public class SiegeRewardReward : BaseReward
    {
        public SiegeRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Siege, 1) },
            };
        }
    }

    public class MagicRewardReward : BaseReward
    {
        public MagicRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Magic, 1) },
            };
        }
    }

    public class ArmyRewardReward : BaseReward
    {
        public ArmyRewardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Army, 1) },
            };
        }
    }

    public class ReturnMidAfterMovementReward : BaseReward
    {
        public ReturnMidAfterMovementReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.RETURN_TO_CENTER_ON_MOVEMENT, Permanent = false } }
            };
        }
    }

    public class NoFractionMovementReward : BaseReward
    {
        public NoFractionMovementReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.NO_FRACTION_MOVEMENT, Permanent = false } }
            };
        }
    }

    public class ReturnMidReward : BaseReward
    {
        public ReturnMidReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.RETURN_TO_CENTER }
            };
        }
    }

    public class RotatePawnReward : BaseReward
    {
        public RotatePawnReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.ROTATE_PAWN }
            };
        }
    }

    public class SwapTokensReward : BaseReward
    {
        public SwapTokensReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.SWAP_TOKENS }
            };
        }
    }

    public class BanishRoyalCardReward : BaseReward
    {
        public BanishRoyalCardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.BANISH_ROYAL_CARD }
            };
        }
    }


    public class GoldForEachProphecy : BaseReward
    {
        public GoldForEachProphecy(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.GOLD_FOR_PROPHECY }
            };
        }
    }

    public class GetThreeArtifactsReward : BaseReward
    {
        public GetThreeArtifactsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.GET_THREE_RANDOM_ARTIFACTS }
            };
        }
    }

    public class RefreshMercenariesReward : BaseReward
    {
        public RefreshMercenariesReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.REFRESH_MERCENARIES }
            };
        }
    }

    public class CummulativePointsReward : BaseReward
    {
        public CummulativePointsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                EndGameAura = new List<EndGameAura>() { new EndGameAura { Aura = EndGameAuraType.CUMMULATIVE_POINTS, Value1 = Value1 } }
            };
        }
    }

    public class ThreePointsReward : BaseReward
    {
        public ThreePointsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                EndGameAura = new List<EndGameAura>() { new EndGameAura { Aura = EndGameAuraType.THREE_POINTS, Value1 = Value1 } }
            };
        }
    }

    public class GetRewardFromCurrentTileReward : BaseReward
    {
        public GetRewardFromCurrentTileReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.GET_REWARD_FROM_TILE }
            };
        }
    }

    public class TeleportToPortalReward : BaseReward
    {
        public TeleportToPortalReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.START_TELEPORT_MINI_PHASE }
            };
        }
    }

    public class TakeThreeArtifactsReward : BaseReward
    {
        public TakeThreeArtifactsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.TAKE_THREE_ARTIFACTS }
            };
        }
    }

    public class StartArtifactPickMiniPhaseReward : BaseReward
    {
        public StartArtifactPickMiniPhaseReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.START_PICK_ARTIFACT_MINI_PHASE }
            };
        }
    }

    public class StartArtifactsPickMiniPhaseReward : BaseReward
    {
        public StartArtifactsPickMiniPhaseReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.START_PICK_ARTIFACTS_MINI_PHASE }
            };
        }
    }

    public class MoraleReward : BaseReward
    {
        public MoraleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Morale = 1
            };
        }
    }

    public class SignetsIntoPointsReward : BaseReward
    {
        public SignetsIntoPointsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                EndGameAura = new List<EndGameAura>() { new EndGameAura { Aura = EndGameAuraType.SIGNETS_INTO_POINTS, Value1 = Value1 } }
            };
        }
    }

    public class PointsPerMercenaryOfFactionReward : BaseReward
    {
        public PointsPerMercenaryOfFactionReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                EndGameAura = new List<EndGameAura>() { new EndGameAura { Aura = EndGameAuraType.POINTS_OF_MERCENARY_OF_FACTION, Value1 = Value1 } }
            };
        }
    }

    public class MoraleAndSignetReward : BaseReward
    {
        public MoraleAndSignetReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Morale = 2,
                HeroResources = new List<HeroResource> { new HeroResource(ResourceHeroType.Signet, 1) },
            };
        }
    }

    public class FulfillProphecyReward : BaseReward
    {
        public FulfillProphecyReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.FULFILL_PROPHECY }
            };
        }
    }

    public class LockMercenaryReward : BaseReward
    {
        public LockMercenaryReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.LOCK_CARD }
            };
        }
    }

    public class ReplaceNextHeroCardReward : BaseReward
    {
        public ReplaceNextHeroCardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.REPLACE_HERO }
            };
        }
    }

    public class RerollMercenaryReward : BaseReward
    {
        public RerollMercenaryReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.REROLL_MERCENARY }
            };
        }
    }

    public class BuffHeroReward : BaseReward
    {
        public BuffHeroReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.BUFF_HERO }
            };
        }
    }

    public class GetRandomArtifactReward : BaseReward
    {
        public GetRandomArtifactReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.GET_RANDOM_ARTIFACT }
            };
        }
    }

    public class ReplayArtifactReward : BaseReward
    {
        public ReplayArtifactReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.REPLAY_ARTIFACT }
            };
        }
    }

    public class GetGoldForBuildingsReward : BaseReward
    {
        public GetGoldForBuildingsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.GOLD_FOR_BUILDINGS }
            };
        }
    }

    public class DuelMagicReward : BaseReward
    {
        public DuelMagicReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.DUEL_MAGIC }
            };
        }
    }

    public class DuelSiegeReward : BaseReward
    {
        public DuelSiegeReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.DUEL_SIEGE }
            };
        }
    }

    public class DuelArmyReward : BaseReward
    {
        public DuelArmyReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Effects = new List<EffectType>() { EffectType.DUEL_ARMY }
            };
        }
    }

    public class TeleportationRewardOneFreeMovementReward : BaseReward
    {
        public TeleportationRewardOneFreeMovementReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.TELEPORTATION_REWARD_ONE_FREE_MOVEMENT, Permanent = true } }
            };
        }
    }

    public class TeleportationRewardOneFreeNotPermanentMovementReward : BaseReward
    {
        public TeleportationRewardOneFreeNotPermanentMovementReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.TELEPORTATION_REWARD_ONE_FREE_MOVEMENT, Permanent = false } }
            };
        }
    }

    public class BuyCardsByAnyResourceReward : BaseReward
    {
        public BuyCardsByAnyResourceReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.BUY_CARDS_BY_ANY_RESOURCE, Permanent = true } }
            };
        }
    }

    public class BlockTileReward : BaseReward
    {
        public BlockTileReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() { new AuraTypeWithLongevity { Aura = AurasType.BLOCK_TILE, Permanent = true } }
            };
        }
    }

    public class AdditionalMovementBasedOnFactionReward : BaseReward
    {
        public AdditionalMovementBasedOnFactionReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.ADD_ADDITIONAL_MOVEMENT_BASED_ON_FRACTION, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class CheaperMercenaryBasedOnFactionReward : BaseReward
    {
        public CheaperMercenaryBasedOnFactionReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.MAKE_CHEAPER_MERCENARIES, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithoutGoldReward : BaseReward
    {
        public GoldOnTilesWithoutGoldReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITHOUT_GOLD, Permanent = false, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithIronReward : BaseReward
    {
        public GoldOnTilesWithIronReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_IRON, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithWoodReward : BaseReward
    {
        public GoldOnTilesWithWoodReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_WOOD, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }


    public class GoldOnTilesWithGemsReward : BaseReward
    {
        public GoldOnTilesWithGemsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_GEMS, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }


    public class GoldOnTilesWithNiterReward : BaseReward
    {
        public GoldOnTilesWithNiterReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_NITER, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithMysticFogReward : BaseReward
    {
        public GoldOnTilesWithMysticFogReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_MYSTIC_FOG, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithSignetFogReward : BaseReward
    {
        public GoldOnTilesWithSignetFogReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_SIGNET, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithArtifactReward : BaseReward
    {
        public GoldOnTilesWithArtifactReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_ARTIFACT, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class GoldOnTilesWithRerollReward : BaseReward
    {
        public GoldOnTilesWithRerollReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILES_WITH_REROLL, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class EmptyMoveOnTilesWithSignetReward : BaseReward
    {
        public EmptyMoveOnTilesWithSignetReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVE_ON_TILES_WITH_SIGNET, Permanent = true, Value1 = Value1 }
                }
            };
        }
    }

    public class WoodReward : BaseReward
    {
        public WoodReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Wood, 1)
                }
            };
        }
    }

    public class IronReward : BaseReward
    {
        public IronReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Iron, 1)
                }
            };
        }
    }

    public class GemsReward : BaseReward
    {
        public GemsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Gems, 1)
                }
            };
        }
    }

    public class NiterReward : BaseReward
    {
        public NiterReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Niter, 1)
                }
            };
        }
    }

    public class MysticFogReward : BaseReward
    {
        public MysticFogReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.MysticFog, 1)
                }
            };
        }
    }

    public class WoodAndNiterReward : BaseReward
    {
        public WoodAndNiterReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Wood, 1),
                new Resource(ResourceType.Niter, 1)
                }
            };
        }
    }

    public class IronAndWoodReward : BaseReward
    {
        public IronAndWoodReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Iron, 1),
                new Resource(ResourceType.Wood, 1)
                }
            };
        }
    }

    public class GemsAndIronReward : BaseReward
    {
        public GemsAndIronReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Gems, 1),
                new Resource(ResourceType.Iron, 1)
                }
            };
        }
    }

    public class NiterAndGemsReward : BaseReward
    {
        public NiterAndGemsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                Resources = new List<Resource> {
                new Resource(ResourceType.Niter, 1),
                new Resource(ResourceType.Gems, 1)
                }
            };
        }
    }

    public class SignetsReward : BaseReward
    {
        public SignetsReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                HeroResources = new List<HeroResource> {
                new HeroResource(ResourceHeroType.Signet, 2)
                }
            };
        }
    }

    public class FullMovementIntoEmptyReward : BaseReward
    {
        public FullMovementIntoEmptyReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.FULL_MOVEMENT_INTO_EMPTY, Permanent = false }
                }
            };
        }
    }

    public class GoldIntoMovementEmptyReward : BaseReward
    {
        public GoldIntoMovementEmptyReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_FOR_MOVEMENT, Permanent = false }
                }
            };
        }
    }

    public class ArtifactWhenCloseToCastleReward : BaseReward
    {
        public ArtifactWhenCloseToCastleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.ARTIFACT_WHEN_CLOSE_TO_CASTLE, Permanent = true }
                }
            };
        }
    }

    public class FullMoveWhenCloseToCastleReward : BaseReward
    {
        public FullMoveWhenCloseToCastleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.FULL_MOVE_WHEN_CLOSE_TO_CASTLE, Permanent = true }
                }
            };
        }
    }

    public class EmptyMoveWhenCloseToCastleReward : BaseReward
    {
        public EmptyMoveWhenCloseToCastleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVE_WHEN_CLOSE_TO_CASTLE, Permanent = true }
                }
            };
        }
    }

    public class ArtifactOnRoyalCardReward : BaseReward
    {
        public ArtifactOnRoyalCardReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.ARTIFACT_ON_ROYAL_CARD, Permanent = true }
                }
            };
        }
    }

    public class CheaperBuildingsAuraReward : BaseReward
    {
        public CheaperBuildingsAuraReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.CHEAPER_BUILDINGS, Permanent = true }
                }
            };
        }
    }

    public class ChangeHeroSidesAfterPlayReward : BaseReward
    {
        public ChangeHeroSidesAfterPlayReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.CHANGE_SIDES_OF_HERO_AFTER_PLAY, Permanent = false }
                }
            };
        }
    }

    public class GoldOnTeleportReward : BaseReward
    {
        public GoldOnTeleportReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILE_TELEPORT, Permanent = true }
                }
            };
        }
    }

    public class EmptyMoveOnTpReward : BaseReward
    {
        public EmptyMoveOnTpReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVE_ON_TILES_WITH_TELEPORT, Permanent = true }
                }
            };
        }
    }

    public class EmptyMoveOnCastleReward : BaseReward
    {
        public EmptyMoveOnCastleReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.EMPTY_MOVE_ON_TILE_WITH_CASTLE, Permanent = true }
                }
            };
        }
    }

    public class ExtraArtifactRerollReward : BaseReward
    {
        public ExtraArtifactRerollReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.EXTRA_ARTIFACT_REROLL, Permanent = true }
                }
            };
        }
    }

    public class InstantWinDuelReward : BaseReward
    {
        public InstantWinDuelReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.INSTANT_WIN_DUEL, Permanent = false }
                }
            };
        }
    }

    public class GoldOnDuelTilesReward : BaseReward
    {
        public GoldOnDuelTilesReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                AurasTypes = new List<AuraTypeWithLongevity>() {
                    new AuraTypeWithLongevity { Aura = AurasType.GOLD_ON_TILE_DUEL, Permanent = true }
                }
            };
        }
    }


    public class NoReward : BaseReward
    {
        public NoReward(int value1, int value2) : base(value1, value2) { }

        public override Reward OnReward()
        {
            return new Reward
            {
                EmptyReward = true
            };
        }
    }


    public static class RewardFactory
    {
        private static readonly Dictionary<int, BaseReward> _rewards;

        static RewardFactory()
        {
            string filePath = "Data/SpecialEffect.json";
            List<ReqProphecies> rewardDataList = new List<ReqProphecies>();

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                rewardDataList = JsonSerializer.Deserialize<List<ReqProphecies>>(jsonData) ?? new List<ReqProphecies>();
            }

            _rewards = new Dictionary<int, BaseReward>();

            foreach (var rewardData in rewardDataList)
            {
                switch (rewardData.Id)
                {
                    case 1:
                        _rewards.Add(rewardData.Id, new ReturnMidReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 2:
                        _rewards.Add(rewardData.Id, new ReturnMidAfterMovementReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 3:
                        _rewards.Add(rewardData.Id, new EmptyMovesIntoFullRewardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 4:
                        _rewards.Add(rewardData.Id, new AllBasicsResourceReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 5:
                        _rewards.Add(rewardData.Id, new EmptyMovementRewardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 6:
                        _rewards.Add(rewardData.Id, new FullMovementRewardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 7:
                        _rewards.Add(rewardData.Id, new ThreeGold(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 8:
                        _rewards.Add(rewardData.Id, new FiveGold(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 9:
                        _rewards.Add(rewardData.Id, new TeleportToPortalReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 10:
                        _rewards.Add(rewardData.Id, new NoFractionMovementReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 11:
                        _rewards.Add(rewardData.Id, new GetRewardFromCurrentTileReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 12:
                        _rewards.Add(rewardData.Id, new CummulativePointsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 13:
                        _rewards.Add(rewardData.Id, new RefreshMercenariesReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 14:
                        _rewards.Add(rewardData.Id, new MoraleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 15:
                        _rewards.Add(rewardData.Id, new SiegeRewardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 16:
                        _rewards.Add(rewardData.Id, new ArmyRewardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 17:
                        _rewards.Add(rewardData.Id, new MagicRewardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 18:
                        _rewards.Add(rewardData.Id, new StartArtifactsPickMiniPhaseReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 19:
                        _rewards.Add(rewardData.Id, new TakeThreeArtifactsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 41:
                        _rewards.Add(rewardData.Id, new MoraleAndSignetReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 42:
                        _rewards.Add(rewardData.Id, new BlockTileReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 43:
                        _rewards.Add(rewardData.Id, new BuffHeroReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 44:
                        _rewards.Add(rewardData.Id, new BuyCardsByAnyResourceReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 45:
                        _rewards.Add(rewardData.Id, new TeleportationRewardOneFreeMovementReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 46:
                        _rewards.Add(rewardData.Id, new SignetsIntoPointsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 47:
                        _rewards.Add(rewardData.Id, new SevenGold(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 48:
                        _rewards.Add(rewardData.Id, new FulfillProphecyReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 49:
                        _rewards.Add(rewardData.Id, new LockMercenaryReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 50:
                        _rewards.Add(rewardData.Id, new WoodReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 51:
                        _rewards.Add(rewardData.Id, new IronReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 52:
                        _rewards.Add(rewardData.Id, new GemsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 53:
                        _rewards.Add(rewardData.Id, new NiterReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 54:
                        _rewards.Add(rewardData.Id, new MysticFogReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 55:
                        _rewards.Add(rewardData.Id, new RerollMercenaryReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 56:
                        _rewards.Add(rewardData.Id, new GetRandomArtifactReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 57:
                        _rewards.Add(rewardData.Id, new StartArtifactPickMiniPhaseReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 58:
                        _rewards.Add(rewardData.Id, new GetRewardFromCurrentTileReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 59:
                        _rewards.Add(rewardData.Id, new CheaperBuildingsAuraReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 60:
                        _rewards.Add(rewardData.Id, new FullMovementIntoEmptyReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 61:
                        _rewards.Add(rewardData.Id, new GoldIntoMovementEmptyReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 62:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithoutGoldReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 63:
                        _rewards.Add(rewardData.Id, new SignetsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 68:
                        _rewards.Add(rewardData.Id, new ReplayArtifactReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 69:
                        _rewards.Add(rewardData.Id, new ArtifactWhenCloseToCastleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 70:
                        _rewards.Add(rewardData.Id, new GoldWhenCloseToCastleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 71:
                        _rewards.Add(rewardData.Id, new FullMoveWhenCloseToCastleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 72:
                        _rewards.Add(rewardData.Id, new EmptyMoveWhenCloseToCastleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 73:
                        _rewards.Add(rewardData.Id, new ReplaceNextHeroCardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 74:
                        _rewards.Add(rewardData.Id, new SwapTokensReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 76:
                        _rewards.Add(rewardData.Id, new ChangeHeroSidesAfterPlayReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 77:
                        _rewards.Add(rewardData.Id, new AdditionalMovementBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 78:
                        _rewards.Add(rewardData.Id, new AdditionalMovementBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 79:
                        _rewards.Add(rewardData.Id, new AdditionalMovementBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 80:
                        _rewards.Add(rewardData.Id, new AdditionalMovementBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 81:
                        _rewards.Add(rewardData.Id, new CheaperMercenaryBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 82:
                        _rewards.Add(rewardData.Id, new CheaperMercenaryBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 83:
                        _rewards.Add(rewardData.Id, new CheaperMercenaryBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 84:
                        _rewards.Add(rewardData.Id, new CheaperMercenaryBasedOnFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 85:
                        _rewards.Add(rewardData.Id, new NoReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 86:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithArtifactReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 87:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithRerollReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 88:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithWoodReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 89:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithGemsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 90:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithIronReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 91:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithNiterReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 92:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithNiterReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 93:
                        _rewards.Add(rewardData.Id, new GoldOnTilesWithMysticFogReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 94:
                        _rewards.Add(rewardData.Id, new EmptyMoveOnTilesWithSignetReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 95:
                        _rewards.Add(rewardData.Id, new ArtifactOnRoyalCardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 96:
                        _rewards.Add(rewardData.Id, new TeleportationRewardOneFreeNotPermanentMovementReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 103:
                        _rewards.Add(rewardData.Id, new PointsPerMercenaryOfFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 104:
                        _rewards.Add(rewardData.Id, new PointsPerMercenaryOfFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 105:
                        _rewards.Add(rewardData.Id, new PointsPerMercenaryOfFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 106:
                        _rewards.Add(rewardData.Id, new PointsPerMercenaryOfFactionReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 107:
                        _rewards.Add(rewardData.Id, new EmptyMovementOnHeroWithSignetReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 108:
                        _rewards.Add(rewardData.Id, new EmptyMovementOnHeroWithMoraleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 109:
                        _rewards.Add(rewardData.Id, new EmptyMovementOnHeroWithNoEmptyMovementReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 110:
                        _rewards.Add(rewardData.Id, new GoldOnTeleportReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 111:
                        _rewards.Add(rewardData.Id, new BanishRoyalCardReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 112:
                        _rewards.Add(rewardData.Id, new GoldForEachProphecy(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 113:
                        _rewards.Add(rewardData.Id, new AdjacentTileAuraReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 114:
                        _rewards.Add(rewardData.Id, new GetThreeArtifactsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 115:
                        _rewards.Add(rewardData.Id, new ThreePointsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 116:
                        _rewards.Add(rewardData.Id, new GetGoldForBuildingsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 117:
                        _rewards.Add(rewardData.Id, new WoodAndNiterReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 118:
                        _rewards.Add(rewardData.Id, new IronAndWoodReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 119:
                        _rewards.Add(rewardData.Id, new GemsAndIronReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 120:
                        _rewards.Add(rewardData.Id, new NiterAndGemsReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 121:
                        _rewards.Add(rewardData.Id, new EmptyMoveOnTpReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 122:
                        _rewards.Add(rewardData.Id, new GoldWhenNoGoldReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 124:
                        _rewards.Add(rewardData.Id, new ExtraArtifactRerollReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 125:
                        _rewards.Add(rewardData.Id, new EmptyAndFullMovementReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 126:
                        _rewards.Add(rewardData.Id, new EmptyMoveOnCastleReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 123:
                        _rewards.Add(rewardData.Id, new RotatePawnReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 134:
                        _rewards.Add(rewardData.Id, new MarketGold(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 127:
                        _rewards.Add(rewardData.Id, new  GoldOnDuelTilesReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 128:
                        _rewards.Add(rewardData.Id, new InstantWinDuelReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 129:
                        _rewards.Add(rewardData.Id, new DuelMagicReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 130:
                        _rewards.Add(rewardData.Id, new DuelSiegeReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                    case 131:
                        _rewards.Add(rewardData.Id, new DuelArmyReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;
                       
                    case 132:
                        _rewards.Add(rewardData.Id, new InstantWinDuelReward(rewardData.IntValue1, rewardData.IntValue2));
                        break;



                }
            }
        }


        public static BaseReward GetRewardById(int id)
        {
            _rewards.TryGetValue(id, out var reward);
            return reward!;
        }

        public static IEnumerable<BaseReward> GetAllRewards()
        {
            return _rewards.Values;
        }
    }
}
