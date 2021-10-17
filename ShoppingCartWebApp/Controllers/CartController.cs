using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;

namespace ShoppingCartWebApp.Controllers
{
    public class CartController : Controller
    {


        private DBContext dbContext;
        private DB db;
        public CartController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext);
        }

        public ActionResult Index()
        {
            return View();

        }
        public ActionResult ShoppingCart(string clickedBtn)
        {
            string sessionId = Request.Cookies["SessionId"];
            //string username = "jean";
            
            Session session = dbContext.Sessions.FirstOrDefault(
            x => x.Id == sessionId
            );
            if (clickedBtn == null)
            {
                
                //string username = session.User.Username;
                string userId = session.User.Id;
                var tupList = db.getCartViewList(userId);
                List<int> QuantityList = tupList.Item1;
                List<Product> ProductList = tupList.Item2;
                ViewData["ProductList"] = ProductList;
                ViewData["QuantityList"] = QuantityList;
                double tp = 0;
                double pp;

                List<float> pricelist = new List<float>();
                foreach (var product in ProductList)
                {
                    pricelist.Add(product.Price);
                }

                var res = pricelist.Zip(QuantityList, (n, w) => new { Price = n, Quantity = w });
                foreach (var a in res)
                {
                    pp = a.Price * a.Quantity;

                    tp += pp;
                }
                ViewData["tp"] = Convert.ToString(tp);
            }
            else
            {
                
                //string username = session.User.Username;
                string userId = session.User.Id;
                string startStr = clickedBtn.Substring(0, 1);

                //string username = "jean";
                //add 
                if (startStr == "a")
                {
                    string productId = clickedBtn.Substring(2);
                    db.AddLibraryToCart(userId, productId);

                }
                else if (startStr == "r")
                { //reduce
                    string productId = clickedBtn.Substring(2);
                    db.ReduceProductFromCart(userId, productId);
                }


                var tupList = db.getCartViewList(userId);
                List<int> QuantityList = tupList.Item1;
                List<Product> ProductList = tupList.Item2;
                ViewData["ProductList"] = ProductList;
                ViewData["QuantityList"] = QuantityList;
                double tp = 0;
                double pp;

                List<float> pricelist = new List<float>();
                foreach (var product in ProductList)
                {
                    pricelist.Add(product.Price);
                }

                var res = pricelist.Zip(QuantityList, (n, w) => new { Price = n, Quantity = w });
                foreach (var a in res)
                {
                    pp = a.Price * a.Quantity;

                    tp += pp;
                }
                ViewData["tp"] = Convert.ToString(tp);
            }
            return View();
        }

        public IActionResult ContinueShopping()
        {
            return RedirectToAction("Index", "Gallery");
        }

        public IActionResult Checkout()
        {
            string sessionId = Request.Cookies["SessionId"];
            Session session = dbContext.Sessions.FirstOrDefault(
            x => x.Id == sessionId
            );
            string userid = session.User.Id;
            //string username = "jean";
            db.checkOutCartView(userid);
            //db.checkOutCartView(user.Id);
            return RedirectToAction("Summary", "MyPurchases");
        }
        
        

    }
}
