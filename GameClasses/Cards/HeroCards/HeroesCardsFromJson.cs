using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class HeroesCardsFromJson
    {

        public static readonly List<HeroCardCombinedFromJson> HeroesCombinedFromJsonList;

        static HeroesCardsFromJson()
        {
            string filePath = "Data/HeroCards.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                HeroesCombinedFromJsonList = JsonSerializer.Deserialize<List<HeroCardCombinedFromJson>>(jsonData) ?? new List<HeroCardCombinedFromJson>();
            }
            else
            {
                HeroesCombinedFromJsonList = new List<HeroCardCombinedFromJson>();
            }
        }
    }
}