using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
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
        public IActionResult Index()
        {
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );

                List<Cart> cart = dbContext.carts.Where(x =>
               x.user.Id == session.User.Id
                    ).ToList();

               
                
                var groupedItems = from item in cart
                                   group item by item.product.ProductName;

                foreach(var grp in groupedItems)
                {
                    Debug.WriteLine($"{grp.Key} = {grp.Count()}");
                  
                    foreach (var item in grp)
                        Debug.WriteLine($"{item.product.ProductName}");
                }

                ViewData["cart"] = groupedItems;
                
            }
            return View();
        }
    }
}
