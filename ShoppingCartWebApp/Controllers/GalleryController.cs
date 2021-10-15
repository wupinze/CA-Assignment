using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


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

        /*public IActionResult Index()
        {

            *//*List<Product> products = db.GetProductsList(); *//*


            //ViewData["products"] = products;
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
        }*/

        /*public IActionResult Search(string searchStr)
        {
            List<Product> products = db.SearchProducts(searchStr);
            ViewData["products"] = products;
            ViewData["searchStr"] = searchStr;
            return View();
        }*/

        //[HttpPost]
        /*public IActionResult Index(string searchStr)
        {
            List<Product> products = db.SearchProducts(searchStr);
            ViewData["products"] = products;
            ViewData["srchproducts"] = products;
            ViewData["searchStr"] = searchStr;

            return View();
        }*/

        public IActionResult Index()
        {
            Session session = GetSession();
            if (session == null)
            {
                return RedirectToAction("Index", "Logout");
            }

            List<ShoppingCartWebApp.Models.Product> products = dbContext.products.Where(x =>
                x.Id != null
            ).ToList();
            /*List<ShoppingCartWebApp.Models.Product> NoSrchproducts = dbContext.products.Where(x =>
                x.Id == null
            ).ToList();
            ViewData["srchproducts"] = NoSrchproducts;*/
            ViewData["products"] = products;

            /*if (GetCart() == null)
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
            
            ViewData["cart"] = cart;*/
            int count = CartCount();
            ViewData["cartcount"] = count;
            return View();
        }

        public IActionResult AddProductToCart([FromBody] PdtToCart product)
        {
            Session session = GetSession();

            //if no session
            /*if (session == null)
            {
                session = CreateTempSession();
                dbContext.Sessions.Add(session);
                dbContext.SaveChanges();

                // ask browser to save and send back these cookies next time
                Response.Cookies.Append("SessionId", session.Id.ToString());
            }*/

            ViewData["session"] = session;

            Guid productId = Guid.Parse(product.ProductId);

            string productName = product.ProductName;

            Product productData = dbContext.products.FirstOrDefault(x =>
                x.Id == productId);

            if (productData == null)
            {
                return Json(new { status = "fail" });
            }

            Cart cart = new Cart
            {
            };

            productData.carts.Add(cart);
            session.User.carts.Add(cart);
            
            /*User user = dbContext.Users.FirstOrDefault(x =>
                x.Id == session.UserId);*/
            //user.carts.Add(cart);

            dbContext.SaveChanges();

            return Json(new { status = "success" });
        }

        public int CartCount()
        {
            Session session = GetSession();
            Guid userid = session.UserId;
            List<Cart> carts = dbContext.carts.Where(x =>
                x.user.Id == userid).ToList();

            int count = carts.Count();
            return count;
        }

        public IActionResult GoToCart()
        {
            return RedirectToAction("Index", "Cart");
        }

        public Session GetSession()
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

        //TEMP SESSION
        private Session CreateTempSession()
        {
            User tempuser = CreateTempUser();

            Session tempsession = new Session
            {
                User = tempuser
            };

            return tempsession;
        }

        private User CreateTempUser()
        {
            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes("temp"));
            User tempuser = new User
            {
                Id = new Guid(),
                Username = "temp",
                PassHash = hash
            };

            dbContext.Add(new User
            {
                Username = tempuser.Username,
                PassHash = tempuser.PassHash
            });

            dbContext.SaveChanges();

            return tempuser;
        }
    }
}