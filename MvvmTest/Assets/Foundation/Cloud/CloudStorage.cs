using System;
using System.Net;
using Foundation.Cloud.Internal;
using Foundation.Core;
using LITJson;
using UnityEngine;

namespace Foundation.Cloud
{
    /// <summary>
    /// Data Access Layer for the Cloud System
    /// </summary>
    [InjectorService]
    public class CloudStorage
    {
        #region Static

        [JsonIgnore]
        private static CloudStorage _instance;
        [JsonIgnore]
        public static CloudStorage Instance
        {
            get { return _instance ?? (_instance = new CloudStorage()); }
        }

        #endregion

        #region Shared

        protected CloudConfig Config
        {
            get { return CloudConfig.Instance; }
        }

        protected CloudAccount Account
        {
            get { return CloudAccount.Instance; }
        }


        private HttpServiceClient _client;
        protected HttpServiceClient Client
        {
            get
            {

                if (_client == null)
                {
                    _client = new HttpServiceClient();
                    _client.Headers["ApplicationId"] = Config.AppId;
                }
                return _client;
            }
        }

        void HandleError(HttpTask task)
        {
            if (task.IsFaulted)
            {
                // rare case of 405. IIS returns a html response.
                if (task.StatusCode == HttpStatusCode.MethodNotAllowed)
                    throw new Exception("405 : Method not allowed");

                throw task.Exception;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<T[]> Get<T>(StorageQuery<T> query) where T : class
        {
            if (!Config.IsValid)
                return new Task<T[]>(new Exception("Configuration is invalid"));
            
            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            return Task.Run(() =>
            {
                var action = string.Format("api/2/Storage/Query/{0}{1}", meta.TableName, query);

                var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;
                
                var task = Client.PostAsync(url);

                task.Wait();

                HandleError(task);

                return JsonMapper.ToObject<T[]>(task.Result);
            });
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> Get<T>(string id) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));
            
            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            StorageMetadata.RegisterType<T>();

            return Task.Run(() =>
            {
                var action = string.Format("api/2/Storage/Get/{0}", id);

                var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

                var task = Client.PostAsync(url);

                task.Wait();

                HandleError(task);

                return string.IsNullOrEmpty(task.Result) ? null : JsonMapper.ToObject<T>(task.Result);
            });
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task Create<T>(T entity) where T : class
        {
            return Create(entity, StorageACL.Public, null);
        }

        /// <summary>
        /// Creates a write protected object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="acl">protection group</param>
        /// <param name="param">User name</param>
        /// <returns></returns>
        public Task Create<T>(T entity, StorageACL acl, string param) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            if (!Account.IsAuthenticated)
                return new Task<T>(new Exception("Not Authenticated"));


            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            var model = new StorageRequest
            {
                AppId = Config.AppId,
                ObjectId = meta.GetId(entity),
                ObjectScore = meta.GetScore(entity),
                ObjectType = meta.TableName,
                ObjectData = JsonMapper.ToJson(entity),
                AclParam = param,
                AclType = (StorageACLType)acl,
            };

            var action = string.Format("api/2/Storage/Create");

            var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

            return Task.Run(() =>
            {
                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);
            });
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task Update<T>(T entity) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            if (!Account.IsAuthenticated)
                return new Task<T>(new Exception("Not Authenticated"));


            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            var model = new StorageRequest
            {
                AppId = Config.AppId,
                ObjectId = meta.GetId(entity),
                ObjectScore = meta.GetScore(entity),
                ObjectType = meta.TableName,
                ObjectData = JsonMapper.ToJson(entity),
            };

            var action = string.Format("api/2/Storage/Update");

            var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

            return Task.Run(() =>
            {
                Debug.Log(JsonMapper.ToJson(model));
                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);
            });
        }

        /// <summary>
        /// Post Single Property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public Task UpdateProperty<T>(T entity, string propertyName, string propertyValue) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            if (!Account.IsAuthenticated)
                return new Task<T>(new Exception("Not Authenticated"));

            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            var model = new StorageProperty
            {
                AppId = Config.AppId,
                ObjectId = meta.GetId(entity),
                PropertyName = propertyName,
                PropertyValue = propertyValue,
            };

            var action = string.Format("api/2/Storage/UpdateProperty");

            var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

            return Task.Run(() =>
            {
                Debug.Log(JsonMapper.ToJson(model));
                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);
            });
        }

        /// <summary>
        /// Post Single Property increment / decrement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="delta">change</param>
        /// <returns></returns>
        public Task UpdateDelta<T>(T entity, string propertyName, float delta = 1) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            if (!Account.IsAuthenticated)
                return new Task<T>(new Exception("Not Authenticated"));


            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            var model = new StorageDelta
            {
                AppId = Config.AppId,
                ObjectId = meta.GetId(entity),
                PropertyName = propertyName,
                Delta = delta,
                IsFloat = true,
            };

            var action = string.Format("api/2/Storage/UpdateDelta");

            var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

            return Task.Run(() =>
            {
                Debug.Log(JsonMapper.ToJson(model));
                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);
            });
        }

        /// <summary>
        /// Post Single Property increment / decrement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="delta">change</param>
        /// <returns></returns>
        public Task UpdateDelta<T>(T entity, string propertyName, int delta = 1) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            if (!Account.IsAuthenticated)
                return new Task<T>(new Exception("Not Authenticated"));


            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            var model = new StorageDelta
            {
                AppId = Config.AppId,
                ObjectId = meta.GetId(entity),
                PropertyName = propertyName,
                Delta = delta,
                IsFloat = false,
            };

            var action = string.Format("api/2/Storage/UpdateDelta");

            var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

            return Task.Run(() =>
            {
                Debug.Log(JsonMapper.ToJson(model));
                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);
            });
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task Delete<T>(T entity) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            if (!Account.IsAuthenticated)
                return new Task<T>(new Exception("Not Authenticated"));


            Client.Headers["ApplicationId"] = Config.AppId;
            Client.Headers["Session"] = Account.Session;

            var meta = StorageMetadata.GetMetadata<T>();

            var action = string.Format("api/2/Storage/Delete/{0}", meta.GetId(entity));

            var url = Config.Path.EndsWith("/") ? Config.Path + action : Config.Path + "/" + action;

            return Task.Run(() =>
            {
                var task = Client.PostAsync(url);

                task.Wait();

                HandleError(task);
            });
        }

        #endregion
    }
}