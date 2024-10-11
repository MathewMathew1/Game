using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{
    public class RewardHandlerManager
    {
        private readonly GameContext _gameContext;

        public RewardHandlerManager(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void HandleReward(PlayerInGame player, Reward reward)
        {
            player.ReceiveRewards(reward);
            _gameContext.EffectManager.RunEffects(reward.Effects, player);
            if (reward.Morale != null)
            {
                _gameContext.PlayerManager.AddMoraleToPlayer(player, reward.Morale.Value);
            }
        }

    }
}

