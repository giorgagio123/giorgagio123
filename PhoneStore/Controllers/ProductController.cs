using Microsoft.AspNetCore.Mvc;
using PhoneStore.Factories;
using PhoneStore.Models.Product;
using PhoneStore.Services.Pictures;
using PhoneStore.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductModelFactory _productModelFactory;

        public ProductController(IProductModelFactory productModelFactory)
        {
            _productModelFactory = productModelFactory;
        }

        public IActionResult Index(ProductFilterViewModel filterModel)
        {
            var model = _productModelFactory.PrepareProductmodel(filterModel);
            //var xx = RenderPartialViewToString("_ProductList", model);
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Index(ProductFilterViewModel filterModel)
        //{
        //    var products = _productModelFactory.PrepareProductmodel(filterModel);
        //    var xx = RenderPartialViewToString("_ProductList", products);
            
        //    return Json(new { success = true, view = xx });
        //}

        public IActionResult ProductDetails(int id)
        {
            var model = _productModelFactory.PrepareProductDetailsModel(id);
            
            return View(model);
        }
    }
}
