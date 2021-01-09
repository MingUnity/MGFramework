using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 基础对象池中的对象
    /// </summary>
    public abstract class BasicPoolObject : IPoolObject
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
        /// 设置父节点
        /// </summary>
        public void SetParent(Transform parent)
        {
            if (ReferenceEquals(parent, null))
            {
                return;
            }

            _root.SetParent(parent, false);
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