using System;
using System.Collections;
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
            _tracker.Clear();

            int childCount = this.transform.childCount;

            if (childCount > 0)
            {
                RectTransform[] children = new RectTransform[childCount];

                for (int i = 0; i < childCount; i++)
                {
                    RectTransform trans = this.transform.GetChild(i) as RectTransform;
                    _tracker.Add(this, trans, DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Anchors);
                    trans.anchorMax = 0.5f * Vector2.one;
                    trans.anchorMin = 0.5f * Vector2.one;

                    children[i] = trans;
                }

                float delta = 2 * Mathf.PI / childCount;

                for (int i = 0; i < childCount; i++)
                {
                    float angle = i * delta;

                    float deltaX = _radius * Mathf.Cos(angle - 0.5f * Mathf.PI);

                    float deltaY = _radius * -Mathf.Sin(angle - 0.5f * Mathf.PI);

                    children[i].anchoredPosition = new Vector2(deltaX, deltaY);
                }
            }
        }
    }
}
