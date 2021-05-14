using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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
            var client = fixture.CreateClient();
            var db = fixture.Server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            _productService = new ProductService(new EfRepository<Product>(db), new Mock<IAccountService>().Object);
        }

        [Fact]
        public void AddProduct_ShouldAddNewProduct_ToDatabase()
        {
            var newProduct = new Product { Id = 6, Name = "TEST" };

            _productService.InsertProduct(newProduct);
            //assert
            var products = _productService.GetAllProducts();
            Assert.Single(products, newProduct);
        }
    }
}
