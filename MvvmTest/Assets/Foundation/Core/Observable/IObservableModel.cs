// --------------------------------------
//  Unity Foundation
//  IBindable.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using UnityEngine;

namespace Foundation.Core
{ 
    /// <summary>
    /// BindingInfo Message raised by the IBindable when value changes
    /// </summary>
    [Serializable]
    public class ObservableMessage : IDisposable
    {
        /// <summary>
        /// The sender of this message
        /// </summary>
        [SerializeField]
        public object Sender;

        /// <summary>
        /// Property, Field, or Member name
        /// </summary>
        [SerializeField]
        public string Name;

        /// <summary>
        /// Property,Field or Method arguments
        /// </summary>
        [SerializeField]
        public object Value;

        /// <summary>
        /// Casts as value helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CastValue<T>()
        {
            return (T)Value;
        }

        public void Dispose()
        {
            Name = null;
            Value = Sender = null;
        }

        public override string ToString()
        {
            return "BindingMessage " + Name + " " + Value;
        }
    }

    /// <summary>
    /// Interface of objects that support binding using the binding system.
    /// This system uses reflection to facilitate communication between parts.
    /// </summary>
    public interface IObservableModel 
    {
        /// <summary>
        /// Raised when a property is changed.
        /// </summary>
        event Action<ObservableMessage> OnBindingUpdate;

        /// <summary>
        /// Raises OnBindingMessage
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="paramater"></param>
        void RaiseBindingUpdate(string memberName, object paramater);
 
        /// <summary>
        /// Gets the value of the property of field.
        /// Calls the method with return value
        /// </summary>
        /// <param name="memberName"></param>
        object GetValue(string memberName);

        /// <summary>
        /// Calls the method with argument and return value
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="paramater"></param>
        object GetValue(string memberName, object paramater);

        /// <summary>
        /// Calls the method.
        /// Starts coroutine.
        /// </summary>
        /// <param name="memberName"></param>
        void Command(string memberName);
        
        /// <summary>
        /// Sets the value of a property or field.
        /// Calls the method.
        /// Starts coroutine.
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="paramater"></param>
        void Command(string memberName, object paramater);
    }
}