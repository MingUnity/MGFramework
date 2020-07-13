using UnityEngine.EventSystems;

namespace MGFramework.InputModule
{
    /// <summary>
    /// 失焦
    /// </summary>
    public interface IUnFocusHandler: IEventSystemHandler
    {
        void OnUnFoucs(PointerEventData eventData);
    }
}
