using UnityEngine.EventSystems;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 聚焦
    /// </summary>
    public interface IFocusHandler : IEventSystemHandler
    {
        void OnFocus(PointerEventData eventData);
    }
}