
using SportEvents.Controllers.Utility;
using SportEvents.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SportEvents.Controllers
{
    public class LoginController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }

        //POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                login.HashPassword(); // Zahashování hesla v loginu

                if (db.IsUserRegistered(login.Email, login.Password)) // Zjistí zda je uživatel registrován
                {
                    Session["LoginSession"] = login.Email; // Vytvoření Session prozatím jen s Emailem uživatele
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewBag.Error = "Neexistující uživatel nebo chybné heslo";
                    return View();
                }
            }
            return View();
        }
	}
}