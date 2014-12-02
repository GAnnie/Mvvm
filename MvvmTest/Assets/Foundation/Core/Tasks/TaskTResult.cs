using System;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------
//  Unity Foundation
//  ConsoleSetup.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

namespace Foundation
{
    /// <summary>
    /// System.Threading.Tasks.Task implementation
    /// </summary>
    public class Task<TResult> : Task
    {
        #region public fields
        Func<TResult> _function;
        Delegate _function2;
#if UNITY_IOS
        protected new List<Delegate> OnComplete;
#else
        protected new List<Action<Task<TResult>>> OnComplete;
#endif

        /// <summary>
        /// get the result of the task. Blocking. It is recommended you yield on the wait before accessing this value
        /// </summary>
        public TResult Result;

        #endregion

        #region ctor

        public Task()
        {

        }

        /// <summary>
        /// Returns the task in the Success state.
        /// </summary>
        /// <param name="result"></param>
        public Task(TResult result)
            : this()
        {
            Status = TaskStatus.Success;
            Strategy = TaskStrategy.Custom;
            Result = result;
        }

        /// <summary>
        /// Creates a new background Task strategy
        /// </summary>
        /// <param name="function"></param>
        public Task(Func<TResult> function)
            : this()
        {
            if (function == null)
                throw new ArgumentNullException("function");

            _function = function;
            Strategy = TaskStrategy.BackgroundThread;
        }

        /// <summary>
        /// Creates a new background Task strategy
        /// </summary>
        public Task(Delegate function, object param)
            : this()
        {
            if (function == null)
                throw new ArgumentNullException("function");

            _function2 = function;
            Paramater = param;
            Strategy = TaskStrategy.BackgroundThread;
        }

        /// <summary>
        /// Creates a new task with a specific strategy
        /// </summary>
        public Task(Func<TResult> function, TaskStrategy mode)
            : this()
        {
            if (function == null)
                throw new ArgumentNullException("function");

            if (mode == TaskStrategy.Coroutine)
                throw new ArgumentException("Mode can not be coroutine");

            _function = function;
            Strategy = mode;
        }


        /// <summary>
        /// Creates a new task with a specific strategy
        /// </summary>
        public Task(Delegate function, object param, TaskStrategy mode)
            : this()
        {
            if (function == null)
                throw new ArgumentNullException("function");

            if (mode == TaskStrategy.Coroutine)
                throw new ArgumentException("Mode can not be coroutine");

            _function2 = function;
            Paramater = param;
            Strategy = mode;
        }

        /// <summary>
        /// Creates a new Coroutine  task
        /// </summary>
        public Task(IEnumerator routine)
        {
            if (routine == null)
                throw new ArgumentNullException("routine");


            _routine = routine;
            Strategy = TaskStrategy.Coroutine;
        }

        /// <summary>
        /// Creates a new Task in a Faulted state
        /// </summary>
        /// <param name="ex"></param>
        public Task(Exception ex)
        {
            Exception = ex;
            Strategy = TaskStrategy.Custom;
            Status = TaskStatus.Faulted;
        }

        /// <summary>
        /// Creates a new task
        /// </summary>
        public Task(TaskStrategy mode)
            : this()
        {
            Strategy = mode;
        }
        #endregion

        #region protected methods

        protected override void Execute()
        {
            try
            {
                if (_function2 != null)
                {
                    Result = (TResult)_function2.DynamicInvoke(Paramater);
                }
                else if (_function != null)
                {
                    Result = _function();
                }
                Status = TaskStatus.Success;
            }
            catch (Exception ex)
            {
                Exception = ex;
                Status = TaskStatus.Faulted;
                if (LogErrors)
                    Logger.LogException(ex);
            }
        }


        protected override void OnTaskComplete()
        {
            if (OnComplete != null)
            {
                for (int i = 0;i < OnComplete.Count;i++)
                {
#if UNITY_IOS
                    OnComplete[i].DynamicInvoke(this);
#else
                    OnComplete[i](this);
#endif
                }
            }
        }

        #endregion

        #region public

        /// <summary>
        /// Called after the task is complete
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task<TResult> ContinueWith(Action<Task<TResult>> action)
        {
            if (IsCompleted)
            {
                action(this);
            }
            else
            {
#if UNITY_IOS
                if (OnComplete == null)
                    OnComplete = new List<Delegate>(2);
#else
                if (OnComplete == null)
                    OnComplete = new List<Action<Task<TResult>>>(2);
#endif
                OnComplete.Add(action);
            }

            return this;
        }

        /// <summary>
        /// will throw if faulted
        /// </summary>
        /// <returns></returns>
        public new Task<TResult> ThrowIfFaulted()
        {
            if (IsFaulted)
                throw Exception;
            return this;
        }

        public new Task<TResult> Wait()
        {
            base.Wait();

            return this;
        }

        public override void Dispose()
        {
            base.Dispose();

            Result = default(TResult);
            _function = null;
            _function2 = null;
            OnComplete = null;
        }
        #endregion

    }
}
