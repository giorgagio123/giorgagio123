using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneStore.Core.Infrastructure.DependencyManagement;
using PhoneStore.Core.Infrastructure.Routing;
using PhoneStore.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, IServiceCollection services, IConfiguration configuration)
        {
            builder.RegisterType<RouteProvider>().As<IRouteProvider>().SingleInstance();
            builder.RegisterType<ProductModelFactory>().As<IProductModelFactory>().SingleInstance();
        }
    }
}
