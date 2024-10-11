namespace BoardGameBackend.Models
{
    public enum ResourceType
    {
        Gold,
        Wood,
        Iron,
        Gems,
        Niter,
        MysticFog
    }

    public class ResourceInfoJson {
        public required int Id { get; set; }
        public required string NameEng { get; set; }
    }

    public class ResourceManagerViewModel
    {
        public required Dictionary<ResourceType, int> Resources { get; set; }
    }
}