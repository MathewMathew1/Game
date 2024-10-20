using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class RolayCardsFactory
    {

        public static readonly List<RolayCardFromJson> RolayCardsFromJsonList;

        static RolayCardsFactory()
        {
            string filePath = "Data/RolayCards.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                RolayCardsFromJsonList = JsonSerializer.Deserialize<List<RolayCardFromJson>>(jsonData) ?? new List<RolayCardFromJson>();
            }
            else
            {
                RolayCardsFromJsonList = new List<RolayCardFromJson>();
            }
        }
    }
}