using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// Slider扩展
    /// </summary>
    public class SliderEx : Slider
    {
        /// <summary>
        /// 设置值 且不抛事件
        /// </summary>
        public void SetValueWithoutNotify(float value)
        {
            Set(value, false);
        }
    }
}
