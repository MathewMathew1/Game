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
    [Route("api/artifact")]
    public class ArtifactController : ControllerBase
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly IMapper _mapper;

        public ArtifactController(IHubContext<LobbyHub> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("take/{id}")]
        [Authorize]
        [PhaseOrMiniPhaseCheckFilter(typeof(ArtifactPhase), typeof(ArtifactPickMiniPhase))]
        public ActionResult PickArtifact(string id, [FromBody] ArtifactPickModel artifactPickModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var correctlyTookArtifacts = gameContext.ArtifactManager.PickArtifacts(artifactPickModel.ArtifactsIds, player);
                if(correctlyTookArtifacts == false){
                    return BadRequest(new { Error = "Unable to take artifacts." });
                }

                return Ok(new { Message = "Turn ended successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("play/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(ArtifactPhase))]
        public ActionResult PlayedArtifact(string id, [FromBody] ArtifactPlayModel artifactPlayModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var correctlyPlayedArtifact = gameContext.ArtifactManager.PlayArtifactByIndex(artifactPlayModel.ArtifactId, player, artifactPlayModel.IsFirstEffect);
                if(correctlyPlayedArtifact == false){
                    return BadRequest(new { Error = "Unable to play artifact." });
                }

                return Ok(new { Message = "Artifact played successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPost("reroll/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(ArtifactPhase))]
        public ActionResult RerollArtifact(string id, [FromBody] ArtifactRerollModel artifactRerollModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var correctlyRerolledArtifact = gameContext.ArtifactManager.RerollArtifactByIndex(artifactRerollModel.ArtifactId, player);
                if(correctlyRerolledArtifact == false){
                    return BadRequest(new { Error = "Unable to reroll artifact." });
                }

                return Ok(new { Message = "Artifact rerolled successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }
    }
}