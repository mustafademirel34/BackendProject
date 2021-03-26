using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        // Api çalýþtýrýlýrken ilk baþlatýlan yer burasý


        public Startup(IConfiguration configuration)//appsettings.json dosyasýný okumamýzý saðlar
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container
            //AOP - Hata,performans,transaction, cache, validation yönetimleri için bir mimari
            // 
            services.AddControllers(); // controller'lar eklenir
            // autofac ile çalýþacaðýmýz için alt iki satýr iptal edildi
            //services.AddSingleton<IProductService,ProductManager>();
            //services.AddSingleton<IProductDal, EfProductDal>();


            //(her yapýlan istekte oluþan context'tir.)
            // kullanýcýnýn istekte bulunup, yanýtýnýn verilmesine kadar süreçte çalýþýr, takip eder
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //*AddDependencyResolvers ile çaðrýldýðý için kapatýldý

            //Cors Injection
            services.AddCors();//*


            // Configuration ile appsettings.json dosyasýnda "TokenOptions" bölümü "TokenOptions" tipinde okunur
            var tokenOptions = Configuration.GetSection(key: "TokenOptions").Get<TokenOptions>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // doðrulama þemasý, Json web token Bearer
                .AddJwtBearer(options => // bir doðrulama eklenir
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tokenOptions'daki kendi verilerimiz için bir doðrulama yapýsý oluþturuluyor,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)//bizim belirlediðimiz güvenlik anahtarý
                    };
                });

            //httpcontext injection aþamasýnda devreye giremediði için burada ekledik
            //httpcontext'i çalýþmýyordu, bu yüzden servicetool ile çaðrýlýyordu ancak
            //burada baþka servislerin de eklenerek kullanýlabileceði bir yapý oluþturuldu
            //bunun için metot geniþletmeye gidildi
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Bu yöntem çalýþma zamaný tarafýndan çaðrýlýr. HTTP istek ardýþýk düzenini yapýlandýrmak için bu yöntemi kullanýn.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Angular projemizden istek gelirse izin vereðiz. Böylelikle Angular tarafýný tanýmýþ olacaðýz.
            //AllowAnyHeader() = herhangi bir istek

            //Access to XMLHttpRequest at 'https://localhost:44308/api/products/getall' from origin 'http://localhost:4021' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
            //Get Failed hatasý alýyordum. AlloAnyHeader'i, Origin'a çevirdim.
            //
            app.UseCors(builder=>builder.WithOrigins("http://localhost:4021/").AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            //Doðrulama
            app.UseAuthentication();

            //Çalýþma
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
