namespace MGFramework.UIModule
{
    /// <summary>
    /// 滚动节点
    /// </summary>
    public interface ISuperScrollNode
    {
        /// <summary>
        /// 移至尾节点
        /// </summary>
        void TurnLast();

        /// <summary>
        /// 移至首节点
        /// </summary>
        void TurnFirst();

        /// <summary>
        /// 重置需要异步加载的数据
        /// </summary>
        void ResetAsyncData();
    }
}