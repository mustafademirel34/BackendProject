using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        //Sonuç sınıfı, buna göre başarı durumu ve mesaj bilgisi mevcut

        // :this ile o sınıfın kendisini kastederiz.
        // iki parametre gönderince, message işlenir, success ise diğerine gönderilir
        // o da orada işlenir. bu durum, sadece success veya iki parametre için yapıldı
        // ++

        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(bool success)
        {
            Success = success;
        }

        // Business kısmında işlemler için geri dönüş olarak kullanacağımız 
        // bir sınıf oluşturduk
        public bool Success { get; }

        public string Message { get; }
    }
}
