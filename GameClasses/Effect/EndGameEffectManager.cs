using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class EndGameEffectManager
    {
        private readonly Dictionary<EndGameAuraType, Action<EndGameAura, PlayerInGame, ScorePointsTable>> effectActions;
        private readonly Dictionary<EndGameAuraType, Action<EndGameAuraType, PlayerInGame, int, ScorePointsTable>> summedEffectActions;
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

            summedEffectActions = new Dictionary<EndGameAuraType, Action<EndGameAuraType, PlayerInGame, int, ScorePointsTable>>
            {
                { EndGameAuraType.CUMMULATIVE_POINTS, CummulativePoints },
            };
        }

        public ScorePointsTable GetPoints(List<EndGameAura> effects, PlayerInGame player, ScorePointsTable scorePointsTable)
        {
            var points = 0;

            // Step 1: Count occurrences of each EndGameAuraType in the effects list
            var effectGroups = effects.GroupBy(effect => effect.Aura)
                              .ToDictionary(group => group.Key, group => group.ToList());

            // Step 2: Loop through each effect
            foreach (var effectCount in effectGroups)
            {
                var effect = effectCount.Key;
                var list = effectCount.Value;

                if (effectActions.ContainsKey(effect))
                {
                    for(var i=0; i< list.Count; i++){
                      effectActions[effect](list[i], player, scorePointsTable);
                    }                
                }
                else if (summedEffectActions.ContainsKey(effect))
                {
                    summedEffectActions[effect](effect, player, list.Count, scorePointsTable); 
                }

            }

            return scorePointsTable;
        }

        private void CummulativePoints(EndGameAuraType effect, PlayerInGame player, int amount , ScorePointsTable scorePointsTable)
        {
            var points = new List<int> { 0, 1, 3, 6, 10, 15 };

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
