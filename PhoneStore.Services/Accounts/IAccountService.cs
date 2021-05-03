using Microsoft.AspNetCore.Identity;
using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PhoneStore.Data;

namespace PhoneStore.Services.Accounts
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        Task<ApplicationUser> FindByNameAsync(string userName);

        Task<bool> RoleExistsAsync(string role);

        Task<IdentityResult> CreateRoleAsync(IdentityRole role);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnfailure);

        Task SignOutAsync();

        bool IsInRole(string role);

        string GetUserId();
    }
}
