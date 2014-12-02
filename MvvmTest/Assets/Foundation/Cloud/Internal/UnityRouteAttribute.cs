using System;
using System.Linq;

namespace Foundation.Cloud.Internal
{
    /// <summary>
    /// Defines the HTTP path and verb for a API message
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UnityRouteAttribute : Attribute
    {
        /// <summary>
        /// Controller / Action Method
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public Type ReturnType = null;

        public UnityRouteAttribute(string route)
        {
            Path = route;
        }

        public UnityRouteAttribute(string route, Type returns)
        {
            Path = route;
            ReturnType = returns;
        }

        /// <summary>
        /// Helper for getting routes
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        /// <returns>{Controller}/{Action}</returns>
        public static UnityRouteAttribute GetRoute<T>()
        {
            return GetRoute(typeof(T));
        }

        /// <summary>
        /// Helper for getting routes
        /// </summary>
        /// <returns>{Controller}/{Action}</returns>
        public static UnityRouteAttribute GetRoute(object message)
        {
            if (!(message is Type))
            {
                return GetRoute(message.GetType());
            }

            var type = (Type)message;

            if (type.IsArray)
            {
                return GetRoute(type.GetElementType());
            }

            if (type.IsGenericType)
            {
                return GetRoute(type.GetGenericArguments().First());
            }

            return type.GetCustomAttributes(typeof(UnityRouteAttribute), true).FirstOrDefault() as UnityRouteAttribute;
        }
    }
}