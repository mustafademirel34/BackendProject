using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // MVC, API vs. bir süre uğraştığım için her gördüğümü not almıyorum çünkü
    // o kadarını da anlıyorum :)
    public class ProductsController : ControllerBase
    {
        //loosely coupled
        //naming convention
        //Ioc Container - Inversion of control
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public string Get()
        {
            return "Hiç bir komut verilmezse burası çalışır. Burayı ben ekledim :D";
        }
        // ok bad, html dönüşleri
        [HttpGet("getall")] // isim vererek /getall şeklinde kontrol etmesini sağlayabilirsin.
        public IActionResult GetAll()
        {
            Thread.Sleep(3000);

            IProductService ps = new ProductManager(
                new EfProductDal(), new CategoryManager(new EfCategoryDal()));
            var result = ps.GetAll();
            if (result.Success)
                return Ok(result); // data dersen sadece onu döndürür
            //result'un kendisi bizim success,message, ve data işlemlerini de verir
            else
                return BadRequest(result);

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]//post aşamasında karşıdan veri gönderilmesi gerekir
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Product product)
        {
            Thread.Sleep(10);
            var result = _productService.Update(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("test")]
        public string TransactionTest(Product product)
        {
            _productService.AddTransactionalTest(product);
            return "Projeyi tekrar başlatarak getall kontrol et";
        }
    }
}
