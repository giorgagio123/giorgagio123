using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using PhoneStore.Core.Infrastructure.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Product}/{action=Index}/{id?}");
        }
    }
}
