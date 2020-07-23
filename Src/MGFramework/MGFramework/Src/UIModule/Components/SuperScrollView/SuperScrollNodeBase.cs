using UnityEngine;

namespace MGFramework.UIModule
{
    public abstract class SuperScrollNodeBase : ISuperScrollNode
    {
        /// <summary>
        /// 根节点
        /// </summary>
        protected abstract Transform Root { get; }

        /// <summary>
        /// 重置需要异步加载的数据
        /// </summary>
        public abstract void ResetAsyncData();

        /// <summary>
        /// 移至首节点
        /// </summary>
        public virtual void TurnFirst()
        {
            Root?.SetAsFirstSibling();
        }

        /// <summary>
        /// 移至尾节点
        /// </summary>
        public virtual void TurnLast()
        {
            Root?.SetAsLastSibling();
        }
    }
}
