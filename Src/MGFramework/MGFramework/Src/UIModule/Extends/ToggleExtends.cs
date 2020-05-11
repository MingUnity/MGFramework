using System;
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
        public static Toggle AddValueChangedListener(this Toggle toggle, Action<bool> onValueChanged)
        {
            toggle.onValueChanged.AddListener((val) => onValueChanged?.Invoke(val));

            return toggle;
        }
    }
}
