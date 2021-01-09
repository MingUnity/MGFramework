namespace MGFramework.EventModule
{
    /// <summary>
    /// 事件中心接口
    /// </summary>
    public interface IEventHub
    {
        /// <summary>
        /// 添加监听
        /// </summary>
        void AddListener(int eventId, IEventListener listener);

        /// <summary>
        /// 移除监听
        /// </summary>
        void RemoveListener(int eventId, IEventListener listener);

        /// <summary>
        /// 分发
        /// </summary>
        void Dispatch(int eventId, IEventArgs args);
    }
}
