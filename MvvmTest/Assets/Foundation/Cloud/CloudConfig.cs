using System;
using Foundation.Core;
using LITJson;
using UnityEngine;

namespace Foundation.Cloud
{
    /// <summary>
    /// Configuration for Foundation Cloud Services
    /// </summary>
    [InjectorService]
    public class CloudConfig
    {
        /// <summary>
        /// flag for failed config loading
        /// </summary>
        [JsonIgnore]
        public bool IsValid { get; protected set; }

        /// <summary>
        /// URL to the Web API
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Application Key
        /// </summary>
        public string AppId { get; protected set; }

        /// <summary>
        /// Private Key
        /// </summary>
        public string PrivateKey { get; protected set; }

        private static CloudConfig _instance;
        public static CloudConfig Instance
        {
            get { return _instance ?? (_instance = Create()); }
        }

        static CloudConfig Create()
        {
            try
            {
                if (ConfigurationManager.HasConfig("CloudConfig"))
                {
                    var config = ConfigurationManager.Get<CloudConfig>("CloudConfig");

                    if (string.IsNullOrEmpty(config.AppId))
                    {
                        Debug.LogError("ApplicationId is invalid. Please visit MMOFoundation.com and get an appId.");
                    }

                    config.IsValid = true;
                    return config;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("CloudConfig is malformed");
                Debug.LogException(ex);
                throw;
            }

            return new CloudConfig
            {
                IsValid = false,
            };
        }
    }
}