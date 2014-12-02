using System.Collections.Generic;
using System.Linq;
using System.Text;
using LITJson;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Utility for loading Configuration files
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// Does the Configuration exist ?
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasConfig(string path)
        {
            var r = Resources.Load<TextAsset>(path) != null;

            if (!r)
                Debug.LogWarning(string.Format("Configuration Resources/{0}.txt not found.", path));

            return r;
        }

        /// <summary>
        /// returns the configuration as a string
        /// </summary>
        /// <param name="file">/resources/</param>
        /// <returns></returns>
        public static string GetText(string file)
        {
            var asset = Resources.Load<TextAsset>(file);

            if (asset == null)
            {
                Debug.LogWarning(string.Format("Text Configuration Resources/{0}.txt not found.", file));
                return string.Empty;
            }

            return StripComments(asset.text);
        }

        /// <summary>
        /// returns the configuration as CSV
        /// </summary>
        /// <param name="file">/resources/</param>
        /// <returns></returns>
        public static List<string[]> GetCSV(string file)
        {
            var asset = Resources.Load<TextAsset>(file);

            if (asset == null)
            {
                Debug.LogWarning(string.Format("CSV Configuration Resources/{0}.txt not found.", file));
                return new List<string[]>(0);
            }

            var csv = CsvReader.ReadCSV(StripComments(asset.text));

            var wtf = csv.Where(o => o != null).ToArray();

            return wtf.ToList();
        }

        /// <summary>
        /// returns the configuration using JSON parsing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">/resources/</param>
        /// <returns></returns>
        public static T Get<T>(string file) where T : new()
        {
            return Get(file, new T());
        }

        /// <summary>
        /// returns the configuration using JSON parsing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">/resources/</param>
        /// <param name="defaultValue">default Value</param>
        /// <returns></returns>
        public static T Get<T>(string file, T defaultValue) where T : new()
        {
            var asset = Resources.Load<TextAsset>(file);

            if (asset == null)
            {
                Debug.LogWarning(string.Format("JSON Configuration Resources/{0}.txt not found.", file));
                return defaultValue;
            }

            var stripped = StripComments(asset.text);

            return JsonMapper.ToObject<T>(stripped);
        }

        /// <summary>
        /// Removes comments from configuration files
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string StripComments(string result)
        {
            var split = result.SplitByNewline();

            var sb = new StringBuilder();

            foreach (var s in split)
            {
                if(s.StartsWith("//"))
                    continue;

                if (string.IsNullOrEmpty(s))
                    continue;

                sb.AppendLine(s);
            }

            return sb.ToString().Replace(@"\t", " ");
        }
    }
}