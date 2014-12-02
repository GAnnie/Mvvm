// --------------------------------------
//  Unity Foundation
//  ConsoleSetup.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// An optional setup script for the Terminal system.
    /// Applies the color scheme to the context.
    /// </summary>
    [AddComponentMenu("Foundation/Terminal/TerminalSetup")]
    public class TerminalSetup : MonoBehaviour
    {
        public Color LogColor = Color.white;
        public Color WarningColor = Color.yellow;
        public Color ErrorColor = Color.red;

        public Color SuccessColor = Color.green;
        public Color InputColor = Color.cyan;
        public Color ImportantColor = Color.yellow;
        
        public void Awake()
        {
            Terminal.Instance.LogColor = LogColor;
            Terminal.Instance.WarningColor = WarningColor;
            Terminal.Instance.ErrorColor = ErrorColor;
            Terminal.Instance.SuccessColor = SuccessColor;
            Terminal.Instance.InputColor = InputColor;
            Terminal.Instance.ImportantColor = ImportantColor;
        }
    }
}