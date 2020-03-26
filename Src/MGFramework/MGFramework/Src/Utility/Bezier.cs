using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    public static class Bezier
    {
        /// <summary>
        /// 二次贝塞尔公式
        /// </summary>
        /// <returns></returns>
        public static Vector3 CubicLerp(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }
    }
}