namespace MGFramework.FSM
{
    /// <summary>
    /// 有限状态机接口
    /// </summary>
    public interface IFSMSystem
    {
        /// <summary>
        /// 任意状态
        /// </summary>
        IFSMState AnyState { get; }

        /// <summary>
        /// 添加状态
        /// </summary>
        void AddState(IFSMState state, bool isDefault = false);

        /// <summary>
        /// 设置触发器以过渡状态
        /// </summary>
        void SetTrigger(string trigger, params object[] keys);

        /// <summary>
        /// 跳转默认状态
        /// </summary>
        void TurnDefault();

        /// <summary>
        /// 刷新
        /// </summary>
        void Update();
    }
}
