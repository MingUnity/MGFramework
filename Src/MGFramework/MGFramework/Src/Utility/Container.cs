using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 简易IOC容器
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// 类型节点字典
        /// </summary>
        private static Dictionary<Type, Dictionary<string, ITypeNode>> _dic = new Dictionary<Type, Dictionary<string, ITypeNode>>();

        /// <summary>
        /// 注册
        /// </summary>
        public static void Regist<T, V>(string name = null) where V : class, T, new()
        {
            Regist<T>(name, new NormalTypeNode(typeof(V)));
        }

        /// <summary>
        /// 注册
        /// </summary>
        public static void Regist<T, V>(object name) where V : class, T, new()
        {
            Regist<T, V>(name?.ToString());
        }

        /// <summary>
        /// 注册单例
        /// </summary>
        public static void RegistSingleton<T, V>(V obj, string name) where V : class, T, new()
        {
            SingletonTypeNode node = null;

            if (obj != null)
            {
                node = new SingletonTypeNode(obj);
            }
            else
            {
                node = new SingletonTypeNode(typeof(V));
            }

            Regist<T>(name, node);
        }

        /// <summary>
        /// 注册单例
        /// </summary>
        public static void RegistSingleton<T, V>() where V : class, T, new()
        {
            SingletonTypeNode node = new SingletonTypeNode(typeof(V));

            Regist<T>(string.Empty, node);
        }

        /// <summary>
        /// 注册单例
        /// </summary>
        public static void RegistSingleton<T, V>(string name) where V : class, T, new()
        {
            RegistSingleton<T, V>(null, name);
        }

        /// <summary>
        /// 注册单例
        /// </summary>
        public static void RegistSingleton<T, V>(object name) where V : class, T, new()
        {
            RegistSingleton<T, V>(name?.ToString());
        }


        /// <summary>
        /// 解析
        /// </summary>
        public static T Resolve<T>(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? string.Empty : name;

            T t = default(T);

            Dictionary<string, ITypeNode> dic = null;

            if (_dic.TryGetValue(typeof(T), out dic))
            {
                ITypeNode node = null;

                dic?.TryGetValue(name, out node);

                if (node is NormalTypeNode)
                {
                    NormalTypeNode norNode = node as NormalTypeNode;

                    t = (T)Activator.CreateInstance(norNode.objType);
                }
                else if (node is SingletonTypeNode)
                {
                    SingletonTypeNode singleNode = node as SingletonTypeNode;

                    t = (T)singleNode.Obj;
                }
            }

            if (t == null)
            {
                Debug.LogErrorFormat("<Ming> ## Uni Error ## Cls:Container Func:Resolve Type:{0}{1} Info:Unregistered", typeof(T), !string.IsNullOrEmpty(name) ? $" Name:{name}" : string.Empty);
                throw new InvalidOperationException();
            }

            return t;
        }

        /// <summary>
        /// 解析
        /// </summary>
        public static T Resolve<T>(object name)
        {
            return Resolve<T>(name?.ToString());
        }

        /// <summary>
        /// 注册节点
        /// </summary>
        private static void Regist<T>(string name, ITypeNode node)
        {
            name = string.IsNullOrEmpty(name) ? string.Empty : name;

            Type type = typeof(T);

            Dictionary<string, ITypeNode> dic = null;

            _dic.TryGetValue(type, out dic);

            if (dic == null)
            {
                dic = new Dictionary<string, ITypeNode>();
            }

            dic[name] = node;

            _dic[type] = dic;
        }

        /// <summary>
        /// 类型节点
        /// </summary>
        private interface ITypeNode
        {
        }

        /// <summary>
        /// 单例类型节点
        /// </summary>
        private class SingletonTypeNode : ITypeNode
        {
            private object _obj;

            /// <summary>
            /// 类型
            /// </summary>
            private Type _type;

            /// <summary>
            /// 对象
            /// </summary>
            public object Obj
            {
                get
                {
                    if (_obj == null)
                    {
                        _obj = Activator.CreateInstance(_type);
                    }

                    return _obj;
                }
            }

            public SingletonTypeNode(Type objType)
            {
                this._type = objType;
            }

            public SingletonTypeNode(object obj)
            {
                this._obj = obj;
            }
        }

        /// <summary>
        /// 简单类型节点
        /// </summary>
        private class NormalTypeNode : ITypeNode
        {
            /// <summary>
            /// 实例对象类型
            /// </summary>
            public Type objType;

            public NormalTypeNode(Type objType)
            {
                this.objType = objType;
            }
        }
    }
}