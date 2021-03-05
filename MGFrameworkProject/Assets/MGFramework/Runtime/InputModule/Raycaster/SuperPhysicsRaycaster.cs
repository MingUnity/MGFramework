using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 物理射线检测
    /// </summary>
    public class SuperPhysicsRaycaster : PhysicsRaycaster
    {
        /// <summary>
        /// 缓存碰撞数组
        /// </summary>
        private readonly RaycastHit[] _hits = new RaycastHit[16];

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (RayManager.CurrentRay != null)
            {
                Ray ray = RayManager.CurrentRay.Ray;

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, finalEventMask))
                {
                    var result = new RaycastResult
                    {
                        gameObject = hit.collider.gameObject,
                        module = this,
                        distance = hit.distance,
                        worldPosition = hit.point,
                        worldNormal = hit.normal,
                        screenPosition = eventData.position,
                        index = resultAppendList.Count,
                        sortingLayer = 0,
                        sortingOrder = 0
                    };
                    resultAppendList.Add(result);
                }
            }
            else
            {
                base.Raycast(eventData, resultAppendList);
            }
        }
    }
}
