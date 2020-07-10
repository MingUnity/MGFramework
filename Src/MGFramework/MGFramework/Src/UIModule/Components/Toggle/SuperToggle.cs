using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    [AddComponentMenu("MGFramework/SuperToggle")]
    public class SuperToggle : Toggle
    {
        /// <summary>
        /// 图形元素
        /// </summary>
        [Serializable]
        public class Item
        {
            /// <summary>
            /// 图形对象
            /// </summary>
            public Graphic graphic;

            /// <summary>
            /// 使用通用颜色变化
            /// </summary>
            public bool useGeneral = true;

            /// <summary>
            /// 选中颜色
            /// </summary>
            public Color isOnColor = Color.white;

            /// <summary>
            /// 三态颜色
            /// </summary>
            public ColorBlock color = DefaultColor;
        }

        /// <summary>
        /// 图形集合
        /// </summary>
        public Item[] graphicItems = new Item[1];

        /// <summary>
        /// 选中态图形颜色
        /// </summary>
        public Color generalIsOnColor = Color.white;

        /// <summary>
        /// 通用颜色
        /// </summary>
        public ColorBlock generalColor = DefaultColor;

        /// <summary>
        /// 默认颜色
        /// </summary>
        private static ColorBlock DefaultColor
        {
            get
            {
                return new ColorBlock()
                {
                    normalColor = Color.white,
                    highlightedColor = Color.white,
                    pressedColor = Color.white,
                    disabledColor = Color.white,
                    colorMultiplier = 1,
                    fadeDuration = 0
                };
            }
        }

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

            if (isOn || graphicItems == null || graphicItems.Length <= 0)
            {
                return;
            }

            switch (state)
            {
                case SelectionState.Normal:
                    ProcessNormal();
                    break;

                case SelectionState.Highlighted:
                    ProcessHighlighted();
                    break;

                case SelectionState.Pressed:
                    ProcessPressed();
                    break;

                case SelectionState.Disabled:
                    ProcessDisabled();
                    break;
            }
        }

        private void ProcessDisabled()
        {
            ForEachGraphicColor((g, clr) => CrossFadeGraphicColor(g, clr.disabledColor, clr.fadeDuration));
        }

        private void ProcessPressed()
        {
            ForEachGraphicColor((g, clr) => CrossFadeGraphicColor(g, clr.pressedColor, clr.fadeDuration));
        }

        private void ProcessHighlighted()
        {
            ForEachGraphicColor((g, clr) => CrossFadeGraphicColor(g, clr.highlightedColor, clr.fadeDuration));
        }

        private void ProcessNormal()
        {
            ForEachGraphicColor((g, clr) => CrossFadeGraphicColor(g, clr.normalColor, clr.fadeDuration));
        }

        private void CrossFadeGraphicColor(Graphic graphic, Color color, float duration)
        {
            graphic?.CrossFadeColor(color, duration, true, true);
        }

        private void ForEachGraphicColor(Action<Graphic, ColorBlock> callback)
        {
            for (int i = 0; i < graphicItems.Length; i++)
            {
                Item item = graphicItems[i];

                if (item != null)
                {
                    callback?.Invoke(item.graphic, item.useGeneral ? generalColor : item.color);
                }
            }
        }

        private void OnValueChanged(bool value)
        {
            if (graphicItems != null && graphicItems.Length > 0)
            {
                for (int i = 0; i < graphicItems.Length; i++)
                {
                    Item item = graphicItems[i];

                    if (item != null)
                    {
                        ColorBlock targetClrBlock = item.useGeneral ? generalColor : item.color;
                        Color targetClr = value ? (item.useGeneral ? generalIsOnColor : item.isOnColor) : targetClrBlock.normalColor;

                        item.graphic?.CrossFadeColor(targetClr, targetClrBlock.fadeDuration, true, true);
                    }
                }
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            UpdateSelectionState(eventData);
        }
    }
}
