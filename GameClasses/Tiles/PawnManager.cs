using BoardGameBackend.Helpers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PawnManager
    {
        public Tile _currentTile { get; set; }
        private GameContext _gameContext;


        public PawnManager(Tile tile, GameContext gameContext)
        {
            _currentTile = tile;
            _gameContext = gameContext;
        }

        public void GetRewardFromCurrentTile(PlayerInGame player)
        {
            var tile = _currentTile;
            ITileAction tileAction;
            TileReward tileReward = new TileReward
            {
                Resources = new List<Resource> { },
            };

            if (tile.Token != null)
            {
                GetRewardFromToken(tile, tileReward, player);
            }
            else
            {
                tileAction = TileActionFactory.GetTileAction(tile.TileTypeId);
                tileReward = tileAction.OnEnterReward();
            }

            var eventArgs = new GetCurrentTileReward
            {
                TileReward = tileReward,
                Player = player,
            };

            player.ResourceManager.AddResources(tileReward.Resources);

            if (_currentTile.TileTypeId == TileHelper.MagicTileId && player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) >= TileHelper.MinimumMagic)
            {
                _gameContext.MiniPhaseManager.StartTeleportMiniPhase();
            }

            _gameContext.EventManager.Broadcast("GetCurrentTileReward", ref eventArgs);
        }

        public bool TeleportToPortal(PlayerInGame player, int tileId)
        {
            var tile = _gameContext.GameTiles.GetTileById(tileId);

            //if(player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) < TileHelper.MinimumMagic) return false;

            if (tile.TileTypeId != TileHelper.MagicTileId || tileId == _currentTile.Id) return false;

            _currentTile = tile;

            _gameContext.MiniPhaseManager.EndCurrentMiniPhase();

            var eventArgs = new TeleportationData
            {
                Player = player,
                TileId = tile.Id
            };

            _gameContext.EventManager.Broadcast("TeleportationEvent", ref eventArgs);

            return true;
        }

        public bool SetCurrentTile(Tile tile, PlayerInGame player, bool FullMovement, int? TeleportationTileId)
        {
            if (!CanMoveToTile(tile, player)) return false;


            var fullBoot = player.PlayerHeroCardManager.CurrentHeroCard?.MovementFullLeft > 0 || false;
            var unFullBoot = player.PlayerHeroCardManager.CurrentHeroCard?.MovementUnFullLeft > 0 || false;

            if (!(fullBoot || unFullBoot)) return false;
            if (!fullBoot && FullMovement) return false;

            TileReward tileReward = new TileReward
            {
                Resources = new List<Resource> { },
            };

            if (tile.TileTypeId == TileHelper.MagicTileId && FullMovement)
            {
                if (player.ResourceHeroManager.GetResourceHeroAmount(ResourceHeroType.Magic) < TileHelper.MinimumMagic)
                {
                    return false;
                }

                var teleportedTile = _gameContext.GameTiles.GetTileById(TeleportationTileId!.Value);
                if (teleportedTile.IsInRangeOfCastle(player.PlayerHeroCardManager.CurrentHeroCard!.HeroCard.Faction, 3))
                {
                    return false;
                }

                tileReward.TeleportedTileId = TeleportationTileId;

                var teleportAura = player.AurasTypes.FindIndex(aura => aura.Aura == AurasType.TELEPORTATION_REWARD_ONE_FREE_MOVEMENT);
                if (teleportAura == -1)
                {
                    player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft -= 1;
                }


                var eventArgsMagic = new MoveOnTile
                {
                    MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft,
                    MovementUnFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft,
                    TileReward = tileReward,
                    Player = player,
                    TileId = tile.Id
                };

                player.PlayerHeroCardManager.CurrentHeroCard.VisitedPlaces.Add(tile.Id);
                if (TeleportationTileId != null)
                {
                    player.PlayerHeroCardManager.CurrentHeroCard.VisitedPlaces.Add(TeleportationTileId.Value);
                    _currentTile = teleportedTile;
                    eventArgsMagic.TileId = _currentTile.Id;
                }
                else
                {
                    _currentTile = tile;
                }

                _gameContext.EventManager.Broadcast("MoveOnTile", ref eventArgsMagic);
                return true;
            }

            if (FullMovement)
            {
                player.PlayerHeroCardManager.CurrentHeroCard!.MovementFullLeft -= 1;
                if (tile.Token != null)
                {
                    GetRewardFromToken(tile, tileReward, player);
                }
                else
                {
                    ITileAction tileAction = TileActionFactory.GetTileAction(tile.TileTypeId);
                    tileReward = tileAction.OnEnterReward();
                }
            }
            else
            {
                if (player.PlayerHeroCardManager.CurrentHeroCard!.MovementUnFullLeft > 0)
                {
                    player.PlayerHeroCardManager.CurrentHeroCard!.MovementUnFullLeft -= 1;
                }
                else
                {
                    player.PlayerHeroCardManager.CurrentHeroCard!.MovementFullLeft -= 1;
                }
            }

            tileReward.Resources = player.ResourceManager.AddResources(tileReward.Resources);

            _currentTile = tile;

            var eventArgs = new MoveOnTile
            {
                MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft,
                MovementUnFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft,
                TileReward = tileReward,
                Player = player,
                TileId = tile.Id
            };
            player.PlayerHeroCardManager.CurrentHeroCard.VisitedPlaces.Add(tile.Id);

            _gameContext.EventManager.Broadcast("MoveOnTile", ref eventArgs);

            return true;
        }

        public bool SetBlockTile(int tileId, PlayerInGame player)
        {
            var tiles = _gameContext.GameTiles.Tiles;
            var CurrentTile = _currentTile;

            var tile = _gameContext.GameTiles.GetTileById(tileId);

            if (tile.Id == TileHelper.MagicTileId ||
                tile.Id == CurrentTile.Id ||
                tile.Id == TileHelper.CastleTileId ||
                tile.Id == TileHelper.StartTileId ||
                tile.Token != null)
            {

                return false;
            }

            var token = _gameContext.TokenManager.GetTokenById(TileHelper.BlockTileTokenId);
            tiles.ForEach(tile =>
            {
                if (tile.Token?.Id == TileHelper.BlockTileTokenId)
                {
                    tile.Token = null;
                }
            });
            tile.Token = token;

            var eventArgs = new BlockedTileData
            {
                PlayerId = player.Id,
                TileId = tile.Id,
                Token = token!
            };

            _gameContext.EventManager.Broadcast("BlockedTileEvent", ref eventArgs);


            return true;
        }

        private void GetRewardFromToken(Tile tile, TileReward tileReward, PlayerInGame player)
        {
            tileReward.TokenReward = new TokenReward { Reward = RewardFactory.GetReward(tile.Token.EffectID).OnReward() };
            _gameContext.RewardHandlerManager.HandleReward(player, tileReward.TokenReward.Reward);

            if (tile.Token.Dummy != true)
            {
                tile.Token = null;
            }
        }

        private bool CanMoveToTile(Tile tile, PlayerInGame player)
        {
            if (player.PlayerHeroCardManager.CurrentHeroCard == null) return false;

            if (player.PlayerHeroCardManager.CurrentHeroCard.VisitedPlaces.Contains(tile.Id)) return false;

            bool fulfillRequirementsToMoveToTile = true;
            bool thereIsConnection = false;

            foreach (var connection in _currentTile.Connections)
            {
                if (connection.ToId == tile.Id)
                {
                    thereIsConnection = true;
                    if (connection.Reqs == null) continue;
                    foreach (var req in connection.Reqs)
                    {
                        var requirement = RequirementMovementStore.GetRequirementById(req);
                        bool fulfillThisRequirement = requirement.CheckRequirements(player);
                        if (!fulfillThisRequirement)
                        {
                            fulfillRequirementsToMoveToTile = false;
                            break;
                        }
                    }
                    if (!fulfillRequirementsToMoveToTile) break;
                }
            }

            return fulfillRequirementsToMoveToTile && thereIsConnection;
        }

        public void MoveToCenter(PlayerInGame player, AurasType? AuraUsed = null)
        {
            var tile = _gameContext.GameTiles.GetTileById(27);

            _currentTile = tile;

            if (player.PlayerHeroCardManager.CurrentHeroCard != null)
            {
                player.PlayerHeroCardManager.CurrentHeroCard.VisitedPlaces.Add(tile.Id);
            }

            var eventArgs = new MoveOnTileOnEvent
            {
                AuraUsed = AuraUsed,
                Player = player,
                TileId = tile.Id
            };

            _gameContext.EventManager.Broadcast("MoveOnTileOnEvent", ref eventArgs);
        }
    }

}