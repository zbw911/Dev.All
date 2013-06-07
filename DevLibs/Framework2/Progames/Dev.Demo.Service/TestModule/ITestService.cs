using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Application.MainBoundedContext.TestModule
{
    using Dev.Demo.ViewModels;
    using Dev.Demo.ViewModels.TestDto;

    public interface ITestService
    {
        string[] GetData();
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="UserRegDto"></param>
        /// <returns></returns>
        bool UserReg(UserRegData UserRegDto);

        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<AdminDto> GetAdminList(int page, int pageSize);

        /// <summary>
        /// 根据AdminId获取管理员信息
        /// </summary>
        /// <param name="AdminId"></param>
        /// <returns></returns>
        AdminDto GetAdminByAdminId(int AdminId);

        /// <summary>
        /// 添加管理员功能
        /// </summary>
        /// <param name="UName"></param>
        /// <param name="Pwd"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        bool AddAdmin(string UName,string Pwd,int TypeId);

        /// <summary>
        /// 更改密码功能
        /// </summary>
        /// <param name="AdminId"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        bool UpdatePwd(int AdminId,string NewPwd);

    }
}
