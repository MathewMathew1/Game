namespace BoardGameBackend.Models
{
    public enum ResourceHeroType
    {
        Siege,
        Army,
        Magic,
        Signet
    }

    public class HeroResource
    {
        public ResourceHeroType Type { get; set; }
        public int Amount { get; set; }

        public HeroResource(ResourceHeroType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }


}