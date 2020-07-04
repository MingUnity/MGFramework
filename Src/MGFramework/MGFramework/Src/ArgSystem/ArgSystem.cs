using System;
using System.Collections.Generic;

namespace MGFramework.Args
{
    /// <summary>
    /// 参数存取系统
    /// </summary>
    public static class ArgSystem<T> where T : struct
    {
        /// <summary>
        /// 存储池
        /// </summary>
        private static Dictionary<int, Func<T>> _pool = new Dictionary<int, Func<T>>();

        /// <summary>
        /// 获取数据
        /// </summary>
        public static T Get(int argId)
        {
            Func<T> func = _pool.GetValueAnyway(argId);

            if (func != null)
            {
                return func.Invoke();
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 预存数据
        /// </summary>
        public static void Set(int argId, Func<T> argFunc)
        {
            _pool[argId] = argFunc;
        }

        /// <summary>
        /// 移除
        /// </summary>
        public static void Remove(int argId)
        {
            _pool.Remove(argId);
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        public static void Clear()
        {
            _pool.Clear();
        }
    }
}