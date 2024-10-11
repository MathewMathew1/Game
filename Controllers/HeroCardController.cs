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
    [Route("api/heroCard")]
    public class HeroCardController : ControllerBase
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly IMapper _mapper;

        public HeroCardController(IHubContext<LobbyHub> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("take/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(HeroCardPickingPhase))]
        public ActionResult PickCard(string id, [FromBody] HeroCardPickModel heroCardPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var card = gameContext.HeroCardManager.TakeHeroCard(player, heroCardPickModel.HeroCardId);
                if(card == null){
                    return BadRequest(new { Error = "Unable to take card." });
                }

                return Ok(new { Message = "Turn ended successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("buff/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(BuffHeroMiniPhase))]
        public ActionResult BuffCard(string id, [FromBody] HeroCardPickModel heroCardPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var didBuffSuccessfully = gameContext.HeroCardManager.BuffHeroCard(player, heroCardPickModel.HeroCardId);
                if(didBuffSuccessfully == false){
                    return BadRequest(new { Error = "Unable to buff card." });
                }

                return Ok(new { Message = "Buffed hero card successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }
    }
}