using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneStore.Core;
using PhoneStore.Core.Infrastructure.DependencyManagement;
using PhoneStore.Core.Infrastructure.Logging;
using PhoneStore.Services.Accounts;
using PhoneStore.Services.Infrastructure.Logging;
using PhoneStore.Services.Pictures;
using PhoneStore.Services.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Services.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["ENVIRONMENT"].Contains("Production"))
                builder.RegisterType<AzurePictureService>().As<IPictureService>().InstancePerLifetimeScope();
            else
                builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();

            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<WebHelper>().AsSelf().SingleInstance();
        }
    }
}
