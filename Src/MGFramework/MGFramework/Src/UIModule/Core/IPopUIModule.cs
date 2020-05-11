using System;

namespace MGFramework.UIModule
{
    public interface IPopUIModule
    {
        void Enter(int viewId, bool pushStack = true, Action callback = null);
        void Enter(IntGroup viewGroup, bool pushStack = true, Action callback = null);
        bool Pop(Action callback = null);
        void Quit(int viewId, bool leaveStack = false, Action callback = null, bool destroy = false);
        void Quit(IntGroup viewGroup, bool leaveStack = false, Action callback = null, bool destroy = false);
        void QuitAll(bool destroy = false);
        void ResetStack();
        void Preload(int viewId);
        void Preload(IntGroup viewGroup);
        void UnFocus(int viewId);
        void UnFocus(IntGroup viewGroup);
    }
}