using System;
using UnityEngine;

namespace Foundation
{
    /// <summary>
    /// Use for Debug.Log in background threads
    /// </summary>
    public static class Logger
    {
        public static void Log(object message)
        {
            TaskManager.Log(new TaskManager.TaskLog
            {
                Message = message,
                Type = LogType.Log
            });

        }
        public static void LogError(object message)
        {
            TaskManager.Log(new TaskManager.TaskLog
            {
                Message = message,
                Type = LogType.Error
            });

        }
        public static void LogWarning(object message)
        {
            TaskManager.Log(new TaskManager.TaskLog
            {
                Message = message,
                Type = LogType.Warning
            });
        }
        public static void LogException(Exception message)
        {
            TaskManager.Log(new TaskManager.TaskLog
            {
                Message = message,
                Type = LogType.Exception
            });
        }
    }
}