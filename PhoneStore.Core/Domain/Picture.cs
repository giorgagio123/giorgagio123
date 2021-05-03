using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Domain
{
    public class Picture : BaseEntity
    {
        public byte[] PictureBinary { get; set; }
        
        public string MimeType { get; set; }
        
        public string Filename { get; set; }

        public string SeoFilename { get; set; }

        public bool IsNew { get; set; }

        public int? ProductId { get; set; }

        public Product Product { get; set; }
    }
}
