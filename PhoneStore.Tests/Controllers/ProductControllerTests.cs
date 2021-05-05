using Moq;
using PhoneStore.Controllers;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Factories;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Models.Product;
using PhoneStore.Services.Products;
using PhoneStore.Services.Accounts;
using PhoneStore.Tests.Factory;

namespace PhoneStore.Tests.Controllers
{
    public class ProductControllerTests 
    {
        private IProductModelFactory productModelFactory { get; set; }
        private ProductController Controller { get; }
        private  IProductService productService;
        private readonly IRepository<Product> productRepository;

        public ProductControllerTests()
        {
            var fakeProductRepository = new Mock<IRepository<Product>>();

            fakeProductRepository.Setup(m => m.Table).Returns((new Product[] {
                new Product {Id = 1, Name = "P1", Processor = "test"},
                new Product {Id = 2, Name = "P2"},
                new Product {Id = 3, Name = "P3"},
                new Product {Id = 4, Name = "P4"},
                new Product {Id = 5, Name = "P5"}
            }).AsQueryable<Product>());

            productRepository = fakeProductRepository.Object;

            productService = new ProductService(productRepository, new Mock<IAccountService>().Object);

            productModelFactory = new ProductModelFactory(productService, null);

            Controller = new ProductController(productModelFactory);
        }
        
        [Fact]
        public void Can_Paginate()
        {
            //Arrange
            var filterModel = new ProductFilterViewModel
            {
                PageSize = 3,
                PageNumber = 1
            };

            //act
            var result = Controller.Index(filterModel);
            
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PagedProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public void Can_Filter()
        {
            //Arrange
            var filterModel = new ProductFilterViewModel
            {
                Processor = "test"
            };

            //act
            var result = Controller.Index(filterModel);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PagedProductViewModel>(viewResult.ViewData.Model);
            Assert.Single(model.Products);
        }
    }
}
