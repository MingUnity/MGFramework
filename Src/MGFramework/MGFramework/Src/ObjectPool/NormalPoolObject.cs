using UnityEngine;

namespace MGFramework.Pool
{
    /// <summary>
    /// 通用池中对象
    /// </summary>
    public abstract class NormalPoolObject : IPoolObject
    {
        /// <summary>
        /// 根节点
        /// </summary>
        protected Transform _root;

        /// <summary>
        /// 激活
        /// </summary>
        public bool Active
        {
            get
            {
                return _root.gameObject.activeSelf;
            }
            set
            {
                _root.gameObject.SetActive(value);

                if (value)
                {
                    _root.SetAsLastSibling();

                    OnShow();
                }
                else
                {
                    OnHide();
                }
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create(Transform root)
        {
            _root = root;
            OnCreate();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 创建
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// 显示
        /// </summary>
        protected virtual void OnShow() { }

        /// <summary>
        /// 隐藏
        /// </summary>
        protected virtual void OnHide() { }
    }
}