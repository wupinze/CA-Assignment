using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartWebApp.Controllers
{
    public class LogoutController : Controller
    {
        private DBContext dbContext;
        private DB db;

        public LogoutController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext);
        }

        public IActionResult Index()
        {
            Response.Cookies.Delete("SessionId");
            Response.Cookies.Delete("Username");

            Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId.ToString()
            );

            db.DeleteSessionData(session);
            

            return RedirectToAction("Index", "Login");
        }
    }
}
