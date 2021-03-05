using System.Collections.Generic;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 射线管理
    /// </summary>
    internal static class RayManager
    {
        /// <summary>
        /// 射线列表
        /// </summary>
        private readonly static List<ICustomRay> _rayList = new List<ICustomRay>();

        /// <summary>
        /// 自定义射线优先级比较
        /// </summary>
        private readonly static IComparer<ICustomRay> _comparer = new CustomRayComparer();

        /// <summary>
        /// 当前优先级最高的自定义射线
        /// </summary>
        public static ICustomRay CurrentRay => _rayList.Count == 0 ? null : _rayList[_rayList.Count - 1];

        /// <summary>
        /// 替换自定义射线
        /// </summary>
        public static void AddRay(ICustomRay ray)
        {
            if (!_rayList.Contains(ray))
            {
                _rayList.Add(ray);
                Sort();
            }
        }

        /// <summary>
        /// 销毁当前自定义射线
        /// </summary>
        public static void RemoveRay(ICustomRay ray)
        {
            _rayList.Remove(ray);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public static void Refresh()
        {
            Sort();
        }

        /// <summary>
        /// 排序
        /// </summary>
        private static void Sort()
        {
            _rayList.Sort(_comparer);
        }

        private class CustomRayComparer : IComparer<ICustomRay>
        {
            public int Compare(ICustomRay x, ICustomRay y)
            {
                if (x != null && y != null)
                {
                    return y.Priority > x.Priority ? -1 : 1;
                }

                return 0;
            }
        }
    }
}