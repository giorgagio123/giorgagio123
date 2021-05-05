using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Kendoui
{
    public class DataSourceRequest
    {
        public DataSourceRequest()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }
    }
}
