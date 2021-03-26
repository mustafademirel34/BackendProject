using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.CCC;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Burada kontrol sağlayıp istediğin karşılığı verebilirsin.

            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

            // mesela kategori vermediğim için hata aldım

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            //base.Load(builder);


            //
            //yukarıda bulunanlar için önce aspect' var mı diye kontrol eder
            //

            // hoca ekletti, çalışan uygulama içerisini al
            // çalışırken reflection eder

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // implement edilmiş interface'leri bul, yukarıda ayarladığımız durumlar için
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    // onlar için AspectIncerceptorSelector çağır
                    Selector = new AspectInterceptorSelector() //*
                }).SingleInstance();

            //sanırım değil bütük ihtimal ve hatırladığım kadarıyla üst kısım
            // bizim methodlarda araya girmemizi sağlıyor
            //ioc gerçekleşirken müdahalelerimizi uyguluyoruz.
        }

    }
}
