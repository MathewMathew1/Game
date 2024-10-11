using BoardGameBackend.Mappers;
using BoardGameBackend.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardGameBackend.Managers
{
    public class ArtifactManager
    {
        private List<Artifact> _artifacts;
        public List<Artifact> ArtifactsToPickFrom { get; private set; } = new List<Artifact>();
        public List<Artifact> TossedAwayArtifacts { get; private set; } = new List<Artifact>();
        private int HowManyMercenariesToPick = 2;
        private int _nextInGameIndex = 1;
        private GameContext _gameContext;

        public ArtifactManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _artifacts = new List<Artifact>();

            foreach (var artifactFromJson in ArtifactsFactory.ArtifactsFromJsonList)
            {
                for (int i = 0; i < artifactFromJson.ShuffleX; i++)
                {
                    var mercenary = GameMapper.Instance.Map<Artifact>(artifactFromJson);
                    mercenary.InGameIndex = _nextInGameIndex++;
                    _artifacts.Add(mercenary);
                }
            }

            ShuffleArtifacts();

            _gameContext.EventManager.Subscribe<PlayerInGame>("New player turn", player =>
            {
                SetUpNewArtifacts(player);
            }, priority: 0);

            _gameContext.EventManager.Subscribe<MoveOnTile>("MoveOnTile", moveOnTileData =>
            {
                HandleMoveOnTile(moveOnTileData.TileReward, moveOnTileData.Player);
            }, priority: 5);

            _gameContext.EventManager.Subscribe<GetCurrentTileReward>("GetCurrentTileReward", getCurrentTileReward =>
            {
                HandleMoveOnTile(getCurrentTileReward.TileReward, getCurrentTileReward.Player);
            }, priority: 5);
        }

        public void HandleMoveOnTile(TileReward tileReward, PlayerInGame player)
        {
            if (tileReward.GetRandomArtifact == true)
            {
                var artifact = GetArtifactFromTop();
                player.AddArtifacts(new List<Artifact> { artifact });
                tileReward.Artifact = artifact;
            }
        }

        public void AddArtifactsToPlayer(int amount, PlayerInGame player)
        {
            List<Artifact> artifacts = new List<Artifact>();
            for (var i = 0; i < amount; i++)
            {
                var artifact = GetArtifactFromTop();
                if (artifact == null) break;

                artifacts.Add(artifact);
            }

            player.AddArtifacts(artifacts);

            ArtifactToPickFromData artifactToPickFromData = new ArtifactToPickFromData
            {
                ArtifactsLeft = _artifacts.Count,
                ArtifactsLeftTossed = TossedAwayArtifacts.Count,
                Player = player,
                Artifacts = artifacts,
            };

            _gameContext.EventManager.Broadcast("ArtifactsGivenToPlayer", ref artifactToPickFromData);
        }

        public bool RerollArtifactByIndex(int artifactId, PlayerInGame player)
        {
            var artifact = player.Artifacts.Find(artifact => artifact.InGameIndex == artifactId);

            if (artifact == null) return false;

            var artifactRerolled = GetArtifactFromTop();

            player.RerollArtifact(artifactId, artifactRerolled);
            TossedAwayArtifacts.Add(artifact);

            ArtifactRerolledData artifactPlayedData = new ArtifactRerolledData
            {
                Artifact = artifact,
                Player = player,
                ArtifactRerolled = artifactRerolled
            };
          
            _gameContext.EventManager.Broadcast("ArtifactRerolled", ref artifactPlayedData);

            return true;
        }

        public bool PlayArtifactByIndex(int artifactId, PlayerInGame player, bool isFirstEffect)
        {
            var artifact = player.Artifacts.Find(artifact => artifact.InGameIndex == artifactId);

            if (artifact == null) return false;

            var effectId = isFirstEffect ? artifact.Effect1 : artifact.Effect2;
            if (effectId == -1) return false;

            var canPlayArtifact = CanPlayArtifact(effectId, player);

            if (!canPlayArtifact) return false;

            var artifactRewardClass = RewardFactory.GetReward(effectId);
            var artifactRewards = artifactRewardClass.OnReward();

            player.PlayedArtifact(artifactId);

            _gameContext.RewardHandlerManager.HandleReward(player, artifactRewards);


            ArtifactPlayed artifactPlayedData = new ArtifactPlayed
            {
                Artifact = artifact,
                Player = player,
                FirstEffect = isFirstEffect,
                Reward = artifactRewards
            };

            _gameContext.EventManager.Broadcast("ArtifactPlayed", ref artifactPlayedData);

            return true;
        }

        private bool CanPlayArtifact(int effectId, PlayerInGame player)
        {
            var req = EffectsFactory.GetReqById(effectId);
            if(req == -1 || req == null) return true;

            var requirement = RequirementMovementStore.GetRequirementById(req.Value);
            bool fulfillThisRequirement = requirement.CheckRequirements(player);
            return fulfillThisRequirement;
        }

        public void SetUpNewArtifacts(PlayerInGame player)
        {
            if (player.Artifacts.Count == 0 && _gameContext.PhaseManager.CurrentPhase.GetType() == typeof(ArtifactPhase))
            {
                SetUpNewArtifactsWithoutCondition(player);
            }
        }

        public void SetUpNewArtifactsWithoutCondition(PlayerInGame? player = null)
        {
            PlayerInGame playerToSelectArtifact = player != null ? player : _gameContext.TurnManager.CurrentPlayer; 
            SetArtifactsToPickFrom();
            ArtifactToPickFromData artifactToPickFromData = new ArtifactToPickFromData
            {
                ArtifactsLeft = _artifacts.Count,
                ArtifactsLeftTossed = TossedAwayArtifacts.Count,
                Player = playerToSelectArtifact,
                Artifacts = ArtifactsToPickFrom,
            };
            _gameContext.EventManager.Broadcast("ArtifactsToPick", ref artifactToPickFromData);
        }

        private void SetArtifactsToPickFrom()
        {
            ArtifactsToPickFrom.Clear();

            while (ArtifactsToPickFrom.Count < 3)
            {
                if (_artifacts.Count == 0)
                {
                    if (TossedAwayArtifacts.Count == 0)
                    {
                        break;
                    }

                    _artifacts = TossedAwayArtifacts;
                    TossedAwayArtifacts.Clear();

                }
                var artifact = _artifacts[0];

                ArtifactsToPickFrom.Add(artifact);

                _artifacts.RemoveAt(0);
            }

        }

        private Artifact? GetArtifactFromTop()
        {
            var artifact = _artifacts[0];
            _artifacts.RemoveAt(0);
            return artifact;
        }

        public bool PickArtifacts(List<int> artifactIds, PlayerInGame player)
        {
            var artifactsToPickNumber = Math.Min(HowManyMercenariesToPick, ArtifactsToPickFrom.Count);
            if (artifactIds.Count != artifactsToPickNumber) return false;

            // if (player.Artifacts.Count > 0) return false;

            List<Artifact> pickedArtifacts = new List<Artifact>();

            foreach (var artifactId in artifactIds)
            {
                var artifact = ArtifactsToPickFrom.Find(_artifact => _artifact.InGameIndex == artifactId);

                if (artifact == null) return false;

                pickedArtifacts.Add(artifact);
            }

            player.AddArtifacts(pickedArtifacts);

            ArtifactsToPickFrom.RemoveAll(pickedArtifacts.Contains);
            TossedAwayArtifacts.AddRange(ArtifactsToPickFrom);

            ArtifactsToPickFrom.Clear();

            ArtifactsTaken artifactToPickFromData = new ArtifactsTaken
            {
                ArtifactsLeft = _artifacts.Count,
                ArtifactsLeftTossed = TossedAwayArtifacts.Count,
                Player = player,
                Artifacts = pickedArtifacts,
            };
            _gameContext.EventManager.Broadcast("ArtifactsTaken", ref artifactToPickFromData);

            return true;
        }

        private void ShuffleArtifacts()
        {
            Random rng = new Random();
            _artifacts = _artifacts.OrderBy(m => rng.Next()).ToList();
        }
    }
}
