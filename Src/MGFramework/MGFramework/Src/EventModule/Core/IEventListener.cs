namespace MGFramework.EventModule
{
    /// <summary>
    /// 事件监听
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="args">事件参数</param>
        void HandleEvent(int eventId, IEventArgs args);
    }
}
