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
        /// <param name="pushStack">是否入视图栈</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(int viewId, bool pushStack = true, Action callback = null)
        {
            ViewState state;

            _viewDic.TryGetValue(viewId, out state);

            if (!state.active)
            {
                try
                {
                    OnViewEnterStartEvent?.Invoke(viewId);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                _uiModule?.Enter(viewId, () =>
                {
                    try
                    {
                        callback?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    try
                    {
                        _uiModule?.Focus(viewId);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    try
                    {
                        OnViewEnterCompletedEvent?.Invoke(viewId);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                });

                state.active = true;

                _viewDic[viewId] = state;
            }
            else
            {
                try
                {
                    _uiModule?.Focus(viewId);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
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
        /// <param name="pushStack">是否入视图栈</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(IntGroup viewGroup, bool pushStack = true, Action callback = null)
        {
            int all = viewGroup.Count;
            int done = 0;

            for (int i = 0; i < all; i++)
            {
                Enter(viewGroup[i], false, () =>
                {
                    done++;
                    if (done >= all)
                    {
                        callback?.Invoke();
                    }
                });
            }

            if (pushStack)
            {
                _viewStack.Push(viewGroup);
            }
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="leaveStack">是否出视图栈</param>
        /// <param name="callback">退出完成回调</param>
        /// <param name="destroy">是否销毁视图</param>
        public void Quit(int viewId, bool leaveStack = false, Action callback = null, bool destroy = false)
        {
            ViewState state;

            if (_viewDic.TryGetValue(viewId, out state))
            {
                if (state.active)
                {
                    try
                    {
                        OnViewQuitStartEvent?.Invoke(viewId);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    _uiModule?.Quit(viewId, () =>
                    {
                        try
                        {
                            callback?.Invoke();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        try
                        {
                            _uiModule?.UnFocus(viewId);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        try
                        {
                            OnViewQuitCompletedEvent?.Invoke(viewId);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }, destroy);

                    state.active = false;

                    _viewDic[viewId] = state;
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
        /// <param name="leaveStack">出视图栈</param>
        /// <param name="callback">完成回调</param>
        /// <param name="destroy">销毁</param>
        public void Quit(IntGroup viewGroup, bool leaveStack = false, Action callback = null, bool destroy = false)
        {
            int all = viewGroup.Count;
            int done = 0;

            for (int i = 0; i < all; i++)
            {
                Quit(viewGroup[i], false, () =>
                {
                    done++;
                    if (done >= all)
                    {
                        callback?.Invoke();
                    }
                }, destroy);
            }

            if (leaveStack)
            {
                _viewStack.Delete(viewGroup);
            }
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewId">视图id</param>
        public void UnFocus(int viewId)
        {
            try
            {
                _uiModule?.UnFocus(viewId);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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
        /// <param name="destroy">是否销毁</param>
        public void QuitAll(bool destroy = false)
        {
            _tempQuitList.Clear();

            foreach (int id in _viewDic.Keys)
            {
                _tempQuitList.Add(id);
            }

            for (int i = 0; i < _tempQuitList.Count; i++)
            {
                Quit(_tempQuitList[i], false, null, destroy);
            }

            _viewDic.Clear();
            ResetStack();
        }

        /// <summary>
        /// 退出其他全部视图
        /// </summary>
        /// <param name="stayViewGroup">保留的视图组</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitOtherAll(IntGroup stayViewGroup, bool destroy = false)
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
                Quit(_tempQuitList[i], true, null, destroy);
            }
        }

        /// <summary>
        /// 退出其他全部视图
        /// </summary>
        /// <param name="stayViewId">保留的视图</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitOtherAll(int stayViewId, bool destroy = false)
        {
            _tempQuitList.Clear();

            foreach (int id in _viewDic.Keys)
            {
                if (stayViewId != id)
                {
                    _tempQuitList.Add(id);
                }
            }

            for (int i = 0; i < _tempQuitList.Count; i++)
            {
                Quit(_tempQuitList[i], true, null, destroy);
            }
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

                    if (curActive)
                    {
                        if (_viewStack.Pop(out curId) && _viewStack.Peek(out dstId))
                        {
                            res = true;

                            Quit(curId, false, () =>
                            {
                                Enter(dstId, false, callback);
                            });
                        }
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
        public void Preload(int viewId)
        {
            if (!_viewDic.ContainsKey(viewId))
            {
                _uiModule.Preload(viewId);

                _viewDic[viewId] = new ViewState()
                {
                    active = false
                };
            }
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(IntGroup viewGroup)
        {
            for (int i = 0; i < viewGroup.Count; i++)
            {
                Preload(viewGroup[i]);
            }
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
