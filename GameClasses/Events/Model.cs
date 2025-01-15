namespace BoardGameBackend.Models
{
    public class HeroCardPicked
    {
        public HeroCard Card { get; set; }
        public PlayerInGame Player { get; set; }
        public CurrentHeroCard CurrentHeroCard { get; set; }
        public Reward? Reward { get; set; }

        public HeroCardPicked(HeroCard card, PlayerInGame player, CurrentHeroCard currentHeroCard, Reward? reward = null)
        {
            Card = card;
            Player = player;
            Reward = reward;
            CurrentHeroCard = currentHeroCard;
        }
    }

    public class MercenariesLeftData
    {
        public required int TossedMercenariesAmount { get; set; }
        public required int MercenariesAmount { get; set; }
    }

    public class EndOfRoundMercenaryData
    {
        public required List<Mercenary> Mercenaries { get; set; }
        public required MercenariesLeftData MercenariesLeftData { get; set; }
    }

    public class EndOfRoundData
    {
        public required EndOfRoundMercenaryData EndOfRoundMercenaryData { get; set; }
    }

    public class MoraleAdded
    {
        public required PlayerInGame Player { get; set; }
    }

    public class MercenaryPicked
    {
        public Reward? Reward { get; set; }
        public required Mercenary Card { get; set; }
        public required PlayerInGame Player { get; set; }
        public required List<ResourceInfo> ResourcesSpend { get; set; }
        public required Mercenary? MercenaryReplacement { get; set; }
        public required MercenariesLeftData MercenariesLeftData { get; set; }
    }

    public class DragonAcquired
    {
        public Reward? Reward { get; set; }
        public required Dragon Card { get; set; }
        public required PlayerInGame Player { get; set; }
        public required int MovementFullLeft {get; set; }
        public required bool UsedToken {get; set;}
    }

    public class DragonSummonData
    {
        public required Dragon Card { get; set; }
    }
    public class DragonPickData
    {
        public required List<Dragon> Cards { get; set; }
    }
    
    public class DragonSummonEventData{
        public required int TileId {get; set;}
        public required Guid PlayerId {get; set;}
        public required TokenFromJson Token {get; set;}
    }

    public class MercenaryRerolled
    {
        public required Mercenary Card { get; set; }
        public required MercenariesLeftData MercenariesLeftData { get; set; }
    }

    public class PreMercenaryRerolled
    {
        public required Mercenary? MercenaryReplacement { get; set; }
        public required MercenariesLeftData MercenariesLeftData { get; set; }
    }

    public class DummyPhaseStarted
    {
        public required PlayerInGame Player { get; set; }
    }

    public class HeroTurnEnded
    {
        public required PlayerInGame Player { get; set; }
    }

    public class ArtifactPhaseSkipped
    {
        public required Guid PlayerId { get; set; }
    }

    public class NewCardsSetupData
    {
        public required PlayerInGame Player { get; set; }
        public required int TurnCount { get; set; }
        public required List<HeroCardCombined> NewCards { get; set; }
        public required int RoundCount { get; set; }
    }

    public class MoveOnTile
    {
        public required TileReward TileReward { get; set; }
        public required PlayerInGame Player { get; set; }
        public int MovementFullLeft { get; set; }
        public int MovementUnFullLeft { get; set; }
        public int TileId { get; set; }
        public required TileWithType Tile { get; set; }
        public bool AdjacentMovement { get; set; }
    }

    public class MoveOnTileOnEvent
    {
        public required AurasType? AuraUsed { get; set; }
        public required PlayerInGame Player { get; set; }
        public int TileId { get; set; }
    }

    public class NewTokensSetupEventData{
        public required List<TokenTileInfo> NewTokens {get; set;}
    }

    public class MoveOnTileForOtherUsers
    {
        public required TileRewardForOtherUsers TileReward { get; set; }
        public required PlayerInGame Player { get; set; }
        public int MovementFullLeft { get; set; }
        public int MovementUnFullLeft { get; set; }
        public int TileId { get; set; }
        public bool AdjacentMovement { get; set; }
    }


    public class ArtifactToPickFromData
    {
        public required int ArtifactsLeft { get; set; }
        public required int ArtifactsLeftTossed { get; set; }
        public required List<Artifact> Artifacts { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class ArtifactToPickFromDataForOtherUsers
    {
        public required int ArtifactsLeft { get; set; }
        public required int ArtifactsLeftTossed { get; set; }
        public required int ArtifactsAmount { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class ArtifactsTaken
    {
        public required int ArtifactsLeft { get; set; }
        public required int ArtifactsLeftTossed { get; set; }
        public required List<Artifact> Artifacts { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class ArtifactsTakenDataForOtherUsers
    {
        public required int ArtifactsLeft { get; set; }
        public required int ArtifactsLeftTossed { get; set; }
        public required int ArtifactsAmount { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class TileRewardForOtherUsers
    {
        public List<Resource> Resources { get; set; }
        public int? TeleportedTileId { get; set; }
        public bool? RerollMercenaryAction { get; set; }
        public bool? GetRandomArtifact { get; set; }
        public bool? GotArtifact { get; set; }
        public TokenReward? TokenReward { get; set; }
        public bool Dragon { get; set; } = false;
    }

    public class ArtifactPlayed
    {
        public required Artifact Artifact { get; set; }
        public required PlayerInGame Player { get; set; }
        public required bool FirstEffect { get; set; }
        public required Reward Reward { get; set; }
    }

    public class ArtifactRerolledData
    {
        public required Artifact Artifact { get; set; }
        public required PlayerInGame Player { get; set; }
        public required Artifact ArtifactRerolled { get; set; }
    }

    public class ArtifactDiscardData
    {
        public required int ArtifactId { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class ArtifactRerolledDataForOtherUsers
    {
        public required PlayerInGame Player { get; set; }
    }

    public class BuyableMercenariesRefreshed
    {
        public List<Mercenary> NewBuyableMercenaries { get; set; }
        public MercenariesLeftData MercenariesLeftData { get; set; }
    }

    public class GetCurrentTileReward
    {
        public required TileReward TileReward { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class GetCurrentTileRewardForOtherUsers
    {
        public required TileRewardForOtherUsers TileReward { get; set; }
        public required PlayerInGame Player { get; set; }
    }

    public class TeleportationData
    {
        public required PlayerInGame Player { get; set; }
        public int TileId { get; set; }
    }

    public class EndOfGame
    {
        public required Dictionary<Guid, ScorePointsTable> PlayerScores { get; set; }
        public required Dictionary<Guid, TimeSpan> PlayerTimeSpan { get; set; }
        public TimeSpan GameTimeSpan { get; set; }
    }

    public class NewTurnEventData
    {
        public required PlayerInGame Player { get; set; }
        public required PlayerInGame TimeSpendByLastPlayer { get; set; }
    }

    public class StartOfGame
    {
        public required MercenaryData MercenaryData { get; set; }
        public required List<PlayerViewModel> Players { get; set; }
        public required string GameId { get; set; }
        public required List<TokenTileInfo> TokenSetup { get; set; }
        public required List<RolayCard> RolayCards { get; set; }
        public required bool Signets25914 {get; set; } 
    }

    public class FulfillProphecy
    {
        public required int MercenaryId { get; set; }
        public required Guid PlayerId { get; set; }
    }

    public class AddAura
    {
        public AuraTypeWithLongevity Aura { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class LockMercenaryData
    {
        public required LockedByPlayerInfo LockMercenary { get; set; }
        public required int MercenaryId { get; set; }
    }

    public class BuffHeroData
    {
        public required Guid PlayerId { get; set; }
        public required int HeroId { get; set; }
        public required Dictionary<ResourceHeroType, int> HeroResourcesNew { get; set; }
    }

    public class MiniPhaseDataWithDifferentPlayer
    {
        public required Guid PlayerId { get; set; }
    }

    public class BlockedTileData
    {
        public required Guid PlayerId { get; set; }
        public required int TileId { get; set; }
        public required TokenFromJson Token { get; set; }
    }

    public class EndOfPlayerTurn
    {
        public required PlayerInGame Player { get; set; }
    }

    public class RoyalCardPlayed
    {
        public required RolayCard RoyalCard { get; set; }
        public required Reward? Reward { get; set; }
        public required Guid PlayerId { get; set; }
        public required int AmountOfSignetsForNextRoyalCard { get; set; }
    }

    public class GoldIntoMovementEventData
    {
        public required int MovementFullLeft { get; set; }
        public required int GoldLeft { get; set; }
        public required Guid PlayerId { get; set; }
    }

    public class FullMovementIntoEmptyEventData
    {
        public required int MovementFullLeft { get; set; }
        public required int MovementUnFullLeft { get; set; }
        public required Guid PlayerId { get; set; }
    }

    public class ReplaceNextHeroEventData
    {
        public required HeroCard Hero { get; set; }
        public required AuraTypeWithLongevity ReplacementHeroAura { get; set; }
        public required Guid PlayerId { get; set; }
    }

    public class PreHeroCardPickedEventData
    {
        public required Guid PlayerId { get; set; }
        public required HeroCard HeroCard { get; set; }
        public required bool WasOnLeftSide { get; set; }
        public ReplacedHero? ReplacedHero { get; set; }
    }

    public class EndOfTurnEventData
    {
        public required int TurnCount { get; set; }
    }

    public class ResourceReceivedEventData
    {
        public required List<Resource> Resources { get; set; }
        public required string ResourceInfo { get; set; }
        public required Guid PlayerId {get; set;}
    }

    public class BanishRoyalCardEventData
    {
        public required RolayCard RoyalCard { get; set; }
        public required Guid PlayerId {get; set;}
    }

    public class SwapTokensDataEventData
    {
        public required int TileOneId { get; set; }
        public required int TileTwoId { get; set; }
        public required Guid PlayerId {get; set;}
    }

    public class ResourceSpendEventData{
        public required int ResourceSpend {get; set;}
        public required int ResourceLeft {get; set;}
        public required ResourceType ResourceType {get; set;}
        public required Guid PlayerId {get; set;}
    }

    public class RotateTileEventData{
        public required int TileId {get; set;}
        public required Guid PlayerId {get; set;}
    }
}
