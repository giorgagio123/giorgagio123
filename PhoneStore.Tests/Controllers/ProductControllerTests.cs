using Moq;
using PhoneStore.Controllers;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Models.Product;
using PhoneStore.Services.Products;
using PhoneStore.Services.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using PhoneStore.Tests.Factory;
using PhoneStore.Services.Pictures;

namespace PhoneStore.Tests.Controllers
{
    public class FakeUserManager : UserManager<ApplicationUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<ApplicationUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<ApplicationUser>>().Object,
                  new IUserValidator<ApplicationUser>[0],
                  new IPasswordValidator<ApplicationUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
        { }

    }

    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object)
        { }
    }

    public class FakeRoleManager : RoleManager<IdentityRole>
    {
        public FakeRoleManager()
            : base(new Mock<IRoleStore<IdentityRole>>().Object,
                  new Mock<IEnumerable<IRoleValidator<IdentityRole>>>().Object,
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<ILogger<RoleManager<IdentityRole>>>().Object)
        { }
    }

    public class ProductControllerTests : IClassFixture<ApplicationFactory<Startup>>
    {
        private IProductModelFactory productModelFactory { get; set; }

        private ProductController Controller { get; }

        private  IProductService productService;

        private readonly IRepository<Product> productRepository;
        //private readonly IAccountService accountService;
        


        public ProductControllerTests(ApplicationFactory<Startup> fixture)
        {

            //var users = new List<ApplicationUser>
            //{
            //    new ApplicationUser
            //    {
            //        UserName = "Test",
            //        Id = Guid.NewGuid().ToString(),
            //        Email = "test@test.it"
            //    }

            //}.AsQueryable();

            //var fakeUserManager = new Mock<FakeUserManager>();
            //fakeUserManager.Setup(x => x.Users).Returns(users);

            //fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            //fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            //fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            //accountService = new AccountService(fakeUserManager.Object, null, null, new HttpContextAccessor());

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
