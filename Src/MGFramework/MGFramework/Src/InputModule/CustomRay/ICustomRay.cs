using UnityEngine;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 自定义射线
    /// </summary>
    public interface ICustomRay
    {
        /// <summary>
        /// 优先级
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 射线
        /// </summary>
        Ray Ray { get; }
    }
}