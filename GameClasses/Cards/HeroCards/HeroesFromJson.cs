using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class HeroesFromJson
    {

        public static readonly List<HeroCardFromJson> HeroesFromJsonList;

        static HeroesFromJson()
        {
            string filePath = "Data/Heroes.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                HeroesFromJsonList = JsonSerializer.Deserialize<List<HeroCardFromJson>>(jsonData) ?? new List<HeroCardFromJson>();
            }
            else
            {
                HeroesFromJsonList = new List<HeroCardFromJson>();
            }
        }
    }
}