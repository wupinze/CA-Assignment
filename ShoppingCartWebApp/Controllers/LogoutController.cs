using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
namespace ShoppingCartWebApp.Controllers
{
    public class LogoutController : Controller
    {
        //public IActionResult Index()
        //{
        //    // ask client to remove these cookies so that
        //    // they won't be sent over next time
        //    Response.Cookies.Delete("SessionId");
        //    Response.Cookies.Delete("Username");

        //    return RedirectToAction("Index", "Login");
        //}
        private DBContext dbContext;
        private DB db;

        public LogoutController(DBContext dbContext)
        {
            this.dbContext = dbContext;
            db = new DB(this.dbContext);
        }

        public IActionResult Index()
        {
           
                
            

            if (Request.Cookies["sessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );
                if (session.User.Username != "guest") 
                {
                    // only clear cookies and delete session if user is logged in (i.e. not guest)
                    Response.Cookies.Delete("SessionId");
                    Response.Cookies.Delete("Username");
                    db.DeleteSessionData(session);
                }
               
            }


            return RedirectToAction("Index", "Login");
        }
    }
}