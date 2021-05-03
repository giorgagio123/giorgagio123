using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Infrastructure.Routing
{
    public interface IRouteProvider
    {
        void RegisterRoutes(IRouteBuilder routeBuilder);
    }
}
