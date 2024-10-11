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
    [Route("api/mercenary")]
    public class MercenaryController : ControllerBase
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly IMapper _mapper;

        public MercenaryController(IHubContext<LobbyHub> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet("end/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(MercenaryPhase))]
        public ActionResult EndTurn(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                gameContext.BoardManager.EndTurn();
                
                return Ok(new { Message = "Mercenary Turn ended successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("buy/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(MercenaryPhase))]
        public ActionResult BuyMercenary(string id, [FromBody] MercenaryPickModel mercenaryPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var boughtMercenary = gameContext.MercenaryManager.BuyMercenary(
                    mercenaryPickModel.MercenaryId,
                    player
                );
                
                if(!boughtMercenary){
                    return BadRequest(new { Error = "Unable to buy mercenary." });
                }

                return Ok(new { Message = "Bought mercenary successful" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("reroll/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(RerollMercenaryMiniPhase))]
        public ActionResult RerollMercenary(string id, [FromBody] MercenaryPickModel mercenaryPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var rerolledMercenary = gameContext.MercenaryManager.RerollMercenary(
                    mercenaryPickModel.MercenaryId,
                    player
                );
                
                if(!rerolledMercenary){
                    return BadRequest(new { Error = "Unable to reroll mercenary." });
                }

                return Ok(new { Message = "Rerolled mercenary successful" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }
        }

        [HttpPost("fulfill/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(FulfillProphecyMiniPhase))]
        public ActionResult FulfillProphecyMercenary(string id, [FromBody] MercenaryPickModel mercenaryPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var fulfillProphecy = gameContext.MercenaryManager.FulfillProphecy(player,mercenaryPickModel.MercenaryId);
                
                if(!fulfillProphecy){
                    return BadRequest(new { Error = "Unable to fulfill mercenary prophecy." });
                }

                return Ok(new { Message = "Fulfill prophecy successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("lock/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(LockCardMiniPhase))]
        public ActionResult LockMercenary(string id, [FromBody] MercenaryPickModel mercenaryPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var fulfillProphecy = gameContext.MercenaryManager.LockMercenary(player,mercenaryPickModel.MercenaryId);
                
                if(!fulfillProphecy){
                    return BadRequest(new { Error = "Unable to lock mercenary." });
                }

                return Ok(new { Message = "Locked mercenary successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }
    }
}