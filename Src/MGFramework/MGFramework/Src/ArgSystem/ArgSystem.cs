using System;
using System.Collections.Generic;

namespace MGFramework.Args
{
    /// <summary>
    /// 参数存取系统
    /// </summary>
    public static class ArgSystem<T> where T : struct
    {
        private class FuncHandle
        {
            public int Priority { get; private set; }
            public Func<T> Func { get; private set; }

            public FuncHandle(int priority, Func<T> func)
            {
                Priority = priority;
                Func = func;
            }
        }

        private class FuncHandleComparer : IComparer<FuncHandle>
        {
            public int Compare(FuncHandle x, FuncHandle y)
            {
                if (x != null && y != null)
                {
                    return y.Priority > x.Priority ? -1 : 1;
                }

                return 0;
            }
        }

        /// <summary>
        /// 存储池
        /// </summary>
        private static Dictionary<int, List<FuncHandle>> _pool = new Dictionary<int, List<FuncHandle>>();

        /// <summary>
        /// 优先级比较
        /// </summary>
        private readonly static FuncHandleComparer _comparer = new FuncHandleComparer();

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="argId">参数id</param>
        public static T Get(int argId)
        {
            List<FuncHandle> funcs = _pool.GetValueAnyway(argId);

            if (funcs != null && funcs.Count > 0)
            {
                FuncHandle highest = funcs[funcs.Count - 1];

                if (highest?.Func != null)
                {
                    return highest.Func.Invoke();
                }
            }

            return default(T);
        }

        /// <summary>
        /// 预存数据
        /// </summary>
        /// <param name="argId">参数id</param>
        /// <param name="argFunc">获取参数委托</param>
        /// <param name="priority">优先级</param>
        public static void Set(int argId, Func<T> argFunc, int priority = 0)
        {
            List<FuncHandle> funcs = _pool.GetValueAnyway(argId);

            if (funcs == null)
            {
                funcs = new List<FuncHandle>();
            }

            funcs.Add(new FuncHandle(priority, argFunc));

            Sort(funcs);

            _pool[argId] = funcs;
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

        /// <summary>
        /// 排序
        /// </summary>
        private static void Sort(List<FuncHandle> funcHandles)
        {
            funcHandles?.Sort(_comparer);
        }
    }

    public static class ArgSystem<Param, Result> where Param : struct where Result : struct
    {
        private class FuncHandle
        {
            public int Priority { get; private set; }
            public Func<Param, Result> Func { get; private set; }

            public FuncHandle(int priority, Func<Param, Result> func)
            {
                Priority = priority;
                Func = func;
            }
        }

        private class FuncHandleComparer : IComparer<FuncHandle>
        {
            public int Compare(FuncHandle x, FuncHandle y)
            {
                if (x != null && y != null)
                {
                    return y.Priority > x.Priority ? -1 : 1;
                }

                return 0;
            }
        }

        private static Dictionary<int, List<FuncHandle>> _pool = new Dictionary<int, List<FuncHandle>>();

        private readonly static FuncHandleComparer _comparer = new FuncHandleComparer();

        public static Result Get(int argId, Param param)
        {
            List<FuncHandle> funcs = _pool.GetValueAnyway(argId);

            if (funcs != null && funcs.Count > 0)
            {
                FuncHandle highest = funcs[funcs.Count - 1];

                if (highest?.Func != null)
                {
                    return highest.Func.Invoke(param);
                }
            }

            return default(Result);
        }

        public static void Set(int argId, Func<Param, Result> argFunc, int priority = 0)
        {
            List<FuncHandle> funcs = _pool.GetValueAnyway(argId);

            if (funcs == null)
            {
                funcs = new List<FuncHandle>();
            }

            funcs.Add(new FuncHandle(priority, argFunc));

            Sort(funcs);

            _pool[argId] = funcs;
        }

        public static void Remove(int argId)
        {
            _pool.Remove(argId);
        }

        public static void Clear()
        {
            _pool.Clear();
        }

        /// <summary>
        /// 排序
        /// </summary>
        private static void Sort(List<FuncHandle> funcHandles)
        {
            funcHandles?.Sort(_comparer);
        }
    }
}