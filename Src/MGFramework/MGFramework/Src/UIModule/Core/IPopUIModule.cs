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
        void Enter(int viewId, bool pushStack = true, Action callback = null);
        void Enter(IntGroup viewGroup, bool pushStack = true, Action callback = null);
        bool Pop(Action callback = null);
        void Quit(int viewId, bool leaveStack = false, Action callback = null, bool destroy = false);
        void Quit(IntGroup viewGroup, bool leaveStack = false, Action callback = null, bool destroy = false);
        void QuitAll(bool destroy = false);
        void QuitOtherAll(IntGroup stayViewGroup, bool destroy = false);
        void QuitOtherAll(int stayViewId, bool destroy = false);
        void ResetStack();
        void Preload(int viewId);
        void Preload(IntGroup viewGroup);
        void UnFocus(int viewId);
        void UnFocus(IntGroup viewGroup);
    }
}