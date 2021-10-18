using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ShoppingCartWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                //start new session if session is null
                User user = CreateTempUser();
                session = new Session()
                {
                    User = user
                };
                dbContext.Sessions.Add(session);
                dbContext.SaveChanges();
                Response.Cookies.Append("SessionId", session.Id.ToString());
                Response.Cookies.Append("Username", user.Username);
                ViewData["username"] = user.Username;
            }
            else
            {
                ViewData["username"] = Request.Cookies["Username"];
            }

            List<ShoppingCartWebApp.Models.Product> products = dbContext.products.Where(x =>
               x.Id != null
                ).ToList();
            ViewData["username"] = session.User.Username;
            ViewData["products"] = products;
            ViewData["sessionId"] = session.Id;
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
            if(session == null)
            {
                return 0;
            }
            int sum = db.getCarViewTotalQuantity(session.Id);

            return sum;
        }
        
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

