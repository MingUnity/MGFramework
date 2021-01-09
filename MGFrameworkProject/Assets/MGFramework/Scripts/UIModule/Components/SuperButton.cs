using System;
using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    [AddComponentMenu("MGFramework/SuperButton")]
    public class SuperButton : Button
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
            /// 三态颜色
            /// </summary>
            public ColorBlock color = DefaultColor;
        }

        /// <summary>
        /// 图形集合
        /// </summary>
        public Item[] graphicItems = new Item[1];

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

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (graphicItems == null || graphicItems.Length <= 0)
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
    }
}