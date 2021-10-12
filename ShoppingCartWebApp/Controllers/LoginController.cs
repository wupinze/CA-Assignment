using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
using Microsoft.AspNetCore.Http;

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
            return View();
        }
        /*
        public IActionResult Login(IFormCollection form)
        {

        }
        */
    }
}
