using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 通用对象池
    /// </summary>
    public class GeneralObjectPool<T> : IObjectPool<T> where T : IPoolObject, new()
    {
        /// <summary>
        /// 模板
        /// </summary>
        private Transform _template;

        /// <summary>
        /// 父节点
        /// </summary>
        private Transform _parent;

        /// <summary>
        /// 栈
        /// </summary>
        private Stack<T> _stack = new Stack<T>();

        public GeneralObjectPool(Transform template, Transform parent = null, bool isPrefab = true)
        {
            if (template == null)
            {
                throw new ArgumentNullException("<Ming> ## Uni Exception ## Cls:GeneralObjectPool Func:Constructor Info:Template can't be null");
            }

            this._template = template;
            this._parent = parent;

            if (!isPrefab)
            {
                template.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public T Get(Transform parent = null)
        {
            T t = default(T);

            if (_stack.Count > 0)
            {
                t = _stack.Pop();
            }
            else
            {
                t = new T();

                t.Create(Transform.Instantiate<Transform>(_template));
            }

            t.Active = true;

            t.SetParent(!System.Object.ReferenceEquals(parent, null) ? parent : _parent);

            return t;
        }

        /// <summary>
        /// 移除
        /// </summary>
        public void Remove(T t)
        {
            if (t == null)
            {
                return;
            }

            t.Reset();
            t.Active = false;

            _stack.Push(t);
        }
    }
}