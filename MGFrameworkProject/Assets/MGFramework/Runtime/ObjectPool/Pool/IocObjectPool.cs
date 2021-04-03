using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 基于ioc对象池
    /// </summary>
    public class IocObjectPool<T> where T : IPoolObject
    {
        /// <summary>
        /// 模板
        /// </summary>
        private Transform _template;

        /// <summary>
        /// 名字
        /// 用于DI
        /// </summary>
        private string _name;

        /// <summary>
        /// 父节点
        /// </summary>
        private Transform _parent;

        /// <summary>
        /// 栈
        /// </summary>
        private Stack<T> _stack = new Stack<T>();

        public IocObjectPool(Transform template, Transform parent = null, bool isPrefab = true, string name = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException("<Ming> ## Uni Exception ## Cls:IocObjectPool Func:Constructor Info:Template can't be null");
            }

            this._template = template;
            this._name = name;
            this._parent = parent;

            if (!isPrefab)
            {
                _template.gameObject.SetActive(false);
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
                t = Container.Resolve<T>(_name);

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