using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BoardGameBackend.Managers;
using BoardGameBackend.Models;

namespace BoardGameBackend.MiddleWare
{
    public class PhaseOrMiniPhaseCheckFilterAttribute : ActionFilterAttribute
    {
        private readonly Type _requiredPhaseType;
        private readonly Type _requiredMiniPhaseType;

        // Constructor for setting both phase and mini-phase types
        public PhaseOrMiniPhaseCheckFilterAttribute(Type requiredPhaseType, Type requiredMiniPhaseType)
        {
            if (requiredPhaseType == null || !typeof(Phase).IsAssignableFrom(requiredPhaseType))
                throw new ArgumentException("Invalid phase type provided.");
            
            if (requiredMiniPhaseType == null || !typeof(MiniPhase).IsAssignableFrom(requiredMiniPhaseType))
                throw new ArgumentException("Invalid mini-phase type provided.");
            
            _requiredPhaseType = requiredPhaseType;
            _requiredMiniPhaseType = requiredMiniPhaseType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = (UserModel)context.HttpContext.Items["User"];
            var routeValues = context.ActionArguments;
            
            if (routeValues.TryGetValue("id", out var lobbyIdValue) && lobbyIdValue is string lobbyId)
            {
                var lobbyInfo = LobbyManager.GetLobbyById(lobbyId);
                var lobby = lobbyInfo?.Lobby;
                
                if (lobby == null || string.IsNullOrEmpty(lobby.GameId))
                {
                    context.Result = new NotFoundObjectResult("Lobby or game not found.");
                    return;
                }

                var gameContext = GameManager.GetGameById(lobby.GameId);
                if (gameContext == null)
                {
                    context.Result = new NotFoundObjectResult("Game not found.");
                    return;
                }

                var currentPlayer = gameContext.TurnManager.CurrentPlayer;
                if (currentPlayer == null || currentPlayer.Id != user.Id)
                {
                    context.Result = new ForbidResult("It's not your turn.");
                    return;
                }

                var currentMiniPhase = gameContext.MiniPhaseManager.CurrentMiniPhase;

                var currentPhase = gameContext.PhaseManager.CurrentPhase;

                if (currentMiniPhase?.GetType() == _requiredMiniPhaseType || (currentPhase?.GetType() == _requiredPhaseType && currentMiniPhase == null))
                {
                    context.HttpContext.Items["Player"] = currentPlayer;
                    context.HttpContext.Items["Lobby"] = lobby;
                    context.HttpContext.Items["GameContext"] = gameContext;

                    await next();
                }
                else
                {
                    context.Result = new BadRequestObjectResult($"Invalid phase or mini-phase. Current phase: {currentPhase?.Name}, Current mini-phase: {currentMiniPhase?.Name}.");
                }
            }
        }
    }
}
