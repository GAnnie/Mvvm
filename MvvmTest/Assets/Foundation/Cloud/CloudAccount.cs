using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Foundation.Cloud.Internal;
using Foundation.Core;
using LITJson;
using UnityEngine;

namespace Foundation.Cloud
{
    public class CloudAccountTokenResponse
    {
        public bool TokenInEmail { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }


    /// <summary>
    /// Service client for communicating with the Foundation API
    /// </summary>
    [InjectorService]
    public class CloudAccount
    {
        #region Static

        private static CloudAccount _instance;
        public static CloudAccount Instance
        {
            get { return _instance ?? (_instance = new CloudAccount()); }
        }

        public CloudAccount()
        {
            LoadSession();
        }

        #endregion

        #region dependencies

        [JsonIgnore]
        protected CloudConfig Config
        {
            get { return CloudConfig.Instance; }
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
        #endregion

        #region properties

        /// <summary>
        /// Is Signed in ?
        /// </summary>
        public bool IsAuthenticated { get; protected set; }

        /// <summary>
        /// Unique Id (GUID)
        /// </summary>
        public string UserId { get; protected set; }
        
        /// <summary>
        /// User's Email
        /// </summary>
        public string UserEmail { get; protected set; }

        /// <summary>
        /// Authentication Token. Used for Realtime Messenger
        /// </summary>
        public string AuthenticationToken { get; protected set; }

        /// <summary>
        /// Key used to acquire the session server side. Add to header.
        /// </summary>
        public string Session { get; protected set; }

        #endregion

        #region private Methods

        private const string PrefKey = "Foundation.CloudAccount";

        class AccountMetadata
        {
            public bool IsAuthenticated { get; set; }
            public string UserId { get; set; }
            public string UserEmail { get; set; }
            public string AuthenticationToken { get; set; }
            public string Session { get; set; }

        }

        void ReadDetails(AccountDetails model)
        {
            IsAuthenticated = true;
            UserId = model.Id;
            UserEmail = model.Email;
            AuthenticationToken = model.AuthenticationToken;
            Session = model.Session;

            Task.RunOnMain(() =>
            {
                var meta = new AccountMetadata
                {
                    IsAuthenticated = IsAuthenticated,
                    UserId = UserId,
                    UserEmail = UserEmail,
                    AuthenticationToken = AuthenticationToken,
                    Session = Session,
                };

                PlayerPrefs.SetString(PrefKey, JsonMapper.ToJson(meta));
                PlayerPrefs.Save();
            });

            if (Client.Headers.ContainsKey("Session"))
                Client.Headers["Session"] = Session;
            else
                Client.Headers.Add("Session", Session);

        }

        void LoadSession()
        {
            if (PlayerPrefs.HasKey(PrefKey))
            {
                var meta = JsonMapper.ToObject<AccountMetadata>(PlayerPrefs.GetString(PrefKey));
                IsAuthenticated = meta.IsAuthenticated;
                UserId = meta.UserId;
                UserEmail = meta.UserEmail;
                AuthenticationToken = meta.AuthenticationToken;
                Session = meta.Session;

                if (Client.Headers.ContainsKey("Session"))
                    Client.Headers["Session"] = Session;
                else
                    Client.Headers.Add("Session", Session);

                Debug.Log("Signed in as " + UserEmail);
            }
        }

        Task<T> Post<T>(object model) where T : class
        {
            if (!Config.IsValid)
                return new Task<T>(new Exception("Configuration is invalid"));

            var attr = UnityRouteAttribute.GetRoute(model);

            var url = Config.Path.EndsWith("/")
                ? Config.Path + attr.Path
                : Config.Path + "/" + attr.Path;


            return Task<T>.Run(() =>
            {
                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);

                return JsonMapper.ToObject<T>(task.Result);
            });
        }

