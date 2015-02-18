
using SportEvents.Controllers.Utility;
using SportEvents.Models;
using SportEvents.Models.Application;
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
        private UsersBO usersBO = new UsersBO();

        // GET: /Home/Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ForgotPassword()
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
                if (usersBO.IsUserRegistered(login.Email, login.Password)) 
                {
                    User user = usersBO.GetUser(login);
                    Session["UserSession"] = user;
                    TempData["notice"] = "Uživatel " + login.Email + " byl úspěšně přihlášen";
                    return RedirectToAction("index", "Groups");
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
            Session["UserSession"] = null; // vynulování session

            return RedirectToAction("Index", "Home");
        }

        //Post: ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword forgotPassword) // Post metoda pro kontrolu, zda je email v databázi
        {
            if (ModelState.IsValid)
            {
                if (usersBO.IsEmailInDatabase(forgotPassword.Email)) // Je email v databázi
                {
                    string smtpUserName = "sportevents1@seznam.cz";
                    string smtpPassword = "777003862";
                    string smtpHost = "smtp.seznam.cz";
                    int smtpPort = 25;
                    string emailTo = forgotPassword.Email;
                    string subject = string.Format("Nové heslo");
                    string body = string.Format("Vaše nové heslo je 36E45DA");

                    EmailService service = new EmailService();

                    bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);


                   TempData["notice"] = "Na váš email bylo odesláno nové heslo";
                   return RedirectToAction("Index");
                    
                }


                TempData["notice"] = "Zadaný email není v databázi";
                return View();
               

            }

            return View();
        }
	}
}