namespace MGFramework.FSM
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IFSMState
    {
        /// <summary>
        /// 获取目标状态
        /// </summary>
        IFSMState this[string trigger] { get; set; }

        /// <summary>
        /// 进入
        /// </summary>
        void OnEnter(params object[] keys);

        /// <summary>
        /// 退出
        /// </summary>
        void OnExit();

        /// <summary>
        /// 驻留
        /// </summary>
        void OnStay();
    }
}
