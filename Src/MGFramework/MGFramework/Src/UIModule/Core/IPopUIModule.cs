using System;

namespace MGFramework.UIModule
{
    public delegate void OnViewSwitchDelegate(int viewId);

    public interface IPopUIModule
    {
        event OnViewSwitchDelegate OnViewEnterStartEvent;
        event OnViewSwitchDelegate OnViewEnterCompletedEvent;
        event OnViewSwitchDelegate OnViewQuitStartEvent;
        event OnViewSwitchDelegate OnViewQuitCompletedEvent;
        void Enter(int viewId, EnterOptions options = EnterOptions.None, Action callback = null);
        void Enter(IntGroup viewGroup, EnterOptions options = EnterOptions.None, Action callback = null);
        bool Pop(Action callback = null);
        void Quit(int viewId, QuitOptions options = QuitOptions.None, Action callback = null);
        void Quit(IntGroup viewGroup, QuitOptions options = QuitOptions.None, Action callback = null);
        void QuitAll(QuitOptions options = QuitOptions.None);
        void QuitAll(IntGroup stayViewGroup, QuitOptions options = QuitOptions.None, StayOptions stayOptions = StayOptions.None);
        void QuitAll(int stayViewId, QuitOptions options = QuitOptions.None, StayOptions stayOptions = StayOptions.None);
        void ResetStack();
        void Preload(int viewId);
        void Preload(IntGroup viewGroup);
        void UnFocus(int viewId);
        void UnFocus(IntGroup viewGroup);
    }

    /// <summary>
    /// 进入视图选项
    /// </summary>
    [Flags]
    public enum EnterOptions
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 入栈
        /// </summary>
        PushStack = 1,

        /// <summary>
        /// 合并栈顶
        /// </summary>
        CombineStackTop = 2
    }

    /// <summary>
    /// 退出视图选项
    /// </summary>
    [Flags]
    public enum QuitOptions
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 出栈
        /// </summary>
        LeaveStack = 1,

        /// <summary>
        /// 销毁
        /// </summary>
        Destroy = 2
    }

    /// <summary>
    /// 驻留选项
    /// </summary>
    [Flags]
    public enum StayOptions
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 入栈
        /// </summary>
        PushStack = 1,

        /// <summary>
        /// 合并栈顶
        /// </summary>
        CombineStackTop = 2
    }
}