        Task Post(object model)
        {
            if (!Config.IsValid)
                return new Task(new Exception("Configuration is invalid"));

            return Task.Run(() =>
            {
                var attr = UnityRouteAttribute.GetRoute(model);

                var url = Config.Path.EndsWith("/") ? Config.Path + attr.Path : Config.Path + "/" + attr.Path;

                var task = Client.PostAsync(url, JsonMapper.ToJson(model));

                task.Wait();

                HandleError(task);

                if (attr.ReturnType == typeof(AccountDetails))
                    ReadDetails(JsonMapper.ToObject<AccountDetails>(task.Result));
            });
        }

        void HandleError(HttpTask task)
        {
            if (task.IsFaulted)
            {
                // rare case of 405
                if (task.StatusCode == HttpStatusCode.MethodNotAllowed)
                    throw new Exception("405 : Method not allowed");
               
                throw task.Exception;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Requests a new account be created
        /// </summary>
        /// <param name="userId">GUID or other unique pattern</param>
        /// <param name="email">Email, unique, optional</param>
        /// <param name="password">Password, optional</param>
        /// <returns></returns>
        public Task SignUp(string userId, string email, string password)
        {
            return Post(new AccountSignUpRequest
            {
                Email = email,
                Password = password,
                UserId = userId,
            });
        }

        /// <summary>
        /// Requests a new account be created
        /// </summary>
        /// <param name="userName">UserName, possible unique</param>
        /// <param name="email">Email, unique, optional, required for update</param>
        /// <param name="password">Password, optional</param>
        /// <returns></returns>
        public Task SignUp(string email, string password)
        {
            return Post(new AccountSignUpRequest
            {
                Email = email,
                Password = password,
                UserId = Guid.NewGuid().ToString(),
            });
        }

        /// <summary>
        /// Requests a new account be created
        /// </summary>
        /// <param name="userName">UserName, possible unique</param>
        /// <returns></returns>
        public Task SignUp()
        {
            return Post(new AccountSignUpRequest
            {
                Password = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
            });
        }

        /// <summary>
        /// Requests a new account be created
        /// </summary>
        /// <param name="email">account email</param>
        /// <param name="password">account password</param>
        /// <returns></returns>
        public Task SignIn(string email, string password)
        {
            return Post(new AccountSignInRequest
            {
                Password = password,
                Email = email,
            });
        }

        /// <summary>
        /// Authorizes the user to send / receive via the Cloud Messenger
        /// </summary>
        /// <returns></returns>
        public Task Authorize(Dictionary<string, string[]> channels)
        {
            return Post(new AccountAuthorizeRequest
            {
                Channels = channels
            });
        }

        /// <summary>
        /// Signs out. Deletes session. Will require sign in
        /// </summary>
        public void SignOut()
        {
            PlayerPrefs.DeleteKey(PrefKey);
            IsAuthenticated = false;
            Session = string.Empty;
            AuthenticationToken = string.Empty;
            Client.Headers.Remove("Session");
        }

        /// <summary>
        /// Requests a Save token for Account Save or Account Recovery
        /// </summary>
        /// <param name="email">account email</param>
        /// <returns>Token, if email is NOT set</returns>
        public Task<CloudAccountTokenResponse> Token(string email)
        {
            return Post<CloudAccountTokenResponse>(new AccountTokenRequest
            {
                Email = email
            });
        }

        /// <summary>
        /// Requests a change to account details.
        /// </summary>
        /// <param name="userId">User Id </param>
        /// <param name="token">Sent via Email</param>
        /// <param name="newEmail">optional</param>
        /// <param name="newUserName">optional</param>
        /// <param name="newPassword">optional</param>
        /// <returns></returns>
        public Task Update(string userId, string token, string newEmail, string newPassword)
        {
            return Post(new AccountUpdateRequest
            {
                UserId = userId,
                Token = token,
                NewEmail = newEmail,
                NewPassword = newPassword,
            });
        }

        #endregion
    }
}