namespace BoardGameBackend.Models
{
    public class FullGameData{
        public required int Turn {get;set;}
        public required int Round {get;set;}
        public required PhaseType CurrentPhase {get;set;}
        public MiniPhaseType? CurrentMiniPhase {get;set;}
        public required MercenaryData MercenaryData { get; set; }
        public required List<FullPlayerData> PlayersData {get; set; }
        public required string GameId {get; set;}
        public required List<TokenTileInfo> TokenSetup {get; set;}
        public required List<RolayCard> RoyalCards {get;set;}
        public required List<HeroCardCombined> HeroCards {get;set;}
        public required ArtifactInfo ArtifactInfo {get; set;}
        public required List<Player> PlayerBasedOnMorales {get; set;}
        public required int PawnTilePosition {get; set;}
        public required List<Artifact> ArtifactsToPickFrom  {get; set;}
        public required Guid CurrentPlayerId {get; set;}
    }

    public class FullPlayerData{
        public required Player Player {get;set;}
        public required List<AuraTypeWithLongevity> Auras {get;set;}
        public required Dictionary<ResourceType, int> Resources {get;set;}
        public required Dictionary<ResourceHeroType, int> ResourceHero {get;set;}
        public required List<Mercenary> Mercenaries {get;set;}
        public required ArtifactPlayerData Artifacts {get;set;}
        public required RoyalCardsPlayerData RoyalCardsData {get;set;}
        public required PlayerHeroData Heroes {get;set;}
        public required List<TokenFromJson> Tokens {get;set;}
        public required int Morale {get; set;}
        public required int GoldIncome {get; set;}
        public required bool AlreadyPlayedTurn {get; set;}
    }
}