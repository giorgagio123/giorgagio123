using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneStore.Models.Product;
using PhoneStore.Services.Models.Products;
using PhoneStore.Services.Pictures;
using PhoneStore.Services.Products;

namespace PhoneStore.Factories
{
    public class ProductModelFactory : IProductModelFactory
    {
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;

        public ProductModelFactory(IProductService productService, IPictureService pictureService)
        {
            _productService = productService;
            _pictureService = pictureService;
        }

        public PagedProductViewModel PrepareProductmodel(ProductFilterViewModel filterModel = null)
        {
            var products = _productService.GetAllProducts(filterModel, page: filterModel.PageNumber, pageSize: filterModel.PageSize);

            var model = new PagedProductViewModel
            {
                PagingFilterModel = new PagingFilterModel
                {
                    PageIndex = products.PageIndex,
                    PageSize = products.PageSize,
                    TotalCount = products.TotalCount,
                    TotalPages = products.TotalPages,
                    Storage = filterModel.Storage,
                    OperatingSystem = filterModel.OperatingSystem,
                    Proccessor = filterModel.Processor,
                    FromPrice = filterModel.FromPrice,
                    ToPrice = filterModel.ToPrice
                },
                Products = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    PictureUrl = _pictureService?.GetPictureUrl(p.Pictures.FirstOrDefault()),
                    Price = p.Price
                }).ToList()
            };

            return model;
        }

        public ProductDetailsViewModel PrepareProductDetailsModel(int productId)
        {
            var product = _productService.GetProductById(productId);

            if (product != null)
            {
                var model = new ProductDetailsViewModel
                {
                    Name = product.Name,
                    Storage = product.Storage,
                    OperatingSystem = product.OperatingSystem,
                    Price = product.Price,
                    Processor = product.Processor,
                    ScreenResolution = product.ScreenResolution,
                    Size = product.Size,
                    VideoLink = product.VideoLink,
                    PictureUrls = product.Pictures.Any() ? product.Pictures.Select(p => _pictureService.GetPictureUrl(p)).ToList() : new List<string>() { _pictureService.GetPictureUrl(null) }
                };

                return model;
            }

            return new ProductDetailsViewModel();
        }

        public ProductFilterViewModel PrepreProductFilterModel()
        {
            var products = _productService.GetAllProducts().ToList();

            var model = new ProductFilterViewModel
            {
                OperatingSystems = products.Select(p => p.OperatingSystem).Distinct().Select(o => new SelectListItem
                {
                    Text = o,
                    Value = o
                }).ToList(),
                Processors = products.Select(p => p.Processor).Distinct().Select(p => new SelectListItem
                {
                    Text = p,
                    Value = p
                }).ToList(),
                Storages = products.Select(p => p.Storage).Distinct().Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = s.ToString()
                }).ToList(),
            };

            var defaultValue = new SelectListItem
            {
                Text = "All",
                Value = "",
                Selected = true
            };

            model.OperatingSystems.Add(defaultValue);
            model.Processors.Add(defaultValue);
            model.Storages.Add(defaultValue);

            return model;
        }
    }
}