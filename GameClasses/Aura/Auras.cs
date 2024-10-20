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
        FULL_MOVEMENT_INTO_EMPTY,
        GOLD_FOR_MOVEMENT,
        TEMPORARY_SIGNET
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