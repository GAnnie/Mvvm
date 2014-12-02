// --------------------------------------
//  Unity Foundation
//  Vector3Helper.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System.Linq;
using UnityEngine;

namespace Foundation.Core
{
    public static class Vector3Helper
    {
        /// <summary>
        /// From string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Vector3 Parse(string text)
        {
            var args = text.Split(',');

            if (args.Count() != 3)
            {
                Debug.LogError("Vector3.Parse takes an input of float,float,float");
                return Vector3.zero;
            }
            else
            {
                return new Vector3(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]));
            }
        }

        /// <summary>
        /// Transforms Random.insideUnitCircle from 2d to 3d space (swap Y and Z).
        /// </summary>
        /// <returns></returns>
        public static Vector3 To2DSpace(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.y);
        }
    }
}