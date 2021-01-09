using UnityEngine.EventSystems;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 焦点处理
    /// </summary>
    public interface IFocusHandler : IEventSystemHandler
    {
        /// <summary>
        /// 聚焦
        /// </summary>
        void OnFocus(PointerEventData eventData);

        /// <summary>
        /// 失焦
        /// </summary>
        void OnUnFocus(PointerEventData eventData);
    }
}