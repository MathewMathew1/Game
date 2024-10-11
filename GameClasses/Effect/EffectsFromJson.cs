using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class EffectsFactory
    {

        public static readonly List<GameEffect> EffectsFromJsonList;

        static EffectsFactory()
        {
            string filePath = "Data/SpecialEffect.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                EffectsFromJsonList = JsonSerializer.Deserialize<List<GameEffect>>(jsonData) ?? new List<GameEffect>();
            }
            else
            {
                EffectsFromJsonList = new List<GameEffect>();
            }
        }

        public static int? GetReqById(int id)
        {
            var effect = EffectsFromJsonList.Find(e => e.Id == id);
            return effect?.Req; 
        }
    }
}