using System;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 按钮扩展
    /// </summary>
    public static class ButtonExtends
    {
        /// <summary>
        /// 点击效果
        /// </summary>
        public static IPointerClickEffect pointerClickEffect;

        /// <summary>
        /// 添加点击监听
        /// </summary>
        public static Button AddClickListener(this Button button, Action onClick)
        {
            button.onClick.AddListener(() =>
            {
                pointerClickEffect?.OnClickEffect(button);

                onClick?.Invoke();
            });

            return button;
        }
    }
}