using System;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// Mono单例
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

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

        protected MonoSingleton() { }
    }
}