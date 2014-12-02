// --------------------------------------
//  Unity Foundation
//  StringHelper.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Core
{
    public static class StringHelper
    {
        /// <summary>
        /// writes byte[] from String
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Reads a string from byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// splits by newline
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] SplitByNewline(this string s)
        {
            return s.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Removes any newline
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveNewline(this string s)
        {
            return s.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        }

        public static bool IsEmail(this string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                   + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                   + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }

        /// <summary>
        /// Randoms the number.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();

            char ch;

            for (int i = 0;i < size;i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * UnityEngine.Random.value + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generates an id.
        /// </summary>
        /// <returns></returns>
        public static string GenerateId(int size)
        {
            string g = Guid.NewGuid().ToString().Replace("-", "");

            return g.Substring(0, size);
        }
    }
}