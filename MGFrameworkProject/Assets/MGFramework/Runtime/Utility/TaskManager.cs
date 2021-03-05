/// Simple, really.  There is no need to initialize or even refer to TaskManager.  
/// When the first Task is created in an application, a "TaskManager" GameObject  
/// will automatically be added to the scene root with the TaskManager component  
/// attached.  This component will be responsible for dispatching all coroutines  
/// behind the scenes.  
///  
/// Task also provides an event that is triggered when the coroutine exits.  

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MGFramework
{
    /// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.  
    /// It is an error to attempt to start a task that has been stopped or which has  
    /// naturally terminated.  
    public class Task
    {
        public static Task CreateTask(IEnumerator c, bool autoStart = true)
        {
            return new Task(c, autoStart);
        }

        /// Returns true if and only if the coroutine is running.  Paused tasks  
        /// are considered to be running.  
        public bool Running
        {
            get
            {
                return task.Running;
            }
        }

        /// Returns true if and only if the coroutine is currently paused.  
        public bool Paused
        {
            get
            {
                return task.Paused;
            }
        }

        /// Delegate for termination subscribers.  manual is true if and only if  
        /// the coroutine was stopped with an explicit call to Stop().  
        public delegate void FinishedHandler(Task task, bool manual);

        /// Termination event.  Triggered when the coroutine completes execution.  
        public event FinishedHandler Finished;

        /// Creates a new Task object for the given coroutine.  
        ///  
        /// If autoStart is true (default) the task is automatically started  
        /// upon construction.  
        public Task(IEnumerator c, bool autoStart = true)
        {
            task = TaskManager.CreateTask(c);

            task.Finished += TaskFinished;

            if (autoStart)
            {
                Start();
            }
        }

        /// Begins execution of the coroutine  
        public void Start()
        {
            task.Start();
        }

        /// Discontinues execution of the coroutine at its next yield.  
        public void Stop()
        {
            task.Stop();
        }

        public void Pause()
        {
            task.Pause();
        }

        public void Unpause()
        {
            task.Unpause();
        }

        public Coroutine Coroutine
        {
            get
            {
                return task.Coroutine;
            }
        }

        void TaskFinished(bool manual)
        {
            FinishedHandler handler = Finished;
            if (handler != null)
            {
                handler(this, manual);
            }
        }

        internal static void CreateTask(object loadScene)
        {
            throw new NotImplementedException();
        }

        TaskManager.TaskState task;
    }

    public class TaskManager : MonoBehaviour
    {
        public class TaskState
        {
            public bool Running
            {
                get
                {
                    return running;
                }
            }

            public bool Paused
            {
                get
                {
                    return paused;
                }
            }

            public Coroutine Coroutine
            {
                get
                {
                    return coroutine;
                }
            }

            public delegate void FinishedHandler(bool manual);
            public event FinishedHandler Finished;

            IEnumerator enumerator;
            Coroutine coroutine;
            bool running;
            bool paused;
            bool stopped;

            public TaskState(IEnumerator c)
            {
                enumerator = c;
            }

            public void Pause()
            {
                paused = true;
            }

            public void Unpause()
            {
                paused = false;
            }

            public void Start()
            {
                running = true;

                coroutine = singleton.StartCoroutine(CallWrapper());
            }

            public void Stop()
            {
                stopped = true;
                running = false;
            }

            private IEnumerator CallWrapper()
            {
                //yield return null;

                IEnumerator e = enumerator;

                while (running)
                {
                    if (paused)
                    {
                        yield return null;
                    }
                    else
                    {
                        if (e != null && e.MoveNext())
                        {
                            yield return e.Current;
                        }
                        else
                        {
                            running = false;
                        }
                    }
                }

                FinishedHandler handler = Finished;

                if (handler != null)
                {
                    handler(stopped);
                }
            }
        }

        private static TaskManager singleton;

        public static TaskState CreateTask(IEnumerator coroutine)
        {
            if (singleton == null)
            {
                GameObject go = new GameObject("TaskManager");
                singleton = go.AddComponent<TaskManager>();
                DontDestroyOnLoad(go);
            }
            return new TaskState(coroutine);
        }
    }

    /// <summary>
    /// 任务队列
    /// 在某个时间点最多只有一个协程在执行，先加入队列中的先执行，后加入的后执行
    /// </summary>
    public class TaskQueue
    {
        static Dictionary<string, TaskQueue> _taskQueues = new Dictionary<string, TaskQueue>();
        public static TaskQueue GetTaskQueue(string id)
        {
            TaskQueue taskQueue;
            if (!_taskQueues.TryGetValue(id, out taskQueue))
            {
                taskQueue = new TaskQueue();
                _taskQueues[id] = taskQueue;
            }
            return taskQueue;
        }

        public static void RemoveTaskQueue(string id)
        {
            if (_taskQueues.ContainsKey(id))
            {
                _taskQueues.Remove(id);
            }
        }

        private List<TaskInfo> _listTask = new List<TaskInfo>();

        public delegate void DelegateVoid();
        public DelegateVoid OnTaskQueueFinished;

        public TaskInfo Add(IEnumerator c)
        {
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.c = c;
            _listTask.Add(taskInfo);

            if (_listTask.Count == 1)
            {
                Run(_listTask[0]);
            }
            return taskInfo;
        }

        public void Remove(TaskInfo taskInfo)
        {
            if (_listTask.Contains(taskInfo))
            {
                bool removeTaskIsRuning = false;
                if (taskInfo.task != null)
                {
                    taskInfo.task.Stop();
                    removeTaskIsRuning = true;
                }
                _listTask.Remove(taskInfo);

                if (removeTaskIsRuning && _listTask.Count > 0)
                {
                    Run(_listTask[0]);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _listTask.Count; i++)
            {
                if (_listTask[i].task != null)
                {
                    _listTask[i].task.Stop();
                }
            }
            _listTask.Clear();
        }

        void OnFinish(Task task, bool manual)
        {
            if (_listTask.Count > 0)
            {
                _listTask.RemoveAt(0);

                if (_listTask.Count > 0)
                {
                    Run(_listTask[0]);
                }
                else
                {
                    if (OnTaskQueueFinished != null)
                    {
                        OnTaskQueueFinished();
                    }
                }

            }
        }

        void Run(TaskInfo taskInfo)
        {
            taskInfo.task = new Task(taskInfo.c);
            taskInfo.task.Finished += OnFinish;
        }

        public class TaskInfo
        {
            public IEnumerator c;
            public Task task;
        }
    }
}