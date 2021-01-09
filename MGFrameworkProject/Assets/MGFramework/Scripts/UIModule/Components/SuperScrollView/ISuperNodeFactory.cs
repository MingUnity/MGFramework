namespace MGFramework.UIModule
{
    /// <summary>
    /// 滚动列表节点工厂
    /// </summary>
    public interface ISuperNodeFactory
    {
        /// <summary>
        /// 创建节点
        /// </summary>
        ISuperScrollNode Create();

        /// <summary>
        /// 回收节点
        /// </summary>
        void Recycle(ISuperScrollNode node);
    }
}