using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Business.Constants;

namespace Business.BusinessAspects.Autofac
{
    //for JWT
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        //metot için roller
        //detaylandırılabilir
        public SecuredOperation(string roles)//params string[]
        {
            _roles = roles.Split(',');
         
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        protected override void OnBefore(IInvocation invocation)
        {
            //kullanıcı için roller kontrol edilir
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role)) // herhangi bir yetki için 
                {
                    return; // metotu çalıştırmaya devam edebilirsin
                }
            }
            throw new Exception(Messages.AuthorizationDenied);//yetkisiz işlemler için hata döndürülür
        }
    }
}
