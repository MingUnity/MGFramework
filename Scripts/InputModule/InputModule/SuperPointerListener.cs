using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 指针模块事件监听
    /// </summary>
    public static class SuperPointerListener
    {
        public delegate void OnPointerEnterDelegate(RaycastResult raycastResult, GameObject interactiveObj);
        public delegate void OnPointerHoverDelegate(RaycastResult raycastResult, GameObject interactiveObj);
        public delegate void OnPointerExitDelegate(GameObject prevObject);
        public delegate void OnPointerDownDelegate(RaycastResult raycastResult, GameObject interactiveObj);
        public delegate void OnPointerUpDelegate(RaycastResult raycastResult, GameObject interactiveObj);

        public static event OnPointerEnterDelegate OnPointerEnterEvent;
        public static event OnPointerHoverDelegate OnPointerHoverEvent;
        public static event OnPointerExitDelegate OnPointerExitEvent;
        public static event OnPointerDownDelegate OnPointerDownEvent;
        public static event OnPointerUpDelegate OnPointerUpEvent;

        internal static void InvokePointerEnter(RaycastResult raycastResult, GameObject interactiveObj)
        {
            OnPointerEnterEvent?.Invoke(raycastResult, interactiveObj);
        }

        internal static void InvokePointerHover(RaycastResult raycastResult, GameObject interactiveObj)
        {
            OnPointerHoverEvent?.Invoke(raycastResult, interactiveObj);
        }

        internal static void InvokePointerExit(GameObject prevObject)
        {
            OnPointerExitEvent?.Invoke(prevObject);
        }

        internal static void InvokePointerDown(RaycastResult raycastResult, GameObject interactiveObj)
        {
            OnPointerDownEvent?.Invoke(raycastResult, interactiveObj);
        }

        internal static void InvokePointerUp(RaycastResult raycastResult, GameObject interactiveObj)
        {
            OnPointerUpEvent?.Invoke(raycastResult, interactiveObj);
        }
    }
}