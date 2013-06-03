using System;
using System.Collections.Generic;

namespace Domain.Entities.Models
{
    public partial class webpages_UsersInRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual UserExtend UserExtend { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual webpages_Roles webpages_Roles { get; set; }
    }
}
