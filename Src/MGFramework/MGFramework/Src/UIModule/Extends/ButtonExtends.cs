using UnityEngine.Events;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 按钮扩展
    /// </summary>
    public static class ButtonExtends
    {
        /// <summary>
        /// 添加点击监听
        /// </summary>
        public static Button AddClickListener(this Button button, UnityAction onClick)
        {
            button.onClick.AddListener(onClick);

            return button;
        }

        /// <summary>
        /// 移除点击监听
        /// </summary>
        public static Button RemoveClickListener(this Button button,UnityAction onClick)
        {
            button.onClick.RemoveListener(onClick);

            return button;
        }
    }
}