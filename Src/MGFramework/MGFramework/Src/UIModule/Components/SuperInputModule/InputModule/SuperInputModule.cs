using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGFramework.UIModule
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
                CastRay();
                DispatchTrigger();
                DispatchPointerHover(prevObject);

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
        private void DispatchTrigger()
        {
            bool triggerDown = false;
            bool triggerUp = false;

            InputManager.InputUpdate(out triggerDown, out triggerUp);

            ProcessPress(_pointerEventData, triggerDown, triggerUp);
            ProcessMove(_pointerEventData);
            ProcessDrag(_pointerEventData);

            bool interactive = GetInteractive(_pointerEventData.pointerCurrentRaycast.gameObject);

            if (triggerDown)
            {
                SuperInputListener.InvokePointerDown(_pointerEventData.pointerCurrentRaycast, interactive);
            }

            if (triggerUp)
            {
                SuperInputListener.InvokePointerUp(_pointerEventData.pointerCurrentRaycast, interactive);
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
        /// 指针悬浮处理
        /// </summary>
        private void DispatchPointerHover(GameObject prevObject)
        {
            GameObject curObject = _pointerEventData.pointerCurrentRaycast.gameObject;

            bool interactive = GetInteractive(curObject);

            if (curObject != null && curObject == prevObject)
            {
                SuperInputListener.InvokePointerHover(_pointerEventData.pointerCurrentRaycast, interactive);
            }
            else
            {
                if (prevObject != null)
                {
                    SuperInputListener.InvokePointerExit(prevObject);
                }

                if (curObject != null)
                {
                    SuperInputListener.InvokePointerEnter(_pointerEventData.pointerCurrentRaycast, interactive);
                }
            }
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        private void CastRay()
        {
            Vector2 prevPosition = _pointerEventData.position;

            m_RaycastResultCache.Clear();

            _pointerEventData.position = new Vector2(Screen.width, Screen.height) * 0.5f;

            eventSystem.RaycastAll(_pointerEventData, m_RaycastResultCache);

            RaycastResult raycastResult = FindFirstRaycast(m_RaycastResultCache);

            _pointerEventData.position = raycastResult.screenPosition;
            _pointerEventData.pointerCurrentRaycast = raycastResult;
            _pointerEventData.delta = _pointerEventData.position - prevPosition;
        }

        /// <summary>
        /// 获取可交互性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool GetInteractive(GameObject obj)
        {
            return ExecuteEvents.GetEventHandler<IPointerClickHandler>(obj) != null
                || ExecuteEvents.GetEventHandler<IDragHandler>(obj) != null;
        }
    }
}