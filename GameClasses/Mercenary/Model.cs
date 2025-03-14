namespace BoardGameBackend.Models
{
    public class MercenaryFromJson
    {
        public int? Req {get; set;}
        public required int Id { get; set; }
        public required string NameEng { get; set; }
        public required int Siege { get; set; }
        public required int Magic { get; set; }
        public required int Army { get; set; }
        public required int TypeCard { get; set; }
        public required int ShuffleX { get; set; }
        public required int Morale { get; set; }
        public required int IncomeGold { get; set; }
        public required int Faction { get; set; }
        public required List<ResourceJsonInfo> ResourcesNeeded { get; set; }
        public required string BackgroundAtlas { get; set; }
        public required int BackgroundIndex { get; set; }
        public required int ScorePoints { get; set; }
        public string? EffectIconAtlas { get; set; }
        public int? EffectIconIndex { get; set; }
        public int? EffectId { get; set; }
        public string? ToolTipText {get; set;}
        public int? EffectType { get; set; }
        public required bool DragonDLC {get; set;}
    }

    public class ResourceJsonInfo
    {
        public required int ResourceId { get; set; }
        public required int Amount { get; set; }
    }

    public class ResourceInfo
    {
        public required ResourceType Name { get; set; }
        public required int Amount { get; set; }
    }

    public class Mercenary
    {
        public int? Req {get; set;}
        public required int Id { get; set; }
        public required string NameEng { get; set; }
        public required int Siege { get; set; }
        public required int Magic { get; set; }
        public required int Army { get; set; }
        public required int TypeCard { get; set; }
        public required int Morale { get; set; }
        public required int IncomeGold { get; set; }
        public required Fraction Faction { get; set; }
        public int InGameIndex { get; set; }
        public required List<ResourceInfo> ResourcesNeeded { get; set; }
        public int GoldDecrease { get; set; } = 0;
        public required string BackgroundAtlas { get; set; }
        public required int BackgroundIndex { get; set; }
        public required int ScorePoints { get; set; }
        public bool IsAlwaysFulfilled {get; set;} = false;
        public string? EffectIconAtlas { get; set; }
        public int? EffectIconIndex { get; set; }
        public int? EffectId { get; set; }
        public int? EffectType { get; set; }
        public LockedByPlayerInfo? LockedByPlayerInfo {get; set;}
        public string? ToolTipText {get; set;}
        public required bool DragonDLC {get; set;}
        public required int ShuffleX { get; set; }
    }

    public class MercenaryData
    {
        public required List<Mercenary> BuyableMercenaries { get; set; }
        public int RemainingMercenariesAmount { get; set; }
        public required int TossedMercenariesAmount {get; set;}
    }

    public class DragonFullData
    {
        public Dragon? CurrentlySummonedDragon {get; set; }        
        public List<Dragon>? DragonsToPick {get; set; }    
    }

    public class MercenaryFulfillProphecyReturnData
    {
        public int? MercenaryId {get; set;}
        public AurasType? aurasType {get; set;}
    }
}