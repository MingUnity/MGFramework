namespace MGFramework.EventModule
{
    /// <summary>
    /// 事件管理
    /// </summary>
    public class EventManager : Singleton<EventManager>, IEventHub
    {
        private IEventHub _eventHub;

        public EventManager()
        {
            _eventHub = new EventHub();
        }

        public void AddListener(int eventId, IEventListener listener)
        {
            _eventHub.AddListener(eventId, listener);
        }

        public void Dispatch(int eventId, IEventArgs args)
        {
            _eventHub.Dispatch(eventId, args);
        }

        public void RemoveListener(int eventId, IEventListener listener)
        {
            _eventHub.RemoveListener(eventId, listener);
        }
    }
}