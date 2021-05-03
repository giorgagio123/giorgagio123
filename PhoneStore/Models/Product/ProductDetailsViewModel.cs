using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Models.Product
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
            PictureUrls = new List<string>();
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public decimal ScreenResolution { get; set; }

        public string Processor { get; set; }

        public int Storage { get; set; }

        public string OperatingSystem { get; set; }

        public decimal Price { get; set; }

        public string VideoLink { get; set; }

        public IList<string> PictureUrls { get; set; }
    }
}
