using UnityEngine.EventSystems;

namespace MGFramework
{
    /// <summary>
    /// 点击效果
    /// </summary>
    public interface IPointerClickEffect
    {
        /// <summary>
        /// 点击效果
        /// </summary>
        void OnClickEffect(IPointerClickHandler handler);
    }
}
