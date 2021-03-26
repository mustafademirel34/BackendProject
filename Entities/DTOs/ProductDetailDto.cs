using Core;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class ProductDetailDto:IDto
    {
        // data transmission object / veri aktarım nesnesi
        // birleştireceğimiz tabloların şemasını oluşturuyoruz burada
        // Birleştireceğimiz tabloların hangi bilgilerini alacağımızı oluşturuyoruz.
        // Bir IDto'dur, entity değil, çalışma anında oluşturulur.
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public short UnitsInStock { get; set; }
    }
}
    