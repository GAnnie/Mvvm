using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation.Cloud.Internal;
using Foundation.Core;
using LITJson;
using UnityEngine;

namespace Foundation.Cloud
{
    /// <summary>
    /// A in memory database for objects. Use for objects that may be needed if disconnected.
    /// </summary>
    [InjectorService]
    public class CloudCache
    {
        #region Static

        private static CloudCache _instance;
        public static CloudCache Instance
        {
            get { return _instance ?? (_instance = new CloudCache()); }
        }

        #endregion

        #region props

        /// <summary>
        /// Is loaded into memory
        /// </summary>
        public bool IsLoaded { get; private set; }

        const string PrefKey = "Foundation.Cloud.CloudContext";

        private Dictionary<string, Dictionary<string, string>> _cache;

        protected CloudConfig Config
        {
            get { return CloudConfig.Instance; }
        }

        #endregion

        #region ctor, Loading, Saving

        public static void ConfirmInit()
        {
            if (_instance == null)
                _instance = new CloudCache();
        }

        public CloudCache()
        {
            Load();
        }

        /// <summary>
        /// Saves to Prefs
        /// </summary>
        public void Save()
        {
            TaskManager.RunOnMainThread(() =>
            {
                var json = JsonMapper.ToJson(_cache);
                PlayerPrefs.SetString(PrefKey, json);
                PlayerPrefs.Save();
            });
        }

        /// <summary>
        /// Loads from Prefs
        /// </summary>
        public void Load()
        {
            TaskManager.RunOnMainThread(() =>
            {
                if (PlayerPrefs.HasKey(PrefKey))
                {
                    var pref = PlayerPrefs.GetString(PrefKey);
                    _cache = JsonMapper.ToObject<Dictionary<string, Dictionary<string, string>>>(pref);
                }
                else
                {
                    _cache = new Dictionary<string, Dictionary<string, string>>();
                }

                IsLoaded = true;
            });
        }

        /// <summary>
        /// Wait for IsLoaded
        /// </summary>
        public IEnumerator WaitForLoadRoutine()
        {
            yield return 1;

            while (!IsLoaded)
            {
                yield return 1;
            }
        }

        /// <summary>
        /// Wait for IsLoaded
        /// </summary>
        public void WaitForLoad()
        {
            while (!IsLoaded)
            {
                Task.Delay(10);
            }
        }
        #endregion



        /// <summary>
        /// Has Single
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains<T>(string id) where T : class
        {
            if (id == null)
                return false;

            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                return false;

            var list = _cache[meta.TableName];

            return (list.ContainsKey(id));
        }


        /// <summary>
        /// Get Single
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(string id) where T : class
        {
            if (id == null)
                return null;

            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                return null;

            var list = _cache[meta.TableName];

            if (!list.ContainsKey(id))
                return null;

            return JsonMapper.ToObject<T>(list[id]);
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Get<T>() where T : class
        {
            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                return new T[0];

            var list = _cache[meta.TableName];

            return list.Values.Select(o => JsonMapper.ToObject<T>(o));
        }

        /// <summary>
        /// Add or Save
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Set<T>(T entity) where T : class
        {
            if(entity == null)
                return;

            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                _cache.Add(meta.TableName, new Dictionary<string, string>());

            var list = _cache[meta.TableName];
            var id = meta.GetId(entity);

            if (!list.ContainsKey(id))
                list.Add(id, JsonMapper.ToJson(entity));
            else
                list[id] = JsonMapper.ToJson(entity);
        }

        /// <summary>
        /// Add or Save Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void SetCollection<T>(IEnumerable<T> entities) where T : class
        {
            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                _cache.Add(meta.TableName, new Dictionary<string, string>());

            var list = _cache[meta.TableName];

            foreach (var entity in entities)
            {
                var id = meta.GetId(entity);
                if (!list.ContainsKey(id))
                    list.Add(id, JsonMapper.ToJson(entity));
                else
                    list[id] = JsonMapper.ToJson(entity);
            }
        }

        /// <summary>
        /// Remove Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Remove<T>(T entity) where T : class
        {
            if (entity == null)
                return;

            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                _cache.Add(meta.TableName, new Dictionary<string, string>());

            var list = _cache[meta.TableName];
            var id = meta.GetId(entity);

            if (list.ContainsKey(id))
                list.Remove(id);
        }

        /// <summary>
        /// Remove Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void RemoveCollection<T>(IEnumerable<T> entities) where T : class
        {
            var meta = StorageMetadata.GetMetadata<T>();

            if (!_cache.ContainsKey(meta.TableName))
                _cache.Add(meta.TableName, new Dictionary<string, string>());

            var list = _cache[meta.TableName];

            foreach (var entity in entities)
            {
                var id = meta.GetId(entity);

                if (list.ContainsKey(id))
                    list.Remove(id);
            }
        }

        /// <summary>
        /// Remove All Of Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveAll<T>() where T : class
        {
            var meta = StorageMetadata.GetMetadata<T>();

            if (_cache.ContainsKey(meta.TableName))
                _cache.Remove(meta.TableName);
        }

        /// <summary>
        /// Removes Everything
        /// </summary>
        public void RemoveAll()
        {
            _cache.Clear();
            PlayerPrefs.DeleteKey(PrefKey);
        }
    }
}