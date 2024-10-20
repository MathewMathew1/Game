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
        REROLL_MERCENARY
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