namespace BoardGameBackend.Models
{
    public class HeroCardPicked
    {
        public HeroCard Card { get; }
        public PlayerInGame Player { get; }

        public HeroCardPicked(HeroCard card, PlayerInGame player)
        {
            Card = card;
            Player = player;
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

    public class MercenaryPicked
    {
        public required Mercenary Card { get; set; }
        public required PlayerInGame Player { get; set; }
        public required List<ResourceInfo> ResourcesSpend { get; set; }
        public required Mercenary MercenaryReplacement { get; set; }
        public required MercenariesLeftData MercenariesLeftData { get; set; }
    }

    public class MercenaryRerolled
    {
        public required Mercenary Card { get; set; }
        public required Mercenary MercenaryReplacement { get; set; }
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

    public class ArtifactPhaseStarted
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
    }

    public class MoveOnTileOnEvent
    {
        public required AurasType? AuraUsed { get; set; }
        public required PlayerInGame Player { get; set; }
        public int TileId { get; set; }
    }

    public class MoveOnTileForOtherUsers
    {
        public required TileRewardForOtherUsers TileReward { get; set; }
        public required PlayerInGame Player { get; set; }
        public int MovementFullLeft { get; set; }
        public int MovementUnFullLeft { get; set; }
        public int TileId { get; set; }
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
        public int? ExperiencePoints { get; set; }
        public bool? RerollMercenaryAction { get; set; }
        public bool? GetRandomArtifact { get; set; }
        public bool? GotArtifact { get; set; }
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
        public required Artifact ArtifactRerolled { get; set;}
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

    public class EndOfGame{
        public required Dictionary<Guid, ScorePointsTable> PlayerScores {get; set;}
    }

    public class StartOfGame{
        public required MercenaryData MercenaryData { get; set; }
        public required List<PlayerViewModel> Players {get; set; }
        public required string GameId {get; set;}
        public required List<TokenTileInfo> TokenSetup {get; set;}
    }

    public class FulfillProphecy{
        public required int MercenaryId { get; set; }
        public required Guid PlayerId {get; set; }    
    }

    public class AddAura{
        public AuraTypeWithLongevity Aura {get; set;}
        public Guid PlayerId {get; set;}
    }

    public class LockMercenaryData{
        public required LockedByPlayerInfo LockMercenary {get; set;}
        public required int MercenaryId {get; set;}
    }

    public class BuffHeroData{
        public required Guid PlayerId {get; set;}
        public required int HeroId {get; set;}
        public required Dictionary<ResourceHeroType, int> HeroResourcesNew {get; set;}
    }

    public class MiniPhaseDataWithDifferentPlayer{
        public required Guid PlayerId {get; set;}
    }

    public class BlockedTileData{
        public required Guid PlayerId {get; set;}
        public required int TileId {get; set;}
        public required TokenFromJson Token {get; set;}
    }

}
