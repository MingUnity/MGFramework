using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool<T> where T : IPoolObject
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
        /// 栈
        /// </summary>
        private Stack<T> _stack = new Stack<T>();

        public ObjectPool(Transform template, string name = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException("<Ming> ## Uni Exception ## Cls:ObjectPool Func:Constructor Info:Template can't be null");
            }

            this._template = template;
            this._name = name;

            _template.gameObject.SetActive(false);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public T Get()
        {
            T t = default(T);

            if (_stack.Count > 0)
            {
                t = _stack.Pop();
            }
            else
            {
                t = Container.Resolve<T>(_name);

                t.Create(Transform.Instantiate<Transform>(_template, _template.parent));
            }

            t.Active = true;

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

            t.Active = false;
            t.Reset();

            _stack.Push(t);
        }
    }
}