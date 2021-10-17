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

        public IActionResult Index(bool? proceedStatus)
        {
            if (proceedStatus.HasValue)
            {
                if (proceedStatus == false)
                {
                    ViewData["nullFields"] = "Fields cannot be empty";
                    ViewData["createSuccess"] = null;
                    return View();
                }
                else if (proceedStatus == true)
                {
                    ViewData["nullFields"] = null;
                    ViewData["createSuccess"] = "User successfully created";
                    return View();
                }
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

            bool proceedStatus;

            if (username == "" || password == "" /*|| name == "" */)
            {
                proceedStatus = false;
                return RedirectToAction("Index", "Register", proceedStatus);
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
            proceedStatus = true;
            return RedirectToAction("Index", "Register", proceedStatus);
        }
    }
}
