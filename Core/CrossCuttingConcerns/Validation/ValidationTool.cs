using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        // Core katmanında, projenin kullanması için ortak araçlar içermektedir
        // Burada, doğrulayıcı bir metot var. Bir etkinleştirici ve nesne istiyor.
        // IValidator ögesi örneğin bir ProductValidator verildiğinde ve kontrol edilmesi
        // istenen bir nesne verildiğinde ilgili işlemler ile doğrular.
        // verilen validator, nesnenin kendi kurallarını içermektedir.

        //aspects/autofac/validation - metotların üzerine yazılacak attribute oluşturur
        //ve çalıştırılacak metotu, parametrelerini kontrol eder
        // doğrulama işini buraya gönderir* validate metotu nesneleri, validator ile doğrular

        public static void Validate(IValidator validator,object entity)
        {
            // kontrol edilmesi istenen nesne, tipiyle beraber tanımlanıyor.
            // bir nevi kalıba dökülüyor gibi düşünün
            var context = new ValidationContext<object>(entity);

            // daha sonra kalıbın doğrulanması isteniyor
            var result = validator.Validate(context);

            // hatalar varsa döndürür
            if (!result.IsValid)
            {
                //fluentvalidation
                throw new ValidationException(result.Errors);
            }
        }
    }
}
