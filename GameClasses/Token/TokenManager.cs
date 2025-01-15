namespace BoardGameBackend.Models
{
    public class TokenManager
    {
        private readonly List<TokenFromJson> _tokens;
        public readonly List<TokenFromJson> AllTokens;

        public TokenManager(bool dragonDLC)
        {

            AllTokens = new List<TokenFromJson>(TokensFactory.TokensFromJsonList);
            _tokens = new List<TokenFromJson>();
            foreach(var token in AllTokens)
            {
                if(token.InStartingPool && (dragonDLC || !token.DragonDLC))
                    _tokens.Add(token);
            }
            ShuffleTokens();
        }

        private void ShuffleTokens()
        {
            Random rng = new Random();
            int n = _tokens.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                TokenFromJson value = _tokens[k];
                _tokens[k] = _tokens[n];
                _tokens[n] = value;
            }
        }

        public TokenFromJson? GetTokenById(int tokenId)
        {       
            var token = AllTokens.Find(token => token.Id==tokenId);
            return token;
        }

        public TokenFromJson? DrawToken()
        {
            if (_tokens.Count == 0)
            {
                throw new InvalidOperationException("No more tokens left in the stack.");
            }

            TokenFromJson topToken = _tokens[0];
            _tokens.RemoveAt(0);
            return topToken;
        }

        public int RemainingTokens()
        {
            return _tokens.Count;
        }
    }
}
