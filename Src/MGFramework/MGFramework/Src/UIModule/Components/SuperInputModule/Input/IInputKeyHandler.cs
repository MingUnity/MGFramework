namespace MGFramework.UIModule
{
    /// <summary>
    /// 输入键处理
    /// </summary>
    public interface IInputKeyHandler
    {
        /// <summary>
        /// 按下
        /// </summary>
        bool TriggerDown { get; }

        /// <summary>
        /// 抬起
        /// </summary>
        bool TriggerUp { get; }
    }
}