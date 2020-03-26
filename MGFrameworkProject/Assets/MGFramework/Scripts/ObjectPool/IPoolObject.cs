using UnityEngine;

namespace MGFramework.Pool
{
    /// <summary>
    /// 池中对象
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
        void Create(Transform root);

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}