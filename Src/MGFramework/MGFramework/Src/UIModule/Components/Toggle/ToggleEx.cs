using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// Toggle扩展
    /// </summary>
    [Obsolete("Please use SuperToggle instead")]
    public class ToggleEx : Toggle
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
        /// 悬浮态 文本颜色
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

        /// <summary>
        /// 选中态 文本颜色
        /// </summary>
        public Color isOnTextColor = Color.white;

        protected override void OnEnable()
        {
            base.OnEnable();

            OnValueChanged(isOn);

            onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onValueChanged.RemoveListener(OnValueChanged);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!isOn)
            {
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

        private void OnValueChanged(bool value)
        {
            if (text != null)
            {
                if (value)
                {
                    text.color = isOnTextColor;
                }
                else
                {
                    text.color = normalTextColor;
                }
            }
        }
    }
}
