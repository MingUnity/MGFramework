﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 通用输入模块
    /// </summary>
    public class SuperInputModule : StandaloneInputModule
    {
        /// <summary>
        /// 当前pointerEventData
        /// </summary>
        private PointerEventData _pointerEventData;

        /// <summary>
        /// 使用自定义射线
        /// </summary>
        public bool useCustomRay = true;

        public override void Process()
        {
            if (useCustomRay)
            {
                if (_pointerEventData == null)
                {
                    _pointerEventData = new PointerEventData(eventSystem);
                }

                bool usedEvent = SendUpdateEventToSelectedObject();

                GameObject prevObject = _pointerEventData.pointerCurrentRaycast.gameObject;

                _pointerEventData.Reset();

                CastRay(_pointerEventData);
                DispatchTrigger(_pointerEventData);
                DispatchPointerHover(_pointerEventData, prevObject);

                if (eventSystem.sendNavigationEvents)
                {
                    if (!usedEvent)
                    {
                        usedEvent |= SendMoveEventToSelectedObject();
                    }

                    if (!usedEvent)
                    {
                        SendSubmitEventToSelectedObject();
                    }
                }
            }
            else
            {
                base.Process();
            }
        }

        /// <summary>
        /// 交互输入响应处理
        /// </summary>
        private void DispatchTrigger(PointerEventData eventData)
        {
            bool triggerDown = false;
            bool triggerUp = false;

            InputManager.InputUpdate(out triggerDown, out triggerUp);

            ProcessPress(eventData, triggerDown, triggerUp);
            ProcessMove(eventData);
            ProcessDrag(eventData);

            GameObject interactiveObj = GetInteractiveObj(eventData.pointerCurrentRaycast.gameObject);

            if (triggerDown)
            {
                SuperPointerListener.InvokePointerDown(eventData.pointerCurrentRaycast, interactiveObj);
            }

            if (triggerUp)
            {
                SuperPointerListener.InvokePointerUp(eventData.pointerCurrentRaycast, interactiveObj);
            }
        }

        /// <summary>
        /// 处理点击
        /// </summary>
        private void ProcessPress(PointerEventData pointerEventData, bool pressed, bool released)
        {
            GameObject curObject = pointerEventData.pointerCurrentRaycast.gameObject;

            if (pressed)
            {
                ModifyFocus(pointerEventData);

                pointerEventData.eligibleForClick = true;
                pointerEventData.delta = Vector2.zero;
                pointerEventData.dragging = false;
                pointerEventData.useDragThreshold = true;
                pointerEventData.pressPosition = pointerEventData.position;
                pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;

                DeselectIfSelectionChanged(curObject, pointerEventData);

                var newPressed = ExecuteEvents.ExecuteHierarchy(curObject, pointerEventData, ExecuteEvents.pointerDownHandler);

                if (newPressed == null)
                {
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(curObject);
                }

                float time = Time.unscaledTime;

                if (newPressed == pointerEventData.lastPress)
                {
                    float diffTime = time - pointerEventData.clickTime;
                    if (diffTime < 0.3f)
                    {
                        ++pointerEventData.clickCount;
                    }
                    else
                    {
                        pointerEventData.clickCount = 1;
                    }

                    pointerEventData.clickTime = time;
                }
                else
                {
                    pointerEventData.clickCount = 1;
                }

                pointerEventData.pointerPress = newPressed;
                pointerEventData.rawPointerPress = curObject;
                pointerEventData.clickTime = time;
                pointerEventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(curObject);

                if (pointerEventData.pointerDrag != null)
                {
                    ExecuteEvents.Execute(pointerEventData.pointerDrag, pointerEventData, ExecuteEvents.initializePotentialDrag);
                }
            }

            if (released)
            {
                ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerUpHandler);

                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(curObject);

                if (pointerEventData.pointerPress == pointerUpHandler && pointerEventData.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEventData.pointerDrag != null && pointerEventData.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(curObject, pointerEventData, ExecuteEvents.dropHandler);
                }

                pointerEventData.eligibleForClick = false;
                pointerEventData.pointerPress = null;
                pointerEventData.rawPointerPress = null;

                if (pointerEventData.pointerDrag != null && pointerEventData.dragging)
                {
                    ExecuteEvents.Execute(pointerEventData.pointerDrag, pointerEventData, ExecuteEvents.endDragHandler);
                }

                pointerEventData.dragging = false;
                pointerEventData.pointerDrag = null;

                if (curObject != pointerEventData.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerEventData, null);
                    HandlePointerExitAndEnter(pointerEventData, curObject);
                }
            }
        }

        /// <summary>
        /// 改变焦点
        /// </summary>
        private void ModifyFocus(PointerEventData pointerEventData)
        {
            GameObject prevObject = pointerEventData.pointerPressRaycast.gameObject;
            GameObject curObject = pointerEventData.pointerCurrentRaycast.gameObject;

            GameObject prevFocusHandler = ExecuteEvents.GetEventHandler<IFocusHandler>(prevObject);
            GameObject curFocusHandler = ExecuteEvents.GetEventHandler<IFocusHandler>(curObject);

            if (prevFocusHandler != curFocusHandler)
            {
                ExecuteEvents.Execute(prevFocusHandler, pointerEventData, ExecuteEventsEx.UnFocusHandler);
                ExecuteEvents.Execute(curFocusHandler, pointerEventData, ExecuteEventsEx.FocusHandler);
            }
        }

        /// <summary>
        /// 指针悬浮处理
        /// </summary>
        private void DispatchPointerHover(PointerEventData eventData, GameObject prevObject)
        {
            GameObject curObject = eventData.pointerCurrentRaycast.gameObject;

            GameObject interactiveObj = GetInteractiveObj(curObject);

            if (curObject != null && curObject == prevObject)
            {
                SuperPointerListener.InvokePointerHover(eventData.pointerCurrentRaycast, interactiveObj);
            }
            else
            {
                SuperPointerListener.InvokePointerExit(prevObject);

                if (curObject != null)
                {
                    SuperPointerListener.InvokePointerEnter(eventData.pointerCurrentRaycast, interactiveObj);
                }
            }
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        private void CastRay(PointerEventData eventData)
        {
            Vector2 prevPosition = eventData.position;

            m_RaycastResultCache.Clear();

            eventData.position = new Vector2(Screen.width, Screen.height) * 0.5f;

            eventSystem.RaycastAll(eventData, m_RaycastResultCache);

            RaycastResult raycastResult = FindFirstRaycast(m_RaycastResultCache);

            eventData.position = raycastResult.screenPosition;
            eventData.pointerCurrentRaycast = raycastResult;
            eventData.delta = eventData.position - prevPosition;
        }

        /// <summary>
        /// 获取可交互性对象
        /// </summary>
        private GameObject GetInteractiveObj(GameObject obj)
        {
            GameObject dragObj = ExecuteEvents.GetEventHandler<IDragHandler>(obj);
            GameObject clickObj = ExecuteEvents.GetEventHandler<IPointerClickHandler>(obj);

            return clickObj ? clickObj : dragObj ? dragObj : null;
        }
    }
}