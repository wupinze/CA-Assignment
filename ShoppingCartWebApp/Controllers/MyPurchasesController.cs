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
        private DB db; 

        public MyPurchasesController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext); 
        }

        public IActionResult Summary(string sessionId)
        {
            // Test code <start> - to be removed after link up 
            string userName = "john";
            User user1 = dbContext.Users.FirstOrDefault(x => x.Username == userName);
            // Test code <end>

            List<PurchasesItem> purchases = db.getPurchaseHistory2(user1.Id);
  
            ViewData["purchases"] = purchases;


            List<PurchaseHistory> phList = dbContext.purHistories.Where(x => x.user.Id == user1.Id).ToList();

            List<Product> productList = new List<Product>();

            foreach (PurchaseHistory ph in phList)
            {
                Product product = dbContext.products.FirstOrDefault(x => x.Id == ph.product.Id);
                productList.Add(product);
            }

            ViewData["phList"] = phList;
            ViewData["productList"] = productList;



            return View(); 
        }


        public IActionResult List(/*string sessionId*/)
        {
            // Test code <start> - to be removed after link up 
            string username = "john";
            User user1 = dbContext.Users.FirstOrDefault(x => x.Username == username);
            // Test code <end> 

            List<PurchasesItem> purchases = db.getPurchaseHistory2(user1.Id);

            ViewData["purchases"] = purchases;
      
            return View();
        }
    }
}