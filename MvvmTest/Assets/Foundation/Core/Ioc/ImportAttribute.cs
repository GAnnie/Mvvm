// --------------------------------------
//  Unity Foundation
//  ImportAttribute.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;

namespace Foundation.Core
{
    /// <summary>
    /// Decorate a public field or property with this. Tells the Injector to resolve the dependency.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ImportAttribute : Attribute
    {
     
        /// <summary>
        /// Optional lookup key
        /// </summary>
        public string InjectKey { get; private set; }

        /// <summary>
        /// Is there an optional lookup key ?
        /// </summary>
        public bool HasKey
        {
            get
            {
                return !string.IsNullOrEmpty(InjectKey);
            }
        }

        /// <summary>
        /// Uses standard type match lookup
        /// </summary>
        public ImportAttribute()
        {
            
        }

        /// <summary>
        /// Uses an Optional lookup key
        /// </summary>
        /// <param name="key"></param>
        public ImportAttribute(string key)
        {
            InjectKey = key;
        }
    }
}