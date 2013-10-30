using System;
using System.Collections.Generic;
using System.Linq;

namespace Dev.Wcf.User
{
    /// <summary>
    /// 验证器
    /// </summary>
    public static class AuthUserManager
    {

        private static IUsers _users;



        internal static bool CheckUser(string username, string password)
        {
            var list = GetList();
            if (list.Count == 0)
                throw new Exception("用户标识列表不能为空");

            return list.FirstOrDefault(x => x.UserName == username && x.Password == password) != null;
        }


        private static List<AuthUser> GetList()
        {
            if (_users == null)
                _users = new WebConfigUsers();

            return _users.GetList();
        }


        /// <summary>
        /// 设置当前的用户提取方法
        /// </summary>
        /// <param name="users"></param>
        public static void SetCurrent(IUsers users)
        {
            _users = users;
        }
    }
}