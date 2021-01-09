using UnityEngine;

namespace MGFramework
{
    public interface IObjectPool<T> where T : IPoolObject
    {
        T Get(Transform parent = null);
        void Remove(T t);
    }
}
