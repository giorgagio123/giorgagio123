using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Models.Product
{
    public class PagedProductViewModel
    {
        public PagedProductViewModel()
        {
            Products = new List<ProductViewModel>();
            PagingFilterModel = new PagingFilterModel();
        }

        public PagingFilterModel PagingFilterModel { get; set; }

        public IList<ProductViewModel> Products { get; set; } 
    }

    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }
    }

    public class PagingFilterModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

        public string Proccessor { get; set; }

        public string OperatingSystem { get; set; }

        public int? Storage { get; set; }

        public decimal? FromPrice { get; set; }

        public decimal? ToPrice { get; set; }
    }
}
