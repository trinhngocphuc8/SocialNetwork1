using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain
{
    public class PostComment
    {
        [Key]
        public int PostComment_id { get; set; }

        public int Post_id { get; set; }

        public int User_id { get; set; }


        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CommentDate { get; set; }


        [DataType(DataType.MultilineText)]
        [Required]
        public string Comment { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        
        [JsonIgnore]
        public virtual Post Post { get; set; }


    }
}
