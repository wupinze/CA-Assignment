using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApp.Models;

namespace ShoppingCartWebApp.Controllers
{
    public class MyPurchasesController : Controller
    {
        private DBContext dbContext;

        public MyPurchasesController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Summary()
        {
            return View(); 
        }


        public IActionResult List()
        {
            return View();
        }
    }
}