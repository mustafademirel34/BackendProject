using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Utilities.Interceptors
{

    public class AspectInterceptorSelector : IInterceptorSelector
    {
        // Bir sınıfın, metotların attribute bilgisini okur ve önemine göre sıralar.
        //dep.resolver tarafında çalıştırılıyor

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            //MethodInterceptionBaseAttribute tipinde sınıfın özel attribute'larını okunur
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>
                (true).ToList();

            //aynı şekilde metot attributeları okur
            var methodAttributes = type.GetMethod(method.Name)
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true);

            //sınıf attribute'ları metotlara eklenir, (*veya tersi)
            classAttributes.AddRange(methodAttributes);

            //classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger)));

            // önceliğine göre sırala ve döndür
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }

}
