using BoardGameBackend.Providers;
using BoardGameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace BoardGameBackend.Managers.EventListeners
{
    public class HeroCardEventListener : IEventListener
    {
        private readonly IHubContextProvider _hubContextProvider;

        public HeroCardEventListener(IHubContextProvider hubContextProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        public void SubscribeEvents(GameContext gameContext)
        {
            var gameId = gameContext.GameId;

            gameContext.EventManager.Subscribe<BuffHeroData>("HeroCardBuffed", data =>
            {
                BroadcastHeroCardBuffed(data, gameId);
            }, priority: 10);

            gameContext.EventManager.Subscribe<HeroCardPicked>("HeroCardPicked", args =>
            {
                BroadcastHeroCardPicked( args, gameId);
            }, priority: 10);

            gameContext.EventManager.Subscribe<NewCardsSetupData>("NewCardsSetup", data =>
            {
                BroadcastNewCardsSetup(data, gameId);
            }, priority: 10);


        }

        public void BroadcastHeroCardPicked(HeroCardPicked data, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("CardPicked", data);
        }

        public void BroadcastNewCardsSetup(NewCardsSetupData data, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("NewCardsSetup", data);
        }

        public void BroadcastHeroCardBuffed(BuffHeroData data, string gameId)
        {
            var hubContext = _hubContextProvider!.LobbyHubContext;
            hubContext.Clients.Group(LobbyManager.GetLobbyByGameId(gameId)!.Id).SendAsync("HeroCardBuffed", data);
        }

    }
}