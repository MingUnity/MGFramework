using MGFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 自定义UI Raycaster
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class SuperGraphicRaycaster : BaseRaycaster
    {
        private Canvas _canvas;
        private Camera _eventCamera;
        [NonSerialized] private readonly List<Graphic> _raycastResults = new List<Graphic>();

        /// <summary>
        /// Priority of the raycaster based upon sort order.
        /// </summary>
        /// <returns>
        /// The sortOrder priority.
        /// </returns>
        public override int sortOrderPriority
        {
            get
            {
                // We need to return the sorting order here as distance will all be 0 for overlay.
                if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    return _canvas.sortingOrder;

                return base.sortOrderPriority;
            }
        }

        /// <summary>
        /// Priority of the raycaster based upon render order.
        /// </summary>
        /// <returns>
        /// The renderOrder priority.
        /// </returns>
        public override int renderOrderPriority
        {
            get
            {
                // We need to return the sorting order here as distance will all be 0 for overlay.
                if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    return _canvas.rootCanvas.renderOrder;

                return base.renderOrderPriority;
            }
        }

        /// <summary>
        /// The camera that will generate rays for this raycaster.
        /// </summary>
        /// <returns>
        /// - Null if Camera mode is ScreenSpaceOverlay or ScreenSpaceCamera and has no camera.
        /// - canvas.worldCanvas if not null
        /// - Camera.main.
        /// - Ming Modify 只在初始化的时候赋值，节省获取Camera.main的时耗，提供一个刷新方法以供动态改变摄像机
        /// </returns>
        public override Camera eventCamera => _eventCamera;

        protected override void Awake()
        {
            base.Awake();
            Refresh();
        }

        /// <summary>
        /// 刷新参数
        /// </summary>
        public void Refresh()
        {
            //画布赋值
            _canvas = this.GetComponent<Canvas>();

            //摄像机赋值
            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay || (_canvas.renderMode == RenderMode.ScreenSpaceCamera && _canvas.worldCamera == null))
            {
                _eventCamera = null;
            }
            else
            {
                _eventCamera = _canvas.worldCamera != null ? _canvas.worldCamera : Camera.main;
            }
        }

        /// <summary>
        /// Perform the raycast against the list of graphics associated with the Canvas.
        /// </summary>
        /// <param name="eventData">Current event data</param>
        /// <param name="resultAppendList">List of hit objects to append new results to.</param>
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            //<Ming> Modify : Canvas必不为空(若为空则为开发时异常!)，节省canvas判空消耗(UnityEngine.Object重载了==和!=)
            //if (_canvas == null)
            //    return;

            var canvasGraphics = GraphicRegistry.GetGraphicsForCanvas(_canvas);

            if (canvasGraphics == null || canvasGraphics.Count == 0)
                return;

            Vector2 eventPosition = GetEventPosition(eventData);

            // Convert to view space
            Vector2 pos;
            if (_eventCamera == null)
            {
                float w = Screen.width;
                float h = Screen.height;

                pos = new Vector2(eventPosition.x / w, eventPosition.y / h);
            }
            else
            {
                pos = _eventCamera.ScreenToViewportPoint(eventPosition);
            }

            // If it's outside the camera's viewport, do nothing
            if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
                return;

            Ray ray = new Ray();

            if (_eventCamera != null)
            {
                ray = _eventCamera.ScreenPointToRay(eventPosition);
            }

            _raycastResults.Clear();

            float distance = 0;

            Graphic graphic = Raycast(_canvas, _eventCamera, eventPosition, canvasGraphics, ray, out distance);

            if (graphic != null)
            {
                var castResult = new RaycastResult
                {
                    gameObject = graphic.gameObject,
                    module = this,
                    distance = distance,
                    screenPosition = eventPosition,
                    index = resultAppendList.Count,
                    depth = graphic.depth,
                    sortingLayer = _canvas.sortingLayerID,
                    sortingOrder = _canvas.sortingOrder,
                    worldPosition = ray.origin + ray.direction * distance,
                    worldNormal = -graphic.transform.forward
                };
                resultAppendList.Add(castResult);
            }
        }

        /// <summary>
        /// Ming Modify
        /// 1、返回最上层图形
        /// </summary>
        private Graphic Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, Ray ray, out float distance)
        {
            // Necessary for the event system
            int totalCount = foundGraphics.Count;

            Graphic output = null;
            distance = 0;
            int maxDepth = -1;

            //根据开发制作预制体普遍习惯(节点从上往下开发)
            //节点越下，graphic注册越迟 所以采用倒序遍历，可以通过depth过滤很多graphic的判定
            for (int i = totalCount - 1; i >= 0; i--)
            {
                Graphic graphic = foundGraphics[i];

                int depth = graphic.depth;

                if (depth <= maxDepth)
                    continue;

                if (depth == -1 || !graphic.raycastTarget || graphic.canvasRenderer.cull)
                    continue;

                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera))
                    continue;

                if (eventCamera != null && eventCamera.WorldToScreenPoint(graphic.rectTransform.position).z > eventCamera.farClipPlane)
                    continue;

                if (graphic.Raycast(pointerPosition, eventCamera))
                {
                    //判断距离
                    if (_eventCamera == null || _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        distance = 0;
                    }
                    else
                    {
                        Transform trans = graphic.transform;
                        distance = (Vector3.Dot(trans.forward, trans.position - ray.origin) / Vector3.Dot(trans.forward, ray.direction));

                        // Check to see if the go is behind the camera.
                        if (distance < 0)
                            continue;
                    }

                    output = graphic;
                    maxDepth = depth;
                }
            }

            return output;
        }

        /// <summary>
        /// 获取事件位置
        /// </summary>
        private Vector2 GetEventPosition(PointerEventData eventData)
        {
            ICustomRay customRay = RayManager.CurrentRay;

            if (customRay != null)
            {
                Ray ray = customRay.Ray;

                Vector3 worldEventPos = MathHelper.GetIntersectWithLineAndPlane(ray.origin, ray.direction, _canvas.transform.forward, _canvas.transform.position);

                if (_eventCamera != null)
                {
                    return _eventCamera.WorldToScreenPoint(worldEventPos);
                }
                else
                {
                    return eventData.position;
                }
            }
            else
            {
                return eventData.position;
            }
        }
    }
}