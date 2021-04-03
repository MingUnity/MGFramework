using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 预制体键值集合
    /// </summary>
    [AddComponentMenu("MGFramework/PrefabTable")]
    public class PreafabTable : MonoBehaviour
    {
        /// <summary>
        /// 元素
        /// </summary>
        [System.Serializable]
        public class Item
        {
            /// <summary>
            /// 键
            /// </summary>
            public string key;

            /// <summary>
            /// 预制体对象
            /// </summary>
            public GameObject prefab;
        }

        /// <summary>
        /// 预制体集合
        /// </summary>
        [SerializeField]
        private Item[] _keyValuePrefabs;

        /// <summary>
        /// 根据索引取对应预制体
        /// </summary>
        public GameObject Get(string key)
        {
            if (_keyValuePrefabs == null || string.IsNullOrEmpty(key))
            {
                return null;
            }

            GameObject result = null;

            for (int i = 0; i < _keyValuePrefabs.Length; i++)
            {
                Item item = _keyValuePrefabs[i];

                if (item.key == key)
                {
                    result = item.prefab;
                    break;
                }
            }

            return result;
        }
    }
}