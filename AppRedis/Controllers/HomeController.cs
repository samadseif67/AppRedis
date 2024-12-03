using AppRedis.Entites;
using AppRedis.Models;
using AppRedis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AppRedis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;
        public HomeController(ILogger<HomeController> logger, DbContextClass dbContext, ICacheService cacheService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public IActionResult Index()
        {
            ViewBag.LstProduct = GetProduct();
            return View();
        }

        //************************************************************************







        //**********************************************************************



        public IEnumerable<Product> GetProduct()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = _dbContext.Products.ToList();
            _cacheService.SetData<IEnumerable<Product>>("product", cacheData, expirationTime);
            return cacheData;
        }

        public async Task<Product> AddProduct(Product value)
        {
            var obj = await _dbContext.Products.AddAsync(value);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
            return obj.Entity;
        }



        public void Updateproduct(Product product)
        {
            _dbContext.Products.Update(product);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }

        public void DeleteProduct(int Id)
        {
            var filteredData = _dbContext.Products.Where(x => x.ProductId == Id).FirstOrDefault();
            _dbContext.Remove(filteredData);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }
         

        //***********************************************************************





    }
}
