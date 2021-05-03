using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using PhoneStore.Core.Domain;
using PhoneStore.Data;
using PhoneStore.Services.Accounts;
using PhoneStore.Services.Products;
using PhoneStore.Tests.Factory;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PhoneStore.Tests.Controllers
{
    public class CustomerControllerTests : IClassFixture<ApplicationFactory<Startup>>
    {
        private IProductService _productService;

        public CustomerControllerTests(ApplicationFactory<Startup> fixture)
        {
            var xui = fixture.CreateClient();
            var db = fixture.Server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            _productService = new ProductService(new EfRepository<Product>(db), new Mock<IAccountService>().Object);
            //_productService = new Mock<IProductService>().Object;
        }

        [Fact]
        public void AddCategory_ShouldAddNewCategory_ToDatabase()
        {

            var newProduct = new Product { Name = "TEST" };

            _productService.InsertProduct(newProduct);
            //assert
            var products = _productService.GetAllProducts();
            Assert.Single(products, newProduct);
        }
    }
}
