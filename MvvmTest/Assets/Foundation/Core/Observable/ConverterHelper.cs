// --------------------------------------
//  Unity Foundation
//  BindingConverter.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;

namespace Foundation.Core
{
    /// <summary>
    /// helper method for converting parameters
    /// </summary>
    public static class ConverterHelper
    {
        /// <summary>
        /// Converts the parameter to the desired type
        /// </summary>
        /// <param name="paramater"></param>
        /// <returns></returns>
        public static object ConvertTo<T>(object paramater)
        {
            return ConvertTo(typeof(T), paramater);
        }

        /// <summary>
        /// Converts the parameter to the desired type
        /// </summary>
        /// <param name="desiredType"></param>
        /// <param name="paramater"></param>
        /// <returns></returns>
        public static object ConvertTo(Type desiredType, object paramater)
        {
            if (paramater == null)
                return null;

            if (desiredType.IsInstanceOfType(paramater))
                return paramater;


            if (desiredType.IsEnum)
            {
                return Enum.Parse(desiredType, paramater.ToString());
            }
            else if (desiredType == typeof(string))
            {
                return paramater.ToString();
            }
            else if (desiredType == typeof(bool))
            {
                return bool.Parse(paramater.ToString());
            }
            else if (desiredType == typeof(int))
            {
                return int.Parse(paramater.ToString());
            }
            else if (desiredType == typeof(float))
            {
                return float.Parse(paramater.ToString());
            }
            else if (desiredType == typeof(long))
            {
                return long.Parse(paramater.ToString());
            }
            else if (desiredType == typeof(double))
            {
                return double.Parse(paramater.ToString());
            }
            else if (desiredType == typeof(short))
            {
                return short.Parse(paramater.ToString());
            }
            else if (desiredType == typeof(object) && paramater is string)
            {
             //  return Serializer.FromJson((string)paramater, desiredType);
            }

            return null;
        }
    }
}