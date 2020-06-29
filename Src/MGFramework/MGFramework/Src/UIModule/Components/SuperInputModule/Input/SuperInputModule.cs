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
        /// 指针悬浮中
        /// </summary>
        private bool _pointerHovering;

        /// <summary>
        /// 使用自定义射线
        /// </summary>
        public bool useCustomRay;

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
                DispatchPointerHover(prevObject);
                DispatchTrigger();

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
            bool triggerDown = InputManager.TriggerDown;
            bool triggering = InputManager.Trigger;
            bool triggerUp = InputManager.TriggerUp;

            bool pressed = (triggerDown || triggering) && !triggerUp;
            bool released = triggerUp || (!triggerDown && !triggering);

            ProcessPress(_pointerEventData, pressed, released);

            if (!released)
            {
                ProcessMove(_pointerEventData);
                ProcessDrag(_pointerEventData);
            }

            bool interactive = GetInteractive(_pointerEventData.pointerCurrentRaycast.gameObject);

            if (triggerDown && !triggering && !triggerUp)
            {
                SuperInputListener.InvokePointerDown(_pointerEventData.pointerCurrentRaycast, interactive);
            }

            if (!triggerDown && !triggering && triggerUp)
            {
                SuperInputListener.InvokePointerUp(_pointerEventData.pointerCurrentRaycast, interactive);
            }
        }

        /// <summary>
        /// 处理点击
        /// </summary>
        private void ProcessPress(PointerEventData pointerEventData, bool pressed, bool released)
        {
            GameObject currentOverGo = pointerEventData.pointerCurrentRaycast.gameObject;

            if (pointerEventData.pointerEnter != currentOverGo)
            {
                HandlePointerExitAndEnter(pointerEventData, currentOverGo);
                pointerEventData.pointerEnter = currentOverGo;
            }
            
            if (pressed)
            {
                pointerEventData.eligibleForClick = true;
                pointerEventData.delta = Vector2.zero;
                pointerEventData.dragging = false;
                pointerEventData.useDragThreshold = true;
                pointerEventData.pressPosition = pointerEventData.position;
                pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, pointerEventData);
                
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEventData, ExecuteEvents.pointerDownHandler);

                if (newPressed == null)
                {
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
                }
                
                float time = Time.unscaledTime;

                if (newPressed == pointerEventData.lastPress)
                {
                    var diffTime = time - pointerEventData.clickTime;
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
                pointerEventData.rawPointerPress = currentOverGo;
                pointerEventData.clickTime = time;
                pointerEventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

                if (pointerEventData.pointerDrag != null)
                {
                    ExecuteEvents.Execute(pointerEventData.pointerDrag, pointerEventData, ExecuteEvents.initializePotentialDrag);
                }
            }
            
            if (released)
            {
                ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerUpHandler);
                
                GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
                
                if (pointerEventData.pointerPress == pointerUpHandler && pointerEventData.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEventData.pointerDrag != null && pointerEventData.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEventData, ExecuteEvents.dropHandler);
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
                
                ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerEnter, pointerEventData, ExecuteEvents.pointerExitHandler);
                pointerEventData.pointerEnter = null;
            }
        }

        /// <summary>
        /// 指针悬浮处理
        /// </summary>
        private void DispatchPointerHover(GameObject prevObject)
        {
            GameObject curObject = _pointerEventData.pointerCurrentRaycast.gameObject;

            bool interactive = GetInteractive(curObject);

            if (_pointerHovering && curObject != null && curObject == prevObject)
            {
                SuperInputListener.InvokePointerHover(_pointerEventData.pointerCurrentRaycast, interactive);
            }
            else
            {
                if (prevObject != null || (curObject == null && _pointerHovering))
                {
                    SuperInputListener.InvokePointerExit(prevObject);
                    _pointerHovering = false;
                }

                if (curObject != null)
                {
                    SuperInputListener.InvokePointerEnter(_pointerEventData.pointerCurrentRaycast, interactive);
                    _pointerHovering = true;
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
            return _pointerEventData.pointerPress != null
                               || ExecuteEvents.GetEventHandler<IPointerClickHandler>(obj) != null
                               || ExecuteEvents.GetEventHandler<IDragHandler>(obj) != null;
        }
    }
}