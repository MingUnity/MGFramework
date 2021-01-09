using System;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 视图接口
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 交互
        /// </summary>
        IPresenter Presenter { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        void Create(Action callback = null);

        /// <summary>
        /// 预加载
        /// </summary>
        void Preload(Action callback = null, bool instantiate = true);

        /// <summary>
        /// 显示
        /// </summary>
        void Show(Action callback = null);

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide(Action callback = null);

        /// <summary>
        /// 聚焦
        /// </summary>
        void Focus();

        /// <summary>
        /// 失焦
        /// </summary>
        void UnFocus();

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy();
    }
}