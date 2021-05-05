using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Tests.Factory
{
    public class ApplicationFactory<TestStartUp> : WebApplicationFactory<TestStartUp> where TestStartUp : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("testing").ConfigureServices(b => {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("testing");

                b.AddSingleton<ApplicationDbContext>(x => new ApplicationDbContext(options.Options));
            });

            base.ConfigureWebHost(builder);
        }
    }
}
