namespace BoardGameBackend.Models
{  
    public enum AurasType
    {
        RETURN_TO_CENTER_ON_MOVEMENT,
        ONE_EMPTY_MOVEMENT,
        ONE_FULL_MOVEMENT,
        EMPTY_MOVES_INTO_FULL,
        NO_FRACTION_MOVEMENT,
        FULFILL_PROPHECY,
        TELEPORTATION_REWARD_ONE_FREE_MOVEMENT,
        BUY_CARDS_BY_ANY_RESOURCE,
        BLOCK_TILE,
        ADD_ADDITIONAL_MOVEMENT_BASED_ON_FRACTION,
        MAKE_CHEAPER_MERCENARIES,
        GOLD_ON_TILES_WITHOUT_GOLD,
        GOLD_ON_TILES_WITH_IRON,
        GOLD_ON_TILES_WITH_WOOD,
        GOLD_ON_TILES_WITH_NITER,
        GOLD_ON_TILES_WITH_MYSTIC_FOG,
        GOLD_ON_TILES_WITH_GEMS,
        GOLD_ON_TILES_WITH_REROLL,
        GOLD_ON_TILES_WITH_ARTIFACT,
        GOLD_ON_TILES_WITH_SIGNET,
        EMPTY_MOVE_ON_TILES_WITH_SIGNET,
        ARTIFACT_ON_ROYAL_CARD,
        FULL_MOVEMENT_INTO_EMPTY,
        GOLD_FOR_MOVEMENT,
        TEMPORARY_SIGNET,
        ARTIFACT_WHEN_CLOSE_TO_CASTLE,
        FULL_MOVE_WHEN_CLOSE_TO_CASTLE,
        EMPTY_MOVE_WHEN_CLOSE_TO_CASTLE,
        CHEAPER_BUILDINGS,
        CHANGE_SIDES_OF_HERO_AFTER_PLAY,
        REPLACE_NEXT_HERO
    }

    public enum EndGameAuraType
    {
        CUMMULATIVE_POINTS,   
        SIGNETS_INTO_POINTS   
    }

    public class AuraTypeWithLongevity{
        public AurasType Aura {get; set;}
        public int? Value1 {get; set;}
        public bool Permanent {get; set;}
    }
}