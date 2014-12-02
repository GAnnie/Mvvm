// --------------------------------------
//  Unity Foundation
//  CachedMessage.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;

namespace Foundation.Core
{
    /// <summary>
    /// Marks the message as cached
    /// </summary> 
    public class CachedMessage : Attribute
    {
        /// <summary>
        /// Only one message of this type should exist.
        /// Setting this to true will clear the cache for this message type.
        /// </summary>
        public bool OnePerType { get; set; }
    }
}