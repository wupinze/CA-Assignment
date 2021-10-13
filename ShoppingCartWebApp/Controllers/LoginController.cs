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
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );

                if (session == null)
                {
                    return RedirectToAction("Index", "Logout");
                }

                return RedirectToAction("", "");
            }

            string errorMessage = (string)TempData["loginError"];
            if (errorMessage != null)
            {
                ViewData["loginError"] = errorMessage;
            }

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

            if (user == null)
            {
                TempData["loginError"] = "Invalid username or password";
                return RedirectToAction("Index", "Login");
            }

            Session session = new Session()
            {
                User = user
            };
            dbContext.Sessions.Add(session);
            dbContext.SaveChanges();

            Response.Cookies.Append("SessionId", session.Id.ToString());
            Response.Cookies.Append("Username", user.Username);

            return RedirectToAction("", "");
        }
        
    }
}
