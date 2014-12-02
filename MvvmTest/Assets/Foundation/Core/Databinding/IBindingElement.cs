// --------------------------------------
//  Unity Foundation
//  IBindingElement.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 


namespace Foundation.Core.Databinding
{
    /// <summary>
    /// Interface for a UI interaction that receives binding messages from the context
    /// </summary>
    public interface IBindingElement
    {
        /// <summary>
        /// Manager
        /// </summary>
        BindingContext Context { get; }

        /// <summary>
        /// Access to Model
        /// </summary>
        IObservableModel Model { get; set; }

        /// <summary>
        /// Handle Updates
        /// </summary>
        /// <param name="message"></param>
        void OnBindingMessage(ObservableMessage message);
    }
}