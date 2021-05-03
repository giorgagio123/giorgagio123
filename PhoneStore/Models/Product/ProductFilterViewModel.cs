using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneStore.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Models.Product
{
    public class ProductFilterViewModel : ProductFilterModel
    {
        public List<SelectListItem> Processors { set; get; }

        public List<SelectListItem> Storages { set; get; }

        public List<SelectListItem> OperatingSystems { set; get; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; } = 3;
    }
}
