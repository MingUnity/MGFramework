using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 精灵图片的载体
    /// </summary>
    [AddComponentMenu("MGFramework/SpriteContainer")]
    public class SpriteContainer : MonoBehaviour
    {
        /// <summary>
        /// 图片集合
        /// </summary>
        [SerializeField]
        private Sprite[] _sprites;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count => _sprites == null ? 0 : _sprites.Length;

        /// <summary>
        /// 根据索引取对应图片
        /// </summary>
        public Sprite this[int index]
        {
            get
            {
                return _sprites?.GetValueAnyway(index);
            }
        }
    }
}