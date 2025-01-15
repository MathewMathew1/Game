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
        [PhaseOrMiniPhaseCheckFilter(typeof(ArtifactPhase), typeof(ArtifactReplayMiniPhase))]
        public ActionResult PlayedArtifact(string id, [FromBody] ArtifactPlayModel artifactPlayModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var correctlyPlayedArtifact = gameContext.ArtifactManager.HandleArtifactPlay(artifactPlayModel.ArtifactId, player, artifactPlayModel.IsFirstEffect, artifactPlayModel.ReplayArtifact);
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

        [HttpGet("skip/{id}")]
        [Authorize]
        [PhaseCheckFilter(typeof(ArtifactPhase))]
        public ActionResult SkipArtifactPhase(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var correctlySkippedPhase = gameContext.ArtifactManager.EndArtifactTurn(player);
                if(correctlySkippedPhase == false){
                    return BadRequest(new { Error = "Unable to skip artifact phase." });
                }

                return Ok(new { Message = "Artifact phase skipped successfully." });
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

        [HttpPost("discardformoveconfirm/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(DiscardArtifactForFullMovement))]
        public ActionResult DiscardArtifactForMoveConfirm(string id, [FromBody] ArtifactRerollModel artifactRerollModel)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var bActionResponse = gameContext.ArtifactManager.DisardArtifactForMovement(artifactRerollModel.ArtifactId, player);
                if(bActionResponse == false){
                    return BadRequest(new { Error = "Unable to discard artifact for move." });
                }

                return Ok(new { Message = "Artifact discarded for movement successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }

        [HttpPatch("discardformovereject/{id}")]
        [Authorize]
        [MiniPhaseCheckFilter(typeof(DiscardArtifactForFullMovement))]
        public ActionResult DiscardArtifactForMoveReject(string id)
        {
            try
            {
                UserModel user = (UserModel)Request.HttpContext.Items["User"]!;
                var gameContext = (GameContext)Request.HttpContext.Items["GameContext"]!;
                var player = (PlayerInGame)Request.HttpContext.Items["Player"]!;
           
                var bActionResponse = gameContext.ArtifactManager.RejectDisardArtifactForMovement(player);
                if(bActionResponse == false){
                    return BadRequest(new { Error = "Unable to reject discard artifact for move." });
                }

                return Ok(new { Message = "Rejected artifact discard for movement successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Error = "Unexpected error" });
            }

        }
    }
}