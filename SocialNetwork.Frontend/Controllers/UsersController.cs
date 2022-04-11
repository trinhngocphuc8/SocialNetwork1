using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SocialNetwork.Domain;
using SocialNetwork.Frontend.Models;

namespace SocialNetwork.Frontend.Controllers
{
    
    public class UsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Gender).Include(u => u.UserDepartment).Include(u => u.UserRole);
            return View(users.ToList());
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




        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Gender_id = new SelectList(db.Genders, "Gender_id", "Name");
            ViewBag.Department_id = new SelectList(db.UserDepartments, "Department_id", "Name");
            ViewBag.Role_id = new SelectList(db.UserRoles, "Role_id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_id,Fullname,Email,Password,Gender_id,Role_id,Department_id,Image")] User user, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                var userAsp = new ApplicationUser { UserName = user.Email, Email = user.Email, };
                var result = UserManager.Create(userAsp, user.Password);

                if (result.Succeeded)

                {
                    
                        SignInManager.SignIn(userAsp, isPersistent: false, rememberBrowser: false);
                    
                string filename = Path.GetFileName(File.FileName);
                string _filename = DateTime.Now.ToString("hhmmssfff") + filename;
                string path = Path.Combine(Server.MapPath("~/Images/"), _filename);
                user.Image = "~/Images/" + _filename;

                db.Users.Add(user);
                if (File.ContentLength < 10000000)
                {
                    if (db.SaveChanges() > 0)
                    {
                        File.SaveAs(path);
                    }
                }
                else
                {
                    ViewBag.msg = "File must less than or equal to 1 MB";
                }

                    
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }



            }
                

            ViewBag.Gender_id = new SelectList(db.Genders, "Gender_id", "Name", user.Gender_id);
            ViewBag.Department_id = new SelectList(db.UserDepartments, "Department_id", "Name", user.Department_id);
            ViewBag.Role_id = new SelectList(db.UserRoles, "Role_id", "Name", user.Role_id);
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
            ViewBag.Gender_id = new SelectList(db.Genders, "Gender_id", "Name", user.Gender_id);
            ViewBag.Department_id = new SelectList(db.UserDepartments, "Department_id", "Name", user.Department_id);
            ViewBag.Role_id = new SelectList(db.UserRoles, "Role_id", "Name", user.Role_id);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_id,Fullname,Email,Gender_id,Role_id,Department_id,Image")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gender_id = new SelectList(db.Genders, "Gender_id", "Name", user.Gender_id);
            ViewBag.Department_id = new SelectList(db.UserDepartments, "Department_id", "Name", user.Department_id);
            ViewBag.Role_id = new SelectList(db.UserRoles, "Role_id", "Name", user.Role_id);
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
