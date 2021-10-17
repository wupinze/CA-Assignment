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
    public class RecommendController : Controller
    {
        private DBContext dbContext;
        private DB db;

        public RecommendController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext);
        }
        public IActionResult Index()
        {
            Session session = GetSession();
           
            //test username
            string username = session.User.Username;
            User user = dbContext.Users.FirstOrDefault(x => x.Username == username);

            List<PurchaseHistory> phList = dbContext.purHistories.Where(x => x.user.Id== user.Id).ToList();

            List<Product> boughtList = new List<Product>();
            List<Product> products = db.GetProductsList();

           
            foreach (PurchaseHistory ph in phList)
            {
                Product product = dbContext.products.FirstOrDefault(x => x.Id == ph.product.Id);
                boughtList.Add(product);

            }
    
            var recList = products.Where(x => !boughtList.Any(y => x.Id == y.Id)).ToList();

            ViewData["boughtList"] = boughtList;
            ViewData["products"] = products;
            ViewData["recList"] = recList;

            int count = CartCount();
            ViewData["cartcount"] = count;
            return View();
        }


        public IActionResult AddProductToCart([FromBody] PdtToCart product)
        {
            Session session = GetSession();


            ViewData["session"] = session;

            string productId = product.ProductId;

            string productName = product.ProductName;

            Product productData = dbContext.products.FirstOrDefault(x =>
                x.Id == productId.ToString());

            if (productData == null)
            {
                return Json(new { status = "fail" });
            }

            Cart cart = new Cart
            {
            };

            productData.carts.Add(cart);
            session.User.carts.Add(cart);



            dbContext.SaveChanges();

            return Json(new { status = "success" });
        }

        public int CartCount()
        {

            Session session = GetSession();
            if (session == null)
            {
                Debug.WriteLine("Cart Count, Session null, return 0");
                return 0;
            }
            Debug.WriteLine("Cart Count");
            Debug.WriteLine($"Recommend/CartCount, user: {session.User.Username}, session: {session.Id}");
            string userid = session.UserId;
            List<Cart> carts = dbContext.carts.Where(x =>
                x.user.Id == userid.ToString()).ToList();

            int count = carts.Count();
            return count;
        }

        public IActionResult GoToCart()
        {
            return RedirectToAction("Index", "Cart");
        }

        public Session GetSession()
        {
            // return session from database which corresponds to cookie in http request from client
            Debug.WriteLine("GetSession, session Id" + Request.Cookies["SessionId"]);
            if (Request.Cookies["SessionId"] == null)
            {
                return null;
            }

            string sessionId = Request.Cookies["SessionId"];
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId.ToString()
            );

            return session;
        }

        /*private Cart GetCart()
        {
            if (Request.Cookies["CartId"] == null)
            {
                return null;
            }

            string cartId = Guid.Parse(Request.Cookies["CartId"]);
            Cart cart = dbContext.carts.FirstOrDefault(x =>
                x.Id == cartId.ToString()
            );

            return cart;
        }*/

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
                Id = Guid.NewGuid().ToString(),
                Username = "guest",
                PassHash = hash
            };

            dbContext.Add(tempuser);

            dbContext.SaveChanges();

            return tempuser;
        }

    }
}
