using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MGFramework
{
    /// <summary>
    /// 事件触发器扩展
    /// </summary>
    public static class EventTriggerExtends
    {
        /// <summary>
        /// 点击效果
        /// </summary>
        public static IPointerClickEffect pointerClickEffect;

        /// <summary>
        /// 添加监听
        /// </summary>
        public static EventTrigger AddListener(this EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            if (trigger.triggers == null)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            EventTrigger.Entry target = null;

            for (int i = 0; i < trigger.triggers.Count; i++)
            {
                EventTrigger.Entry entry = trigger.triggers[i];

                if (entry != null && entry.eventID == type)
                {
                    target = entry;

                    break;
                }
            }

            if (target == null)
            {
                target = new EventTrigger.Entry()
                {
                    eventID = type
                };

                trigger.triggers.Add(target);
            }

            if (target.callback == null)
            {
                target.callback = new EventTrigger.TriggerEvent();
            }

            target.callback.AddListener(action);

            return trigger;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public static EventTrigger RemoveListener(this EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            if (trigger.triggers != null)
            {
                for (int i = 0; i < trigger.triggers.Count; i++)
                {
                    EventTrigger.Entry entry = trigger.triggers[i];

                    if (entry != null && entry.eventID == type && entry.callback != null)
                    {
                        entry.callback.RemoveListener(action);
                    }
                }
            }

            return trigger;
        }

        /// <summary>
        /// 添加点击监听
        /// </summary>
        public static EventTrigger AddClickListener(this EventTrigger trigger, Action onClick)
        {
            return trigger.AddListener(EventTriggerType.PointerClick, (data) =>
            {
                if (pointerClickEffect != null)
                {
                    pointerClickEffect.OnClickEffect(trigger);
                }

                if (onClick != null)
                {
                    onClick.Invoke();
                }
            });
        }
    }
}