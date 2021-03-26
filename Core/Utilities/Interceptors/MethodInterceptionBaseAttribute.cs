using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    // Bir özel Attribute tanımlanmış. 
    //Sınıf ve metotlara uygulanabilir, birden fazla kullanabilir ve Inherit edilen alanlarda da kullanılabilir?

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    // Soyut şekilde bir sınıf, Attribute ve Interceptor 'dan kalıt almış.
    // IInterceptor Castle 'dan bir ie
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        // Dahil edilecek attribute'ların öncelik sıralamaları için 
        public int Priority { get; set; }

        public virtual void Intercept(IInvocation invocation)
        {
            // metotları sorgulayacağımız base alan (kalıt verilen yerde override edilerek çalıştırılması sağlanır)
        }
    }
}
