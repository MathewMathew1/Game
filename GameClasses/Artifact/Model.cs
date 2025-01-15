namespace BoardGameBackend.Models
{
    public class ArtifactFromJson
    {
        public required int Id { get; set; }
        public required string NameEng { get; set; }
        public required string NamePL { get; set; }
        public required string TextPL { get; set; }
        public required int ShuffleX { get; set; }
        public required string EffectIconAtlas { get; set; }
        public required int EffectIconIndex { get; set; }
        public required int Effect1 { get; set; }
        public required int Effect2 { get; set; }
        public required string BackgroundAtlas { get; set; }
        public required int BackgroundIndex { get; set; }
        public required int EffectType { get; set; }
        public required bool SecondEffectSuperior { get; set; }
        public required bool DragonDLC{get; set;}
    }

    public class Artifact
    {
        public required int Id { get; set; }
        public required string NameEng { get; set; }
        public required string NamePL { get; set; }
        public required string TextPL { get; set; }
        public required int ShuffleX { get; set; }
        public required string EffectIconAtlas { get; set; }
        public required int EffectIconIndex { get; set; }
        public required int Effect1 { get; set; }
        public required int Effect2 { get; set; }
        public int InGameIndex { get; set; }
        public required string BackgroundAtlas { get; set; }
        public required int BackgroundIndex { get; set; }
        public required int EffectType { get; set; }
        public required bool SecondEffectSuperior { get; set; }
        public required bool DragonDLC{get; set;}
        
    }

    public class LockedByPlayerInfo{
        public required Guid PlayerId { get; set;}
        public required string PlayerName { get; set;}
    }

    public class ArtifactInfo{
        public required List<Artifact> ArtifactToPickFrom {get; set;}
        public required int ArtifactsLeftAmount { get; set;}
        public required int ArtifactsTossedAwayAmount { get; set;}
    }

    public class ArtifactPlayerData{
        public required List<Artifact> ArtifactsPlayed {get; set;}
        public required List<Artifact> ArtifactsOwned {get; set;}
    }
}