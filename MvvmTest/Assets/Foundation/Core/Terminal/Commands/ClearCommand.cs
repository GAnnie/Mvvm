// --------------------------------------
//  Unity Foundation
//  ClearCommand.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Extends the console with 'about me' command
    /// </summary>
    [AddComponentMenu("Foundation/Terminal/Clear")]
    public class ClearCommand : MonoBehaviour
    {
        void Awake()
        {
            Terminal.Add(new TerminalCommand
            {
                Label = "Clear",
                Method = () => Terminal.Clear()
            });
        }

    }
}
