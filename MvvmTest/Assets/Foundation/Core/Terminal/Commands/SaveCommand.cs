﻿// --------------------------------------
//  Unity Foundation
//  SaveCommand.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Foundation.Core
{


    /// <summary>
    /// Extends the terminal with 'about me' command
    /// </summary>
    [AddComponentMenu("Foundation/Terminal/Save")]
    public class SaveCommand : MonoBehaviour
    {
        private void Awake()
        {
            Terminal.Add(
                new TerminalCommand
                {
                    Label = "Save",
                    Method = () => DoSave()
                });
        }

        private void DoSave()
        {

#if UNITY_WEBPLAYER
            Terminal.LogWarning("Save is not supported on web player");
#else

            var sb = new StringBuilder();

            foreach (var message in Terminal.Items)
            {
                sb.AppendLine(message.Formatted);
            }

            var n = string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

            var path = Application.persistentDataPath + "/Console/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(path + n, sb.ToString());


            Terminal.Log(String.Format("Console Saved To '{0}'", n));

#endif

        }
    }
}