using System.Collections.Generic;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 输入管理
    /// </summary>
    public static class InputManager
    {
        /// <summary>
        /// 输入键处理器
        /// </summary>
        private static readonly List<IInputKeyHandler> _inputKeyHandlerList = new List<IInputKeyHandler>();

        /// <summary>
        /// 按下
        /// </summary>
        internal static bool TriggerDown
        {
            get
            {
                bool res = false;

                for (int i = 0; i < _inputKeyHandlerList.Count; i++)
                {
                    res |= _inputKeyHandlerList[i].TriggerDown;
                }

                return res;
            }
        }

        /// <summary>
        /// 抬起
        /// </summary>
        internal static bool TriggerUp
        {
            get
            {
                bool res = false;

                for (int i = 0; i < _inputKeyHandlerList.Count; i++)
                {
                    res |= _inputKeyHandlerList[i].TriggerUp;
                }

                return res;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        public static void Add(IInputKeyHandler handler)
        {
            if(!_inputKeyHandlerList.Contains(handler))
            {
                _inputKeyHandlerList.Add(handler);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        public static void Remove(IInputKeyHandler handler)
        {
            _inputKeyHandlerList.Remove(handler);
        }
    }
}