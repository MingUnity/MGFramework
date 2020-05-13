using System.Collections.Generic;

namespace MGFramework.FSM
{
    /// <summary>
    /// FSM状态
    /// </summary>
    public abstract class FSMState : IFSMState
    {
        private Dictionary<int, IFSMState> _transitionDic = new Dictionary<int, IFSMState>();

        /// <summary>
        /// 获取目标状态
        /// </summary>
        public IFSMState this[int transition]
        {
            get
            {
                IFSMState res = null;

                _transitionDic?.TryGetValue(transition, out res);

                return res;
            }
            set
            {
                if (_transitionDic != null)
                {
                    _transitionDic[transition] = value;
                }
            }
        }

        /// <summary>
        /// 进入
        /// </summary>
        public abstract void OnEnter(params object[] keys);

        /// <summary>
        /// 退出
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// 驻留
        /// </summary>
        public abstract void OnStay();
    }
}
