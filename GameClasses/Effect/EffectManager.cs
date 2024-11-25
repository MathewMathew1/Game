using BoardGameBackend.Helpers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{

    public class Effect
    {
        public EffectType Type { get; set; }

    }

    public class EffectManager
    {
        private readonly Dictionary<EffectType, Action<EffectType, PlayerInGame>> effectActions;
        public GameContext _gameContext { get; set; }
        private DuelManager _duelManager { get; set; }

        public EffectManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _duelManager = new DuelManager(gameContext);
            effectActions = new Dictionary<EffectType, Action<EffectType, PlayerInGame>>
            {
                { EffectType.RETURN_TO_CENTER, RunReturnToCenter },
                { EffectType.REFRESH_MERCENARIES, RefreshMercenaries },
                { EffectType.GET_REWARD_FROM_TILE, GetRewardFromCurrentTile },
                { EffectType.START_TELEPORT_MINI_PHASE, StartTeleportMiniPhase },
                { EffectType.TAKE_THREE_ARTIFACTS, TakeThreeArtifacts },
                { EffectType.START_PICK_ARTIFACT_MINI_PHASE, StartPickArtifactMiniPhase},
                { EffectType.START_PICK_ARTIFACTS_MINI_PHASE, StartPickArtifactsMiniPhase},
                { EffectType.FULFILL_PROPHECY, FulfillProphecyReward},
                {EffectType.LOCK_CARD, StartLockCardMiniPhase},
                {EffectType.BUFF_HERO, StartBuffHeroMiniPhase},
                {EffectType.REROLL_MERCENARY, StartRerollMercenary},
                {EffectType.GET_RANDOM_ARTIFACT, AddRandomArtifact},
                {EffectType.REPLAY_ARTIFACT, ReplayArtifact},
                {EffectType.REPLACE_HERO, ReplaceNextHero},
                {EffectType.GET_THREE_RANDOM_ARTIFACTS, AddThreeRandomArtifact},
                {EffectType.GOLD_FOR_PROPHECY, GoldForEachProphecy},
                {EffectType.GOLD_FOR_BUILDINGS, GoldForEachBuilding},
                {EffectType.BANISH_ROYAL_CARD, BanishRoyalCardHero},
                {EffectType.SWAP_TOKENS, SwapTokens},
                {EffectType.ROTATE_PAWN, RotatePawnMiniPhase},
                {EffectType.DUEL_ARMY, DuelArmy},
                {EffectType.DUEL_SIEGE, DuelSiege},
                 {EffectType.DUEL_MAGIC, DuelMagic},
            };
        }

        public void RunEffects(List<EffectType> effects, PlayerInGame player)
        {
            foreach (var effect in effects)
            {
                if (effectActions.ContainsKey(effect))
                {
                    effectActions[effect](effect, player);
                }
                else
                {
                    Console.WriteLine($"No handler for effect type: {effect}");
                }
            }
        }

        private void RunReturnToCenter(EffectType effect, PlayerInGame player)
        {
            _gameContext.PawnManager.MoveToCenter(player);
        }

        private void DuelMagic(EffectType effect, PlayerInGame player)
        {
            var reward = _duelManager.ArtifactDuel(player, ResourceHeroType.Magic);

            ApplyRewardFromDuel(reward, player);
        }

        private void DuelArmy(EffectType effect, PlayerInGame player)
        {
            var reward = _duelManager.ArtifactDuel(player, ResourceHeroType.Army);

            ApplyRewardFromDuel(reward, player);
        }

        private void DuelSiege(EffectType effect, PlayerInGame player)
        {
            var reward = _duelManager.ArtifactDuel(player, ResourceHeroType.Siege);

            ApplyRewardFromDuel(reward, player);
        }

        private void ApplyRewardFromDuel(PlayerReward? playerReward, PlayerInGame player)
        {
            if (playerReward == null) return;

            if (playerReward.Artifact)
            {
                _gameContext.ArtifactManager.AddArtifactsToPlayer(1, player);
            }

            if (playerReward.Gold > 0)
            {
                player.ResourceManager.AddResource(ResourceType.Gold, playerReward.Gold);

                ResourceReceivedEventData resourceReceivedEventData = new ResourceReceivedEventData
                {
                    Resources = new List<Resource> { new Resource(ResourceType.Gold, playerReward.Gold) },
                    ResourceInfo = $"has received {playerReward.Gold} gold for duel",
                    PlayerId = player.Id,
                };
                _gameContext.EventManager.Broadcast("ResourceReceivedEvent", ref resourceReceivedEventData);
            }

            if (playerReward.Reroll)
            {
                _gameContext.MiniPhaseManager.StarRerollMercenaryMiniPhase();
            }
        }

        private void RefreshMercenaries(EffectType effect, PlayerInGame player)
        {
            _gameContext.MercenaryManager.RefreshBuyableMercenaries();
        }

        private void GetRewardFromCurrentTile(EffectType effect, PlayerInGame player)
        {
            _gameContext.PawnManager.GetRewardFromCurrentTile(player);
        }

        private void StartTeleportMiniPhase(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StartTeleportMiniPhase();
        }

        private void TakeThreeArtifacts(EffectType effect, PlayerInGame player)
        {
            _gameContext.ArtifactManager.AddArtifactsToPlayer(3, player);
        }

        private void StartRerollMercenary(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StarRerollMercenaryMiniPhase();
        }

        private void StartPickArtifactMiniPhase(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StartArtifactPickMiniPhase();
            _gameContext.ArtifactManager.SetUpNewArtifactsWithoutCondition(2);
        }

        private void StartPickArtifactsMiniPhase(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StartArtifactPickMiniPhase();
            _gameContext.ArtifactManager.SetUpNewArtifactsWithoutCondition(3);
        }

        private void StartLockCardMiniPhase(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StartLockCardMiniPhase();
        }

        private void ReplayArtifact(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.ReplayArtifactMiniPhase(player);
        }

        private void ReplaceNextHero(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StarReplaceHeroMiniPhase();
        }

        private void SwapTokens(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StartSwapTokensMiniPhase();
        }

        private void RotatePawnMiniPhase(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StarRotatePawnMiniPhase();
        }

        private void BanishRoyalCardHero(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StarBanishRoyalCardMiniPhase();
        }

        private void StartBuffHeroMiniPhase(EffectType effect, PlayerInGame player)
        {
            _gameContext.MiniPhaseManager.StartBuffHeroMiniPhase();
        }

        private void GoldForEachProphecy(EffectType effect, PlayerInGame player)
        {
            var amountOfProphecy = player.PlayerMercenaryManager.Mercenaries.Count(m => m.TypeCard == MercenaryHelper.ProphecyCardType);

            player.ResourceManager.AddResource(ResourceType.Gold, amountOfProphecy);

            ResourceReceivedEventData resourceReceivedEventData = new ResourceReceivedEventData
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, 1) },
                ResourceInfo = $"has received {amountOfProphecy} gold for starting close to castle",
                PlayerId = player.Id,
            };
            _gameContext.EventManager.Broadcast("ResourceReceivedEvent", ref resourceReceivedEventData);
        }

        private void GoldForEachBuilding(EffectType effect, PlayerInGame player)
        {
            var amountOfProphecy = player.PlayerMercenaryManager.Mercenaries.Count(m => m.TypeCard == MercenaryHelper.BuildingCardType);

            player.ResourceManager.AddResource(ResourceType.Gold, amountOfProphecy);

            ResourceReceivedEventData resourceReceivedEventData = new ResourceReceivedEventData
            {
                Resources = new List<Resource> { new Resource(ResourceType.Gold, amountOfProphecy) },
                ResourceInfo = $"has received {amountOfProphecy} gold for starting close to castle",
                PlayerId = player.Id,
            };
            _gameContext.EventManager.Broadcast("ResourceReceivedEvent", ref resourceReceivedEventData);
        }

        private void AddRandomArtifact(EffectType effect, PlayerInGame player)
        {
            _gameContext.ArtifactManager.AddArtifactsToPlayer(1, player);
        }

        private void AddThreeRandomArtifact(EffectType effect, PlayerInGame player)
        {
            _gameContext.ArtifactManager.AddArtifactsToPlayer(3, player);
        }

        private void FulfillProphecyReward(EffectType effect, PlayerInGame player)
        {
            var setMiniPhaseData = player.SetFulfillProphecy();
            if (setMiniPhaseData.aurasType != null)
            {
                var eventArgs = new AddAura
                {
                    Aura = new AuraTypeWithLongevity { Aura = setMiniPhaseData.aurasType.Value, Permanent = true },
                    PlayerId = player.Id
                };
                _gameContext.EventManager.Broadcast("AddAura", ref eventArgs);
                return;
            }
            if (setMiniPhaseData.MercenaryId != null)
            {
                var eventArgs = new FulfillProphecy
                {
                    MercenaryId = setMiniPhaseData.MercenaryId.Value,
                    PlayerId = player.Id
                };
                _gameContext.EventManager.Broadcast("FulfillProphecy", ref eventArgs);
                return;
            }
            if (setMiniPhaseData.MercenaryId == null)
            {
                _gameContext.MiniPhaseManager.StartMercenaryFulfillMiniPhase();
            }

        }

    }
}
