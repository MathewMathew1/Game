using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class EndGameEffectManager
    {
        private readonly Dictionary<EndGameAuraType, Action<EndGameAuraType, PlayerInGame, ScorePointsTable>> effectActions;
        private readonly Dictionary<EndGameAuraType, Action<EndGameAuraType, PlayerInGame, int, ScorePointsTable>> summedEffectActions;
        public GameContext _gameContext { get; set; }

        public EndGameEffectManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            effectActions = new Dictionary<EndGameAuraType, Action<EndGameAuraType, PlayerInGame, ScorePointsTable>>
            {
                { EndGameAuraType.SIGNETS_INTO_POINTS, SignetsIntoPoints }
            };

            summedEffectActions = new Dictionary<EndGameAuraType, Action<EndGameAuraType, PlayerInGame, int, ScorePointsTable>>
            {
                { EndGameAuraType.CUMMULATIVE_POINTS, CummulativePoints },
            };
        }

        public ScorePointsTable GetPoints(List<EndGameAuraType> effects, PlayerInGame player, ScorePointsTable scorePointsTable)
        {
            var points = 0;

            // Step 1: Count occurrences of each EndGameAuraType in the effects list
            var effectCounts = effects.GroupBy(effect => effect)
                                      .ToDictionary(group => group.Key, group => group.Count());

            // Step 2: Loop through each effect
            foreach (var effectCount in effectCounts)
            {
                var effect = effectCount.Key;
                var count = effectCount.Value;

                if (effectActions.ContainsKey(effect))
                {
                    for(var i=0; i< count; i++){
                      effectActions[effect](effect, player, scorePointsTable);
                    }                
                }
                else if (summedEffectActions.ContainsKey(effect))
                {
                    summedEffectActions[effect](effect, player, count, scorePointsTable); 
                }

            }

            return scorePointsTable;
        }

        private void CummulativePoints(EndGameAuraType effect, PlayerInGame player, int amount , ScorePointsTable scorePointsTable)
        {
            var points = new List<int> { 0, 1, 3, 7, 13, 21 };

            var maxAmount = Math.Min(points.Count, amount);
            scorePointsTable.ArtefactPoints += points[maxAmount];
        }

        private void SignetsIntoPoints(EndGameAuraType effect, PlayerInGame player, ScorePointsTable scorePointsTable)
        {
            var pointsAwarded = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Signet) * 1;

            scorePointsTable.TokenPoints +=  pointsAwarded;
        }


    }
}
