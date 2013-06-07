using System;
using System.Collections.Generic;

namespace Dev.Demo.Entities2.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public int Typeid { get; set; }
        public string Userid { get; set; }
        public string Pwd { get; set; }
        public string Uname { get; set; }
        public string Tname { get; set; }
        public string Email { get; set; }
        public DateTime? Logintime { get; set; }
        public string Loginip { get; set; }
        public virtual Admintype Admintype { get; set; }
    }


}
