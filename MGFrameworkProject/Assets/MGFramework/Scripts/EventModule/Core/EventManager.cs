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

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">监听对象接口</param>
        public void AddListener(int eventId, IEventListener listener)
        {
            _eventHub.AddListener(eventId, listener);
        }

        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="args">事件参数</param>
        public void Dispatch(int eventId, IEventArgs args)
        {
            _eventHub.Dispatch(eventId, args);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="listener">监听对象接口</param>
        public void RemoveListener(int eventId, IEventListener listener)
        {
            _eventHub.RemoveListener(eventId, listener);
        }
    }
}