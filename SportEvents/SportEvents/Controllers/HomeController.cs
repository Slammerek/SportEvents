﻿
using SportEvents.Controllers.Utility;
using SportEvents.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SportEvents.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /Home/Index
        public ActionResult Index()
        {
            return View();
        }

        //POST: /Home/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login) // Post metoda pro prihlaseni do systemu
        {
            if (ModelState.IsValid)
            {
                if (db.IsUserRegistered(login.Email, UtilityMethods.CalculateHashMd5(login.Password))) // Zahashuje heslo a zjistí zda je uživatel registrován
                {
                    Session["LoginSession"] = login.Email; // Vytvoření Session
                    TempData["notice"] = "Vítejte";
                    return RedirectToAction("ListOfUsers", "Users");
                }
                else
                {
                    ViewBag.Error = "Neexistující uživatel nebo chybné heslo";
                    return View();
                }
            }
            return View();
        }

        //GET: Home/Logout

        public ActionResult Logout()
        {
            Session["LoginSession"] = null; // vynulování session

            return RedirectToAction("Index", "Home");
        }
	}
}