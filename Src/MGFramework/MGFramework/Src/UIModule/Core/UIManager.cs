using System;
using System.Collections.Generic;

namespace MGFramework.UIModule
{
    /// <summary>
    /// UI管理
    /// </summary>
    public sealed class UIManager : Singleton<UIManager>
    {
        private IUIModule _uiModule;

        /// <summary>
        /// 视图栈
        /// 用于存储需要入栈的视图
        /// </summary>
        private SpecialStack<IntGroup> _viewStack = new SpecialStack<IntGroup>();

        /// <summary>
        /// 所有激活过视图字典
        /// </summary>
        private Dictionary<int, ViewState> _viewDic = new Dictionary<int, ViewState>();

        public UIManager()
        {
            _uiModule = new UIModule();
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        public void Enter(int viewId, Action callback = null, bool pushStack = true)
        {
            ViewState state;

            _viewDic.TryGetValue(viewId, out state);

            if (!state.active)
            {
                state.active = true;

                _viewDic[viewId] = state;

                _uiModule?.Enter(viewId, callback);
            }

            if (pushStack)
            {
                _viewStack.Push(IntGroup.Get(viewId));
            }
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="callback">完成回调</param>
        /// <param name="pushStack">入栈</param>
        public void Enter(IntGroup viewGroup, Action callback = null, bool pushStack = true)
        {
            int all = viewGroup.Count;
            int done = 0;

            for (int i = 0; i < all; i++)
            {
                Enter(viewGroup[i], () =>
                {
                    done++;
                    if (done >= all)
                    {
                        callback?.Invoke();
                    }
                }, false);
            }

            if (pushStack)
            {
                _viewStack.Push(viewGroup);
            }
        }

        /// <summary>
        ///  退出视图
        /// </summary>
        public void Quit(int viewId, Action callback = null, bool leaveStack = false, bool destroy = false)
        {
            ViewState state;

            if (_viewDic.TryGetValue(viewId, out state))
            {
                if (state.active)
                {
                    state.active = false;

                    _viewDic[viewId] = state;

                    _uiModule?.Quit(viewId, callback, destroy);
                }
            }

            if (leaveStack)
            {
                _viewStack.Delete(IntGroup.Get(viewId));
            }
        }


        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="callback">完成回调</param>
        /// <param name="leaveStack">出栈</param>
        /// <param name="destroy">销毁</param>
        public void Quit(IntGroup viewGroup, Action callback = null, bool leaveStack = false, bool destroy = false)
        {
            int all = viewGroup.Count;
            int done = 0;

            for (int i = 0; i < all; i++)
            {
                Quit(viewGroup[i], () =>
                {
                    done++;
                    if (done >= all)
                    {
                        callback?.Invoke();
                    }
                }, false, destroy);
            }

            if (leaveStack)
            {
                _viewStack.Delete(viewGroup);
            }
        }

        /// <summary>
        /// 退出所有页面
        /// </summary>
        public void QuitAll(bool destroy = false)
        {
            foreach (int id in _viewDic.Keys)
            {
                _uiModule?.Quit(id, null, destroy);
            }

            _viewDic.Clear();

            ResetStack();
        }

        /// <summary>
        /// 弹出视图
        /// </summary>
        public bool Pop(Action callback = null)
        {
            bool res = false;

            IntGroup curId = IntGroup.Empty;
            IntGroup dstId = IntGroup.Empty;

            if (_viewStack.Count > 1)
            {
                if (_viewStack.Pop(out curId) && _viewStack.Peek(out dstId))
                {
                    res = true;

                    Quit(curId, () =>
                    {
                        Enter(dstId, callback, false);
                    }, false);
                }
            }

            return res;
        }

        /// <summary>
        /// 清空栈
        /// </summary>
        public void ResetStack()
        {
            _viewStack.Clear();
        }

        /// <summary>
        /// 视图状态
        /// </summary>
        private struct ViewState
        {
            /// <summary>
            /// 激活
            /// </summary>
            public bool active;
        }
    }
}