using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 预制体的载体
    /// </summary>
    [AddComponentMenu("MGFramework/PrefabContainer")]
    public class PrefabContainer : MonoBehaviour
    {
        /// <summary>
        /// 预制体集合
        /// </summary>
        [SerializeField]
        private GameObject[] _prefabs;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count => _prefabs == null ? 0 : _prefabs.Length;

        /// <summary>
        /// 根据索引取对应预制体
        /// </summary>
        public GameObject this[int index]
        {
            get
            {
                return _prefabs?.GetValueAnyway(index);
            }
        }
    }
}