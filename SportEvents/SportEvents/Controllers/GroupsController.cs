using SportEvents.Models.Application;
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
        
        GroupsBO groupsBO = new GroupsBO();

        // GET: /Groups/
        public ActionResult Index()
        {
            return View(groupsBO.Index());
        }

        public ActionResult IndexCreator()
        {
            User user = (User)Session["UserSession"];
            return View(groupsBO.IndexCreator(user.UserId));
        }

        public ActionResult IndexMember()
        {
            User user = (User)Session["UserSession"];
            return View(groupsBO.IndexMember(user.UserId));
        }

        // GET: /Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = groupsBO.GetGroupById(id);

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
        public ActionResult Create([Bind(Include = "Id,Name,Description,EndOfPaymentPeriod")] Group group)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserSession"] != null)
                {
                    User user = (User)Session["UserSession"];
                    
                    groupsBO.CreateGroup(group, user);
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
            Group group = groupsBO.GetGroupById(id);
   
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
                    User user = (User)Session["UserSession"];
                    Group group = groupsBO.GetGroupById(id);
                    
                    groupsBO.AddUserToGroup(group, user);

                    
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
            Group group = groupsBO.GetGroupById(id);

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
                groupsBO.EditGroup(group);
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
            Group group = groupsBO.GetGroupById(id);

            return View(group);
        }

        // POST: /Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = groupsBO.GetGroupById(id);

            groupsBO.DeleteGroup(group);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                groupsBO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
