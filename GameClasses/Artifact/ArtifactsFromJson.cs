using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class ArtifactsFactory
    {

       public static readonly List<ArtifactFromJson> ArtifactsFromJsonList;

        static ArtifactsFactory()
        {
            string filePath = "Data/Artifacts.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                ArtifactsFromJsonList = JsonSerializer.Deserialize<List<ArtifactFromJson>>(jsonData) ?? new List<ArtifactFromJson>();
            }
            else
            {
                ArtifactsFromJsonList = new List<ArtifactFromJson>();
            }
        }
    }
}