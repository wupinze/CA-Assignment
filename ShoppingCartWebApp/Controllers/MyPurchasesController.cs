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

        public IActionResult Summary(string username)
        {
            User user = dbContext.Users.FirstOrDefault(x => x.Username == username);
            List<PurchaseHistory> phList = dbContext.purHistories.Where(x => x.UserId == user.Id).ToList();
            ViewData["phList"] = phList; 
            return View(); 
        }


        public IActionResult List(/*string username*/)
        {
            string username = "kate";
            User user = dbContext.Users.FirstOrDefault(x => x.Username == username);
            List<PurchaseHistory> phList = dbContext.purHistories.Where(x => x.UserId == user.Id).ToList();

            List<Product> productList = new List<Product>();

            foreach(PurchaseHistory ph in phList)
            {
                Product product = dbContext.products.FirstOrDefault(x => x.Id == ph.ProductId);
                productList.Add(product); 
            }

            ViewData["phList"] = phList;
            ViewData["productList"] = productList;
      

            return View();
        }
    }
}