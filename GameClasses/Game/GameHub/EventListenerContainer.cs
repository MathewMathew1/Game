using BoardGameBackend.Providers;

namespace BoardGameBackend.Managers.EventListeners
{
    public class EventListenerContainer
    {
        private readonly List<IEventListener> _listeners = new();

        public EventListenerContainer(IHubContextProvider hubContextProvider)
        {
            _listeners.Add(new HeroCardEventListener(hubContextProvider));
            _listeners.Add(new TileEventListener(hubContextProvider));
            _listeners.Add(new ArtifactEventListener(hubContextProvider));
            _listeners.Add(new MercenaryEventListener(hubContextProvider));
            _listeners.Add( new PhaseEventListener(hubContextProvider));
            _listeners.Add( new MiniPhaseEventListener(hubContextProvider));
            _listeners.Add( new OtherEventListener(hubContextProvider));
        }

        public void SubscribeAll(GameContext gameContext)
        {
            foreach (var listener in _listeners)
            {
                listener.SubscribeEvents(gameContext);
            }
        }
    }
}