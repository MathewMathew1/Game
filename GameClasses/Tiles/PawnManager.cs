using BoardGameBackend.Helpers;
using BoardGameBackend.Models;

namespace BoardGameBackend.Managers
{
    public class PawnManager
    {
        public TileWithType _currentTile { get; set; }
        private GameContext _gameContext;
        private DuelManager _duelManager;

        public PawnManager(TileWithType tile, GameContext gameContext)
        {
            _currentTile = tile;
            _gameContext = gameContext;
            _duelManager = new DuelManager(gameContext);
        }

        public void StopSwappingTokens()
        {
            _gameContext.MiniPhaseManager.EndCurrentMiniPhase();
            _gameContext.TurnManager.EndTurn();
        }

        public bool SwapTokens(int tileIdOne, int tileIdTwo, PlayerInGame player)
        {
            var tile = _gameContext.GameTiles.GetTileById(tileIdOne);
            var tileTwo = _gameContext.GameTiles.GetTileById(tileIdTwo);

            if (tile == null || tileTwo == null) return false;

            if (tile.Token == null || tile.TileTypeId == TileHelper.CastleTileId || tileTwo.Token == null || tileTwo.TileTypeId == TileHelper.CastleTileId) return false;

            (tile.Token, tileTwo.Token) = (tileTwo.Token, tile.Token);

            var eventArgs = new SwapTokensDataEventData
            {
                PlayerId = player.Id,
                TileOneId = tile.Id,
                TileTwoId = tileTwo.Id
            };

            _gameContext.EventManager.Broadcast("SwapTokensDataEvent", ref eventArgs);

            return true;
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
                if (tileReward.Duel != null)
                {
                    _duelManager.Duel(player, tileReward.Duel.DuelHeroStat, tileReward);
                }
            }

            var eventArgs = new GetCurrentTileReward
            {
                TileReward = tileReward,
                Player = player,
            };

            if (tileReward.EmptyMovement && player.PlayerHeroCardManager.CurrentHeroCard != null)
            {
                player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft += 1;
            }

            PossiblyAddTemporarySignet(tileReward.TempSignet, player);

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

        public bool FullMovementIntoEmptyMovement(PlayerInGame player)
        {
            var auraIndex = player.AurasTypes.FindIndex(a => a.Aura == AurasType.FULL_MOVEMENT_INTO_EMPTY);

            if (auraIndex == -1) return false;

            if (player.PlayerHeroCardManager.CurrentHeroCard == null) return false;

            var canBuy = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft > 0;

            if (canBuy == false) return false;

            player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft -= 1;
            player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft += 2;
            player.AurasTypes.RemoveAt(auraIndex);


            var eventArgs = new FullMovementIntoEmptyEventData
            {
                PlayerId = player.Id,
                MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft,
                MovementUnFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft
            };

            _gameContext.EventManager.Broadcast("FullMovementIntoEmptyEvent", ref eventArgs);

            return true;
        }

        public bool RotatePawn(PlayerInGame player, int rotatedTileId)
        {
            var tile = _gameContext.GameTiles.GetTileById(rotatedTileId);

            if (tile.RotateID != _currentTile.RotateID) return false;

            if (tile.Id == _currentTile.Id) return false;

            _currentTile = tile;

            var eventArgs = new RotateTileEventData
            {
                TileId = tile.Id,
                PlayerId = player.Id,
            };

            _gameContext.EventManager.Broadcast("RotateTileEvent", ref eventArgs);


            return true;
        }

        public bool GoldIntoMovement(PlayerInGame player)
        {
            var auraIndex = player.AurasTypes.FindIndex(a => a.Aura == AurasType.GOLD_FOR_MOVEMENT);

            if (auraIndex == -1) return false;

            List<ResourceInfo> resource = new List<ResourceInfo> { new ResourceInfo { Name = ResourceType.Gold, Amount = 2 } };
            var canBuy = player.ResourceManager.CheckForResourceAndRemoveThem(resource, _gameContext.EventManager);

            if (canBuy == false) return false;

            if (player.PlayerHeroCardManager.CurrentHeroCard == null) return false;

            player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft += 1;
            player.AurasTypes.RemoveAt(auraIndex);


            var eventArgs = new GoldIntoMovementEventData
            {
                GoldLeft = player.ResourceManager.GetResourceAmount(ResourceType.Gold),
                PlayerId = player.Id,
                MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft
            };

            _gameContext.EventManager.Broadcast("GoldIntoMovementEvent", ref eventArgs);

            return true;
        }

