using System.Text.Json;

namespace BoardGameBackend.Models
{
    public static class TokensFactory
    {

       public static readonly List<TokenFromJson> TokensFromJsonList;

        static TokensFactory()
        {
            string filePath = "Data/Tokens.json";
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                TokensFromJsonList = JsonSerializer.Deserialize<List<TokenFromJson>>(jsonData) ?? new List<TokenFromJson>();
            }
            else
            {
                TokensFromJsonList = new List<TokenFromJson>();
            }
        }
    }
}