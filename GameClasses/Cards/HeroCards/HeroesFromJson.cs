using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class HeroesFromJson
    {

        public static readonly List<HeroCardFromJson> HeroesFromJsonList;
        public static readonly List<HeroCardFromJson> HeroesFromJsonListPre032025;

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
            filePath = "Data/HeroesPre0325.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                HeroesFromJsonListPre032025 = JsonSerializer.Deserialize<List<HeroCardFromJson>>(jsonData) ?? new List<HeroCardFromJson>();
            }
            else
            {
                HeroesFromJsonListPre032025 = new List<HeroCardFromJson>();
            }
        }
    }
}