using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 基于预制体对象池
    /// </summary>
    public class PrefabObjectPool<T> : IObjectPool<T> where T : IPoolObject
    {
        /// <summary>
        /// 预制体
        /// </summary>
        private Transform _prefab;

        /// <summary>
        /// 父节点
        /// </summary>
        private Transform _parent;

        /// <summary>
        /// 栈
        /// </summary>
        private Stack<T> _stack = new Stack<T>();

        public PrefabObjectPool(Transform prefab, Transform parent = null)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException("<Ming> ## Uni Exception ## Cls:PrefabObjectPool Func:Constructor Info:Prefab can't be null");
            }

            this._prefab = prefab;
            this._parent = parent;
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
                t = System.Activator.CreateInstance<T>();

                t.Create(Transform.Instantiate<Transform>(_prefab));
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