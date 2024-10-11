using BoardGameBackend.Managers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace BoardGameBackend.MiddleWare
{
    public class PhaseCheckFilterAttribute : ActionFilterAttribute
    {
        private readonly Type _requiredPhaseType;

        public PhaseCheckFilterAttribute(Type requiredPhaseType)
        {
            if (requiredPhaseType == null || !typeof(Phase).IsAssignableFrom(requiredPhaseType))
            {
                throw new ArgumentException("Invalid phase type provided.");
            }
            _requiredPhaseType = requiredPhaseType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = (UserModel)context.HttpContext.Items["User"]!;
            var routeValues = context.ActionArguments;
            if (routeValues.TryGetValue("id", out var lobbyIdValue) && lobbyIdValue is string lobbyId)
            {
                var lobby = LobbyManager.GetLobbyById(lobbyId);

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

                var currentPlayer = gameContext!.TurnManager.CurrentPlayer;
                if (currentPlayer== null && currentPlayer?.Id != user.Id)
                {
                    context.Result = new ForbidResult("It's not your turn.");
                }

                var currentMiniPhase = gameContext.MiniPhaseManager.CurrentMiniPhase;

                if (currentMiniPhase != null)
                {
                    context.Result = new ForbidResult($"Can't perform this action while {currentMiniPhase.Name} is on.");
                    return;
                }

                var currentPhase = gameContext.PhaseManager.CurrentPhase!;

                if (currentPhase.GetType() != _requiredPhaseType)
                {
                    context.Result = new BadRequestObjectResult($"Invalid phase. Current phase is {currentPhase.Name}, but {((Phase)Activator.CreateInstance(_requiredPhaseType))!.Name} is required.");
                    return;
                }

                context.HttpContext.Items["Player"] = currentPlayer;
                context.HttpContext.Items["Lobby"] = lobby;
                context.HttpContext.Items["GameContext"] = gameContext;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}