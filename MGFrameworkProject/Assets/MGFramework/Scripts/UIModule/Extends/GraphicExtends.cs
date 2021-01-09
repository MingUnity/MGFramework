using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// UI图形组件扩展
    /// </summary>
    public static class GraphicExtends
    {
        /// <summary>
        /// 设置不透明度
        /// </summary>
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            Color clr = graphic.color;

            clr.a = alpha;

            graphic.color = clr;
        }

        /// <summary>
        /// 获取不透明度
        /// </summary>
        public static float GetAlpha(this Graphic graphic)
        {
            return graphic.color.a;
        }
    }
}
