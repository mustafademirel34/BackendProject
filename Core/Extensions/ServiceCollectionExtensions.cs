using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    //extensionlar static olur
    public static class ServiceCollectionExtensions
    {
        //genişletmek istediğimiz olayı this ile veririz, sonrasında gerekli parametreler verilebilir
        public static IServiceCollection AddDependencyResolvers
            (this IServiceCollection serviceCollection, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                //gönderilen servisler tek tek yüklendi
                module.Load(serviceCollection);
            }
            
            return ServiceTool.Create(serviceCollection);//burada ise build edildi
        }
    }
}
