// -------------------------------------
//  Domain	: Realtime.co
//  Author	: Nicholas Ventimiglia
//  Product	: Messaging and Storage
//  Copyright (c) 2014 IBT  All rights reserved.
//  -------------------------------------

using System;
using System.Net;
using Foundation.Cloud.Internal;

namespace Foundation.Core
{
    /// <summary>
    /// Return result of the HttpServiceClient
    /// </summary>
    public class HttpTask : Task
    {
        /// <summary>
        /// Computed from WebResponse
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// HTTP Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Specific error details
        /// </summary>
        public ResponseMetadata Metadata { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HttpTask()
        {
            Strategy = TaskStrategy.Custom;
        }

        public HttpTask(Exception ex)
        {

            Strategy = TaskStrategy.Custom;
            Exception = ex;
            Status = TaskStatus.Faulted;
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}