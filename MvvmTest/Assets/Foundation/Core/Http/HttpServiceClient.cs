// -------------------------------------
//  Domain	: Realtime.co
//  Author	: Nicholas Ventimiglia
//  Product	: Messaging and Storage
//  Copyright (c) 2014 IBT  All rights reserved.
//  -------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Foundation.Cloud;
using Foundation.Cloud.Internal;
using LITJson;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// A http client which returns HttpTasks's
    /// </summary>
    public class HttpServiceClient
    {
        /// <summary>
        /// content type Header. Default value of "application/json"
        /// </summary>
        public string ContentType = "application/json";

        /// <summary>
        /// Accept Header. Default value of "application/json"
        /// </summary>
        public string Accept = "application/json";

        /// <summary>
        /// Http Headers Collection
        /// </summary>
        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpTask GetAsync(string url)
        {
            var state = new HttpTask
            {
                Status = TaskStatus.Running,
            };

            TaskManager.StartRoutine(RunAsync(state, url));

            return state;
        }

        IEnumerator RunAsync(HttpTask task, string url)
        {
            var www = new WWW(url);

            yield return www;

            task.StatusCode = GetCode(www);

            if (!string.IsNullOrEmpty(www.error))
            {
                task.Exception = new Exception(www.error);
                task.Status = TaskStatus.Faulted;
            }
            else
            {
                task.Result = www.text;
                task.Status = TaskStatus.Success;
            }

        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpTask PostAsync(string url)
        {
            var state = new HttpTask
            {
                Status = TaskStatus.Running,
            };

            TaskManager.StartRoutine(PostAsync(state, url, null));

            return state;
        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public HttpTask PostAsync(string url, string content)
        {
            var state = new HttpTask
            {
                Status = TaskStatus.Running,
            };

            TaskManager.StartRoutine(PostAsync(state, url, content));

            return state;
        }

        IEnumerator PostAsync(HttpTask task, string url, string content)
        {
            if (!Headers.ContainsKey("ACCEPT"))
                Headers.Add("ACCEPT", Accept);
            if (!Headers.ContainsKey("CONTENT-TYPE"))
                Headers.Add("CONTENT-TYPE", ContentType);

 //           Debug.Log("POSTAsync : " + url);
//            Debug.Log(content);

            var www = new WWW(url, content == null ? new byte[1] : Encoding.UTF8.GetBytes(content), Headers);

            yield return www;

            task.StatusCode = GetCode(www);

            if (www.responseHeaders.ContainsKey("MESSAGE"))
            {
                var error = www.responseHeaders["MESSAGE"];
                task.Metadata = JsonMapper.ToObject<ResponseMetadata>(error);
            }

         //   Debug.Log(task.StatusCode);
         //   if (!string.IsNullOrEmpty(www.error))
         //       Debug.Log(www.error);
         //   else
         //       Debug.Log(www.text);


           // Debug.Log("Headers");
          //  foreach (var header in www.responseHeaders)
          //      Debug.Log(header.Key + " : " + header.Value);

            if (!string.IsNullOrEmpty(www.error))
            {
                if (task.Metadata != null)
                {
                    task.Exception = new CloudException(www.error + " " + task.Metadata.Message, task.Metadata.ModelState);
                    task.Status = TaskStatus.Faulted;
                }
                else
                {
                    task.Exception = new Exception(www.error);
                    task.Status = TaskStatus.Faulted;
                }
            }
            else
            {
                task.Result = www.text;
                task.Status = TaskStatus.Success;
            }

        }

        /// <summary>
        /// Parses the HTTPStatus Code from the header status
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        HttpStatusCode GetCode(WWW www)
        {
            if (!www.responseHeaders.ContainsKey("STATUS"))
            {
                return 0;
            }

            var code = www.responseHeaders["STATUS"].Split(' ')[1];
            return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), code);
        }

    }
}