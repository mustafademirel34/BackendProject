using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    // davranış biçimini uyguladığımız örneklerden biri
    // bu sınıf metotlara gönderilen parametrelerin kurallara uygunluğunu kontrol amacıyla kullanılır
    public class ValidationAspect : MethodInterception // Aspect 
    {

        private Type _validatorType;
        // ŞİMDİ ÇÖZDÜM; ATTRIBUTE EKLEDİĞİN ZAMAN ALTTA CONSTRUCTOR YOLUYLA ÇALIŞIYOR
        // PARAMETRE İLE İSTENEN VAL.TYPE --> typeof(ProductValidator) ŞEKLİNDE VERİYORUZ
        // DİKKAT EDERSEN; ATTRIBUTE'I DA AYNI İSİMLE ÇAĞIRIYORUZ
        public ValidationAspect(Type validatorType)
        {
            //Gönderilen validator, IValidator değilse hata ver (FluentValidation)
            //+IsAssignableFrom sanırım gönderilenin IValidator'dan imp. alıp almadığına bakıyor
            // yada karşılaştırıyor // doğrulandı, atanabilir mi? is?
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }
            // geçerli bir doğrulayıcı ise geçirilir
            _validatorType = validatorType;
        }
        // methodinterception'daki onbefore ezilir
        protected override void OnBefore(IInvocation invocation)
        {
            //_validatorType sadece bir tip o yüzden reflection ile bir instance oluşturuluyor
            // çalışma anında bir örneğini oluştur (_validatorType)
            // IValidator için kullanılabilir hale getirir
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            // validator'un çalışma tipini bul, verilen generic tipi alır
            // PV gönderildiği esnada zaten implement almış durumdadır.
            // örneğin productvalidtor verildi, onu base'ine gider (BaseType);
            // ProductValidator : AbstractValidator<Product>
            // GetGenericArguments ile generic yapıdaki tipi alır.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            // çalıştırılmak istenen metot -> invocation'dur. Attr. dahil edildiği için, 
            // metot burada ekstra bir şey yapmadan görünür haldedir.
            // onun argümanlarına yani parametrelerini arayıp, tip karşılaştırması yapılarak
            // ne için doğrulama yapıyorsak, onun parametrelerine ulaşabiliriz.
            // eklenmesi için gönderilen örneğin bir product nesnesi, böylece alınır.
            // parametrelerini bul
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            // aynı tipde birden fazla nesne olabileceğinden dolayı foreach ile gezip
            // hepsini doğrulamasını istiyoruz.
            // sadece add, update için düşünmeyin, ileriyi düşünün, birden fazla güncelleme gönderilebilr.
            foreach (var entity in entities)
            {
                // validationTool kullanarak doğrulama sağlar, doğrulayıcı ve nesne verilir
                ValidationTool.Validate(validator, entity);
            }

        }


    }
}
