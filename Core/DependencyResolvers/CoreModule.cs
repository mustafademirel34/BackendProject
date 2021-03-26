using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            //yaptığımız zaman, IMemoryCache karışılığı oluşuyor
            serviceCollection.AddMemoryCache();//dot.net
            //ServiceTool.ServiceProvider.GetService oluşan değeri alabiliyoruz

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();

            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
