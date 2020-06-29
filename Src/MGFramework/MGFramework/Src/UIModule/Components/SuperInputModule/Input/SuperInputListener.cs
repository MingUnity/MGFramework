using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 输入模块事件监听
    /// </summary>
    public static class SuperInputListener
    {
        public delegate void OnPointerEnterDelegate(RaycastResult raycastResult, bool interactive);
        public delegate void OnPointerHoverDelegate(RaycastResult raycastResult, bool interactive);
        public delegate void OnPointerExitDelegate(GameObject prevObject);
        public delegate void OnPointerDownDelegate(RaycastResult raycastResult, bool interactive);
        public delegate void OnPointerUpDelegate(RaycastResult raycastResult, bool interactive);

        public static event OnPointerEnterDelegate OnPointerEnterEvent;
        public static event OnPointerHoverDelegate OnPointerHoverEvent;
        public static event OnPointerExitDelegate OnPointerExitEvent;
        public static event OnPointerDownDelegate OnPointerDownEvent;
        public static event OnPointerUpDelegate OnPointerUpEvent;

        internal static void InvokePointerEnter(RaycastResult raycastResult, bool interactive)
        {
            OnPointerEnterEvent?.Invoke(raycastResult, interactive);
        }

        internal static void InvokePointerHover(RaycastResult raycastResult, bool interactive)
        {
            OnPointerHoverEvent?.Invoke(raycastResult, interactive);
        }

        internal static void InvokePointerExit(GameObject prevObject)
        {
            OnPointerExitEvent?.Invoke(prevObject);
        }

        internal static void InvokePointerDown(RaycastResult raycastResult, bool interactive)
        {
            OnPointerDownEvent?.Invoke(raycastResult, interactive);
        }

        internal static void InvokePointerUp(RaycastResult raycastResult, bool interactive)
        {
            OnPointerUpEvent?.Invoke(raycastResult, interactive);
        }
    }
}