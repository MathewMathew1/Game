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
    }

    public class ArtifactRerollModel
    {
        public required int ArtifactId { get; set; }
    }
}