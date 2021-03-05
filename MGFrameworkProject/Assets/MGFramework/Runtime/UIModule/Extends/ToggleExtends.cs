using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 选择框扩展
    /// </summary>
    public static class ToggleExtends
    {
        /// <summary>
        /// 添加值改变监听
        /// </summary>
        public static Toggle AddValueChangedListener(this Toggle toggle, UnityAction<bool> onValueChanged)
        {
            toggle.onValueChanged.AddListener(onValueChanged);

            return toggle;
        }

        /// <summary>
        /// 移除值改变监听
        /// </summary>
        public static Toggle RemoveValueChangedListener(this Toggle toggle,UnityAction<bool> onValueChanged)
        {
            toggle.onValueChanged.RemoveListener(onValueChanged);

            return toggle;
        }
    }
}
