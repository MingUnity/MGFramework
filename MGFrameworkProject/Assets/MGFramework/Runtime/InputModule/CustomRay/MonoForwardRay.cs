﻿using UnityEngine;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 前向射线
    /// </summary>
    public sealed class MonoForwardRay : MonoBehaviour, ICustomRay
    {
        /// <summary>
        /// 当前摄像机Transform的缓存
        /// </summary>
        private Transform _trans;

        /// <summary>
        /// 缓存射线
        /// </summary>
        private Ray _cacheRay;

        /// <summary>
        /// 优先级
        /// </summary>
        [SerializeField]
        private int _priority;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (_priority != value)
                {
                    _priority = value;

                    if (isActiveAndEnabled)
                    {
                        RayManager.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// 射线
        /// </summary>
        public Ray Ray
        {
            get
            {
                _cacheRay.origin = _trans.position;
                _cacheRay.direction = _trans.forward;

                return _cacheRay;
            }
        }

        private void Awake()
        {
            _trans = this.transform;
        }

        private void OnEnable()
        {
            RayManager.AddRay(this);
        }

        private void OnDisable()
        {
            RayManager.RemoveRay(this);
        }
    }
}