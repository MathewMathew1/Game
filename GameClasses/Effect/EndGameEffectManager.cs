using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class EndGameEffectManager
    {
        private readonly Dictionary<EndGameAuraType, Action<EndGameAura, PlayerInGame, ScorePointsTable>> effectActions;
        private readonly Dictionary<EndGameAuraType, Action<EndGameAura, PlayerInGame, int, ScorePointsTable>> summedEffectActions;
        public GameContext _gameContext { get; set; }

        public EndGameEffectManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            effectActions = new Dictionary<EndGameAuraType, Action<EndGameAura, PlayerInGame, ScorePointsTable>>
            {
                { EndGameAuraType.SIGNETS_INTO_POINTS, SignetsIntoPoints },
                { EndGameAuraType.THREE_POINTS, ThreePoints },
                { EndGameAuraType.POINTS_OF_MERCENARY_OF_FACTION, PointsForMercenaryPerFaction }    
            };

            summedEffectActions = new Dictionary<EndGameAuraType, Action<EndGameAura, PlayerInGame, int, ScorePointsTable>>
            {
                { EndGameAuraType.CUMMULATIVE_POINTS, CummulativePoints },
            };
        }

        public ScorePointsTable GetPoints(List<EndGameAura> effects, PlayerInGame player, ScorePointsTable scorePointsTable)
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

                if (effectActions.ContainsKey(effect.Aura))
                {
                    for(var i=0; i< count; i++){
                      effectActions[effect.Aura](effect, player, scorePointsTable);
                    }                
                }
                else if (summedEffectActions.ContainsKey(effect.Aura))
                {
                    summedEffectActions[effect.Aura](effect, player, count, scorePointsTable); 
                }

            }

            return scorePointsTable;
        }

        private void CummulativePoints(EndGameAura effect, PlayerInGame player, int amount , ScorePointsTable scorePointsTable)
        {
            var points = new List<int> { 0, 1, 3, 7, 13, 21 };

            var maxAmount = Math.Min(points.Count, amount);
            scorePointsTable.ArtefactPoints += points[maxAmount];
        }

        private void SignetsIntoPoints(EndGameAura effect, PlayerInGame player, ScorePointsTable scorePointsTable)
        {
            var pointsAwarded = player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Signet) * 1;

            scorePointsTable.TokenPoints +=  pointsAwarded;
        }

        private void ThreePoints(EndGameAura effect, PlayerInGame player, ScorePointsTable scorePointsTable)
        {
            var pointsAwarded = 3;

            scorePointsTable.TokenPoints +=  pointsAwarded;
        }

        private void PointsForMercenaryPerFaction(EndGameAura effect, PlayerInGame player, ScorePointsTable scorePointsTable)
        {
            var mercenaries = player.PlayerMercenaryManager.Mercenaries.Count(m => m.Faction?.Id == effect.Value1);
            var pointsAwarded = mercenaries * 1;

            scorePointsTable.RoyalCardPoints +=  pointsAwarded;
        }

    }
}
