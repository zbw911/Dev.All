using System;
using System.Collections.Generic;

namespace Dev.Demo.Entities2.Models
{
    public partial class Admintype
    {
        public Admintype()
        {
            this.Admins = new List<Admin>();
        }

        public int Typeid { get; set; }
        public string Typename { get; set; }
        public bool System { get; set; }
        public string Purviews { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
    }
}
