using BoardGameBackend.Helpers;
using BoardGameBackend.Managers;

namespace BoardGameBackend.Models
{
    public class PlayerInGame
    {
        public ResourceManager ResourceManager { get; set; }
        public Guid Id { get; private set; }   // No required keyword
        public string Name { get; private set; }
        public int Morale { get; set; }
        public PlayerMercenaryManager PlayerMercenaryManager { get; set; } = new PlayerMercenaryManager();
        public List<Artifact> Artifacts { get; set; } = new List<Artifact>();
        public List<Artifact> ArtifactsPlayed { get; set; } = new List<Artifact>();
        public List<AuraTypeWithLongevity> AurasTypes { get; set; } = new List<AuraTypeWithLongevity>();
        public List<EndGameAura> EndGameAuras { get; set; } = new List<EndGameAura>();
        public PlayerHeroCardManager PlayerHeroCardManager = new PlayerHeroCardManager();
        public PlayerRolayCardManager PlayerRolayCardManager = new PlayerRolayCardManager();
        public ResourceHeroManager ResourceHeroManager { get; set; }
        public List<TokenFromJson> Tokens { get; set; } = new List<TokenFromJson>();
        public Dictionary<string, bool> BoolAdditionalStorage = new Dictionary<string, bool>();
        public int Points { get; set; } = 0;
        public bool AlreadyPlayedCurrentPhase = false;

        public PlayerInGame(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            ResourceManager = new ResourceManager(this);
            ResourceHeroManager = new ResourceHeroManager();
        }

        public void AddArtifacts(List<Artifact> artifacts)
        {
            artifacts.ForEach((artifact) => AddArtifact(artifact));
        }

        public ArtifactPlayerData ArtifactPlayerData()
        {
            return new ArtifactPlayerData
            {
                ArtifactsOwned = Artifacts,
                ArtifactsPlayed = ArtifactsPlayed
            };
        }

        private void AddArtifact(Artifact artifact)
        {
            Artifacts.Add(artifact);
        }

        public MercenaryFulfillProphecyReturnData SetFulfillProphecy()
        {
            var mercenaries = PlayerMercenaryManager.GetAllUnfilledProphecies();

            if (mercenaries.Count == 0)
            {
                AurasTypes.Add(new AuraTypeWithLongevity { Aura = AurasType.FULFILL_PROPHECY, Permanent = true });
                return new MercenaryFulfillProphecyReturnData { MercenaryId = null, aurasType = AurasType.FULFILL_PROPHECY };
            }

            if (mercenaries.Count == 1)
            {
                PlayerMercenaryManager.SetMercenaryProphecyFulfill(mercenaries[0].InGameIndex);
                return new MercenaryFulfillProphecyReturnData { MercenaryId = mercenaries[0].InGameIndex };
            }

            return new MercenaryFulfillProphecyReturnData { MercenaryId = null };
        }

