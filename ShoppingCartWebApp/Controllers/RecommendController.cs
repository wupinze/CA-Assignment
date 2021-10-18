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
            ViewData["sessionId"] = session.Id;
            ViewData["username"] = session.User.Username;

            int count = CartCount();
            ViewData["cartcount"] = count;
            return View();
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

    }
}
