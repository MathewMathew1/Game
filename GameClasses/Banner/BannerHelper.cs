using BoardGameBackend.Models;

namespace BoardGameBackend.Helpers
{
    public static class BannerHelper{
       
        public static int BannerRewardTileAction(PlayerInGame player, int bannerType){
            var goldAdded = 0;

            if(player.PlayerMercenaryManager.Mercenaries.Any(mercenary => mercenary.Faction?.Id == bannerType)){
                goldAdded =+ 1;
            }

            if(player.PlayerRolayCardManager.RolayCards.Any(royalCard => royalCard.Faction?.Id == bannerType)){
                goldAdded =+ 1;
            }

            if(player.PlayerHeroCardManager.HeroCardsLeft.Any(card => card.Faction?.Id == bannerType) || player.PlayerHeroCardManager.HeroCardsRight.Any(card => card.Faction?.Id == bannerType)){
                goldAdded =+ 1;
            }

            return goldAdded;
        }
    }
}