        public bool SetCurrentTile(TileWithType tile, PlayerInGame player, bool FullMovement, bool AdjacentMovement, int? TeleportationTileId)
        {
            if (!CanMoveToTile(tile, player)) return false;

            if (AdjacentMovement && !player.AurasTypes.Any(aura => aura.Aura == AurasType.ADJACENT_TILE_REWARD)) return false;


            var fullBoot = player.PlayerHeroCardManager.CurrentHeroCard?.MovementFullLeft > 0 || false;
            var unFullBoot = player.PlayerHeroCardManager.CurrentHeroCard?.MovementUnFullLeft > 0 || false;

            if (!(fullBoot || unFullBoot)) return false;
            if (!fullBoot && FullMovement) return false;

            TileReward tileReward = new TileReward
            {
                Resources = new List<Resource> { },
                EmptyReward = true,
            };

            if (tile.TileTypeId == TileHelper.MagicTileId && FullMovement)
            {

                if (tile.TileType.Req != null)
                {
                    var req = RequirementMovementStore.GetRequirementById(tile.TileType.Req.Value);

                    var fulfillReq = false;
                    if (req == null)
                    {
                        fulfillReq = false;
                    }
                    else
                    {
                        fulfillReq = req.CheckRequirements(player);
                    }

                    if (fulfillReq == false) return false;

                }


                var teleportedTile = _gameContext.GameTiles.GetTileById(TeleportationTileId!.Value);
                if (teleportedTile.IsInRangeOfCastle(player.PlayerHeroCardManager.CurrentHeroCard!.HeroCard.Faction, 3))
                {
                    return false;
                }

                tileReward.TeleportedTileId = TeleportationTileId;

                var teleportAuras = player.AurasTypes.Count(aura => aura.Aura == AurasType.TELEPORTATION_REWARD_ONE_FREE_MOVEMENT);
                player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft - 1 + teleportAuras;

                var eventArgsMagic = new MoveOnTile
                {
                    MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft,
                    MovementUnFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft,
                    TileReward = tileReward,
                    Player = player,
                    TileId = tile.Id,
                    Tile = tile
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
                    if (tileReward.Duel != null)
                    {
                        _duelManager.Duel(player, tileReward.Duel.DuelHeroStat, tileReward);
                    }
                    if (tileReward.EmptyMovement)
                    {
                        player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft += 1;
                    }
                    PossiblyAddTemporarySignet(tileReward.TempSignet, player);
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

            if (!AdjacentMovement)
            {
                _currentTile = tile;
            }

            var eventArgs = new MoveOnTile
            {
                MovementFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementFullLeft,
                MovementUnFullLeft = player.PlayerHeroCardManager.CurrentHeroCard.MovementUnFullLeft,
                TileReward = tileReward,
                Player = player,
                TileId = tile.Id,
                AdjacentMovement = AdjacentMovement,
                Tile = tile
            };
            player.PlayerHeroCardManager.CurrentHeroCard.VisitedPlaces.Add(tile.Id);

            _gameContext.EventManager.Broadcast("MoveOnTile", ref eventArgs);

            return true;
        }

        public void PossiblyAddTemporarySignet(bool addSignet, PlayerInGame player)
        {
            if (!addSignet) return;

            player.ResourceHeroManager.AddResource(ResourceHeroType.Signet, 1);
            player.AurasTypes.Add(new AuraTypeWithLongevity { Permanent = false, Aura = AurasType.TEMPORARY_SIGNET });
        }

        public bool SetBlockTile(int tileId, PlayerInGame player)
        {
            var tiles = _gameContext.GameTiles.Tiles;
            var CurrentTile = _currentTile;

            var tile = _gameContext.GameTiles.GetTileById(tileId);

            if (tile.TileTypeId == TileHelper.MagicTileId ||
                tile.Id == CurrentTile.Id ||
                tile.TileTypeId == TileHelper.CastleTileId ||
                tile.TileTypeId == TileHelper.StartTileId ||
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

        private void GetRewardFromToken(TileWithType tile, TileReward tileReward, PlayerInGame player)
        {
            tileReward.TokenReward = new TokenReward { Reward = RewardFactory.GetRewardById(tile.Token.EffectID).OnReward() };
            _gameContext.RewardHandlerManager.HandleReward(player, tileReward.TokenReward.Reward);

            if (tile.Token.Collectable == true)
            {
                player.Tokens.Add(tile.Token);
                tile.Token = null;
            }
        }

        private bool CanMoveToTile(TileWithType tile, PlayerInGame player)
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