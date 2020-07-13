using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.ExecuteEvents;

namespace MGFramework.InputModule
{
    internal class ExecuteEventsEx
    {
        private static readonly EventFunction<IFocusHandler> _focusHandler = ExecuteFocus;

        public static void ExecuteFocus(IFocusHandler handler, BaseEventData eventData)
        {
            handler.OnFocus(eventData as PointerEventData);
        }

        private static readonly EventFunction<IFocusHandler> _unFocusHandler = ExecuteUnFocus;

        public static void ExecuteUnFocus(IFocusHandler handler, BaseEventData eventData)
        {
            handler.OnUnFocus(eventData as PointerEventData);
        }

        public static EventFunction<IFocusHandler> FocusHandler => _focusHandler;

        public static EventFunction<IFocusHandler> UnFocusHandler => _unFocusHandler;
    }
}
