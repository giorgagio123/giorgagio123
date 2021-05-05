using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Kendoui
{
    public class DataSourceResult
    {
        public object ExtraData { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// Errors
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// Total records
        /// </summary>
        public int Total { get; set; }
    }
}
