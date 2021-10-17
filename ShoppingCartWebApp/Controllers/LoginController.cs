using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingCartWebApp.Controllers
{
    public class LoginController : Controller
    {
        private DBContext dbContext;

        public LoginController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["SessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"];
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId)
                ;

                if (session == null)
                {
                    return RedirectToAction("Index", "Logout");
                }
                
                return RedirectToAction("Index", "Gallery");

            }

            ViewData["loginError"] = TempData["loginError"];


            return View();
        }

        public IActionResult Login(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];

            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(username + password));

            User user = dbContext.Users.FirstOrDefault(x =>
               x.Username == username && x.PassHash == hash);

            if (user == null && (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)))
            {
                TempData["loginError"] = "Invalid username or password";
                return RedirectToAction("Index", "Login");
            }

            if (user == null)
            {
                TempData["loginError"] = "Fields cannot be empty";
                return RedirectToAction("Index", "Login");
            }

            TempData["loginError"] = null;

            Session session = new Session()
            {
                User = user
            };
            dbContext.Sessions.Add(session);
            dbContext.SaveChanges();

            Response.Cookies.Append("SessionId", session.Id.ToString());
            Response.Cookies.Append("Username", user.Username);

            return RedirectToAction("Index", "Gallery");
        }
        
    }
}


