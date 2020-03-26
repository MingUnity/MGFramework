using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// CanvasGroup扩展
    /// </summary>
    public static class CanvasGroupExtends
    {
        /// <summary>
        /// 显示隐藏
        /// </summary>
        public static void SetActive(this CanvasGroup canvasGroup, bool active)
        {
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
        }

        /// <summary>
        /// 是否激活
        /// </summary>
        public static bool IsActive(this CanvasGroup canvasGroup)
        {
            return canvasGroup.alpha > 0;
        }
    }
}
