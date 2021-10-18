
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
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


        
        
        public ActionResult ShoppingCart(string clickedBtn)
        {
            if (Request.Cookies["SessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"];
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId);
                string username = session.User.Username;
                ViewData["username"] = username;

                if (clickedBtn == null)
                {
                    var tupList = db.getCartViewList(sessionId);
                    List<int> QuantityList = tupList.Item1;
                    List<Product> ProductList = tupList.Item2;
                    ViewData["ProductList"] = ProductList;
                    ViewData["QuantityList"] = QuantityList;
                    //Calculation of total price "tp" means total price
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
                    //add 
                    if (startStr == "a")
                    {
                        string productId = clickedBtn.Substring(2);
                        db.AddLibraryToCart(sessionId, productId);

                    }
                    else if (startStr == "r")
                    { //reduce
                        string productId = clickedBtn.Substring(2);
                        db.ReduceProductFromCart(sessionId, productId);
                    }


                    var tupList = db.getCartViewList(sessionId);
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

                if (session == null)
                {
                    return RedirectToAction("Index", "Login");
                }

            }
            return View();

        }

        public IActionResult ContinueShopping()
        {
            return RedirectToAction("Index", "Gallery");
        }

        public IActionResult Checkout()
        {
            string sessionId = Request.Cookies["sessionId"];
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId
            );

            //must be login before checkout
            string username = session.User.Username;
            if(username == "guest")
            {
                return RedirectToAction("Index", "Login");
            }
            
            db.checkOutCartView(sessionId);
            return RedirectToAction("Index", "Recommend");
        }
    }
}
