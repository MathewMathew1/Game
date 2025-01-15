namespace BoardGameBackend.Models
{
    public enum EffectType
    {
        RETURN_TO_CENTER,
        REFRESH_MERCENARIES,
        GET_REWARD_FROM_TILE,
        START_TELEPORT_MINI_PHASE,
        TAKE_THREE_ARTIFACTS,
        START_PICK_ARTIFACT_MINI_PHASE,
        START_PICK_ARTIFACTS_MINI_PHASE,
        FULFILL_PROPHECY,
        LOCK_CARD,
        BUFF_HERO,
        REROLL_MERCENARY,
        GET_RANDOM_ARTIFACT,
        REPLAY_ARTIFACT,
        REPLACE_HERO,
        GET_THREE_RANDOM_ARTIFACTS,
        GOLD_FOR_PROPHECY,
        BANISH_ROYAL_CARD,
        SWAP_TOKENS,
        GOLD_FOR_BUILDINGS,
        ROTATE_PAWN,
        DUEL_MAGIC,
        DUEL_SIEGE,
        DUEL_ARMY,
        OPTIONAL_DISCARD_ARTIFACT_FOR_FULL_MOVE,
        SUMMON_DRAGON,
        PAWN_BLINK_ONE_TILE,
        PICK_TWO_DRAGONS_SUMMON_ONE,
        GET_TWO_RANDOM_ARTIFACTS,
        TWO_FULL_MOVES_NOW,
    }

    public class GameEffect
    {
        public required int Id { get; set; }
        public required string Type { get; set; }
        public required string TextDescription { get; set; }
        public required int Req { get; set; }
        public required int EffectType { get; set; }
        public required int IntValue1 { get; set; }
        public required int IntValue2 { get; set; }
    }
}