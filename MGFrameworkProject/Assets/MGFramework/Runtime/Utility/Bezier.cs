using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 绘制n阶贝塞尔曲线路径
        /// </summary>
        /// <param name="poss">控制点数组</param>
        /// <param name="precision">输出数量</param>
        public static Vector3[] Caculate(Vector3[] poss, int precision)
        {
            //贝塞尔曲线控制点数（阶数）
            int number = poss.Length;

            //控制点数不小于 2
            if (number < 2)
                return null;

            //计算杨辉三角
            int[] mi = new int[number];
            mi[0] = mi[1] = 1;
            for (int i = 3; i <= number; i++)
            {
                int[] t = new int[i - 1];
                for (int j = 0; j < t.Length; j++)
                {
                    t[j] = mi[j];
                }

                mi[0] = mi[i - 1] = 1;
                for (int j = 0; j < i - 2; j++)
                {
                    mi[j + 1] = t[j] + t[j + 1];
                }
            }

            Vector3[] result = new Vector3[precision];

            for (int i = 0; i < precision; i++)
            {
                float t = i * 1.0f / precision;
                double x = 0;
                double y = 0;
                double z = 0;
                for (int j = 0; j < number; j++)
                {
                    Vector3 p = poss[j];
                    double scale = Math.Pow(1 - t, number - j - 1) * Math.Pow(t, j) * mi[j];

                    x += scale * p.x;
                    y += scale * p.y;
                    z += scale * p.z;
                }
                result[i] = new Vector3((float)x, (float)y, (float)z);
            }

            return result;
        }
    }
}