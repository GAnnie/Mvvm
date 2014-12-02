// --------------------------------------
//  Unity Foundation
//  AboutCommand.cs
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
    [AddComponentMenu("Foundation/Terminal/About")]
    public class AboutCommand : MonoBehaviour
    {
        [Multiline]
        public string Text =
        "\r\n"+
        "Terminal by Nicholas Ventimiglia 2013 \r\n" +
        "Part of the Foundation Toolkit \r\n" +
        "www.AvariceOnline.Com \r\n";

        void Awake()
        {
            Terminal.Add(new TerminalCommand
            {
                Label = "About",
                Method = () => Terminal.Log(Text)
            });
        }

    }
}
