using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// UGUI按钮扩展
    /// </summary>
    [AddComponentMenu("MGFramework/ButtonEx")]
    public class ButtonEx : Button
    {
        /// <summary>
        /// 文本
        /// </summary>
        public Text text;

        /// <summary>
        /// 普通态 文本颜色
        /// </summary>
        public Color normalTextColor = Color.white;

        /// <summary>
        /// 高亮态 文本颜色
        /// </summary>
        public Color highlightedTextColor = Color.white;

        /// <summary>
        /// 点击态 文本颜色
        /// </summary>
        public Color pressedTextColor = Color.white;

        /// <summary>
        /// 禁用态 文本颜色
        /// </summary>
        public Color disabledTextColor = Color.white;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            switch (state)
            {
                case SelectionState.Normal:
                    if (text != null)
                    {
                        text.color = normalTextColor;
                    }
                    break;

                case SelectionState.Highlighted:
                    if (text != null)
                    {
                        text.color = highlightedTextColor;
                    }
                    break;

                case SelectionState.Pressed:
                    if (text != null)
                    {
                        text.color = pressedTextColor;
                    }
                    break;

                case SelectionState.Disabled:
                    if (text != null)
                    {
                        text.color = disabledTextColor;
                    }
                    break;
            }
        }
    }
}
