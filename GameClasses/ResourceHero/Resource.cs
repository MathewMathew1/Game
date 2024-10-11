namespace BoardGameBackend.Models
{
    public enum ResourceHeroType
    {
        Siege,
        Magic,
        Army,
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