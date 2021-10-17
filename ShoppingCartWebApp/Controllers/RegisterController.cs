using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartWebApp.Controllers
{
    public class RegisterController : Controller
    {
        private DBContext dbContext;

        public RegisterController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewData["nullFieldPresent"] = TempData["nullFieldPresent"];

            return View();
        }

        public IActionResult IsUsernameOk([FromBody] User user)
        {
            User useronDB = dbContext.Users.FirstOrDefault(x =>
            x.Username == user.Username);

            if (useronDB != null)
            {
                return Json(new { isOkay = false });
            }
            return Json(new { isOkay = true });
        }

        public IActionResult Create(IFormCollection form)
        {
            string name = form["name"];
            string username = form["username"];
            string password = form["password"];


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) 
                // || string.IsNullOrEmpty(name))
            {
                TempData["nullFieldPresent"] = "Fields cannot be empty";
                TempData["createUserSuccess"] = null;
                return RedirectToAction("Index", "Register");
            }

            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(username + password));

            dbContext.Add(new User
            {
                //Name = name,
                Username = username,
                PassHash = hash
            });

            dbContext.SaveChanges();
            TempData["nullFieldPresent"] = null;
            TempData["createUserSuccess"] = "User successfully created";
            return RedirectToAction("Index", "Login");
        }
    }
}
