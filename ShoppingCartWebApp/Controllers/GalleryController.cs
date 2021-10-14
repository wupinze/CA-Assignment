using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace ShoppingCartWebApp.Controllers
{
    public class GalleryController : Controller
    {
        private DBContext dbContext;
        public GalleryController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult AllProducts()
        {
            Session session = GetSession();
            if (session == null)
            {
                return RedirectToAction("Index", "Logout");
            }

            List<ShoppingCartWebApp.Models.Product> products = dbContext.products.Where(x =>
                x.Id != null
            ).ToList();

            ViewData["products"] = products;

            if (GetCart() == null)
            {
                Cart mycart = new Cart()
                {
                    user = session.User
                };
                dbContext.carts.Add(mycart);
                dbContext.SaveChanges();

                Response.Cookies.Append("CartId", mycart.Id.ToString());
            };
            Cart cart = GetCart();

            ViewData["cart"] = cart;

            return View();
        }

        public IActionResult AddProductToCart([FromBody] PdtToCart ptc)
        {
            Session session = GetSession();
            if (session == null)
            {
                return Json(new { status = "fail" });
            }

            Guid productId = Guid.Parse(ptc.ProductId);

            string productName = ptc.ProductName;

            Product productData = dbContext.products.FirstOrDefault(x => 
                x.Id == productId);

            if (productData == null)
            {
                return Json(new { status = "fail" });
            }

            Cart cart = GetCart();

            productData.carts.Add(cart);

            dbContext.SaveChanges();

            return Json(new { status = "success" });
        }

        private Session GetSession()
        {
            if (Request.Cookies["SessionId"] == null)
            {
                return null;
            }

            Guid sessionId = Guid.Parse(Request.Cookies["SessionId"]);
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId
            );

            return session;
        }

        private Cart GetCart()
        {
            if (Request.Cookies["CartId"] == null)
            {
                return null;
            }

            Guid cartId = Guid.Parse(Request.Cookies["CartId"]);
            Cart cart = dbContext.carts.FirstOrDefault(x =>
                x.Id == cartId
            );

            return cart;
        }
    }
}
