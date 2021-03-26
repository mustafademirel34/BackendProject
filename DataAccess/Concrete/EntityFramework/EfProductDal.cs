using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
        //nesne veri erişimleri dataAcess katmanında oluşturulur, implement alınan genel veri erişim sınıfları Core katmanındadır.
        
        public List<ProductDetailDto> GetProductDetails()//product için özel metot
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                //veritabanı için product ve category join işlemi
                var result = from p in context.Products
                             join c in context.Categories
                             on p.CategoryId equals c.CategoryId
                             select new ProductDetailDto
                             {
                                 ProductId = p.ProductId, 
                                 ProductName = p.ProductName, 
                                 CategoryName = c.CategoryName, 
                                 UnitsInStock=p.UnitsInStock
                             };
                return result.ToList();
            }
           
        }
    }
}
