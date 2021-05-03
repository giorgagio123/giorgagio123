using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Logging;
using PhoneStore.Core.Kendoui;
using PhoneStore.Models.Customer;
using PhoneStore.Services.Accounts;
using PhoneStore.Services.Pictures;
using PhoneStore.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly IAccountService _accountService;
        private readonly ILogger _logService;

        public CustomerController(IProductService productService, IPictureService pictureService, IAccountService accountService, ILogger logService)
        {
            _productService = productService;
            _pictureService = pictureService;
            _accountService = accountService;
            _logService = logService;
        }

        public virtual IActionResult ProductList()
        {
            var model = new ProductModel();
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ProductList(DataSourceRequest command)
        {
            var products = _productService.GetAllProducts(page: command.Page - 1, pageSize: command.PageSize, forConcreteUser:true);

            var gridModel = new DataSourceResult
            {
                Data = products.Select(p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }),
                Total = products.TotalCount
            };

            return Json(gridModel);
        }

        public virtual IActionResult LogList() => View();

        [HttpPost]
        public virtual IActionResult LogList(DataSourceRequest command)
        {
            var logs = _logService.GetAllLogs(command.Page, command.PageSize);
            
            var gridModel = new DataSourceResult
            {
                Data = logs.ToList(),
                Total = logs.TotalCount
            };

            return Json(gridModel);
        }

        [HttpGet]
        public virtual IActionResult ProductCreate()
        {
            var model = new ProductModel();
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ProductCreate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name,
                    OperatingSystem = model.OperatingSystem,
                    Price = model.Price,
                    Processor = model.Processor,
                    ScreenResolution = model.ScreenResolution,
                    Size = model.Size,
                    Storage = model.Storage,
                    VideoLink = model.VideoLink,
                    UserId = _accountService.GetUserId()
                };

                _productService.InsertProduct(product);

                var pictureIds = model.AddPictureModel.PictureIds?.Split(",").Select(x => int.TryParse(x, out var value) ? value : 0).Where(x => x != 0).ToList();

                if (pictureIds.Any())
                {
                    product.Pictures = _pictureService.GetPicturesByIds(pictureIds);
                }

                _productService.UpdateProduct(product);

                return RedirectToAction("ProductList");
            }

            return View(model);
        }

        public virtual IActionResult ProductEdit(int id)
        {
            var customerProduct = _productService.GetProductByIdForCustomer(id);

            if (customerProduct.hasProduct.GetValueOrDefault())
            {
                var model = new ProductModel();
                var product = customerProduct.product;

                model.Id = product.Id;
                model.Name = product.Name;
                model.OperatingSystem = product.OperatingSystem;
                model.Price = product.Price;
                model.Processor = product.Processor;
                model.ScreenResolution = product.ScreenResolution;
                model.Size = product.Size;
                model.Storage = product.Storage;
                model.VideoLink = product.VideoLink;
                model.PictureItems = product.Pictures.Select(p => new PictureItem
                {
                    Id = p.Id,
                    Name = p.SeoFilename,
                    Url = _pictureService.GetPictureUrl(p)
                }).ToList();

                return View(model);
            }

            return Forbid();
        }

        [HttpPost]
        public virtual IActionResult ProductEdit(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _productService.GetProductById(model.Id);

                product.Id = model.Id;
                product.Name = model.Name;
                product.OperatingSystem = model.OperatingSystem;
                product.Price = model.Price;
                product.Processor = model.Processor;
                product.ScreenResolution = model.ScreenResolution;
                product.Size = model.Size;
                product.Storage = model.Storage;
                product.VideoLink = model.VideoLink;

                _productService.UpdateProduct(product);

                return RedirectToAction("ProductList");
            }

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult DeletePicture(int id)
        {
            var picture = _pictureService.GetPictureById(id);

            if (picture != null)
            {
                _pictureService.DeletePicture(picture);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public virtual IActionResult DeleteProduct(int id)
        {
            var product = _productService.GetProductById(id);

            if (product != null)
            {
                _productService.DeleteProduct(product);

                return RedirectToAction("ProductList");
            }

            return NoContent();
        }
    }
}
