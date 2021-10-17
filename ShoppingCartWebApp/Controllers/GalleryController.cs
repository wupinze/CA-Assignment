using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
using System.Diagnostics;
namespace ShoppingCartWebApp.Controllers
{
    public class GalleryController : Controller
    {
        private DBContext dbContext;
        private DB db;

        public GalleryController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext);
        }

        public IActionResult Index(string searchStr)
        {
            Session session = GetSession();

            if (session == null)
            {
                return RedirectToAction("Index", "Login");
            }

            List<ShoppingCartWebApp.Models.Product> products = dbContext.products.Where(x =>
                x.Id != null
            ).ToList();

            List<Product> srchproducts = db.SearchProducts(searchStr);
            ViewData["srchproducts"] = srchproducts;  
            ViewData["searchStr"] = searchStr; 
            int count = CartCount();
            ViewData["cartcount"] = count;
            return View();
        }

        public IActionResult AddProductToCart([FromBody] PdtToCart product)
        {
            Session session = GetSession();

            string productId = product.ProductId;

            db.AddLibraryToCart(session.Id, productId);

            return Json(new { status = "success" });
        }

        public int CartCount()
        {
            Session session = GetSession();

            int sum = db.getCarViewTotalQuantity(session.Id);

            return sum;
        }

        /*public IActionResult GoToCart()
        {
            return RedirectToAction("ShoppingCart", "Cart");
        }*/

        public Session GetSession()
        {
            if (Request.Cookies["SessionId"] == null)
            {
                return null;
            }

            string sessionId = Request.Cookies["SessionId"];
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId
            );

            return session;
        }
    }
}
