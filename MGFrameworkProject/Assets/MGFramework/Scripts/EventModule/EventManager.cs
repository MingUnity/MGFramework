using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework.Event
{
    /// <summary>
    /// 事件管理
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        public delegate void Callback();
        public delegate void Callback<T>(T arg1);
        public delegate void Callback<T, U>(T arg1, U arg2);
        public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
        public delegate void Callback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        private class EventReceiver
        {
            public Delegate listener;
        }

        private Dictionary<int, List<EventReceiver>> _dicEvent = new Dictionary<int, List<EventReceiver>>();

        public void AddListener(int eventId, Callback listener)
        {
            AddEvent(eventId, listener);
        }

        public void AddListener<T>(int eventId, Callback<T> listener)
        {
            AddEvent(eventId, listener);
        }

        public void AddListener<T, N>(int eventId, Callback<T, N> listener)
        {
            AddEvent(eventId, listener);
        }

        public void AddListener<T, U, V>(int eventId, Callback<T, U, V> listener)
        {
            AddEvent(eventId, listener);
        }

        public void AddListener<T1, T2, T3, T4>(int eventId, Callback<T1, T2, T3, T4> listener)
        {
            AddEvent(eventId, listener);
        }

        public void Dispatch(int eventId)
        {
            ForEach(eventId, listener => { if (listener is Callback callback) callback.Invoke(); });
        }

        public void Dispatch<T>(int eventId, T param1)
        {
            ForEach(eventId, listener => { if (listener is Callback<T> callback) callback.Invoke(param1); });
        }

        public void Dispatch<T, N>(int eventId, T param1, N param2)
        {
            ForEach(eventId, listener => { if (listener is Callback<T, N> callback) callback.Invoke(param1, param2); });
        }

        public void Dispatch<T, U, V>(int eventId, T param1, U param2, V param3)
        {
            ForEach(eventId, listener => { if (listener is Callback<T, U, V> callback) callback.Invoke(param1, param2, param3); });
        }

        public void Dispatch<T1, T2, T3, T4>(int eventId, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            ForEach(eventId, listener => { if (listener is Callback<T1, T2, T3, T4> callback) callback.Invoke(param1, param2, param3, param4); });
        }

        public void RemoveListener(int eventId, Callback listner)
        {
            RemoveEvent(eventId, listner);
        }

        public void RemoveListener<T>(int eventId, Callback<T> listner)
        {
            RemoveEvent(eventId, listner);
        }

        public void RemoveListener<T, N>(int eventId, Callback<T, N> listner)
        {
            RemoveEvent(eventId, listner);
        }

        public void RemoveListener<T, U, V>(int eventId, Callback<T, U, V> listner)
        {
            RemoveEvent(eventId, listner);
        }

        public void RemoveListener<T1, T2, T3, T4>(int eventId, Callback<T1, T2, T3, T4> listner)
        {
            RemoveEvent(eventId, listner);
        }

        public void RemoveListener(int eventId)
        {
            _dicEvent.Remove(eventId);
        }

        public void RemoveAll()
        {
            _dicEvent.Clear();
        }

        private void AddEvent(int eventId, Delegate listener)
        {
            EventReceiver receiver = new EventReceiver();
            receiver.listener = listener;

            List<EventReceiver> list = _dicEvent.GetValueAnyway(eventId);

            if (list != null)
            {
                list.Add(receiver);
            }
            else
            {
                _dicEvent[eventId] = new List<EventReceiver>() { receiver };
            }
        }

        private void RemoveEvent(int eventId, Delegate listener)
        {
            List<EventReceiver> list = _dicEvent.GetValueAnyway(eventId);
            if (list != null)
            {
                int listCount = list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    if (listener == list[i].listener)
                    {
                        list[i] = null;
                        break;
                    }
                }
            }
        }

        private void ForEach(int eventId, Action<Delegate> action)
        {
            List<EventReceiver> list = _dicEvent.GetValueAnyway(eventId);
            if (list != null)
            {
                int listCount = list.Count;
                for (int i = listCount - 1; i >= 0; i--)
                {
                    EventReceiver receiver = list[i];
                    if (receiver != null)
                    {
                        action?.Invoke(receiver.listener);
                    }
                    else
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
    }
}