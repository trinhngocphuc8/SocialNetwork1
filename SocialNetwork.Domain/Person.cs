using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain
{

    public class Person
    {
        [Key]
        public int Person_id { get; set; }
        public string Fullname { get; set; }

        public string Picture { get; set; }

        public string Email { get; set; }
        public int Gender_id { get; set; }
        public int User_id { get; set; }

        [JsonIgnore]
        public virtual Gender Gender { get; set; }

        [JsonIgnore]
        public virtual UserRole UserRole { get; set; }

        [JsonIgnore]
        public virtual UserDepartment UserDepartment { get; set; }
    }

}
