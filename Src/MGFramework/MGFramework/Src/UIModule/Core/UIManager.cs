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

        public UIManager()
        {
            _module = new PopUIModule();
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="pushStack">是否入视图栈</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(int viewId, bool pushStack = true, Action callback = null)
        {
            _module.Enter(viewId, pushStack, callback);
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="pushStack">是否入视图栈</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter(IntGroup viewGroup, bool pushStack = true, Action callback = null)
        {
            _module.Enter(viewGroup, pushStack, callback);
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
            _module.Quit(viewId, leaveStack, callback, destroy);
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
            _module.Quit(viewGroup, leaveStack, callback, destroy);
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
        /// <param name="destroy">是否销毁</param>
        public void QuitAll(bool destroy = false)
        {
            _module.QuitAll(destroy);
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