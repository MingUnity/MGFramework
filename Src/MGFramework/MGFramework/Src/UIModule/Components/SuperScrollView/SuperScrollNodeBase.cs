using UnityEngine;

namespace MGFramework.UIModule
{
    public abstract class SuperScrollNodeBase : ISuperScrollNode
    {
        protected abstract Transform Root { get; }

        public abstract void ResetAsyncData();

        public virtual void TurnFirst()
        {
            Root?.SetAsFirstSibling();
        }

        public virtual void TurnLast()
        {
            Root?.SetAsLastSibling();
        }
    }
}
