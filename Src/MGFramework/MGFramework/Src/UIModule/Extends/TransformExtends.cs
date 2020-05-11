using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// Transform 扩展
    /// </summary>
    public static class TransformExtends
    {
        /// <summary>
        /// 根据路径寻找
        /// </summary>
        public static T Find<T>(this Transform trans, string path) where T : Component
        {
            T t = null;

            Transform target = trans?.Find(path);

            t = target?.GetComponent<T>();

            return t;
        }
    }
}