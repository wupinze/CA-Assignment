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
            bool nullFields = (bool) TempData["nullFields"];
            if (nullFields)
            {
                ViewData["nullFields"] = "Fields cannot be left empty";
            }

            bool createSuccess = (bool) TempData["createSuccess"];
            if (createSuccess)
            {
                ViewData["createSuccess"] = "User successfully created";
                return RedirectToAction("Login", "Login");
            }

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
            //string name = form["name"];
            string username = form["username"];
            string password = form["password"];

            if (username == "" || password == "" /*|| name == "" */)
            {
                bool nullFields = true;
                TempData["nullFields"] = nullFields;
                return RedirectToAction("Index", "Register");
            }

            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(username + password));

            dbContext.Add(new User
            {
                //Name = name;
                Username = username,
                PassHash = hash
            });

            dbContext.SaveChanges();
            bool createSuccess = true;
            TempData["createSuccess"] = createSuccess;
            return RedirectToAction("Index", "Register");
        }
    }
}
