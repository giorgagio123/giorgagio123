using Microsoft.AspNetCore.Mvc;
using PhoneStore.Factories;
using PhoneStore.Models.Product;
using PhoneStore.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Components
{
    [ViewComponent(Name = "Filters")]
    public class FiltersComponent : ViewComponent
    {
        private readonly IProductModelFactory _productModelFactory;

        public FiltersComponent(IProductModelFactory productModelFactory)
        {
            _productModelFactory = productModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _productModelFactory.PrepreProductFilterModel();

            return View(model);
        }
    }
}
