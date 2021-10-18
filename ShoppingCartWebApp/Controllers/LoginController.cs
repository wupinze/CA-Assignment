using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;


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
            if (Request.Cookies["SessionId"] != null)// if there is a session id
            {
                string sessionId = Request.Cookies["sessionId"];
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );

                if (session == null)// if session is not in session table
                {
                    return RedirectToAction("Index", "Logout");
                }

                else if (Request.Cookies["Username"] != "guest") // else if user is already logged in
                {
                    // redirect to gallery
                    return RedirectToAction("", "");
                }
                //return RedirectToAction("Index", "Gallery");

            }

            ViewData["loginError"] = TempData["loginError"];

            ViewData["createUserSuccess"] = TempData["createUserSuccess"];

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

            if (Request.Cookies["SessionId"] != null)// if session Id exists
            {
           

                //update cookies
                Response.Cookies.Delete("Username");// delete guest username
                Response.Cookies.Append("Username", username);


                // update session object
                string sessionId = Request.Cookies["sessionId"];

                Session currentSession = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId);

                // Get cart entries corresponding to GUEST user
                List<Cart> currentCart = dbContext.carts.Where(x => x.user.Id == currentSession.User.Id).ToList();

                // Change entries to user who is logging in
                foreach (var row in currentCart)
                {
                    row.user = user; // Change entries to user who is logging in
                }
                // change session user first
                User guestUser = currentSession.User;
                currentSession.UserId = user.Id;
                currentSession.User = user;

                // Delete temporary user from Users table

                // get guest user
                List<User> guestUsers = dbContext.Users.Where(x => x.Id == guestUser.Id).ToList();
                foreach(var user1 in guestUsers)
                {
                    dbContext.Remove(user1);
                }

                //Persist changes to database
                dbContext.SaveChanges();


                return RedirectToAction("Index", "Gallery");
            }

          

          

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


