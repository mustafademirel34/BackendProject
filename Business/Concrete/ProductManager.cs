using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCC;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Business.Concrete
{
    // iş kuralları validation'da yazılmaz
    // validation, nesnenin doğruluğunu kontrol eder
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        //özel attributelar ile metotlara müdahale ederiz

        //[SecuredOperation("product.add,admin")]//"","" // yetkiler
        [ValidationAspect(typeof(ProductValidator))] //parametreyi doğrulaması için validator verilir
        [CacheRemoveAspect("IProductService.Get")]//
        public IResult Add(Product product)
        {

            // çalıştırılması gereken iş kuralları ayrı metotlar halinde oluşturulduü
            //BusinessRules ile bunlar çalıştırılır, birinin başarısız olması halinde
            // iş buraya döndürülür. 


            IResult result = BusinessRules.Run(
                 CheckProductCountOfCategory(product.CategoryId),
                 CheckProductNameExists(product.ProductName),
                 CheckIfCategoryLimitExceded()
                 );

            //başarısız olan iş errorResult döndüreceğinden dolayı
            if (result != null)
            {
                return result;//hatalı olan bilgiyi gönderebiliriz
            }

            //aksi halde problem yoktur
            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);


            // business codes
            // validation

            // Burada validation gibi bir çok konu olacağından dolayı, 
            // attribute ile sağlayacağız ve işlemleri otomatikleştirecek.


            //dataresult ile döndürdüğün kadarı api tarafından okunabilir
            // vay 
        }


        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {

            if (DateTime.Now.Hour == 0)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            // durumdan önce çalışılan tip verildi
            return new DataResult<List<Product>>(
                _productDal.GetAll(), true, Messages.ProductsListed
                );
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>
                (_productDal.GetAll(p => p.CategoryId == categoryId));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>
                (_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>
                (_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>
                (_productDal.GetProductDetails());
        }

        private IResult CheckProductCountOfCategory(int categoryId)
        {
            //select count(*) from products where categoryId=1
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;

            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckProductNameExists(string productName)
        {
            //select count(*) from products where categoryId=1
            //->bool result = _productDal.GetAll().Exists(p => p.ProductName == productName);

            var result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if(result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }

        [ValidationAspect(typeof(ProductValidator))]
        //imza içerisinde interfaceler rol oynadığı için sadece product için iproductdal ekleriz.
        [CacheRemoveAspect("IProductService.Get")] // İsminde "get" barındırın tüm keyleri iptal et
        [PerformanceAspect(2)] // metot 2 saniyeden uzun sürerse uyarı verir
        public IResult Update(Product product)
        {
            
            _productDal.Update(product);
            return new SuccessResult();             
        }

        public IResult Delete(Product product)
        {
            throw new NotImplementedException();
        }

        [TransactionScopeAspect]// ile hatalı bir durumda önceki işlem geriye alınır
        public IResult AddTransactionalTest(Product product)
        {
            _productDal.Add(product);

            if (product.UnitPrice < 10)
                throw new Exception();

            product.ProductName = "Asbarapista";
            _productDal.Add(product);

            return new ErrorResult();
        }
    }
}
