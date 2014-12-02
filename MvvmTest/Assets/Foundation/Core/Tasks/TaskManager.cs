// --------------------------------------
//  Unity Foundation
//  ConsoleSetup.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Foundation
{
    /// <summary>
    /// Manager for running coroutines and schedualing actions to runs in the main thread.
    /// Start / Stop coroutines from anywhere.
    /// Supports runnign from background threads
    /// </summary>
    [AddComponentMenu("Foundation/Core/TaskManager")]
    [ExecuteInEditMode]
    public class TaskManager : MonoBehaviour
    {
        public static bool IsMainThread
        {
            get { return !Thread.CurrentThread.IsBackground && !Thread.CurrentThread.IsThreadPoolThread; }
        }

        /// <summary>
        /// Thread Safe logger
        /// </summary>
        public struct TaskLog
        {
            public LogType Type;
            public object Message;
        }

        public struct CoroutineInfo
        {
            public IEnumerator Coroutine;
            public Action OnComplete;
        }

        /// <summary>
        /// Static Accessor
        /// </summary>
        public static TaskManager Instance
        {
            get
            {
                ConfirmInit();
                return _instance;
            }
        }

        /// <summary>
        /// Confirms the instance is ready for use
        /// </summary>
        public static void ConfirmInit()
        {
            if (_instance == null)
            {
                var old = FindObjectsOfType<TaskManager>();
                foreach (var manager in old)
                {
                    if (Application.isEditor)
                        DestroyImmediate(manager.gameObject);
                    else
                        Destroy(manager.gameObject);
                }


                var go = new GameObject("_TaskManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<TaskManager>();
            }

        }

        /// <summary>
        /// Scheduled the routine to run (on the main thread)
        /// </summary>
        public static Coroutine StartRoutine(IEnumerator coroutine)
        {
            //Make sure we are in the main thread
            if (!IsMainThread)
            {
                lock (syncRoot)
                {
                    PendingAdd.Add(coroutine);

                    //Debug.LogWarning("Running coroutines from background thread are not awaitable. Use CoroutineInfo");
                    return null;
                }
            }

            return Instance.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Scheduled the routine to run (on the main thread)
        /// </summary>
        public static void StartRoutine(CoroutineInfo info)
        {
            //Make sure we are in the main thread
            if (!IsMainThread)
            {
                lock (syncRoot)
                {
                    PendingCoroutineInfo.Add(info);
                }
            }
            else
            {
                Instance.StartCoroutine(Instance.RunCoroutineInfo(info));
            }
        }

        /// <summary>
        /// Scheduled the routine to run (on the main thread)
        /// </summary>
        public static void StopRoutine(IEnumerator coroutine)
        {
            //Make sure we are in the main thread
            if (!IsMainThread)
            {
                lock (syncRoot)
                {
                    PendingRemove.Add(coroutine);
                }
            }
            else
            {
                Instance.StartCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Schedules the action to run on the main thread
        /// </summary>
        /// <param name="action"></param>
        public static void RunOnMainThread(Action action)
        {
            //Make sure we are in the main thread
            if (!IsMainThread)
            {
                lock (syncRoot)
                {
                    PendingActions.Add(action);
                }
            }
            else
            {
                action();
            }

        }

        /// <summary>
        /// A thread safe logger
        /// </summary>
        /// <param name="m"></param>
        public static void Log(TaskLog m)
        {
            if (!IsMainThread)
            {
                lock (syncRoot)
                {
                    PendingLogs.Add(m);
                }
            }
            else
            {
                Write(m);
            }

        }

        static void Write(TaskLog m)
        {
            switch (m.Type)
            {
                case LogType.Warning:
                    Debug.LogWarning(m.Message);
                    break;
                case LogType.Error:
                case LogType.Exception:
                    Debug.LogError(m.Message);
                    break;
                case LogType.Log:
                case LogType.Assert:
                    Debug.Log(m.Message);
                    break;
            }
        }

        private static TaskManager _instance;
        private static object syncRoot = new object();
        protected static readonly List<CoroutineInfo> PendingCoroutineInfo = new List<CoroutineInfo>();
        protected static readonly List<IEnumerator> PendingAdd = new List<IEnumerator>();
        protected static readonly List<IEnumerator> PendingRemove = new List<IEnumerator>();
        protected static readonly List<Action> PendingActions = new List<Action>();
        protected static readonly List<TaskLog> PendingLogs = new List<TaskLog>();

        protected void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        protected void Update()
        {
            if (PendingAdd.Count == 0 && PendingRemove.Count == 0 && PendingActions.Count == 0 && PendingLogs.Count == 0 && PendingCoroutineInfo.Count == 0)
                return;

            lock (syncRoot)
            {
                for (int i = 0;i < PendingLogs.Count;i++)
                {
                    Write(PendingLogs[i]);
                }
                for (int i = 0;i < PendingAdd.Count;i++)
                {
                    StartCoroutine(PendingAdd[i]);
                }
                for (int i = 0;i < PendingRemove.Count;i++)
                {
                    StopCoroutine(PendingRemove[i]);
                }
                for (int i = 0;i < PendingCoroutineInfo.Count;i++)
                {
                    StartCoroutine(RunCoroutineInfo(PendingCoroutineInfo[i]));
                }
                for (int i = 0;i < PendingActions.Count;i++)
                {
                    PendingActions[i]();
                }
                PendingAdd.Clear();
                PendingRemove.Clear();
                PendingActions.Clear();
                PendingLogs.Clear();
                PendingCoroutineInfo.Clear();
            }
        }

        IEnumerator RunCoroutineInfo(CoroutineInfo info)
        {
            yield return StartCoroutine(info.Coroutine);

            if (info.OnComplete != null)
                info.OnComplete();
        }
    }
}