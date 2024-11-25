using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{

    public class PlayerReward
    {
        public int Gold { get; set; }
        public bool Artifact { get; set; }
        public bool Reroll { get; set; }
    }

    public class DuelManager
    {
        private readonly GameContext _gameContext;
        private List<PlayerReward> rewardsBasedOnPlace = new List<PlayerReward>();

        private Dictionary<Guid, ScorePointsTable> playerScores = new Dictionary<Guid, ScorePointsTable>();

        public DuelManager(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void Duel(PlayerInGame player, ResourceHeroType resourceHeroType, TileReward tileReward)
        {
            var players = _gameContext.PlayerManager.PlayersBasedOnMorale;
            SetupRewardsForPlayers(players.Count);
            var reward = AssignReward(players, resourceHeroType, player);

            if(reward == null) return;

            tileReward.RerollMercenaryAction = reward.Reroll;
            tileReward.Resources =  new List<Resource> { new Resource(ResourceType.Gold, reward.Gold) };
            tileReward.GetRandomArtifact  = reward.Artifact;
        }

        public PlayerReward? ArtifactDuel(PlayerInGame player, ResourceHeroType resourceHeroType){
            var players = _gameContext.PlayerManager.PlayersBasedOnMorale;
            SetupRewardsForPlayers(players.Count);
            return AssignReward(players, resourceHeroType, player);
        }



        private void SetupRewardsForPlayers(int numberOfPlayers)
        {
            rewardsBasedOnPlace = new List<PlayerReward>();

            rewardsBasedOnPlace.Add(new PlayerReward { Gold = 2, Artifact = true, Reroll = true });

            switch (numberOfPlayers)
            {
                case 1:
                case 2:
                    rewardsBasedOnPlace.Add(new PlayerReward { Gold = 0, Artifact = false, Reroll = false });
                    break;
                case 3:
                    rewardsBasedOnPlace.Add(new PlayerReward { Gold = 2, Artifact = false, Reroll = false });
                    rewardsBasedOnPlace.Add(new PlayerReward { Gold = 0, Artifact = false, Reroll = false });
                    break;
                case 4:
                default:
                    rewardsBasedOnPlace.Add(new PlayerReward { Gold = 2, Artifact = false, Reroll = true }); // 2nd place
                    rewardsBasedOnPlace.Add(new PlayerReward { Gold = 1, Artifact = false, Reroll = false }); // 3rd place
                    break;
            }
        }



        private PlayerReward? AssignReward(List<PlayerInGame> players, ResourceHeroType resourceHeroType, PlayerInGame player)
        {
            var leaderboard = players.OrderByDescending(p => p.ResourceHeroManager.GetResourceHeroAmount(resourceHeroType))
                                     .ThenBy(p => _gameContext.PlayerManager.PlayersBasedOnMorale.IndexOf(p))
                                     .ToList();

            var index = leaderboard.FindIndex(p => p.Id == player.Id);

            if(player.AurasTypes.Find(a => a.Aura == AurasType.INSTANT_WIN_DUEL) != null) index = 0;

            if(index == -1){
                return null;
            }

            var reward = rewardsBasedOnPlace[index];

            return reward;
        }

    }
}