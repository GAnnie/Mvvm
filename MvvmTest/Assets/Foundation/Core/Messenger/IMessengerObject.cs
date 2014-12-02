// --------------------------------------
//  Unity Foundation
//  IMessengerObject.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

namespace Foundation.Core
{
    /// <summary>
    /// Extension interface. Include messenger extension methods for messenger objects.
    /// </summary>
    public interface IMessengerObject
    {

    }

    /// <summary>
    /// Extends IMessengerObject with two static publish extension methods
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IMessagerObjectExt
    {
        /// <summary>
        /// Publishes the object via Messenger.SendMessage.
        /// </summary>
        /// <param name="message">the message</param>
        public static void Publish(this IMessengerObject message)
        {
            //   UnityEngine.Debug.Log(message);
            Messenger.Publish(message.GetType(), message);
        }

        /// <summary>
        /// Publishes the object via Messenger.SendMessage.
        /// </summary>
        /// <param name="message">the message</param>
        /// <param name="ignore">Ignores all delegates member to this instance.</param>
        public static void Publish(this IMessengerObject message, object ignore)
        {
            Messenger.Publish(message.GetType(), message, ignore);
        }

        /// <summary>
        /// Publishes the message to most delegates listening to this message.
        /// Caches the message for late subscribers
        /// </summary>
        /// <param name="message">the message</param>
        public static void PublishAndCache(this IMessengerObject message)
        {
            Messenger.PublishAndCache(message);
        }

        /// <summary>
        /// Publishes the object via Messenger.SendMessage. and caches it
        /// </summary>
        /// <param name="message">the message</param>
        /// <param name="ignore">Object instance to ignore when sending. Used to prevent recursive calls.</param>
        public static void PublishAndCache(this IMessengerObject message, object ignore)
        {
            Messenger.PublishAndCache(message, ignore);
        }

        /// <summary>
        /// Removes the message from the cache
        /// </summary>
        /// <param name="message">the message</param>
        public static void RemoveCache(this IMessengerObject message)
        {
            Messenger.RemoveCache(message);
        }
    }
}