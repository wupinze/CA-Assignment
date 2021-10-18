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

        public IActionResult Summary()
        {
            string sessionId = Request.Cookies["SessionId"]; 
            Session session = dbContext.Sessions.FirstOrDefault(
                x => x.Id == sessionId
                );
            //test
            User user = session.User;
            string username = session.User.Username;

            User user1 = dbContext.Users.FirstOrDefault(x => x.Username == username);

            List<PurchasesItem> purchases = db.getPurchaseHistory(session.Id);
  
            ViewData["purchases"] = purchases;

            List<PurchaseHistory> phList = dbContext.purHistories.Where(x => x.user.Id == user1.Id).ToList();

            List<Product> productList = new List<Product>();

            foreach (PurchaseHistory ph in phList)
            {
                Product product = dbContext.products.FirstOrDefault(x => x.Id == ph.product.Id);
                productList.Add(product);
            }

            ViewData["username"] = username;
            ViewData["phList"] = phList;
            ViewData["productList"] = productList;
            return View(); 
        }


        public IActionResult List(string date, string username, string productId)
        {
            if (productId != null)
            {
                string sessionId = Request.Cookies["SessionId"];
                int shopcartNumber = db.AddLibraryToCart(sessionId, productId);
                return RedirectToAction("ShoppingCart", "Cart");
            }

            User user1 = dbContext.Users.FirstOrDefault(x => x.Username == username);

            Session session = dbContext.Sessions.FirstOrDefault(x => x.User == user1);
            List<PurchasesItem> purchases = db.getPurchaseHistory(session.Id);

            int count = db.getCarViewTotalQuantity(session.Id);


            ViewData["cartcount"] = count;
            ViewData["date"] = date;
            ViewData["username"] = username;
            ViewData["purchases"] = purchases;
            return View();
        }
    }
}