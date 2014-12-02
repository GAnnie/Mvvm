// --------------------------------------
//  Unity Foundation
//  ExportAttribute.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;

namespace Foundation.Core
{
    /// <summary>
    /// Decorate a instance with this to define the export.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
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
        public ExportAttribute()
        {
            
        }

        /// <summary>
        /// Uses an Optional lookup key
        /// </summary>
        /// <param name="key"></param>
        public ExportAttribute(string key)
        {
            InjectKey = key;
        }
    }
}