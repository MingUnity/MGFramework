namespace MGFramework.InputModule
{
    /// <summary>
    /// 输入键处理
    /// </summary>
    public interface IInputKeyHandler
    {
        /// <summary>
        /// 输入更新
        /// </summary>
        void InputUpdate(out bool pointerDown, out bool pointerUp);
    }
}