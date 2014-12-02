// --------------------------------------
//  Unity Foundation
//  ConsoleSetup.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Collections;
using Foundation.Core;
using UnityEngine;

namespace Foundation.Examples
{
    [AddComponentMenu("Foundation/Examples/TaskTests")]
    public class TaskTests : MonoBehaviour
    {
        protected void Awake()
        {
            Terminal.Add(new TerminalCommand
            {
                Label = "Tests",
                Method = () => StartCoroutine(TestTasks())
            });

            Terminal.Add(new TerminalCommand
            {
                Label = "Main",
                Method = MainTest
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Background",
                Method = Background
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Routine",
                Method = Routine
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "BG To Main",
                Method = BackgroundToMain
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "BG To Routine",
                Method = BackgroundToRotine
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "BG To Bg",
                Method = BackgroundToBackground
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "BG To Bg EX",
                Method = BackgroundToBackgroundException
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "BG  EX",
                Method = BackgroundException
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Coroutine state",
                Method = CoroutineTaskState
            });
        }

        #region test routine

        public IEnumerator TestTasks()
        {
            yield return 1;
            Terminal.LogImportant("Tests (9)");

            Task.Run(() => Logger.Log("1 Run Complete"));
            yield return new WaitForSeconds(1);
            Task.Run(Test2, "2 Run With Param Complete");
            yield return new WaitForSeconds(1);
            Task.RunCoroutine(Test3);
            yield return new WaitForSeconds(1);
            Task.RunCoroutine(Test4()).ContinueWith(t => Logger.Log("4 complete"));
            yield return new WaitForSeconds(1);
            Task.RunCoroutine(Test5).ContinueWith(t => Logger.Log("5 complete"));
            yield return new WaitForSeconds(1);
            Task.Run(() => { return "6 Run with Result Complete"; }).ContinueWith(t => Logger.Log(t.Result));
            yield return new WaitForSeconds(1);
            Task.Run<string, string>(Test7, "7 Run with Param and Result").ContinueWith(t => Logger.Log(t.Result));
            yield return new WaitForSeconds(1);
            var t1 = Task.RunCoroutine<string>(Test8).ContinueWith(t => t.Result = "8 Complete");
            yield return new WaitForSeconds(1);
            Logger.Log(t1.Result);
            Task.RunCoroutine<string>(Test9).ContinueWith(t => Logger.Log(t.Result));
        }


        void Test2(string param)
        {
            Debug.Log(param);
        }

        IEnumerator Test3()
        {
            yield return 1;
            Debug.Log("3 Coroutine Complete");
        }
        IEnumerator Test5(Task task)
        {
            yield return 1;
            if (task == null)
                Debug.LogWarning("wtf");
            Debug.Log("5 Coroutine with Task State Complete");
        }

        IEnumerator Test4()
        {
            yield return 1;
            Debug.Log("4 Coroutine");
        }

        string Test7(string param)
        {
            param += " Complete";
            return param;
        }
        IEnumerator Test8()
        {
            yield return 1;
            Debug.Log("8 Coroutine With Result...");
        }
        IEnumerator Test9(Task<string> task)
        {
            yield return 1;
            if (task == null)
                Debug.LogWarning("wtf");

            task.Result = ("9 Coroutine with Task State Complete");
        }
       

        #endregion

        void MainTest()
        {
            Task.RunOnMain(() =>
            {
                Logger.Log("Sleeping...");
                Task.Delay(2000);
                Logger.Log("Slept");
            });
        }

        void Background()
        {
            Task.Run(() =>
            {
                Terminal.Log("Sleeping...");
                Task.Delay(2000);
                Terminal.Log("Slept");
            });
        }

        void Routine()
        {
            Task.RunCoroutine(RoutineFunction());
        }

        IEnumerator RoutineFunction()
        {
            Logger.Log("Sleeping...");
            yield return new WaitForSeconds(2);
            Logger.Log("Slept");
        }



        void BackgroundToMain()
        {
            Task.Run(() =>
            {

                Logger.Log("Thread A Running");

                var task = Task.RunOnMain(() =>
                   {
                       Logger.Log("Sleeping...");
                       Task.Delay(2000);
                       Logger.Log("Slept");
                   });

                while (task.IsRunning)
                {
                    Logger.Log(".");
                    Task.Delay(100);
                }

                Logger.Log("Thread A Done");
            });
        }


        void BackgroundToRotine()
        {
            Task.Run(() =>
            {
                Logger.Log("Thread A Running");

                var task = Task.RunCoroutine(RoutineFunction());

                while (task.IsRunning)
                {
                    Logger.Log(".");
                    Task.Delay(500);
                }

                Logger.Log("Thread A Done");
            });

        }

        void BackgroundToBackground()
        {
            Task.Run(() =>
            {
                Terminal.Log("1 Sleeping...");

                Task.Run(() =>
                {
                    Terminal.Log("2 Sleeping...");
                    Task.Delay(2000);
                    Terminal.Log("2 Slept");
                });
                Task.Delay(2000);
                Terminal.Log("1 Slept");
            });

        }

        void BackgroundToBackgroundException()
        {
            var task1 = Task.Run(() =>
            {
                Terminal.Log("1 Go");

                var task2 = Task.Run(() =>
                {
                    Task.Delay(100);
                    Terminal.Log("2 Go");
                    throw new Exception("2 Fail");
                });

                task2.Wait();

                if (task2.IsFaulted)
                    throw task2.Exception;
            });

            task1.Wait();

            Debug.Log(task1.Status + " " + task1.Exception.Message);

        }

        void BackgroundException()
        {
            var task1 = Task.Run(() =>
            {
                throw new Exception("Hello World");
            });

            task1.Wait();

            Debug.Log(task1.Status + " " + task1.Exception.Message);

        }


        void CoroutineTaskState()
        {
            Task.RunCoroutine<string>(CoroutineTaskStateAsync).ContinueWith(o => Debug.Log(o.Result));
        }

        IEnumerator CoroutineTaskStateAsync(Task<string> task)
        {
            yield return 1;

            task.Result = "Hello World";
        }

    }
}
