using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.ExecuteEvents;

namespace MGFramework.InputModule
{
    internal class ExecuteEventsEx
    {
        private static readonly EventFunction<IFocusHandler> _focusHandler = Execute;

        public static void Execute(IFocusHandler handler, BaseEventData eventData)
        {
            handler.OnFocus(eventData as PointerEventData);
        }

        private static readonly EventFunction<IUnFocusHandler> _unFocusHandler = Execute;

        public static void Execute(IUnFocusHandler handler, BaseEventData eventData)
        {
            handler.OnUnFoucs(eventData as PointerEventData);
        }

        public static EventFunction<IFocusHandler> FocusHandler => _focusHandler;

        public static EventFunction<IUnFocusHandler> UnFocusHandler => _unFocusHandler;
    }
}
