namespace BoardGameBackend.Models
{
    public class ArtifactPickModel
    {
        public required List<int> ArtifactsIds { get; set; }
    }

    public class ArtifactPlayModel
    {
        public required int ArtifactId { get; set; }
        public required bool IsFirstEffect { get; set; }
        public required bool ReplayArtifact {get; set;}
    }

    public class ArtifactRerollModel
    {
        public required int ArtifactId { get; set; }
    }
    public class DragonPickModel
    {
        public required int DragonId { get; set; }
    }
}