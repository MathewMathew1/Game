using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class DragonsFactory
    {

       public static readonly List<DragonFromJson> DragonsFromJsonList;

        static DragonsFactory()
        {
            string filePath = "Data/Dragons.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                DragonsFromJsonList = JsonSerializer.Deserialize<List<DragonFromJson>>(jsonData) ?? new List<DragonFromJson>();
            }
            else
            {
                DragonsFromJsonList = new List<DragonFromJson>();
            }
        }
    }
}