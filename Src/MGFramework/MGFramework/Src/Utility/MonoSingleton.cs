using System;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// Mono单例
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        /// <summary>
        /// 单例对象 
        /// 如果没有找到场景中的对象则创建
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] ts = GameObject.FindObjectsOfType<T>();

                    if (ts != null && ts.Length > 0)
                    {
                        if (ts.Length == 1)
                        {
                            _instance = ts[0];
                        }
                        else
                        {
                            throw new Exception(string.Format("## Uni Exception ## Cls:{0} Info:Singleton not allows more than one instance", typeof(T)));
                        }
                    }
                    else
                    {
                        _instance = new GameObject(string.Format("{0}(Singleton)", typeof(T).ToString())).AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// 单例对象
        /// 根据入参判断 如果没有找到场景中的对象，是否创建新对象
        /// </summary>
        /// <param name="createIfNotExists">如果不存在是否创建新对象</param>
        /// <returns></returns>
        public static T GetInstance(bool createIfNotExists = false)
        {
            if (_instance == null)
            {
                T[] ts = GameObject.FindObjectsOfType<T>();

                if (ts != null && ts.Length > 0)
                {
                    if (ts.Length == 1)
                    {
                        _instance = ts[0];
                    }
                    else
                    {
                        throw new Exception(string.Format("## Uni Exception ## Cls:{0} Info:Singleton not allows more than one instance", typeof(T)));
                    }
                }
                else if (createIfNotExists)
                {
                    _instance = new GameObject(string.Format("{0}(Singleton)", typeof(T).ToString())).AddComponent<T>();
                }
            }

            return _instance;
        }

        protected MonoSingleton() { }

        protected virtual void Awake()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}