using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Foundation.Core
{
    /// <summary>
    /// A non static HttpServiceClient
    /// </summary>
    public class RestServiceClient
    {
        /// <summary>
        /// Timeout in seconds
        /// </summary>
        public int TimeoutInSeconds =
#if UNITY_EDITOR
 25;
#else
            10;
#endif

        /// <summary>
        /// content type Header
        /// </summary>
        public string ContentType = "application/json";

        /// <summary>
        /// Accept Header
        /// </summary>
        public string Accept = "application/json";

        /// <summary>
        /// Number of retries on No Response
        /// </summary>
        public int RetryCount = 1;

        public Dictionary<string, string> Headers = new Dictionary<string, string>();
      
        static RestServiceClient()
        {
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };
        }

        public RestServiceClient()
        {

        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public RestTask GetAsync(string url)
        {
            return RequestAsync(url, "GET", null);
        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public RestTask PostAsync(string url, string content)
        {
            return RequestAsync(url, "POST", content);
        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public RestTask PutAsync(string url, string content)
        {
            return RequestAsync(url, "PUT", content);
        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public RestTask DeleteAsync(string url)
        {
            return RequestAsync(url, "DELETE", null);
        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public RestTask DeleteAsync(string url, string content)
        {
            return RequestAsync(url, "DELETE", content);
        }

        /// <summary>
        /// Begins the Http request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="verb">GET,POST,PUT,DELETE</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public RestTask RequestAsync(string url, string verb, string content)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Timeout = TimeoutInSeconds * 100;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = verb;

            request.UserAgent = "unity3d";
            request.Accept = Accept;
            
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (!String.Equals(verb, "GET") && !String.IsNullOrEmpty(content))
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(content);

                request.ContentType = ContentType;
                request.ContentLength = postBytes.Length;

                using (var newStream = request.GetRequestStream())
                {
                    newStream.Write(postBytes, 0, postBytes.Length);
                }
            }

            var state = new RestTask
            {
                WebRequest = request,
                Status = TaskStatus.Running,
                Content = content,
            };

            // Do the actual async call here
            state.AsyncResult = request.BeginGetResponse(RequestCallback, state);

            // WebRequest timeout won't work in async calls, so we need this instead
            state.WaitHandle = ThreadPool.RegisterWaitForSingleObject(
                state.AsyncResult.AsyncWaitHandle,
                ScanTimeoutCallback,
                state,
                // milliseconds
                (TimeoutInSeconds * 1000),
                true
                );

            return state;
        }

        /// <summary>
        /// Retries the Http request
        /// </summary>
        /// <returns></returns>
        public void Retry(RestTask state)
        {
            state.Status = TaskStatus.Running;

            var request = (HttpWebRequest)WebRequest.Create(state.WebRequest.RequestUri);

            request.Timeout = TimeoutInSeconds * 100;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = state.WebRequest.Method;
            request.UserAgent = "UNITY3D";
            request.Accept = Accept;

            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (!String.Equals(request.Method, "GET") && !String.IsNullOrEmpty(state.Content))
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(state.Content);

                request.ContentType = ContentType;
                request.ContentLength = postBytes.Length;

                using (var newStream = request.GetRequestStream())
                {
                    newStream.Write(postBytes, 0, postBytes.Length);
                }
            }

            // Do the actual async call here
            state.AsyncResult = request.BeginGetResponse(RequestCallback, state);

            // WebRequest timeout won't work in async calls, so we need this instead
            state.WaitHandle = ThreadPool.RegisterWaitForSingleObject(
                  state.AsyncResult.AsyncWaitHandle,
                  ScanTimeoutCallback,
                  state,
                // milliseconds
                  (TimeoutInSeconds * 1000),
                  true
                  );



        }


        void ScanTimeoutCallback(object state, bool timedOut)
        {
            try
            {
                var requestState = (RestTask)state;

                if (timedOut)
                {
                    if (requestState != null)
                    {
                        if (requestState.IsRunning)
                        {
                            requestState.Attempts++;

                            requestState.Exception = new Exception("HttpTask has timed out");
                            requestState.StatusMessage = "HttpTask has timed out";
                            requestState.WebRequest.Abort();

                            if (requestState.Attempts > RetryCount)
                            {
                                //end
                                requestState.Status = TaskStatus.Faulted;
                            }
                            else
                            {
                                Retry(requestState);
                            }
                        }
                    }
                }
                else
                {
                    var registeredWaitHandle = requestState.WaitHandle;
                    if (registeredWaitHandle != null)
                        registeredWaitHandle.Unregister(null);
                }
            }
            catch (Exception ex)
            {
                // Socket Exception, Server is offline
                var requestState = (RestTask)state;
                if (requestState != null)
                {
                    if (requestState.IsRunning)
                    {
                        requestState.Attempts = RetryCount;

                        requestState.Exception = new Exception("Could not communicate with server", ex);
                        requestState.StatusMessage = "Could not communicate with server";
                        requestState.StatusCode = 0;
                        requestState.Status = TaskStatus.Faulted;
                        requestState.WebRequest.Abort();
                    }
                }
            }
        }

        void RequestCallback(IAsyncResult asyncResult)
        {
            var state = (RestTask)asyncResult.AsyncState;

            var request = state.WebRequest;

            try
            {
                //Timeout
                if (state.IsFaulted)
                {
                    throw state.Exception;
                }

                state.WebResponse = (HttpWebResponse)request.EndGetResponse(asyncResult);

                if (state.WebResponse.ContentLength == -1)
                {
                    state.Result = ReadChunkedData(state.WebResponse);
                    state.IsChunked = true;
                }
                else
                {
                    var stream = state.WebResponse.GetResponseStream();

                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, new UTF8Encoding());
                        state.Result = reader.ReadToEnd();
                        reader.Close();
                        stream.Close();
                    }
                }

                state.StatusCode = state.WebResponse.StatusCode;
                state.StatusMessage = state.WebResponse.StatusDescription;

                state.Status = (int)state.StatusCode >= 400 && (int)state.StatusCode < 600 ? TaskStatus.Faulted : TaskStatus.Success;
            }
            catch (WebException e)
            {
                UnityEngine.Debug.LogException(e);
                state.WebResponse = (HttpWebResponse)e.Response;

                if (state.WebResponse != null)
                {
                    state.StatusCode = state.WebResponse.StatusCode;
                    var stream = state.WebResponse.GetResponseStream();
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, new UTF8Encoding());
                        var msg = reader.ReadToEnd();
                        state.Exception = new Exception(msg, e);
                        reader.Close();
                        stream.Close();
                    }
                    else
                    {
                        state.Exception = e;
                    }
                }
                else
                {
                    state.WebRequest.Abort();
                    state.Exception = e;
                }
              
                state.Status = TaskStatus.Faulted;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
                state.StatusCode = HttpStatusCode.RequestTimeout;
                state.StatusMessage = e.Message;

                state.Exception = e;
                state.Status = TaskStatus.Faulted;
            }

            if (state.WebResponse != null)
                state.WebResponse.Close();
        }

        string ReadChunkedData(HttpWebResponse response)
        {
            var sb = new StringBuilder();
            var buf = new byte[8192];
            var resStream = response.GetResponseStream();

            if (resStream != null)
            {
                int count;

                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                    }
                } while (count > 0);
            }


            return sb.ToString();
        }
    }
}
