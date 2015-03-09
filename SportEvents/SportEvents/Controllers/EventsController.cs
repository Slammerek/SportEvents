using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportEvents.Models;
using SportEvents.Models.Application;

namespace SportEvents.Controllers
{
    //nic
    public class EventsController : Controller
    {
        private DataContext db = new DataContext();
        private EventsBO evBo = new EventsBO();

        // GET: Events
        public ActionResult Index(string sortOrder)
        {
            
            User user = (User)Session["UserSession"];
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date" : "Date";
            var events = from s in db.Events select s;
            switch (sortOrder)
            {
                case "name":
                    events = events.OrderBy(s => s.Name);
                    break;
                case "date":
                    events = events.OrderBy(s => s.TimeOfEvent);
                    break;
                default :
                    
                    break;
    
            }
            //return View(db.AllEventsWhereIsUserCreator(user.Id)); pokud chceme vratit jen udalosti, kde je clovek zakladatel
            return View(@events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            var user = (User) Session["UserSession"];

            var groups = evBo.GetGroupsWhereUserIsAdmin(user);

            ViewBag.GroupId = new SelectList(groups, "GroupId", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TimeOfEvent,RepeatUntil,GroupId,Place,Description,Price,Repeat,Interval,CreatorId")] Event @event)
        {
            User user = (User)Session["UserSession"];

            if (ModelState.IsValid)
            {
                @event.CreatorId = user.UserId;
                if (db.IsUserCreatorOfGroup(user.UserId, @event.GroupId))
                {
                    if (@event.Repeat != 0) 
                    {
                      //  double differenceInWeeks = ((@event.RepeatUntil - @event.TimeOfEvent).TotalDays/7);
                        // TODO: ukládat do databáze i RepeatUntil a interval
                        for (DateTime dT = @event.TimeOfEvent ; dT <= @event.RepeatUntil; dT = dT.AddDays(7*@event.Interval)) {
                            db.Events.Add(@event);
                            db.SaveChanges();
                            @event.TimeOfEvent = @event.TimeOfEvent.AddDays(7*@event.Interval); 
                        }
                            
                    }
                    else
                    {
                        db.Events.Add(@event);
                        db.SaveChanges();
                    }
                    TempData["notice"] = "Událost " + @event.Name + " byla vytvořena uživatelem " + user.Email;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "Uživatel " + user.Email + " není zakladatelem skupiny s ID:" + @event.GroupId;
                    return RedirectToAction("Index");
                    
                }
                
                
            }


            var groups = evBo.GetGroupsWhereUserIsAdmin(user);

            ViewBag.GroupId = new SelectList(groups, "GroupId", "Name");
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name");
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TimeOfEvent,RepeatUntil,GrpId,Place,Description,Price,Repeat,Interval")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name");
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
