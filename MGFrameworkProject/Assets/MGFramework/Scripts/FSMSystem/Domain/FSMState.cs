using System.Collections.Generic;

namespace MGFramework.FSM
{
    /// <summary>
    /// FSM状态
    /// </summary>
    public abstract class FSMState : IFSMState
    {
        private Dictionary<string, IFSMState> _transitionDic = new Dictionary<string, IFSMState>();

        /// <summary>
        /// 获取目标状态
        /// </summary>
        public IFSMState this[string trigger]
        {
            get
            {
                IFSMState res = null;

                _transitionDic?.TryGetValue(trigger, out res);

                return res;
            }
            set
            {
                if (_transitionDic != null)
                {
                    _transitionDic[trigger] = value;
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
