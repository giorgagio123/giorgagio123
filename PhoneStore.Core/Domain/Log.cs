using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Domain
{
    public class Log : BaseEntity
    {
        public string Request { get; set; }

        public string RequestInformation { get; set; }
        
        public string Response { get; set; }

        public string ResponseInformation { get; set; }
        
        public string IpAddress { get; set; }
    }
}
