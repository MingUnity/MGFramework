namespace MGFramework.UIModule
{
    /// <summary>
    /// 交互
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// 视图
        /// </summary>
        IView View { get; set; }
        
        /// <summary>
        /// 装载
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载
        /// </summary>
        void Uninstall();

        /// <summary>
        /// 创建完成
        /// </summary>
        void OnCreateCompleted();

        /// <summary>
        /// 显示开始
        /// </summary>
        void OnShowStart();

        /// <summary>
        /// 显示完成
        /// </summary>
        void OnShowCompleted();

        /// <summary>
        /// 隐藏开始
        /// </summary>
        void OnHideStart();

        /// <summary>
        /// 隐藏完成
        /// </summary>
        void OnHideCompleted();
    }
}
