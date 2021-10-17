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
            Session session = dbContext.Sessions.FirstOrDefault(
                x => x.Id == sessionId
                );
            string username = session.User.Username;

            List<PurchasesItem> purchases = db.getPurchaseHistory(sessionId);

            ViewData["purchases"] = purchases;

            List<PurchaseHistory> phList = dbContext.purHistories.Where(x => x.user.Id == session.User.Id).ToList();

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


        public IActionResult List(string date, string username)
        {
            User user1 = dbContext.Users.FirstOrDefault(x => x.Username == username);

            Session session = dbContext.Sessions.FirstOrDefault(x => x.User == user1);
            List<PurchasesItem> purchases = db.getPurchaseHistory(session.Id);

            ViewData["date"] = date;
            ViewData["username"] = username;
            ViewData["purchases"] = purchases;
            return View();
        }

        public IActionResult BuyAgain(string productId)
        {
            string sessionId = Request.Cookies["SessionId"];

            db.AddLibraryToCart(sessionId, productId);

            return RedirectToAction("ShoppingCart", "Cart"); 
        }
    }
}