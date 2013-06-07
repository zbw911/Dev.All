using Dev.Demo.Entities2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Style2.Repository.Test.Repository
{
    public interface ITestRepository
    {
        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<Admin> GetAdminList(int page, int pageSize);

        /// <summary>
        /// 根据AdminId获取管理员信息
        /// </summary>
        /// <param name="AdminId"></param>
        /// <returns></returns>
        Admin GetAdminByAdminId(int AdminId);

        /// <summary>
        /// 添加管理员功能
        /// </summary>
        /// <param name="UName"></param>
        /// <param name="Pwd"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        bool AddAdmin(string UName, string Pwd, int TypeId);

        /// <summary>
        /// 更改密码功能
        /// </summary>
        /// <param name="AdminId"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        bool UpdatePwd(int AdminId, string NewPwd);
    }
}
