using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public static class ServiceTool
    { 
        public static IServiceProvider ServiceProvider { get; private set; } // servis sağlayıcı

        public static IServiceCollection Create(IServiceCollection services)//çalıştırılacak servisleri build eder, (api)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
