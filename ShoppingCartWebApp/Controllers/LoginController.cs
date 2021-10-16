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
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
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
                //else if (Request.Cookies["Username"] == "guest")
                //{
                //    // not sure if this can be an else block rather than else-if
                //    // so this is if there is an active guest session
                //}
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
            // modify from this line onwards
            if (Request.Cookies["SessionId"] != null)// if session Id exists
            {
                Debug.WriteLine("Existing session:");
                Debug.WriteLine($"Login/Login, user: {Request.Cookies["Username"]}, session: {Request.Cookies["SessionId"]}");
                Response.Cookies.Delete("Username");
                Response.Cookies.Append("Username", username);
                // below probably still shows guest as client hasn't sent new request
                Debug.WriteLine($"Login/Login, user: {Request.Cookies["Username"]}, session: {Request.Cookies["SessionId"]}");
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
            Debug.WriteLine("Create New session");
            Debug.WriteLine($"Login/Login, user: {Request.Cookies["Username"]}, session: {Request.Cookies["SessionId"]}");
            return RedirectToAction("Index", "Gallery");
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
    }

    
}


