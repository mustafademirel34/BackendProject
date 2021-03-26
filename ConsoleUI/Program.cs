using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductTest();
            //CategoryTest();



        }

        private static void CategoryTest()
        {
            CategoryManager cm = new CategoryManager(new EfCategoryDal());

          
        }

        private static void ProductTest()
        {
            //ProductManager pm = new ProductManager(new EfProductDal());

            //var result = pm.GetProductDetails();

            //if (result.Success)
            //{
            //    foreach (var item in pm.GetProductDetails().Data)
            //    {
            //        Console.WriteLine(item.ProductId + " " + item.ProductName + " - " + item.CategoryName);
            //    }
            //}
            //else
            //    Console.WriteLine(result.Message);

            Console.ReadLine();
        }
    }
}
