using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace MGFramework
{
    public class Loom : MonoBehaviour
    {
        public static int maxThreads = 8;

        private static int _numThreads;

        private List<Action> _currentActions = new List<Action>();

        private List<Action> _actions = new List<Action>();

        private static bool _initDone;

        private static Loom _current;

        public static Loom Current
        {
            get
            {
                Initialize();

                return _current;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            if (!_initDone)
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                _initDone = true;

                GameObject g = new GameObject("Loom");

                DontDestroyOnLoad(g);

                _current = g.AddComponent<Loom>();
            }

        }

        public struct DelayedQueueItem
        {
            public float time;

            public Action action;
        }

        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        public static void QueueOnMainThread(Action action)
        {
            if (action == null)
            {
                return;
            }

            QueueOnMainThread(action, 0f);
        }

        public static void QueueOnMainThread(Action action, float time)
        {
            if (time != 0)
            {
                if (Current != null)
                {
                    lock (Current._delayed)
                    {
                        Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                    }
                }
            }
            else
            {
                if (Current != null)
                {
                    lock (Current._actions)
                    {
                        Current._actions.Add(action);
                    }
                }
            }
        }

        public static void RunAsync(Action a)
        {
            Initialize();

            while (_numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }

            Interlocked.Increment(ref _numThreads);

            ThreadPool.QueueUserWorkItem(RunAction, a);
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
            }
            finally
            {
                Interlocked.Decrement(ref _numThreads);
            }

        }

        void OnDisable()
        {
            if (_current == this)
            {
                _current = null;
            }
        }

        void Update()
        {
            if (_actions.Count > 0)
            {
                lock (_actions)
                {
                    _currentActions.AddRange(_actions);

                    _actions.Clear();
                }

                try
                {
                    foreach (var a in _currentActions)
                    {
                        a();
                    }
                }
                finally
                {
                    _currentActions.Clear();
                }
            }

            if (_delayed.Count > 0)
            {
                lock (_delayed)
                {
                    _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));

                    foreach (var item in _currentDelayed)
                    {
                        _delayed.Remove(item);
                    }
                }

                try
                {
                    foreach (var delayed in _currentDelayed)
                    {
                        delayed.action();
                    }

                }
                finally
                {
                    _currentDelayed.Clear();
                }
            }
        }
    }
}