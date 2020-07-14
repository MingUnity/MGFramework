using System;
using System.Collections.Generic;

namespace MGFramework.UIModule
{
    /// <summary>
    /// UI管理
    /// </summary>
    public sealed class UIManager : Singleton<UIManager>, IPopUIModule
    {
        private IPopUIModule _module;

        /// <summary>
        /// 进入视图开始事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewEnterStartEvent
        {
            add
            {
                _module.OnViewEnterStartEvent += value;
            }
            remove
            {
                _module.OnViewEnterStartEvent -= value;
            }
        }

        /// <summary>
        /// 进入视图完成事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewEnterCompletedEvent
        {
            add
            {
                _module.OnViewEnterCompletedEvent += value;
            }
            remove
            {
                _module.OnViewEnterCompletedEvent -= value;
            }
        }

        /// <summary>
        /// 退出视图开始事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewQuitStartEvent
        {
            add
            {
                _module.OnViewQuitStartEvent += value;
            }
            remove
            {
                _module.OnViewQuitStartEvent -= value;
            }
        }

        /// <summary>
        /// 退出视图完成事件
        /// </summary>
        public event OnViewSwitchDelegate OnViewQuitCompletedEvent
        {
            add
            {
                _module.OnViewQuitCompletedEvent += value;
            }
            remove
            {
                _module.OnViewQuitCompletedEvent -= value;
            }
        }

        public UIManager()
        {
            _module = new PopUIModule();
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="options">选项</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(int viewId, EnterOptions options = EnterOptions.None, Action callback = null)
        {
            _module.Enter(viewId, options, callback);
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="options">选项</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(IntGroup viewGroup, EnterOptions options = EnterOptions.None, Action callback = null)
        {
            _module.Enter(viewGroup, options, callback);
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="options">选项</param>
        /// <param name="callback">退出完成回调</param>
        public void Quit(int viewId, QuitOptions options = QuitOptions.None, Action callback = null)
        {
            _module.Quit(viewId, options, callback);
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="options">选项</param>
        /// <param name="callback">完成回调</param>
        public void Quit(IntGroup viewGroup, QuitOptions options = QuitOptions.None, Action callback = null)
        {
            _module.Quit(viewGroup, options, callback);
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewId">视图id</param>
        public void UnFocus(int viewId)
        {
            _module.UnFocus(viewId);
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        public void UnFocus(IntGroup viewGroup)
        {
            _module.UnFocus(viewGroup);
        }

        /// <summary>
        /// 退出所有视图
        /// </summary>
        public void QuitAll(bool destory = false)
        {
            _module.QuitAll(destory);
        }

        /// <summary>
        /// 退出所有视图
        /// </summary>
        /// <param name="stayStackId">保持在堆栈内的视图id</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitAll(int stayStackId, bool destroy = false)
        {
            _module.QuitAll(stayStackId, destroy);
        }

        /// <summary>
        /// 退出所有视图
        /// </summary>
        /// <param name="stayStackGroup">保持在堆栈内的视图组</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitAll(IntGroup stayStackGroup, bool destroy = false)
        {
            _module.QuitAll(stayStackGroup, destroy);
        }

        /// <summary>
        /// 退出其他全部视图
        /// </summary>
        /// <param name="stayViewGroup">保留的视图组</param>
        /// <param name="options">选项</param>
        public void QuitOtherAll(IntGroup stayViewGroup, QuitOptions options = QuitOptions.None)
        {
            _module.QuitOtherAll(stayViewGroup, options);
        }

        /// <summary>
        /// 退出其他全部视图
        /// </summary>
        /// <param name="stayViewId">保留的视图</param>
        /// <param name="options">选项</param>
        public void QuitOtherAll(int stayViewId, QuitOptions options = QuitOptions.None)
        {
            _module.QuitOtherAll(stayViewId, options);
        }

        /// <summary>
        /// 弹出视图
        /// 回到上一级视图
        /// </summary>
        /// <param name="callback">完成回调</param>
        /// <returns>是否弹出成功</returns>
        public bool Pop(Action callback = null)
        {
            return _module.Pop(callback);
        }

        /// <summary>
        /// 清空视图栈
        /// </summary>
        public void ResetStack()
        {
            _module.ResetStack();
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(int viewId)
        {
            _module.Preload(viewId);
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(IntGroup viewGroup)
        {
            _module.Preload(viewGroup);
        }
    }
}