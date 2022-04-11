using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Domain;

namespace SocialNetwork.Frontend.Controllers
{
    //[Authorize]
    public class UserDepartmentController : Controller
    {
        private DataContext db = new DataContext();

        // GET: UserDepartment
        public ActionResult Index()
        {
            return View(db.UserDepartments.ToList());
        }

        // GET: UserDepartment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDepartment userDepartment = db.UserDepartments.Find(id);
            if (userDepartment == null)
            {
                return HttpNotFound();
            }
            return View(userDepartment);
        }

        // GET: UserDepartment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDepartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Department_id,Name")] UserDepartment userDepartment)
        {
            if (ModelState.IsValid)
            {
                db.UserDepartments.Add(userDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDepartment);
        }

        // GET: UserDepartment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDepartment userDepartment = db.UserDepartments.Find(id);
            if (userDepartment == null)
            {
                return HttpNotFound();
            }
            return View(userDepartment);
        }

        // POST: UserDepartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Department_id,Name")] UserDepartment userDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDepartment);
        }

        // GET: UserDepartment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDepartment userDepartment = db.UserDepartments.Find(id);
            if (userDepartment == null)
            {
                return HttpNotFound();
            }
            return View(userDepartment);
        }

        // POST: UserDepartment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserDepartment userDepartment = db.UserDepartments.Find(id);
            db.UserDepartments.Remove(userDepartment);
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
