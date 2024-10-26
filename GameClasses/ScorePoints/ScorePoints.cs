using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class ScorePointsManager
    {
        private readonly GameContext _gameContext;
        private List<int> AmountOfPointsBasedOnPlace = new List<int> { 6, 0, 0, 0 };

        private Dictionary<Guid, ScorePointsTable> playerScores = new Dictionary<Guid, ScorePointsTable>();

        public ScorePointsManager(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public Dictionary<Guid, ScorePointsTable> FinalScore()
        {
            var players = _gameContext.PlayerManager.PlayersBasedOnMorale;
            SetupPointsForPlace(players.Count);

            foreach (var player in players)
            {
                playerScores[player.Id] = new ScorePointsTable();
            }


            for (var i = 0; i < players.Count; i++)
            {
                playerScores[players[i].Id].MoralePoints.Points = i + 1;
                playerScores[players[i].Id].MoralePoints.Power = players[i].Morale;
            }

            AwardPoints(players, ResourceHeroType.Army);
            AwardPoints(players, ResourceHeroType.Siege);
            AwardPoints(players, ResourceHeroType.Magic);
            
            SetupPointsForCards(players);
            SetupProphecyPoints(players);

            foreach (var player in players)
            {
                var scoreTable = playerScores[player.Id];
                scoreTable.PointsOverall = scoreTable.ArmyPoints.Points +
                                           scoreTable.SiegePoints.Points +
                                           scoreTable.MagicPoints.Points +
                                           scoreTable.MercenaryPoints +
                                           scoreTable.OraclePoints +
                                           scoreTable.RoyalCardPoints+
                                           scoreTable.HeroPoints +
                                           scoreTable.TokenPoints +
                                           scoreTable.ArtefactPoints;
            }

            var sortedPlayerScores = playerScores
                .OrderByDescending(kv => kv.Value.PointsOverall) 
                .ThenBy(kv => players.IndexOf(players.First(p => p.Id == kv.Key))) 
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return sortedPlayerScores;
        }

        private void SetupProphecyPoints(List<PlayerInGame> players){
            players.ForEach(p => {
                var points = 0;
                p.PlayerMercenaryManager.Mercenaries.ForEach(mercenary =>{
                    if(mercenary.EffectId != null){
                        var req = ProphecyRequirementStore.GetRequirementById(mercenary.EffectId.Value);
                        if(req != null){
                            points += req.GetProphecyPoints(p, mercenary);
                        }              
                    }                  
                });
                playerScores[p.Id].OraclePoints = points;
            });
        }

 

        private void SetupPointsForCards(List<PlayerInGame> players)
        {
            players.ForEach(p =>
            {
                var mercenariesPoints = 0;
                var heroPoints = 0;
                var royalCardsPoints = 0;

                p.PlayerMercenaryManager.Mercenaries.ForEach(mercenary =>
                {
                    mercenariesPoints += mercenary.ScorePoints;

                });

                p.PlayerHeroCardManager.HeroCardsLeft.ForEach(hero =>
                {
                    heroPoints += hero.ScorePoints;

                });

                p.PlayerHeroCardManager.HeroCardsRight.ForEach(hero =>
                {
                    heroPoints += hero.ScorePoints;
                });

                p.PlayerRolayCardManager.RolayCards.ForEach(card =>
                {
                    royalCardsPoints += card.ScorePoints;
                });


                playerScores[p.Id].HeroPoints = heroPoints;
                playerScores[p.Id].MercenaryPoints = mercenariesPoints;
                playerScores[p.Id].RoyalCardPoints = royalCardsPoints;
                playerScores[p.Id] = _gameContext.EndGameEffectManager.GetPoints(p.EndGameAuras,p, playerScores[p.Id]);
                
            });


        }

        private void AwardPoints(List<PlayerInGame> players, ResourceHeroType resourceType)
        {
            var leaderboard = players.OrderByDescending(p => p.ResourceHeroManager.GetResourceHeroAmount(resourceType))
                                     .ThenBy(p => _gameContext.PlayerManager.PlayersBasedOnMorale.IndexOf(p))
                                     .ToList();

            for (int i = 0; i < leaderboard.Count; i++)
            {
                var player = leaderboard[i];
                var points = AmountOfPointsBasedOnPlace[i];

                switch (resourceType)
                {
                    case ResourceHeroType.Army:
                        playerScores[player.Id].ArmyPoints.Points += points;
                        playerScores[player.Id].ArmyPoints.Power = player.ResourceHeroManager.GetResourceHeroAmount(resourceType);
                        break;
                    case ResourceHeroType.Siege:
                        playerScores[player.Id].SiegePoints.Points += points;
                        playerScores[player.Id].SiegePoints.Power = player.ResourceHeroManager.GetResourceHeroAmount(resourceType);
                        break;
                    case ResourceHeroType.Magic:
                        playerScores[player.Id].MagicPoints.Points += points;
                        playerScores[player.Id].MagicPoints.Power = player.ResourceHeroManager.GetResourceHeroAmount(resourceType);
                        break;
                }
            }
        }

        private void SetupPointsForPlace(int amountOfPlayers)
        {
            switch (amountOfPlayers)
            {
                case 1:
                    AmountOfPointsBasedOnPlace = new List<int> { 6, 0, 0, 0 };
                    break;
                case 2:
                    AmountOfPointsBasedOnPlace = new List<int> { 6, 0, 0, 0 };
                    break;
                case 3:
                    AmountOfPointsBasedOnPlace = new List<int> { 6, 3, 0, 0 };
                    break;
                default:
                    AmountOfPointsBasedOnPlace = new List<int> { 6, 3, 1, 0 };
                    break;
            }
        }

    }
}