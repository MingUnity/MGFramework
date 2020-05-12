using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 圆形布局
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("MGFramework/CircleLayoutGroup")]
    public class CircleLayoutGroup : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 100;

        private DrivenRectTransformTracker _tracker;

        /// <summary>
        /// 子节点
        /// </summary>
        private List<RectTransform> _children = new List<RectTransform>();

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                if (_radius != value)
                {
                    _radius = value;

                    Refresh();
                }
            }
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void OnDisable()
        {
            _tracker.Clear();
        }

        private void OnValidate()
        {
            Refresh();
        }

        private IEnumerator OnTransformChildrenChanged()
        {
            yield return null;

            Refresh();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh()
        {
            _children.Clear();
            _tracker.Clear();

            int childCount = this.transform.childCount;

            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    RectTransform trans = this.transform.GetChild(i) as RectTransform;
                    if (trans.gameObject.activeSelf)
                    {
                        _tracker.Add(this, trans, DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Anchors);
                        trans.anchorMax = 0.5f * Vector2.one;
                        trans.anchorMin = 0.5f * Vector2.one;
                        _children.Add(trans);
                    }
                }

                float delta = 2 * Mathf.PI / _children.Count;

                for (int i = 0; i < _children.Count; i++)
                {
                    float angle = i * delta;

                    float deltaX = _radius * Mathf.Cos(angle - 0.5f * Mathf.PI);

                    float deltaY = _radius * -Mathf.Sin(angle - 0.5f * Mathf.PI);

                    _children[i].anchoredPosition = new Vector2(deltaX, deltaY);
                }
            }
        }
    }
}
