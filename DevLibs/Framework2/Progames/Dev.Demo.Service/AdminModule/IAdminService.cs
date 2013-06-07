namespace Dev.Demo.Application.MainBoundedContext.AdminModule
{
    using System.Collections.Generic;

    using Dev.Demo.Entities2.Models;
    using Dev.Demo.ViewModels;

    using SysManager.Service;

    public interface IAdminService
    {
        IEnumerable<AdminInfoDto> GetAdminList(int typeid, string keyword, int pageNum, int numperPage, out int count);

        AdminInfoDto GetAdmin(int id);

        List<Admintype> GetAdminTypeList();

        bool ExistUserid(string userid);

        bool ExistUName(string uname);

        BaseState AddAdmin(Admin collection);

        BaseState EditAdmin(Admin collection);

        void DeleteAdmin(int id);

        void DeleteAdmin(int[] AdminIds);

        List<Admintype> GetAdminGroupList();

        Admintype GetAdminGroup(int id);

        BaseState AddAdminGroup(string typename, string[] purviewKey);

        BaseState DeleteAdminGroup(int typeid);

        BaseState DeleteAdminGroup(int[] typeids);

        BaseState EditAdminGroup(int id, string typename, string[] purviewKey);

        List<Admintype> GetAdminGroupList(string purviewName, int pageNum, int numperPage, out int count);

        /// <summary>
        /// 根据关键字查找
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        List<KeyValuePair<string, string>> GetPurviewByKeyWord(string keyword, int top = 10);

        BaseState Login(string username, string password, string loginip, out AdminInfoDto adminifo);

        BaseState ChangePwd(int adminid, string oldpwd, string newpwd);

        bool DeleteAdminByUserId(string userid);
    }
}