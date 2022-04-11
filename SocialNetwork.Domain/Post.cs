using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork.Domain
{
    public class Post
    {
        [Key]
        public int Post_id { get; set; }

        public int User_id { get; set; }

        public int Category_id { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }
        
        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [Required]
        public string Contents { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public ICollection<PostComment> PostComments { get; set; }




    }
}
