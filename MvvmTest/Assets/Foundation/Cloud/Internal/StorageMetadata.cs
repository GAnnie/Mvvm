using System;
using System.Reflection;
using Foundation.Core;
using UnityEngine;

namespace Foundation.Cloud.Internal
{


    /// <summary>
    /// Utility class for serialization of clr objects using reflection
    /// </summary>
    public class StorageMetadata
    {
        #region static
        protected static ThreadSafeDictionary<Type, StorageMetadata> KnowenReflections = new ThreadSafeDictionary<Type, StorageMetadata>();

        /// <summary>
        /// Registers a type using Annotations 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterType<T>() where T : class
        {
            var type = typeof(T);

            if (KnowenReflections.ContainsKey(type))
                return;

            var table = (StorageTable)Attribute.GetCustomAttribute(type, typeof(StorageTable));
            if (table == null)
                throw new Exception("StorageTable Attribute is required");

            var meta = new StorageMetadata
            {
                TableName = table.TableName,
                ObjectType = type,
            };

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (string.IsNullOrEmpty(meta.IdPropertyName))
                {
                    if (prop.HasAttribute<StorageIdentity>())
                    {
                        if (prop.PropertyType != typeof(string))
                            Debug.LogError("StorageIdentity must be string");

                        meta.IdPropertyName = prop.Name;
                    }
                }
                else if (string.IsNullOrEmpty(meta.ScorePropertyName))
                {
                    if (prop.HasAttribute<StorageScore>())
                    {
                        if (prop.PropertyType != typeof(int) &&
                            prop.PropertyType != typeof(uint) &&
                            prop.PropertyType != typeof(short) &&
                            prop.PropertyType != typeof(double) &&
                            prop.PropertyType != typeof(float))
                            Debug.LogError("StorageIdentity must be a number");

                        meta.ScorePropertyName = prop.Name;

                    }
                }
                else
                    break;
            }

            if (string.IsNullOrEmpty(meta.IdPropertyName))
                throw new Exception("StorageIdentity Attribute is required");

            KnowenReflections.Add(type, meta);

        }

        /// <summary>
        /// Registers a type without annotations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        public static void RegisterType<T>(StorageMetadata meta) where T : class
        {
            var type = typeof(T);

            if (string.IsNullOrEmpty(meta.IdPropertyName))
                throw new Exception("StorageIdentity Attribute is required");

            if (string.IsNullOrEmpty(meta.TableName))
                throw new Exception("StorageTable Attribute is required");

            if (KnowenReflections.ContainsKey(type))
                return;

            KnowenReflections.Add(type, meta);
        }

        /// <summary>
        /// Returns the metadata
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static StorageMetadata GetMetadata<T>() where T : class{
        
            RegisterType<T>();

            return KnowenReflections[typeof (T)];
        }

        #endregion

        #region fields
        public Type ObjectType;
        public string TableName;
        public string IdPropertyName;
        public string ScorePropertyName;
        #endregion

        #region methods
        public string GetId(object instance)
        {
            return (string)ObjectType.GetProperty(IdPropertyName).GetValue(instance, null);
        }

        public float GetScore(object instance)
        {
            if (string.IsNullOrEmpty(ScorePropertyName))
                return 0f;

            return float.Parse(ObjectType.GetProperty(ScorePropertyName).GetValue(instance, null).ToString());
        }

        #endregion
    }
}
