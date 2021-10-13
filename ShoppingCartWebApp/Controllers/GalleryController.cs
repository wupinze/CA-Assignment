using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
using System.Diagnostics;
namespace ShoppingCartWebApp.Controllers
{
    public class GalleryController : Controller
    {
        private DBContext dbContext;
        private DB db;

        public GalleryController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext);
        }
        public IActionResult Index()
        {

            List<Product> products = db.GetProductsList();
            foreach (var product in products)
                Debug.WriteLine(product.ProductName);
            return View();
        }
    }
}
