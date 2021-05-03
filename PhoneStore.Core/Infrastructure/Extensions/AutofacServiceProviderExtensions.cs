using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneStore.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PhoneStore.Core.Infrastructure.Extensions
{
    public static class AutofacServiceProviderExtensions
    {
        public static IServiceProvider _serviceProvider;

        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            List<string> addedAssemblyNames = new List<string>();
            List<Assembly> assemblies = new List<Assembly>();
            var containerBuilder = new ContainerBuilder();

            foreach (string assemblyPath in Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories))
            {
                var assembly = Assembly.LoadFile(assemblyPath);
                if (assembly != null)
                {
                    assemblies.Add(assembly);
                }

            }

            var assignTypeFrom = typeof(IDependencyRegistrar);
            var result = new List<Type>();
            foreach (var a in assemblies)
            {
                Type[] types = a.GetTypes();
                if (types != null)
                {
                    foreach (var t in types)
                    {
                        if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition))
                        {
                            if (!t.IsInterface)
                            {
                                result.Add(t);
                            }
                        }
                    }
                }
            }
            
            var instances = result
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, services, configuration);
            
            containerBuilder.Populate(services);

            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            //create service provider
            return _serviceProvider;
        }
    }
}
