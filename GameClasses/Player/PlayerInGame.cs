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
        public List<EndGameAuraType> EndGameAuras { get; set; } = new List<EndGameAuraType>();
        public PlayerHeroCardManager PlayerHeroCardManager = new PlayerHeroCardManager();
        public ResourceHeroManager ResourceHeroManager { get; set; }
        public int Points { get; set; } = 0;
        public bool AlreadyPlayedCurrentPhase = false;

        public PlayerInGame(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            ResourceManager = new ResourceManager();
            ResourceHeroManager = new ResourceHeroManager();
        }

        public void AddArtifacts(List<Artifact> artifacts)
        {
            artifacts.ForEach((artifact) => AddArtifact(artifact));
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
                AurasTypes.Add(new AuraTypeWithLongevity{Aura = AurasType.FULFILL_PROPHECY, Permanent = true});
                return new MercenaryFulfillProphecyReturnData { MercenaryId = null, aurasType = AurasType.FULFILL_PROPHECY };
            }

            if (mercenaries.Count == 1)
            {
                PlayerMercenaryManager.SetMercenaryProphecyFulfill(mercenaries[0].InGameIndex);
                return new MercenaryFulfillProphecyReturnData { MercenaryId = mercenaries[0].InGameIndex };
            }

            return new MercenaryFulfillProphecyReturnData { MercenaryId = null };
        }

        public void SetCurrentHeroCard(HeroCard card, bool left)
        {
            var addedFullMovements = 0;
            var addedEmptyMovements = 0;

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
            }

            var FullMovement = card.MovementFull + addedFullMovements;
            var EmptyMovement = card.MovementEmpty + addedEmptyMovements;
            if (AurasTypes.Any(a => a.Aura == AurasType.EMPTY_MOVES_INTO_FULL))
            {
                FullMovement = FullMovement + EmptyMovement;
                EmptyMovement = 0;
            }

            var NoFractionMovement = AurasTypes.Any(a => a.Aura == AurasType.NO_FRACTION_MOVEMENT);

            PlayerHeroCardManager.CurrentHeroCard = new CurrentHeroCard
            {
                HeroCard = card,
                LeftSide = left,
                MovementFullLeft = FullMovement,
                MovementUnFullLeft = EmptyMovement,
                NoFractionMovement = NoFractionMovement
            };
            ResourceHeroManager.AddResource(ResourceHeroType.Siege, card.Siege);
            ResourceHeroManager.AddResource(ResourceHeroType.Army, card.Army);
            ResourceHeroManager.AddResource(ResourceHeroType.Magic, card.Magic);
            ResourceHeroManager.AddResource(ResourceHeroType.Signet, card.RoyalSignet);
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
            AurasTypes = AurasTypes.FindAll(aura => aura.Permanent == true);
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