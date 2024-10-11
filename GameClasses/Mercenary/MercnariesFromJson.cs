using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class MercenariesFactory
    {

       public static readonly List<MercenaryFromJson> MercenariesFromJsonList;

        static MercenariesFactory()
        {
            string filePath = "Data/Mercenaries.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                MercenariesFromJsonList = JsonSerializer.Deserialize<List<MercenaryFromJson>>(jsonData) ?? new List<MercenaryFromJson>();
            }
            else
            {
                MercenariesFromJsonList = new List<MercenaryFromJson>();
            }
        }
    }
}