namespace BoardGameBackend.Helpers
{
    public static class InitJsonManager{
        
        private static Dictionary<string, string> jsonDictionary = new Dictionary<string, string>();
        static InitJsonManager()
        {
            string filePath = "Data/SpecialEffect.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("SpecialEffect.json", File.ReadAllText(filePath));

            filePath = "Data/TilesData.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("TilesData.json", File.ReadAllText(filePath));

            filePath = "Data/Reqs.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("Reqs.json", File.ReadAllText(filePath));

            filePath = "Data/TileTypes.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("TileTypes.json", File.ReadAllText(filePath));
        }
        public static Dictionary<string, string> GetJSONDictionary()
        {
            return jsonDictionary;
        }
    }
}