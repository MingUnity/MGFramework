using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// IOC容器
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// 类型节点字典
        /// 默认16个类型
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, ITypeNode>> _dic = new Dictionary<Type, Dictionary<string, ITypeNode>>(16);

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void Regist<T, V>(string name = null) where V : class, T, new()
        {
            Regist<T>(name, new NormalTypeNode(typeof(V)));
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void Regist<T, V>(object name) where V : class, T, new()
        {
            Regist<T, V>(name?.ToString());
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="obj">已实例化对象</param>
        /// <param name="name">名字</param>
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
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        public static void RegistSingleton<T, V>() where V : class, T, new()
        {
            SingletonTypeNode node = new SingletonTypeNode(typeof(V));

            Regist<T>(string.Empty, node);
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void RegistSingleton<T, V>(string name) where V : class, T, new()
        {
            RegistSingleton<T, V>(null, name);
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void RegistSingleton<T, V>(object name) where V : class, T, new()
        {
            RegistSingleton<T, V>(name?.ToString());
        }


        /// <summary>
        /// 解析对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <param name="name">名字</param>
        /// <returns>对应实例化对象</returns>
        public static T Resolve<T>(string name = null)
        {
            return (T)Resolve(typeof(T), name);
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <param name="name">名字</param>
        /// <returns>对应实例化对象</returns>
        public static T Resolve<T>(object name)
        {
            return Resolve<T>(name?.ToString());
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="name">名字</param>
        /// <returns>对应实例化对象</returns>
        public static object Resolve(Type type, string name = null)
        {
            object res = null;

            name = string.IsNullOrEmpty(name) ? string.Empty : name;

            Dictionary<string, ITypeNode> dic = null;

            if (_dic.TryGetValue(type, out dic))
            {
                ITypeNode node = null;

                dic?.TryGetValue(name, out node);

                if (node is NormalTypeNode)
                {
                    NormalTypeNode norNode = node as NormalTypeNode;

                    res = Activator.CreateInstance(norNode.objType);

                    GenerateInterfaceField(res);
                }
                else if (node is SingletonTypeNode)
                {
                    SingletonTypeNode singleNode = node as SingletonTypeNode;

                    res = singleNode.Obj;
                }
            }

            if (res == null)
            {
                Debug.LogErrorFormat("<Ming> ## Uni Error ## Cls:Container Func:Resolve Type:{0}{1} Info:Unregistered", type, !string.IsNullOrEmpty(name) ? $" Name:{name}" : string.Empty);
            }

            return res;
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
        /// 构建接口字段
        /// </summary>
        private static void GenerateInterfaceField(object target)
        {
            GenerateInterfaceField(target, target?.GetType());
        }

        /// <summary>
        /// 构建接口字段
        /// </summary>
        private static void GenerateInterfaceField(object target, Type type)
        {
            if (target == null || type == null)
            {
                return;
            }

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfos != null && fieldInfos.Length > 0)
            {
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    FieldInfo fieldInfo = fieldInfos[i];

                    if (fieldInfo != null && fieldInfo.FieldType.IsInterface)
                    {
                        object[] autoAttrs = fieldInfo.GetCustomAttributes(typeof(AutoBuildAttribute), true);

                        string name = null;

                        //含有autoBuild属性 可以自动实例化对象
                        if (autoAttrs != null && autoAttrs.Length > 0)
                        {
                            foreach (AutoBuildAttribute attr in autoAttrs)
                            {
                                if (attr != null)
                                {
                                    name = attr.name;
                                    break;
                                }
                            }

                            fieldInfo.SetValue(target, Resolve(fieldInfo.FieldType, name));
                        }
                    }
                }
            }

            GenerateInterfaceField(target, type.BaseType);
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
                        
                        GenerateInterfaceField(_obj);
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