using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain
{
    public class DataContext:DbContext
    {
        public DataContext():base ("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }



        public System.Data.Entity.DbSet<SocialNetwork.Domain.UserRole> UserRoles { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.UserDepartment> UserDepartments { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.Gender> Genders { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.Person> People { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.User> Users { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork.Domain.PostComment> PostComments { get; set; }

    }
}
