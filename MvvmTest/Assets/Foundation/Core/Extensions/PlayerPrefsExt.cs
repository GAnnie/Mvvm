// --------------------------------------
//  Unity Foundation
//  PlayerPrefsExt.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Extension methods for PlayerPrefs
    /// </summary>
    public static class PlayerPrefsExt
    {
        /// <summary>
        /// Returns the PlayerPref serialized as a bool.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key, 0) == 1;
        }

        /// <summary> 
        /// Returns the PlayerPref serialized as a bool.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        /// <summary>
        /// Sets the PlayerPref with a boolean value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
}