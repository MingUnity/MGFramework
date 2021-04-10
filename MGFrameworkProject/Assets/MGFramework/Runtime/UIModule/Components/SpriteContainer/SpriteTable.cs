using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 精灵图片键值集合
    /// </summary>
    [AddComponentMenu("MGFramework/SpriteTable")]
    public class SpriteTable : MonoBehaviour
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
            /// 图片对象
            /// </summary>
            public Sprite sprite;
        }

        /// <summary>
        /// 图片集合
        /// </summary>
        [SerializeField]
        private Item[] _keyValueSprites;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count => _keyValueSprites == null ? 0 : _keyValueSprites.Length;

        /// <summary>
        /// 根据索引取对应图片
        /// </summary>
        public Sprite Get(string key)
        {
            if (_keyValueSprites == null || string.IsNullOrEmpty(key))
            {
                return null;
            }

            Sprite result = null;

            for (int i = 0; i < _keyValueSprites.Length; i++)
            {
                Item item = _keyValueSprites[i];

                if (item.key == key)
                {
                    result = item.sprite;
                    break;
                }
            }

            return result;
        }
    }
}