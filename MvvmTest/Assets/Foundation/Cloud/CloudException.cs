using System;
using System.Collections.Generic;

namespace Foundation.Cloud
{
    /// <summary>
    /// Extends with ModelState Errors
    /// </summary>
    public class CloudException : Exception
    {
        /// <summary>
        /// Validation Errors. apiEntity.PropertyName by errors[]
        /// </summary>
        public Dictionary<string, string[]> ModelState { get; set; }

        public CloudException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public CloudException(string message, Exception inner, Dictionary<string, string[]> modelState)
            : base(message, inner)
        {
            ModelState = modelState;
        }

        public CloudException(string message, Dictionary<string, string[]> modelState)
            : base(message)
        {
            ModelState = modelState;
        }

        public IEnumerable<string> GetErrors()
        {
            if (ModelState != null && ModelState.Count > 0)
            {
                foreach (var key in ModelState.Keys)
                {
                    foreach (var s in ModelState[key])
                    {
                        yield return s;
                    }
                }
            }
            else
            {
                yield return Message;
            }
        }
    }
}