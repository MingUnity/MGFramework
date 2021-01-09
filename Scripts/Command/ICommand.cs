namespace MGFramework.Command
{
    /// <summary>
    /// 命令抽象接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        void Execute();

        /// <summary>
        /// 撤销
        /// </summary>
        void Undo();
    }
}
