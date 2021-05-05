using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Data;
using PhoneStore.Models.Account;
using PhoneStore.Services.Accounts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Controllers
{
    public class AccountController : BaseController
    {
        private readonly string adminUserName = "admin"; 
        private readonly string adminEmail = "admin@gmail.com";
        private readonly string adminPassword = "Admin123!";
        
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Register() => View(new RegisterModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userExist = await _accountService.FindByNameAsync(model.UserName);
            if (userExist != null)
            {
                ModelState.AddModelError("", "User Already Exists");
            }

            var user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await _accountService.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            if (!await _accountService.RoleExistsAsync(UserRoles.User))
                await _accountService.CreateRoleAsync(new IdentityRole(UserRoles.User));

            await _accountService.AddToRoleAsync(user, UserRoles.User);

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin()
        {
            var userExist = await _accountService.FindByNameAsync(adminUserName);

            if (userExist == null)
            {
                var user = new ApplicationUser()
                {
                    Email = adminEmail,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = adminUserName
                };

                var result = await _accountService.CreateUserAsync(user, adminPassword);

                if (!await _accountService.RoleExistsAsync(UserRoles.Admin))
                    await _accountService.CreateRoleAsync(new IdentityRole(UserRoles.Admin));

                await _accountService.AddToRoleAsync(user, UserRoles.Admin);
            }

            AddNotification(NotifyType.Success, $"user name: {adminUserName}; Password: {adminPassword}", true);

            return RedirectToAction("Login");
        }

        public IActionResult Login(string returnUrl = "") => View(new LoginModel() { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.FindByNameAsync(model.UserName);
                
                if (user != null)
                {
                    var result = await _accountService.PasswordSignInAsync(model.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Product");
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            AddNotification(NotifyType.Error, "Denied", true);

            return View();
        }
    }
}
