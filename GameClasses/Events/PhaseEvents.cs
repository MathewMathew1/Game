namespace BoardGameBackend.Models
{
    public class HeroCardPickingEventArgs : EventArgs
    {
        public string GameId { get; }
        public List<HeroCardCombined> NewCards { get; }

        public HeroCardPickingEventArgs(string gameId, List<HeroCardCombined> newCards)
        {
            GameId = gameId;
            NewCards = newCards;
        }
    }
}