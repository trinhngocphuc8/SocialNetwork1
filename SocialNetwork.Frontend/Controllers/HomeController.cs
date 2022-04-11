using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.Domain;
using SocialNetwork.Frontend.Helpers;
using SocialNetwork.Frontend.Models;
using PagedList.Mvc;
using PagedList;

namespace SocialNetwork.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly LocalDataContext db = new LocalDataContext();

        //public async Task<int> GetUserId()
        //{
        //    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var currentUser = manager.FindById(User.Identity.GetUserId());
        //    return await UsersHelper.GetUserId(currentUser.Email);

        //}
        public ActionResult Index(int? i)
        {

            //var userId = await GetUserId();
            var posts = db.Posts.Include(p => p.Category).Include(p => p.User)
                .Include(u => u.PostComments)
                .OrderByDescending(u => u.PostDate);
            return View(posts.ToList().ToPagedList(i ?? 1, 5));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Information about Greenwich School Vietnam";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}