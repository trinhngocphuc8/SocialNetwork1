
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace SocialNetwork.Domain
{
    public class User
    {
        [Key]
        public int User_id { get; set; }
        
        [Required]
        public string Fullname { get; set; }
        
        [Required]
        public string Email { get; set; }

        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public int Gender_id { get; set; }

        public int Role_id { get; set; }
       
        public int Department_id { get; set; }

        [AllowHtml]
        public string Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }




        // Build Relationship

        [JsonIgnore]
        public virtual UserRole UserRole { get; set; }

        [JsonIgnore]
        public virtual UserDepartment UserDepartment { get; set; }

        [JsonIgnore]
        public virtual Gender Gender { get; set; }

            
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }

        [JsonIgnore]
        public ICollection<PostComment> PostComments { get; set; }

    }
}