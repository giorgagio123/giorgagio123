using Moq;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using PhoneStore.Services.Accounts;
using PhoneStore.Services.Models.Products;
using PhoneStore.Services.Products;
using PhoneStore.Tests.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PhoneStore.Tests.Services
{
    public class ProductServiceTests : IClassFixture<ApplicationFactory<Startup>>
    {
        private const string userId = "115";

        private IProductService _productService;
        private Mock<IAccountService> _fakeAccountService;
        private readonly IRepository<Product> _productRepository;

        public ProductServiceTests(ApplicationFactory<Startup> fixture)
        {
            var client = fixture.CreateClient();
            var db = fixture.Server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            _productRepository = new EfRepository<Product>(db);

            _fakeAccountService = new Mock<IAccountService>();
            _fakeAccountService.Setup(a => a.GetUserId()).Returns(userId);

            _productService = new ProductService(_productRepository, _fakeAccountService.Object);
        }

        [Fact]
        public void GetAllProducts_Filter()
        {
            //arrange
            var nameFilterModel = new ProductFilterModel { Name = "P2" };
            var processorFilterModel = new ProductFilterModel { Processor = "i3" };
            var operatingSystemFilterModel = new ProductFilterModel { OperatingSystem = "Linux" };

            //act
            var filteredByName = _productService.GetAllProducts(nameFilterModel);
            var filteredByProcessor = _productService.GetAllProducts(processorFilterModel);
            var filteredByOpertingSystem = _productService.GetAllProducts(operatingSystemFilterModel);

            //assert
            Assert.Equal(2, filteredByName.Count);
            Assert.Equal(3, filteredByProcessor.Count);
            Assert.Single(filteredByOpertingSystem);
        }

        // ???
        [Fact]
        public void Update_product()
        {
            //arrange
            var allProducts = _productService.GetAllProducts();
            var product = allProducts.FirstOrDefault(p => p.Name.Contains("P1"));
            
            //act 
            product.Name = "Update";
            _productService.UpdateProduct(product);

            //assert
            Assert.Single(allProducts.Where(x => x.Name.Contains("Update")));
        }

        [Fact]
        public void Get_Product_If_User_Has()
        {
            //arrange
            _fakeAccountService.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);

            var productId = _productService.GetAllProducts().FirstOrDefault(p => p.UserId.Contains(userId))?.Id;

            //act 
            var product = _productService.GetProductByIdForCustomer(productId.GetValueOrDefault()).product;

            //assert
            Assert.NotNull(product);
        }

        [Fact]
        public void Get_Any_Product_For_Admin()
        {
            //arrange
            _fakeAccountService.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
            var rnd = new Random();

            //act
            var randromProduct = _productService.GetProductByIdForCustomer(rnd.Next(0,3)).product;

            //assert
            Assert.NotNull(randromProduct);
        }
    }
}
