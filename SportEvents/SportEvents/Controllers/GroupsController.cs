using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SportEvents.Models
{
    public class GroupsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /Groups/
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        // GET: /Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: /Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserSession"] != null)
                {
                    User user = (User)Session["UserSession"];
                    user = db.GetUserByEmail(user.Email);
                    group.Creator = user;
                    user.Groups.Add(group);
                    group.Users.Add(user);
                    group.CreateTime = DateTime.Now;
                    group.StartOfPaymentPeriod = DateTime.Now;
                    group.EndOfPaymentPeriod = DateTime.Now; // TODO: aby se cas konce obdobi zadaval pri vytvoreni skupiny
                    // neuklada se do databaze enum typ
                    db.Groups.Add(group);
                    db.SaveChanges();
                    TempData["notice"] = "Skupina " + group.Name + " byla úspěšně vytvořena uživatelem " + user.Email;
                    return RedirectToAction("Index");
                    
                }

                
            }

            return View(group);
        }

        // GET: /Groups/AddUserToGroup/5
        public ActionResult AddUserToGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        // POST: /Groups/AddUserToGroup/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("AddUserToGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserToGroup(int id)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserSession"] != null)
                {
                    Group group = db.Groups.Find(id);
                    User user = (User)Session["UserSession"];
                    user = db.GetUserByEmail(user.Email);

                    user.Groups.Add(group);
                    group.Users.Add(user);
                    
                    db.SaveChanges();

                    return RedirectToAction("Index");  
                }
            }

            return View();
        }

        // GET: /Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: /Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: /Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: /Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
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
