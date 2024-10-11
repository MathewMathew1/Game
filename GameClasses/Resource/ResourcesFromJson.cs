using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class ResourcesFactory
    {

       public static readonly List<ResourceInfoJson> MercenariesFromJsonList;

        static ResourcesFactory()
        {
            string filePath = "Data/Resource.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                MercenariesFromJsonList = JsonSerializer.Deserialize<List<ResourceInfoJson>>(jsonData) ?? new List<ResourceInfoJson>();
            }
            else
            {
                MercenariesFromJsonList = new List<ResourceInfoJson>();
            }
        }
    }
}