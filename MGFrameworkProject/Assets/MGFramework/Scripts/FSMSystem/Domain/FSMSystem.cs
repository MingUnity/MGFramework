using System.Collections.Generic;

namespace MGFramework.FSM
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public sealed class FSMSystem : IFSMSystem
    {
        /// <summary>
        /// 状态列表
        /// </summary>
        private List<IFSMState> _stateList = new List<IFSMState>();

        /// <summary>
        /// 当前状态
        /// </summary>
        private IFSMState _curState;

        /// <summary>
        /// 默认状态
        /// </summary>
        private IFSMState _defaultState;

        /// <summary>
        /// 任意状态
        /// </summary>
        private IFSMState _anyState = new AnyState();

        /// <summary>
        /// 任意状态
        /// </summary>
        public IFSMState AnyState => _anyState;

        /// <summary>
        /// 添加状态
        /// </summary>
        public void AddState(IFSMState state, bool isDefault = false)
        {
            if (state != null)
            {
                _stateList?.Add(state);

                if (isDefault)
                {
                    _defaultState = state;

                    _curState = state;

                    state.OnEnter();
                }
            }
        }

        /// <summary>
        /// 跳转默认状态
        /// </summary>
        public void TurnDefault()
        {
            TurnState(_curState, _defaultState);
        }

        /// <summary>
        /// 设置触发器以过渡状态
        /// </summary>
        public void SetTrigger(string trigger, params object[] keys)
        {
            IFSMState targetState = _anyState[trigger];

            if (targetState == null && _curState != null)
            {
                targetState = _curState[trigger];
            }

            TurnState(_curState, targetState, keys);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Update()
        {
            _curState?.OnStay();
        }

        /// <summary>
        /// 跳转状态
        /// </summary>
        private void TurnState(IFSMState oldState, IFSMState newState, params object[] keys)
        {
            if (newState != null && _stateList != null && _stateList.Contains(newState))
            {
                oldState?.OnExit();

                _curState = newState;

                newState.OnEnter(keys);
            }
        }
    }
}
