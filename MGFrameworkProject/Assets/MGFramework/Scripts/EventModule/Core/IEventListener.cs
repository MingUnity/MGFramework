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
        void HandleEvent(int eventId, IEventArgs args);
    }
}
