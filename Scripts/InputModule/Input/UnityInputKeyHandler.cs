using UnityEngine;

namespace MGFramework.InputModule
{
    /// <summary>
    /// unity输入处理
    /// </summary>
    public sealed class UnityInputKeyHandler : BaseInputKeyHandler
    {
        /// <summary>
        /// 输入更新
        /// </summary>
        public override void InputUpdate(out bool pointerDown, out bool pointerUp)
        {
            pointerDown = Input.GetMouseButtonDown(0);

            if (!pointerDown)
            {
                TouchPhase phase = TouchPhase.Began;

                pointerDown = GetFirstTouchPhase(out phase) && phase == TouchPhase.Began;
            }

            pointerUp = Input.GetMouseButtonUp(0);

            if (!pointerUp)
            {
                TouchPhase phase = TouchPhase.Began;

                pointerUp = GetFirstTouchPhase(out phase) && (phase == TouchPhase.Canceled || phase == TouchPhase.Ended);
            }
        }

        /// <summary>
        /// 获取第一个手指触碰的phase
        /// </summary>
        private bool GetFirstTouchPhase(out TouchPhase touchPhase)
        {
            bool res = false;

            touchPhase = TouchPhase.Began;

            Touch[] touch = Input.touches;

            if (touch != null && touch.Length > 0)
            {
                Touch first = touch[0];

                touchPhase = first.phase;

                res = true;
            }

            return res;
        }
    }
}
