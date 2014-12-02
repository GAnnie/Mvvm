//// --------------------------------------
////  Unity Foundation
////  PlayerPrefsExt.cs
////  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
////  All rights reserved.
////  -------------------------------------
//// 

//using System;
//using UnityEngine;
//using System.Linq;

//namespace Foundation.System
//{
//    /// <summary>
//    /// Extension ScriptableObject with a 
//    /// </summary>
//    public static class ScriptableObjectExt
//    {
//        /// <summary>
//        /// Returns the PlayerPref serialized as a bool.
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public static Type[] Get ()
//        {
//            var types = typeof(ScriptableObject).Assembly.GetTypes().Where(o => o.IsSubclassOf(typeof(ScriptableObject)));


//            var so = new ScriptableObject();

//            ScriptableObject.CreateInstance<>()
//            foreach (var type in types)
//            {
//                type.

//            }
//        }

//        /// <summary> 
//        /// Returns the PlayerPref serialized as a bool.
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="defaultValue"></param>
//        /// <returns></returns>
//        public static bool GetBool(string key, bool defaultValue)
//        {
//            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
//        }

//        /// <summary>
//        /// Sets the PlayerPref with a boolean value.
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="value"></param>
//        public static void SetBool(string key, bool value)
//        {
//            PlayerPrefs.SetInt(key, value ? 1 : 0);
//        }
//    }
//}