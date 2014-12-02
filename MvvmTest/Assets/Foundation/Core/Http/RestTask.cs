using System;
using System.Net;
using System.Threading;

namespace Foundation.Core
{
    /// <summary>
    /// Return result of the HTTPClient
    /// </summary>
    public class RestTask : Task
    {
        // State
        public IAsyncResult AsyncResult { get; set; }
        public RegisteredWaitHandle WaitHandle { get; set; }
        public WebRequest WebRequest { get; set; }
        public HttpWebResponse WebResponse { get; set; }

        /// <summary>
        /// Computed from WebResponse
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Computed from WebResponse
        /// </summary>
        public string StatusMessage = string.Empty;

        /// <summary>
        /// Chunked responses crash unity for some reason
        /// I have sent an error report
        /// </summary>
        public bool IsChunked { get; set; }

        /// <summary>
        /// Computed from WebResponse
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Number of attempts (Retry count)
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// original content
        /// </summary>
        public string Content { get; set; }

        public RestTask()
        {
            Strategy = TaskStrategy.Custom;
        }

        public override void Dispose()
        {
            base.Dispose();

            StatusMessage = string.Empty;
            Result = string.Empty;
            Content = string.Empty;

            IsChunked = false;
            Attempts = 0;
            StatusCode = 0;

            WebResponse = null;
            WebRequest = null;
            WaitHandle = null;
            AsyncResult = null;
        }
    }
}