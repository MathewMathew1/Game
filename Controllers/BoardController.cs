using Microsoft.AspNetCore.Mvc;
using BoardGameBackend.Managers;
using BoardGameBackend.Models;
using BoardGameBackend.Hubs;
using Microsoft.AspNetCore.SignalR;
using AutoMapper;
using BoardGameBackend.MiddleWare;

namespace BoardGameBackend.Controllers
{
    [ApiController]
    [Route("api/board")]
    public class BoardController : ControllerBase
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly IMapper _mapper;

        public BoardController(IHubContext<LobbyHub> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet("end/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(BoardPhase))]
        public ActionResult EndTurn(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                gameContext.BoardManager.EndTurn();

                return Ok(new { Message = "Hero Turn ended successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("moveToTile/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(BoardPhase))]
        public ActionResult MoveToTile(string id, [FromBody] MoveToTileDto moveToTileDto)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                var movementDoneCorrectly = gameContext.PawnManager.SetCurrentTile(
                    gameContext.GameTiles.GetTileById(moveToTileDto.TileId),
                    player,
                    moveToTileDto.FullMovement,
                    moveToTileDto.AdjacentMovement ?? false,
                    moveToTileDto.TeleportationPlace
                );

                if (!movementDoneCorrectly)
                {
                    return BadRequest(new { Error = "Unable to move to a tile." });
                }

                return Ok(new { Message = "Hero Turn ended successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("teleport/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(TeleportMiniPhase))]
        public ActionResult TeleportToTile(string id, [FromBody] TeleportToTile teleportToTile)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                var movementDoneCorrectly = gameContext.PawnManager.TeleportToPortal(player, teleportToTile.TileId);

                if (!movementDoneCorrectly)
                {
                    return BadRequest(new { Error = "Unable to teleport to a tile." });
                }

                return Ok(new { Message = "Teleported to tile successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }
        }

        [HttpPost("block/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(BlockTileMiniPhase))]
        public ActionResult BlockTile(string id, [FromBody] TeleportToTile teleportToTile)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                var blockedTile = gameContext.PawnManager.SetBlockTile(teleportToTile.TileId, player);

                if (!blockedTile)
                {
                    return BadRequest(new { Error = "Unable to block a tile." });
                }

                return Ok(new { Message = "Blocked tile successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }
        }

        [HttpPost("swap/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(SwapTokenMiniPhase))]
        public ActionResult SwapTokens(string id, [FromBody] SwapTokensData data)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                var swappedTokens = gameContext.PawnManager.SwapTokens(data.TileIdOne, data.TileIdTwo, player);

                if (!swappedTokens)
                {
                    return BadRequest(new { Error = "Unable to swap tokens." });
                }

                return Ok(new { Message = "Swapped tokens successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }
        }

        [HttpGet("stopSwapping/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(SwapTokenMiniPhase))]
        public ActionResult StopSwappingTokens(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                gameContext.PawnManager.StopSwappingTokens();

                return Ok(new { Message = "Stopped swapping tokens successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }
        }

        [HttpGet("goldIntoMovement/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(BoardPhase))]
        public ActionResult GoldIntoMovement(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                var successfullyConvertedGoldIntoMovement = gameContext.PawnManager.GoldIntoMovement(player);

                if (!successfullyConvertedGoldIntoMovement)
                {
                    return BadRequest(new { Error = "Unable to convert gold into movement." });
                }

                return Ok(new { Message = "Converted gold into movement successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpGet("convertMovement/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(BoardPhase))]
        public ActionResult convertMovement(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;

                var successfullyConvertedMovements = gameContext.PawnManager.FullMovementIntoEmptyMovement(player);

                if (!successfullyConvertedMovements)
                {
                    return BadRequest(new { Error = "Unable to convert full movement into empty." });
                }

                return Ok(new { Message = "Converted full movement into empty movement successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }
    }
}