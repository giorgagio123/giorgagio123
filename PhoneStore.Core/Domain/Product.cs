using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Domain
{
    public class Product : BaseEntity
    {
        private ICollection<Picture> _picture;

        public string Name { get; set; }

        public int Size { get; set; }

        public decimal ScreenResolution { get; set; }

        public string Processor { get; set; }

        public int Storage { get; set; }

        public string OperatingSystem { get; set; }

        public decimal Price { get; set; }

        public string VideoLink { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        //picture
        public virtual ICollection<Picture> Pictures
        {
            get { return _picture ?? (_picture = new List<Picture>()); }
            set { _picture = value; }
        }
    }
}
