using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Models.Customer
{
    public class ProductModel
    {
        public ProductModel()
        {
            AddPictureModel = new ProductPictureModel();
            PictureItems = new List<PictureItem>();
        }

        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Size")]
        public int Size { get; set; }

        [DisplayName("ScreenResolution")]
        public decimal ScreenResolution { get; set; }

        [DisplayName("Processor")]
        public string Processor { get; set; }

        [DisplayName("Storage")]
        public int Storage { get; set; }

        [DisplayName("OperatingSystem")]
        public string OperatingSystem { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("VideoLink")]
        public string VideoLink { get; set; }

        public ProductPictureModel AddPictureModel { get; set; }

        public IList<PictureItem> PictureItems { get; set; }
    }

    public partial class ProductPictureModel
    {
        [UIHint("DragAndDropPicture")]
        [DisplayName("Picture")]
        public string PictureIds { get; set; }
    }

    public class PictureItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
