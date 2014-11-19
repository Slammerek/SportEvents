using SportEvents.Controllers.Utility;
using SportEvents.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SportEvents.Controllers
{
    public class UsersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Users/ListOfUsers
        public ActionResult ListOfUsers()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Registration
        public ActionResult Registration()
        {
            return View();
        }



        // POST: Users/Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.IsEmailInDatabase(user.Email)) // Pokud databáze zadaný e-mail obsahuje, vrátí nás na formulář pro registraci
                {
                    ViewBag.Error = "Uživatel pod tímto emailem je již registrován";
                    return View();
                }

                user.RegistrationTime = DateTime.Now; // Vytvoření data registrace
                user.Password = UtilityMethods.CalculateHashMd5(user.Password);
                user.PasswordComparison = UtilityMethods.CalculateHashMd5(user.PasswordComparison);
                db.Users.Add(user); // uložení uživatele a uložení změn
                db.SaveChanges();
                return RedirectToAction("ListOfUsers");
                
            }

            return View();

        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Password,FirstName,Surname,Telephone,RegistrationTime")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListOfUsers");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("ListOfUsers");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}