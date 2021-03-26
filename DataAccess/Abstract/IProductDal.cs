using Core.DataAccess;
using Core.Entities;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        // Veritabanı işlemleri için her tablo için bize göre her sınıf için bir
        // Data Access Layer / Veri erişim katmanı ekliyoruz. Bu katmanlar veri tasarımı için
        // gerekli imzaları oluşturuyoruz. IEntitiyRepository tüm sınıflar için genel bir base yapısı görmekte,
        // aldığı generic tipine bürünerek 
        // sınıflar için temel imzaları oluşturuyor. Inherit alan IProductDal gibi class interface'leri de
        // bunu kendi içinde benimsiyor, Buraya ise sınıflara özel metot imzalarını oluşturmak için kullanırız.
        // Yani sadece Product 'a özel bir veri tabanı işlemi verilecekse aşağıdaki gibi burada oluşturulur.

        List<ProductDetailDto> GetProductDetails();
    }
}
