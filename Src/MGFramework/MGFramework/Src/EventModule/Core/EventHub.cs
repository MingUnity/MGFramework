using System.Collections.Generic;

namespace MGFramework.EventModule
{
    /// <summary>
    /// 事件系统
    /// </summary>
    public class EventHub : IEventHub
    {
        private readonly Dictionary<int, List<IEventListener>> _eventDic = new Dictionary<int, List<IEventListener>>();

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddListener(int eventId, IEventListener listener)
        {
            if (_eventDic != null)
            {
                List<IEventListener> listeners = null;

                if (_eventDic.TryGetValue(eventId, out listeners))
                {
                    if (listeners == null)
                    {
                        listeners = new List<IEventListener>();

                        _eventDic[eventId] = listeners;
                    }
                }
                else
                {
                    listeners = new List<IEventListener>();

                    _eventDic[eventId] = listeners;
                }

                listeners?.Add(listener);
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveListener(int eventId, IEventListener listener)
        {
            List<IEventListener> listeners = null;

            _eventDic?.TryGetValue(eventId, out listeners);

            if (listeners != null && listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        /// <summary>
        /// 分发
        /// </summary>
        public void Dispatch(int eventId, IEventArgs args)
        {
            List<IEventListener> listeners = null;

            _eventDic?.TryGetValue(eventId, out listeners);

            if (listeners != null)
            {
                int count = listeners.Count;

                for (int i = count - 1; i >= 0; i--)
                {
                    IEventListener listener = listeners[i];

                    listener?.HandleEvent(eventId, args);
                }
            }
        }
    }
}
