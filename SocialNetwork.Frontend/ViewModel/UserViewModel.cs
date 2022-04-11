using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Frontend.ViewModel
{
    public class UserViewModel    
    {
        [Required]
        public int User_id { get; set; }

        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Gender_id { get; set; }
        [Required]
        public int Role_id { get; set; }
        [Required]
        public int Department_id { get; set; }

        [Required]
        [AllowHtml]
        public byte[] Image { get; set; }
    }
}