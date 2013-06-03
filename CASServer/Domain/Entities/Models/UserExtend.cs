using System;
using System.Collections.Generic;

namespace Domain.Entities.Models
{
    public partial class UserExtend
    {
        public UserExtend()
        {
            this.webpages_UsersInRoles = new List<webpages_UsersInRoles>();
        }

        public int UserId { get; set; }
        public decimal Uid { get; set; }
        public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }
}
