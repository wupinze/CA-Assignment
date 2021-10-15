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


        /*public ActionResult viewcart()
        {
            
            db.getCartViewList(User.Id);
            var tupList = db.getCartViewList(User.Id);
            List<int> QuantityList = tupList.Item1;
            List<Product> ProductList = tupList.Item2;


            ViewData["ProductList"] = ProductList;
            return View();
        }*/
        public IActionResult Index()
        {

            return View();

        }
        public ActionResult test(string clickedBtn)
        {

            if (clickedBtn == null)
            {
                string username = "jean";
                var tupList = db.getCartViewList(username);
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
                string startStr = clickedBtn.Substring(0, 1);

                string username = "jean";
                //add 
                if (startStr == "a")
                {
                    string productId = clickedBtn.Substring(2);
                    db.AddLibraryToCart(username, productId);
                    
                }
                else if (startStr == "r"){ //reduce
                    string productId = clickedBtn.Substring(2);
                    db.ReduceProductFromCart(username, productId);
                }


                var tupList = db.getCartViewList(username);
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
            string username = "jean";
            db.checkOutCartView(username);
            //db.checkOutCartView(user.Id);
            return RedirectToAction("Summary", "MyPurchases");
        }
        
        

    }
}
