using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundation.Cloud
{
    /// <summary>
    /// Helper for building query strings
    /// </summary>
    /// <remarks>
    /// http://docs.oasis-open.org/odata/odata/v4.0/os/part1-protocol/odata-v4.0-os-part1-protocol.html#_Toc372793692
    /// http://msdn.microsoft.com/en-us/library/hh169248(v=nav.71).aspx
    /// </remarks>
    public class StorageQuery<T> where T : class
    {
        // ReSharper disable InconsistentNaming
        protected int skip;
        protected int take;
        protected string orderBy;
        protected List<string> filters = new List<string>(); 

        protected void AddFilter(string format, params object[] values)
        {
            filters.Add(string.Format(format, values));
        }

        /// <param name="key">property name</param>
        /// <param name="value">String, Number</param>
        /// <returns></returns>
        public StorageQuery<T> WhereContains(string key, object value)
        {
            AddFilter("{0} co {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">String, Number</param>
        /// <returns></returns>
        public StorageQuery<T> WhereStartsWith(string key, object value)
        {
            AddFilter("{0} sw {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">String, Number</param>
        /// <returns></returns>
        public StorageQuery<T> WhereEndsWiths(string key, object value)
        {
            AddFilter("{0} ew {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">String, Number</param>
        /// <returns></returns>
        public StorageQuery<T> WhereEquals(string key, object value)
        {
            AddFilter("{0} eq {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">String, Number</param>
        /// <returns></returns>
        public StorageQuery<T> WhereNotEquals(string key, object value)
        {
            AddFilter("{0} ne {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">Number, DateTime</param>
        /// <returns></returns>
        public StorageQuery<T> WhereGreaterThan(string key, object value)
        {
            AddFilter("{0} gt {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">Number, DateTime</param>
        /// <returns></returns>
        public StorageQuery<T> WhereLessThan(string key, object value)
        {
            AddFilter("{0} lt {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">Number, DateTime</param>
        /// <returns></returns>
        public StorageQuery<T> WhereGreaterThanOrEqualTo(string key, object value)
        {
            AddFilter("{0} ge {1}", key, ReadValue(value));
            return this;
        }

        /// <param name="key">property name</param>
        /// <param name="value">Number, DateTime</param>
        /// <returns></returns>
        public StorageQuery<T> WhereLessThanOrEqualTo(string key, object value)
        {
            AddFilter("{0} le {1}", key, ReadValue(value));
            return this;
        }

        /// <summary>
        /// Orders the selection by the property
        /// </summary>
        /// <param name="key">property name</param>
        /// <returns></returns>
        public StorageQuery<T> OrderByDescending(string key)
        {
            orderBy = string.Format("orderby={0} desc", key);
            return this;
        }

        /// <summary>
        /// Orders the selection by the property
        /// </summary>
        /// <param name="key">property name</param>
        /// <returns></returns>
        public StorageQuery<T> OrderBy(string key)
        {
            orderBy = string.Format("orderby={0}", key);
            return this;
        }

        /// <summary>
        /// For paging.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public StorageQuery<T> Skip(int count)
        {
            skip = count;
            return this;
        }
        
        /// <summary>
        /// For paging.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public StorageQuery<T> Take(int count)
        {
            take = count;
            return this;
        }

        #region internal

        public int GetTake()
        {
            return skip;
        }

        public int GetSkip()
        {
            return skip;
        }

        protected string ReadValue(object value)
        {
            if (value is DateTime)
            {
                return ((DateTime) value).ToString("yyyy-MM-ddTHH:mm");
            }
            return value.ToString();
        }

        public override string ToString()
        {
            var first = true;
            var sb = new StringBuilder();

            if (filters.Any())
            {
                ClausePrefix(sb, true);
                first = false;
                sb.AppendFormat("filter=");

                for (int i = 0;i < filters.Count;i++)
                {
                    if (i > 0)
                        sb.AppendFormat(" and ");

                    sb.Append(filters[i]);
                }
            }
          

            if (!string.IsNullOrEmpty(orderBy))
            {
                ClausePrefix(sb, first);
                first = false;
                sb.Append(orderBy);
            }

            if (skip > 0)
            {
                ClausePrefix(sb, first);
                first = false;
                sb.AppendFormat("skip={0}", skip);
            }
            
            if (take > 0)
            {
                ClausePrefix(sb, first);
                sb.AppendFormat("top={0}", take);
            }

            // Unity will throw a 400 BadRequest if there are spaces in the query string.
            // Convert them to %20 (ascii equivalent) to get around this.
            return sb.ToString().Replace(" ", "%20");
        }

        void ClausePrefix(StringBuilder sb, bool first)
        {
            sb.Append(first ? "?$" : "&$");
        }

        #endregion
    }
}