        public CurrentHeroCard SetCurrentHeroCard(HeroCard card, bool left, GameContext gameContext, HeroCard unusedHeroCard, ReplacedHero? replacedHero)
        {
            var addedFullMovements = 0;
            var addedEmptyMovements = 0;
            var noFractionMovement = AurasTypes.Any(a => a.Aura == AurasType.NO_FRACTION_MOVEMENT);
            var tile = gameContext.PawnManager._currentTile;

            foreach (var aura in AurasTypes)
            {
                if (aura.Aura == AurasType.ONE_FULL_MOVEMENT)
                {
                    addedFullMovements += 1;
                }
                else if (aura.Aura == AurasType.ONE_EMPTY_MOVEMENT)
                {
                    addedEmptyMovements += 1;
                }
                else if (aura.Aura == AurasType.EMPTY_MOVEMENT_WHEN_HERO_HAS_SIGNET && card.RoyalSignet > 0)
                {
                    addedEmptyMovements += 1;
                }
                else if (aura.Aura == AurasType.EMPTY_MOVEMENT_WHEN_HERO_HAS_MORALE && card.Morale > 0)
                {
                    addedEmptyMovements += 1;
                }
                else if (aura.Aura == AurasType.EMPTY_MOVEMENT_WHEN_HERO_HAS_EMPTY_MOVEMENT && card.MovementEmpty == 0)
                {
                    addedEmptyMovements += 1;
                }
                else if (aura.Aura == AurasType.ADD_ADDITIONAL_MOVEMENT_BASED_ON_FRACTION && card.Faction.Id == aura.Value1)
                {
                    addedEmptyMovements += 1;
                }
                else if (aura.Aura == AurasType.EMPTY_MOVE_WHEN_CLOSE_TO_CASTLE && TileDistanceHelper.IsInRange(2, tile, card.Faction.Id) && noFractionMovement == false)
                {
                    addedEmptyMovements += 1;
                }
                else if (aura.Aura == AurasType.FULL_MOVE_WHEN_CLOSE_TO_CASTLE && TileDistanceHelper.IsInRange(1, tile, card.Faction.Id) && noFractionMovement == false)
                {
                    addedFullMovements += 1;
                }
                else if (aura.Aura == AurasType.ARTIFACT_WHEN_CLOSE_TO_CASTLE && TileDistanceHelper.IsInRange(2, tile, card.Faction.Id) && noFractionMovement == false)
                {
                    gameContext.ArtifactManager.AddArtifactsToPlayer(1, this);
                }
                else if (aura.Aura == AurasType.GOLD_WHEN_CLOSE_TO_CASTLE && TileDistanceHelper.IsInRange(3, tile, card.Faction.Id) && noFractionMovement == false)
                {
                    ResourceManager.AddResource(ResourceType.Gold, 1);

                    ResourceReceivedEventData resourceReceivedEventData = new ResourceReceivedEventData {
                        Resources = new List<Resource> { new Resource(ResourceType.Gold, 1) },
                        ResourceInfo = $"has received 1 gold for starting close to castle",
                        PlayerId = Id,
                    };
                    gameContext.EventManager.Broadcast("ResourceReceivedEvent", ref resourceReceivedEventData);
                }
            }

            var fullMovement = card.MovementFull + addedFullMovements;
            var emptyMovement = card.MovementEmpty + addedEmptyMovements;
            if (AurasTypes.Any(a => a.Aura == AurasType.EMPTY_MOVES_INTO_FULL))
            {
                fullMovement = fullMovement + emptyMovement;
                emptyMovement = 0;
            }

            PlayerHeroCardManager.CurrentHeroCard = new CurrentHeroCard
            {
                HeroCard = card,
                UnUsedHeroCard = unusedHeroCard,
                LeftSide = left,
                ReplacedHeroCard = replacedHero,
                MovementFullLeft = fullMovement,
                MovementUnFullLeft = emptyMovement,
                NoFractionMovement = noFractionMovement
            };

            var cardAddedResourceFrom = replacedHero != null ? replacedHero.HeroCard : card;

            ResourceHeroManager.AddResource(ResourceHeroType.Siege, cardAddedResourceFrom.Siege);
            ResourceHeroManager.AddResource(ResourceHeroType.Army, cardAddedResourceFrom.Army);
            ResourceHeroManager.AddResource(ResourceHeroType.Magic, cardAddedResourceFrom.Magic);

            return PlayerHeroCardManager.CurrentHeroCard;
        }

