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
    [Route("api/royalCard")]
    public class RolayCardController: ControllerBase
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly IMapper _mapper;

        public RolayCardController(IHubContext<LobbyHub> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("take/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(RoyalCardPickMiniPhase))]
        public ActionResult EndTurn(string id, [FromBody] RoyalCardPickModel royalCardPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var pickedCard = gameContext.RolayCardManager.PickCardForPlayer(player, royalCardPickModel.RoyalCardId);
                if(pickedCard == null){
                    return BadRequest(new { Error = "Unable to pick royal card." });
                }
                
                return Ok(new { Message = "Royal card picked successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

    }
}