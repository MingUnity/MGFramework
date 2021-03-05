using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 弹出UI模块
    /// </summary>
    public sealed class PopUIModule : IPopUIModule
    {
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

        /// <summary>
        /// 临时需要退出的列表集合
        /// </summary>
        private List<int> _tempQuitList = new List<int>();

        /// <summary>
        /// 进入视图开始事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewEnterStartEvent;

        /// <summary>
        /// 进入视图完成事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewEnterCompletedEvent;

        /// <summary>
        /// 退出视图开始事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewQuitStartEvent;

        /// <summary>
        /// 退出视图完成事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewQuitCompletedEvent;

        public PopUIModule()
        {
            _uiModule = new UIModule();
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="options">选项</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(int viewId, EnterOptions options = EnterOptions.None, Action callback = null)
        {
            ProcessEnterOptions(IntGroup.Get(viewId), options);

            ViewState state;
            _viewDic.TryGetValue(viewId, out state);

            if (!state.active)
            {
                state.active = true;
                _viewDic[viewId] = state;

                OnViewEnterStartEvent?.Invoke(viewId);

                _uiModule?.Enter(viewId, () =>
                {
                    callback?.Invoke();
                    _uiModule?.Focus(viewId);
                    OnViewEnterCompletedEvent?.Invoke(viewId);
                });
            }
            else
            {
                _uiModule?.Focus(viewId);
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="options">选项</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(IntGroup viewGroup, EnterOptions options = EnterOptions.None, Action callback = null)
        {
            ProcessEnterOptions(viewGroup, options);

            int all = viewGroup.Count;
            int done = 0;

            for (int i = 0; i < all; i++)
            {
                Enter(viewGroup[i], EnterOptionsFilter(EnterOptions.CombineStackTop | EnterOptions.PushStack, options), () =>
                {
                    done++;
                    if (done >= all)
                    {
                        callback?.Invoke();
                    }
                });
            }
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="options">选项</param>
        /// <param name="callback">退出完成回调</param>
        public void Quit(int viewId, QuitOptions options = QuitOptions.None, Action callback = null)
        {
            ViewState state;

            if (_viewDic.TryGetValue(viewId, out state))
            {
                if (state.active)
                {
                    state.active = false;
                    _viewDic[viewId] = state;

                    OnViewQuitStartEvent?.Invoke(viewId);

                    _uiModule?.Quit(viewId, () =>
                    {
                        callback?.Invoke();
                        _uiModule?.UnFocus(viewId);
                        OnViewQuitCompletedEvent?.Invoke(viewId);

                    }, options.HasFlag(QuitOptions.Destroy));
                }
                else
                {
                    callback?.Invoke();
                }
            }
            else
            {
                callback?.Invoke();
            }

            ProcessQuitOptions(IntGroup.Get(viewId), options);
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="options">选项</param>
        /// <param name="callback">完成回调</param>
        public void Quit(IntGroup viewGroup, QuitOptions options = QuitOptions.None, Action callback = null)
        {
            int all = viewGroup.Count;
            int done = 0;

            for (int i = 0; i < all; i++)
            {
                Quit(viewGroup[i], QuitOptionsFilter(QuitOptions.LeaveStack, options), () =>
                {
                    done++;
                    if (done >= all)
                    {
                        callback?.Invoke();
                    }
                });
            }

            ProcessQuitOptions(viewGroup, options);
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewId">视图id</param>
        public void UnFocus(int viewId)
        {
            _uiModule?.UnFocus(viewId);
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        public void UnFocus(IntGroup viewGroup)
        {
            for (int i = 0; i < viewGroup.Count; i++)
            {
                UnFocus(viewGroup[i]);
            }
        }

        /// <summary>
        /// 退出所有视图
        /// </summary>
        /// <param name="options">选项</param>
        public void QuitAll(QuitOptions options = QuitOptions.None)
        {
            _tempQuitList.Clear();

            foreach (int id in _viewDic.Keys)
            {
                _tempQuitList.Add(id);
            }

            for (int i = 0; i < _tempQuitList.Count; i++)
            {
                Quit(_tempQuitList[i], QuitOptionsFilter(QuitOptions.LeaveStack, options), null);
            }

            if (options.HasFlag(QuitOptions.LeaveStack))
            {
                ResetStack();
            }
        }

        /// <summary>
        /// 退出所有视图
        /// 会有部分视图驻留
        /// </summary>
        /// <param name="stayViewGroup">驻留视图组</param>
        /// <param name="options">退出的视图选项</param>
        /// <param name="stayOptions">驻留的视图选项</param>
        public void QuitAll(IntGroup stayViewGroup, QuitOptions options = QuitOptions.None, StayOptions stayOptions = StayOptions.None)
        {
            _tempQuitList.Clear();

            foreach (int id in _viewDic.Keys)
            {
                if (!stayViewGroup.Contains(id))
                {
                    _tempQuitList.Add(id);
                }
            }

            for (int i = 0; i < _tempQuitList.Count; i++)
            {
                Quit(_tempQuitList[i], QuitOptionsFilter(QuitOptions.LeaveStack, options), null);
            }

            if (options.HasFlag(QuitOptions.LeaveStack))
            {
                ResetStack();
            }

            ProcessStayOptions(stayViewGroup, stayOptions);
        }

        /// <summary>
        /// 退出所有视图
        /// 会有部分视图驻留
        /// </summary>
        /// <param name="stayViewId">驻留视图</param>
        /// <param name="options">退出的视图选项</param>
        /// <param name="stayOptions">驻留的视图选项</param>
        public void QuitAll(int stayViewId, QuitOptions options = QuitOptions.None, StayOptions stayOptions = StayOptions.None)
        {
            QuitAll(IntGroup.Get(stayViewId), options, stayOptions);
        }

        /// <summary>
        /// 弹出视图
        /// 回到上一级视图
        /// </summary>
        /// <param name="callback">完成回调</param>
        /// <returns>是否弹出成功</returns>
        public bool Pop(Action callback = null)
        {
            bool res = false;

            IntGroup curId = IntGroup.Empty;
            IntGroup dstId = IntGroup.Empty;

            if (_viewStack.Count > 1)
            {
                if (_viewStack.Peek(out curId))
                {
                    //当前视图组 是否有视图激活
                    bool curActive = false;

                    for (int i = 0; i < curId.Count; i++)
                    {
                        ViewState state = _viewDic.GetValueAnyway(curId[i]);

                        if (state.active)
                        {
                            curActive = true;
                            break;
                        }
                    }

                    if (curActive) //栈顶界面有显示的，则出栈并隐藏 显示上一级界面
                    {
                        if (_viewStack.Pop(out curId) && _viewStack.Peek(out dstId))
                        {
                            res = true;

                            IntGroup toQuit = curId - dstId;

                            Quit(toQuit, QuitOptions.None, () =>
                           {
                               Enter(dstId, EnterOptions.None, callback);
                           });
                        }
                    }
                    else //栈顶界面无显示，则显示栈顶界面
                    {
                        res = true;

                        Enter(curId, EnterOptions.None, callback);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 清空视图栈
        /// </summary>
        public void ResetStack()
        {
            _viewStack.Clear();
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(int viewId, bool instantiate = true)
        {
            if (!_viewDic.ContainsKey(viewId))
            {
                _uiModule.Preload(viewId, instantiate);

                _viewDic[viewId] = new ViewState()
                {
                    active = false
                };
            }
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(IntGroup viewGroup, bool instantiate = true)
        {
            for (int i = 0; i < viewGroup.Count; i++)
            {
                Preload(viewGroup[i], instantiate);
            }
        }

        /// <summary>
        /// 处理进入选项
        /// </summary>
        private void ProcessEnterOptions(IntGroup viewGroup, EnterOptions options)
        {
            if (options.HasFlag(EnterOptions.PushStack) && !options.HasFlag(EnterOptions.CombineStackTop))
            {
                ProcessPushStack(viewGroup);
            }
            else if (options.HasFlag(EnterOptions.PushStack) && options.HasFlag(EnterOptions.CombineStackTop))
            {
                ProcessPushCombineStackTop(viewGroup);
            }
            else if (!options.HasFlag(EnterOptions.PushStack) && options.HasFlag(EnterOptions.CombineStackTop))
            {
                ProcessCombineStackTop(viewGroup);
            }
        }

        /// <summary>
        /// 处理驻留选项
        /// </summary>
        private void ProcessStayOptions(IntGroup viewGroup, StayOptions options)
        {
            if (options.HasFlag(StayOptions.PushStack) && !options.HasFlag(StayOptions.CombineStackTop))
            {
                ProcessPushStack(viewGroup);
            }
            else if (options.HasFlag(StayOptions.PushStack) && options.HasFlag(StayOptions.CombineStackTop))
            {
                ProcessPushCombineStackTop(viewGroup);
            }
            else if (!options.HasFlag(StayOptions.PushStack) && options.HasFlag(StayOptions.CombineStackTop))
            {
                ProcessCombineStackTop(viewGroup);
            }
        }

        /// <summary>
        /// 合并栈顶
        /// </summary>
        private void ProcessCombineStackTop(IntGroup viewGroup)
        {
            IntGroup top = IntGroup.Empty;

            if (_viewStack.Pop(out top))
            {
                IntGroup newTop = IntGroup.Combine(top, viewGroup);
                _viewStack.Push(newTop);
            }
            else
            {
                _viewStack.Push(viewGroup);
            }
        }

        /// <summary>
        /// 合并栈顶并入栈
        /// </summary>
        private void ProcessPushCombineStackTop(IntGroup viewGroup)
        {
            IntGroup top = IntGroup.Empty;

            if (_viewStack.Peek(out top))
            {
                IntGroup newTop = IntGroup.Combine(top, viewGroup);
                _viewStack.Push(newTop);
            }
            else
            {
                _viewStack.Push(viewGroup);
            }
        }

        /// <summary>
        /// 处理入栈
        /// </summary>
        private void ProcessPushStack(IntGroup viewGroup)
        {
            _viewStack.Push(viewGroup);
        }

        /// <summary>
        /// 处理退出选项
        /// </summary>
        private void ProcessQuitOptions(IntGroup viewGroup, QuitOptions options)
        {
            if (options.HasFlag(QuitOptions.LeaveStack))
            {
                ProcessLeaveStack(viewGroup);
            }
        }

        /// <summary>
        /// 处理出栈
        /// </summary>
        private void ProcessLeaveStack(IntGroup viewGroup)
        {
            _viewStack.Delete(viewGroup);

            for (int i = 0; i < _viewStack.Count; i++)
            {
                IntGroup view = _viewStack[i];

                IntGroup resultView = view - viewGroup;

                _viewStack[i] = resultView;
            }
        }

        /// <summary>
        /// 退出选项过滤器
        /// </summary>
        private QuitOptions QuitOptionsFilter(QuitOptions filter, QuitOptions src)
        {
            return src & (~filter);
        }

        /// <summary>
        /// 进入选项过滤器
        /// </summary>
        private EnterOptions EnterOptionsFilter(EnterOptions filter, EnterOptions src)
        {
            return src & (~filter);
        }

        /// <summary>
        /// 转化选项
        /// </summary>
        private QuitOptions ToOptions(bool leaveStack, bool destroy)
        {
            QuitOptions result = QuitOptions.None;
            if (leaveStack)
            {
                result |= QuitOptions.LeaveStack;
            }
            if (destroy)
            {
                result |= QuitOptions.Destroy;
            }
            return result;
        }
    }
}