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
        // Api �al��t�r�l�rken ilk ba�lat�lan yer buras�


        public Startup(IConfiguration configuration)//appsettings.json dosyas�n� okumam�z� sa�lar
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container
            //AOP - Hata,performans,transaction, cache, validation y�netimleri i�in bir mimari
            // 
            services.AddControllers(); // controller'lar eklenir
            // autofac ile �al��aca��m�z i�in alt iki sat�r iptal edildi
            //services.AddSingleton<IProductService,ProductManager>();
            //services.AddSingleton<IProductDal, EfProductDal>();


            //(her yap�lan istekte olu�an context'tir.)
            // kullan�c�n�n istekte bulunup, yan�t�n�n verilmesine kadar s�re�te �al���r, takip eder
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //*AddDependencyResolvers ile �a�r�ld��� i�in kapat�ld�

            //Cors Injection
            services.AddCors();//*


            // Configuration ile appsettings.json dosyas�nda "TokenOptions" b�l�m� "TokenOptions" tipinde okunur
            var tokenOptions = Configuration.GetSection(key: "TokenOptions").Get<TokenOptions>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // do�rulama �emas�, Json web token Bearer
                .AddJwtBearer(options => // bir do�rulama eklenir
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tokenOptions'daki kendi verilerimiz i�in bir do�rulama yap�s� olu�turuluyor,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)//bizim belirledi�imiz g�venlik anahtar�
                    };
                });

            //httpcontext injection a�amas�nda devreye giremedi�i i�in burada ekledik
            //httpcontext'i �al��m�yordu, bu y�zden servicetool ile �a�r�l�yordu ancak
            //burada ba�ka servislerin de eklenerek kullan�labilece�i bir yap� olu�turuldu
            //bunun i�in metot geni�letmeye gidildi
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Bu y�ntem �al��ma zaman� taraf�ndan �a�r�l�r. HTTP istek ard���k d�zenini yap�land�rmak i�in bu y�ntemi kullan�n.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Angular projemizden istek gelirse izin vere�iz. B�ylelikle Angular taraf�n� tan�m�� olaca��z.
            //AllowAnyHeader() = herhangi bir istek

            //Access to XMLHttpRequest at 'https://localhost:44308/api/products/getall' from origin 'http://localhost:4021' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
            //Get Failed hatas� al�yordum. AlloAnyHeader'i, Origin'a �evirdim.
            //
            app.UseCors(builder=>builder.WithOrigins("http://localhost:4021/").AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            //Do�rulama
            app.UseAuthentication();

            //�al��ma
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
