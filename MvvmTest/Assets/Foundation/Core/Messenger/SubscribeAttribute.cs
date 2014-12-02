// --------------------------------------
//  Unity Foundation
//  SubscribeAttribute.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;

namespace Foundation.Core
{
    /// <summary>
    /// Attribute for identifying method handlers for automatic Messenger subscription
    /// </summary> 
    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute
    {
    }
}