        public void ResetCurrentHeroCard(EventManager eventManager)
        {
            if (PlayerHeroCardManager.CurrentHeroCard == null) return;

            var changeHeroSides = AurasTypes.Any(a => a.Aura == AurasType.CHANGE_SIDES_OF_HERO_AFTER_PLAY);

            var card = PlayerHeroCardManager.CurrentHeroCard.HeroCard;
            if (changeHeroSides)
            {
                ResourceHeroManager.SubtractResource(ResourceHeroType.Siege, card.Siege);
                ResourceHeroManager.SubtractResource(ResourceHeroType.Army, card.Army);
                ResourceHeroManager.SubtractResource(ResourceHeroType.Magic, card.Magic);

                card = PlayerHeroCardManager.CurrentHeroCard.UnUsedHeroCard;

                ResourceHeroManager.AddResource(ResourceHeroType.Siege, card.Siege);
                ResourceHeroManager.AddResource(ResourceHeroType.Army, card.Army);
                ResourceHeroManager.AddResource(ResourceHeroType.Magic, card.Magic);

                PlayerHeroCardManager.ResetCurrentHeroCardReverse();
            }
            else
            {
                card = PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard != null ? PlayerHeroCardManager.CurrentHeroCard.ReplacedHeroCard.HeroCard : card;
            }

            ResourceHeroManager.AddResource(ResourceHeroType.Signet, card.RoyalSignet);
            if (card.Morale > 0)
            {
                Morale += card.Morale;

                var eventArgs = new MoraleAdded
                {
                    Player = this
                };

                eventManager.Broadcast("MoraleAdded", ref eventArgs);
            }


            PlayerHeroCardManager.ResetCurrentHeroCard();
        }

        public void AddMercenary(Mercenary mercenary)
        {
            PlayerMercenaryManager.Mercenaries.Add(mercenary);

            ResourceHeroManager.AddResource(ResourceHeroType.Siege, mercenary.Siege);
            ResourceHeroManager.AddResource(ResourceHeroType.Army, mercenary.Army);
            ResourceHeroManager.AddResource(ResourceHeroType.Magic, mercenary.Magic);

            ResourceManager.AddGoldIncome(mercenary.IncomeGold);
            Points += mercenary.ScorePoints;
        }

        public void ReceiveRewards(Reward reward)
        {
            ResourceManager.AddResources(reward.Resources);
            reward.AurasTypes.ForEach(auraType => AurasTypes.Add(auraType));
            reward.HeroResources.ForEach(resource => ResourceHeroManager.AddResource(resource.Type, resource.Amount));
            reward.EndGameAura.ForEach(auraType => EndGameAuras.Add(auraType));
        }

        public void ResetAura()
        {
            var tempSignets = AurasTypes.Count(aura => aura.Aura == AurasType.TEMPORARY_SIGNET);
            ResourceHeroManager.SubtractResource(ResourceHeroType.Signet, tempSignets);
            AurasTypes = AurasTypes.FindAll(aura => aura.Permanent == true);

            BoolAdditionalStorage.Clear();
        }

        public void PlayedArtifact(int artifactId)
        {
            var artifact = Artifacts.FirstOrDefault(a => a.InGameIndex == artifactId);

            if (artifact != null)
            {
                Artifacts.Remove(artifact);
                ArtifactsPlayed.Add(artifact);
            }
        }

        public void AddRoyalCard(RolayCard rolayCard)
        {
            PlayerRolayCardManager.AddRolayCard(rolayCard);
            ResourceHeroManager.AddResource(ResourceHeroType.Siege, rolayCard.Siege);
            ResourceHeroManager.AddResource(ResourceHeroType.Army, rolayCard.Army);
            ResourceHeroManager.AddResource(ResourceHeroType.Magic, rolayCard.Magic);
            Points += rolayCard.ScorePoints;
        }

        public void RerollArtifact(int artifactId, Artifact artifactRerolled)
        {
            var artifact = Artifacts.FirstOrDefault(a => a.InGameIndex == artifactId);

            if (artifact != null)
            {
                Artifacts.Remove(artifact);
                Artifacts.Add(artifactRerolled);
            }
        }

        public int RemoveAurasOfTypeAndReturnAmountCount(AurasType auraType)
        {
            var amountRemoved = 0;

            for (var i = AurasTypes.Count - 1; i >= 0; i--)
            {
                if (auraType == AurasTypes[i].Aura)
                {
                    amountRemoved += 1;
                    AurasTypes.RemoveAt(i);
                }
            }

            return amountRemoved;
        }
    }





    public class PlayerViewModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required ResourceManagerViewModel ResourceManager { get; set; }
    }
}