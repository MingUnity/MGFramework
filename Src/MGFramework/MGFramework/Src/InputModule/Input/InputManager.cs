using System.Collections.Generic;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 输入管理
    /// </summary>
    internal static class InputManager
    {
        /// <summary>
        /// 输入键处理器
        /// </summary>
        private static readonly List<IInputKeyHandler> _inputKeyHandlerList = new List<IInputKeyHandler>();

        /// <summary>
        /// 添加
        /// </summary>
        public static void Add(IInputKeyHandler handler)
        {
            if (!_inputKeyHandlerList.Contains(handler))
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

        /// <summary>
        /// 输入更新
        /// </summary>
        public static void InputUpdate(out bool pointerDown,out bool pointerUp)
        {
            pointerDown = false;
            pointerUp = false;

            for (int i = 0; i < _inputKeyHandlerList.Count; i++)
            {
                bool tmpDown = false;
                bool tmpUp = false; 

                _inputKeyHandlerList[i].InputUpdate(out tmpDown,out tmpUp);

                //有一个输入对象是pointerDown/Up即视为当前帧完成down/up输入
                pointerDown = pointerDown ? true : tmpDown;
                pointerUp = pointerUp ? true : tmpUp;
            }
        }
    }
}