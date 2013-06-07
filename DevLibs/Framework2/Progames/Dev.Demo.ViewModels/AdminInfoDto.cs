using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.ViewModels
{
    public class AdminInfoDto
    {
        public int AdminId { get; set; }
        public int Typeid { get; set; }
        public string Userid { get; set; }

        public string Uname { get; set; }
        public string Tname { get; set; }
        public string Email { get; set; }
        public DateTime? Logintime { get; set; }
        public string Loginip { get; set; }
        public string Typename { get; set; }
        public string[] PurviewsKeys { get; set; }
    }
}
