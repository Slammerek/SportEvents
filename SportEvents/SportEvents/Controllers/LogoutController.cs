using SportEvents.Controllers.Utility;
using SportEvents.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SportEvents.Controllers
{
    public class LogoutController : Controller
    {
        //GET: Users/Logout

        public ActionResult Logout()
        {
            Session["LoginSession"] = null; // vynulování session

            return RedirectToAction("Index", "Users");
        }
	}
}