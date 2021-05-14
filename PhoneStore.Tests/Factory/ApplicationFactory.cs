using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhoneStore.Tests.Factory
{
    public class ApplicationFactory<TestStartUp> : WebApplicationFactory<TestStartUp> where TestStartUp : class
    {
        private static object Lock = new object();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development").ConfigureServices(services => {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());

                services.AddScoped<ApplicationDbContext>(x => new ApplicationDbContext(options.Options));

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    // Ensure the database is deleted.
                    db.Database.EnsureDeleted();

                    // Seed the database with test data.
                    Utilities.InitializeDbForTests(db);
                }
            }).UseWebRoot("TestFiles").UseSolutionRelativeContentRoot("PhoneStore.Tests");
            
            builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true).Build());

            base.ConfigureWebHost(builder);
        }
    }
}
