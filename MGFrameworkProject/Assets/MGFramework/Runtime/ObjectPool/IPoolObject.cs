using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 对象池中的对象
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// 激活
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="root">根节点</param>
        void Create(Transform root);

        /// <summary>
        /// 设置父节点
        /// </summary>
        void SetParent(Transform parent);

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}