// --------------------------------------
//  Unity Foundation
//  FullScreenCommand.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Extends the terminal with 'Full Screen' command
    /// </summary>
    [AddComponentMenu("Foundation/Terminal/FullScreen")]
    public class FullScreenCommand : MonoBehaviour
    {
        void Awake()
        {
#if UNITY_STANDALONE
            Terminal.Add(new TerminalCommand
            {
                Label = "Full Screen",
                Method = () =>
                {
                    Screen.fullScreen = !Screen.fullScreen;
                }
            });
#endif
        }
    }
}