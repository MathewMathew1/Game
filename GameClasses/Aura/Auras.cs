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
        BLOCK_TILE
    }

    public enum EndGameAuraType
    {
        CUMMULATIVE_POINTS,   
        SIGNETS_INTO_POINTS   
    }

    public class AuraTypeWithLongevity{
        public AurasType Aura {get; set;}
        public bool Permanent {get; set;}
    }
}