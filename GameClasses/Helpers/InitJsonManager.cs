namespace BoardGameBackend.Helpers
{
    public static class InitJsonManager{
        
        private static Dictionary<string, string> jsonDictionary = new Dictionary<string, string>();

        private static List<string> ChangelogList = new List<string>(){
            "Nowe opcje: brak zniżki, brak budynków.",
            "Punkty za moc: (6,0) (6,3,0) (7,4,1,0).",
            "Dodano podgląd kart.",
            "Naprawiono kilka błędów na front-endzie (np. lista kolejności).",
            "Naprawiono efekt targowiska (ekstra monety kiedy ma się 0 monet).",
            "Małe zmiany w w puli artefaktów.",
            "Większe zmiany w puli przepowiedni",
            "Większe zmiany w puli budynków/najemników.",
            "Usunięto tymczasowe sygnety z planszy.",
            "Trochę zbalansowano smoki.",
            "Dodano alternatywne wersje puli bohaterów."
        };
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

            filePath = "Data/Mercenaries.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("Mercenaries.json", File.ReadAllText(filePath));

            filePath = "Data/Artifacts.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("Artifacts.json", File.ReadAllText(filePath));

            filePath = "Data/Dragons.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("Dragons.json", File.ReadAllText(filePath));

            filePath = "Data/Heroes.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("Heroes.json", File.ReadAllText(filePath));

            filePath = "Data/HeroesPre0325.json";
            if (File.Exists(filePath))
                jsonDictionary.Add("HeroesPre0325.json", File.ReadAllText(filePath));
        }
        public static Dictionary<string, string> GetJSONDictionary()
        {
            return jsonDictionary;
        }

        public static List<string> GetChangelogList()
        {
            return ChangelogList;
        }
    }
}