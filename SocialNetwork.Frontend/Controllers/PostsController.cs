using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.Domain;
using SocialNetwork.Frontend.Helpers;
using SocialNetwork.Frontend.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Frontend.Controllers
{
    [ValidateInput(false)]
    [Authorize]
    public class PostsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // Check dùng Helper để kiểm tra nếu đăng nhập rồi thì mới đăng post được.
        public async Task<int> GetUserId()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return await UsersHelper.GetUserId(currentUser.Email);

        }


        #region Comment

      
        public async Task<ActionResult> CreateComment(int id)
        {


            var userId = await GetUserId();
            var userPost = await db.Posts.FindAsync(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }
            var comment = new PostComment
            {
                Post_id = id,
                User_id = userId

            };

            return View(comment);
        }

        
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateComment(PostComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CommentDate = DateTime.Now.ToUniversalTime();
                db.PostComments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(comment);
        }

        #endregion


        #region Post



        // GET: Posts
        public async Task<ActionResult> Index()
        {         
                var posts = db.Posts.Include(p => p.Category).Include(p => p.User);
                return View( await posts.ToListAsync());


        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public async Task<ActionResult> Create()
        {

            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Name");
            var userId = await GetUserId();
            var userpost = new Post { User_id = userId };
            
            return View(userpost);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create( Post post, HttpPostedFileBase[] Files)
        {
            if (ModelState.IsValid)
            {
                    
                    //iterating through multiple file collection 
                    foreach (HttpPostedFileBase file in Files)
                    {
                        //Checking file is available to save.
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                            //Save file to server folder
                            file.SaveAs(ServerSavePath);
                            //assigning file uploaded status to ViewBag for showing message to user.
                            ViewBag.UploadStatus = Files.Count().ToString() + " files uploaded successfully.";
                        }
                    }
                post.PostDate = DateTime.Now.ToUniversalTime();
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Name", post.Category_id);
            ViewBag.User_id = new SelectList(db.Users, "User_id", "Email", post.User_id);
            return View(post);
        }

        //private void SendEmailToStaff(string title, string content)
        //{
        //    var mailAddress = ConfigurationManager.AppSettings["mailAddress"];
        //    var mailPass = ConfigurationManager.AppSettings["mailPass"];


        //    //check role if student - no need
        //    var user = (User)Session["Login"];
        //    //var role = new DataAccess().GetRoleById(user.roleId);

        //    //if (role.Name.ToLower().Contains("student"))
        //    //{
        //    SmtpClient client = new SmtpClient();
        //    client.Port = 587;
        //    client.Host = "smtp.gmail.com";
        //    client.EnableSsl = true;
        //    client.Timeout = 10000;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = new System.Net.NetworkCredential(mailAddress, mailPass);

        //    MailMessage mailMessage = new MailMessage();
        //    mailMessage.To.Add(mailAddress);
        //    mailMessage.From = new MailAddress(mailAddress);
        //    mailMessage.Subject = "[NEW IDEA SUBMITTED]";
        //    mailMessage.Body = MailBody(title, content, user.Email);
        //    mailMessage.BodyEncoding = Encoding.UTF8;
        //    mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //    mailMessage.IsBodyHtml = true;

        //    try
        //    {
        //        client.Send(mailMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //    //}
        //}
        

        //private string MailBody(string title, string content, string mail)
        //{

        //    var body = "";
        //    body += string.Format("<p><h3>Title: </h3>{0}</p>", title);
        //    body += string.Format("<p><h3>Content: </h3>{0}</p>", content);
        //    body += string.Format("<p><h3>User Email: </h3>{0}</p>", mail);

        //    return body;
        //}

        //send email





        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Name", post.Category_id);
            ViewBag.User_id = new SelectList(db.Users, "User_id", "Email", post.User_id);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Name", post.Category_id);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Upload&Download File

        //// GET: Home
        //public ActionResult UploadFiles()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult UploadFiles( HttpPostedFileBase[] File)
        //{

        //    //Ensure model state is valid
        //    if (ModelState.IsValid)
        //    {   //iterating through multiple file collection 
        //        foreach (HttpPostedFileBase file in File)
        //        {
        //            //Checking file is available to save.
        //            if (file != null)
        //            {
        //                var InputFileName = Path.GetFileName(file.FileName);
        //                var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
        //                //Save file to server folder
        //                file.SaveAs(ServerSavePath);
        //                //assigning file uploaded status to ViewBag for showing message to user.
        //                ViewBag.UploadStatus = File.Count().ToString() + " files uploaded successfully.";
        //            }


        //        }
        //    }


        //    return View();
        //}

        #endregion

       
      
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
