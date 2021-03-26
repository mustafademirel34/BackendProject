using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            //methodinterception sayesinde, reflection ile çalıştırılmak istenen metota müdahale edilir
            //metotun namespace, implement base, metot name gibi bilgilerini alır
            //reflectedtype: namespace.baseClass        --  üzerine metot adı da eklenir
            //Core.Aspects.Autofac.Caching.MethodInterception.Intercept gibi bir yapı olacak
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            // metotun prametreleri listeye dönüştürülür
            var arguments = invocation.Arguments.ToList();
            //daha önceki metotun isminin üzerine parametreleri, parantez içerisind eklenecektir
            //string ile join yapılır, bu sayede parametreler virgül ile ayrılarak parantez içerisinde yazdırılır
            //metot ismine eklenir,( parametreleri seç(null olabilir) ??: x null ise <Null> 'dur
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            //eğer cachemanager'da ekliyse
            if (_cacheManager.IsAdd(key))
            {
                //ReturnValue: metotu çalıştırmadan manuel return etmemizi sağlar.
                invocation.ReturnValue = _cacheManager.Get(key);//değeri verdik
                return;//return ediyoruz
            }

            invocation.Proceed();//sorun yoksa metota devam et
            //cachmanager'a ekle
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
