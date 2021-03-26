using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    //Interceptor - araya girmek demektir. Çalıştırılacak metota müdahale edilir kısaca.
    // Attribute sınıfından kalıtım alan MethodInterception'un kendisi de Attribute'dir.
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        // Bir davranış şeması, soyuttur. Kalıt verilecek alan için yönlendirmeler içerir.
        // ! invocation çalıştırılmak istenen metottur.

        // Metotun öncesinde, sonrasında veya hata aldığında ne gibi durumların uygulanacağını
        // bildireceğimiz soyutlardır.
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }

        // Çalıştırılmak istenen metotu kontrolden geçiren alan, parametre şeklinde verilerek kontroller sağlanır.
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed(); // çalıştırılacak olan metot (add, getall vs)
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);//hata oluştuğunda
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    OnSuccess(invocation);//başarılı olduğunda
                }
            }
            OnAfter(invocation);
        }
    }
}
