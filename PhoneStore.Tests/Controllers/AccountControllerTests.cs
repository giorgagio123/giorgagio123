using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PhoneStore.Controllers;
using PhoneStore.Core.Domain;
using PhoneStore.Models.Account;
using PhoneStore.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PhoneStore.Core.Infrastructure.Logging;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.Extensions.Logging;

namespace PhoneStore.Tests.Controllers
{
    public class AccountControllerTests
    {
        private AccountController Controller { get; }
        private IAccountService AccountService { get; }

        public AccountControllerTests()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();

            fakeUserManager.Setup(x => x.Users).Returns(users);
            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(users.FirstOrDefault());

            var fakeSignInManager = new Mock<FakeSignInManager>();

            fakeSignInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var fakeRoleManager = new Mock<FakeRoleManager>();

            fakeRoleManager.Setup(
                    x => x.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            AccountService = new AccountService(fakeUserManager.Object, fakeRoleManager.Object, fakeSignInManager.Object, new HttpContextAccessor());
            Controller = new AccountController(AccountService);
        }

        [Theory]
        [InlineData("aassdd", "Test_user")]
        public async Task Can_Login(string password, string userName)
        {
            //Arrange
            var testUser = new LoginModel
            {
                UserName = userName,
                Password = password
            };

            //Act
            var createdUser = await Controller.Login(testUser);

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(createdUser);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Product", redirectResult.ControllerName);
        }
    }

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
                  null,
                  null,
                  null,
                  null)
        { }
    }
}
