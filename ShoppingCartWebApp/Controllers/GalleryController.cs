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
           

            ViewData["products"] = products;
            return View();
        }

        public IActionResult Search(string searchStr)
        {
            List<Product> products = db.SearchProducts(searchStr);
            ViewData["products"] = products;
            ViewData["searchStr"] = searchStr;
            return View();

        }
    }
}
