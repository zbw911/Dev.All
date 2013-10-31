using System;
using System.Collections.Generic;

namespace Dev.Wcf.User
{
    class WebConfigUsers : IUsers
    {
        private static List<AuthUser> List;
        /// <summary>
        /// 取得用户
        /// </summary>
        /// <returns></returns>
        public List<AuthUser> GetList()
        {

            if (List != null)
                return List;


            List = new List<AuthUser>();

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

            return List;
        }



        /// <summary>
        /// 添加用户
        /// </summary>
        public void AddUser(AuthUser user)
        {
            List.Add(user);
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        public void Empty()
        {
            List.Clear();

            List = null;
        }
    }
}