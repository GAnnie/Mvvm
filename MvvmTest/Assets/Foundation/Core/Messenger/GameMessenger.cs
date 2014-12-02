// --------------------------------------
//  Unity Foundation
//  GameMessenger.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// A higher speed broker for gameplay specific messages.
    /// </summary>
    public class GameMessenger<T>
    {
        /// <summary>
        /// Dictionary of all actions subscribing to the game messages
        /// </summary>
        public static readonly Dictionary<GameObject, List<Action<T>>> Handlers = new Dictionary<GameObject, List<Action<T>>>();

        /// <summary>
        /// Dictionary of all routines subscribing to the game messages
        /// </summary>
        public static readonly Dictionary<GameObject, List<Func<T,IEnumerator>>> Routines = new Dictionary<GameObject, List<Func<T,IEnumerator>>>();

        /// <summary>
        /// Subscribes to the message event
        /// </summary>
        /// <param name="context">the target of the message</param>
        /// <param name="handler">handler</param>
        public static void Subscribe(GameObject context, Action<T> handler)
        {
            if (!Handlers.ContainsKey(context))
            {
                Handlers.Add(context, new List<Action<T>>());
            }

            Handlers[context].Add(handler);
        }

        /// <summary>
        /// Unsubscribes from the message event
        /// </summary>
        public static void Unsubscribe(GameObject context, Action<T> handler)
        {
            if (Handlers.ContainsKey(context))
            {
                Handlers[context].Remove(handler);

                if (Handlers[context].Count == 0)
                    Handlers.Remove(context);
            }
        }

        /// <summary>
        /// Subscribes to the message
        /// </summary>
        /// <param name="context">the target of the message</param>
        /// <param name="handler">handler</param>
        public static void SubscribeCoroutine(GameObject context, Func<T, IEnumerator> handler)
        {
            if (!Routines.ContainsKey(context))
            {
                Routines.Add(context, new List<Func<T,IEnumerator>>());
            }

            Routines[context].Add(handler);

        }

        /// <summary>
        /// Unsubscribes from the message
        /// </summary>
        /// <param name="context">the target of the message</param>
        /// <param name="handler">handler</param>
        public static void UnsubscribeCoroutine(GameObject context, Func<T, IEnumerator> handler)
        {
            if (Routines.ContainsKey(context))
            {
                Routines[context].Remove(handler);

                if (Routines[context].Count == 0)
                    Routines.Remove(context);
            }
        }

        /// <summary>
        /// Unsubscribes all handlers and routines from this context
        /// </summary>
        public static void Unsubscribe(GameObject context)
        {
            Handlers.Remove(context);
            Routines.Remove(context);
        }

        /// <summary>
        /// invokes the listeners for this message event
        /// </summary>
        /// <param name="context">the target of the message</param>
        /// <param name="arg1">parameter to send</param>
        /// <returns>number of methods called</returns>
        public static void Publish(GameObject context, T arg1)
        {
            if (Handlers.ContainsKey(context))
            {
                for (int i = 0; i < Handlers[context].Count; i++)
                {
                    Handlers[context][i].Invoke(arg1);
                }
            }

            if (Routines.ContainsKey(context))
            {
                for (int i = 0; i < Routines[context].Count; i++)
                {
                    TaskManager.StartRoutine(Routines[context][i].Invoke(arg1));
                }
            }
        }
    }
}
