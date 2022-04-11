using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SocialNetwork.Frontend.Models;
using System;

[assembly: OwinStartup(typeof(SocialNetwork.Frontend.Startup))]
namespace SocialNetwork.Frontend
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
            CreateDefaultRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login    
        public void CreateDefaultRolesandUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();

            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admins"))
            {

                // first we create Admin rool    
                role.Name = "Admins";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Adm",
                    Email = "Admin@gmail.com"
                };

                string userPWD = "A@Z200711";

                var Check = UserManager.Create(user, userPWD);
                //Add default User to Role Admin    
                if (Check.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admins");
                }
                else
                {
                    var e = new Exception("Could not add default user");

                    var enumerator = Check.Errors.GetEnumerator();
                    foreach (var error in Check.Errors)
                    {
                        e.Data.Add(enumerator.Current, error);

                    }
                    throw e;
                }
            }

            //// creating Creating Manager role     
            //if (!roleManager.RoleExists("Manager"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Manager";
            //    roleManager.Create(role);

            //}

            //// creating Creating Employee role     
            //if (!roleManager.RoleExists("Employee"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "Employee";
            //    roleManager.Create(role);

            //}
        }
    }
}
