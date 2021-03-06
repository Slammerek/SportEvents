﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportEvents.Models;

namespace SportEvents.Controllers
{
    public class EventsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events;
            return View(events.ToList());
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
            ViewBag.GrpId = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TimeOfEvent,RepeatUntil,GrpId,Place,Description,Price,Repeat,Interval")] Event @event)
        {
            if (ModelState.IsValid)
            {
                User user = (User)Session["UserSession"];

                if (db.IsUserCreatorOfGroup(user.Id, @event.GrpId))
                {
                    if (@event.Repeat == 0) // TODO: místo 0 pro opakování používat 1 jako true
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
                    TempData["notice"] = "Uživatel " + user.Email + " není uživatelem skupiny s ID:" + @event.GrpId;
                    return RedirectToAction("Index");
                    
                }
                
                
            }
            

            ViewBag.GrpId = new SelectList(db.Groups, "Id", "Name", @event.GrpId);
            ViewBag.Error = "Nejste zakladatelem tehle skupiny";
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
            ViewBag.GrpId = new SelectList(db.Groups, "Id", "Name", @event.GrpId);
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
            ViewBag.GrpId = new SelectList(db.Groups, "Id", "Name", @event.GrpId);
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
