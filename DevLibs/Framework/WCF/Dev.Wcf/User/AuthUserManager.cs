using System;
using System.Collections.Generic;
using System.Linq;

namespace Dev.Wcf.User
{
    static class AuthUserManager
    {
        static AuthUserManager()
        {
            var strUserList = System.Configuration.ConfigurationManager.AppSettings["wcfclientuser"];
            var listusers = strUserList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in listusers)
            {
                var userpwdrole = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var user = new AuthUser { UserName = userpwdrole[0], Password = userpwdrole[1] };

                if (userpwdrole.Length > 2)
                    user.Role = userpwdrole[2];

                AddUser(user);
            }
        }
        private static List<AuthUser> list = new List<AuthUser>();
        public static void AddUser(AuthUser user)
        {
            list.Add(user);
        }

        public static bool CheckUser(string username, string password)
        {
            if (list.Count == 0)
                throw new Exception("用户标识列表不能为");

            return list.FirstOrDefault(x => x.UserName == username && x.Password == password) != null;
        }


        private static List<AuthUser> getUserList()
        {

            return list;
        }

    }
}