namespace Dev.Demo.Application.MainBoundedContext.TestModule
{
    using Dev.Demo.Style2.Repository.Repository;
    using Dev.Demo.Style2.Repository.Test.Repository;
    using Dev.Demo.ViewModels;
    using Dev.Demo.ViewModels.TestDto;
    using System.Collections.Generic;
    using Dev.Crosscutting.Adapter;
    using Dev.Demo.Entities2.Models;
    public class TestService : ITestService
    {


        private readonly  ITestRepository _testRepository;
        private readonly IAdmintypeRepository _admintypeRepository;

        public TestService(ITestRepository testRepository,
                            IAdmintypeRepository admintypeRepository)
        {
            this._testRepository = testRepository;
            this._admintypeRepository = admintypeRepository;
        }

        public string[] GetData()
        {
            return new string[] { "aaaa", "bbbb", "dccsdf" };
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="UserRegDto"></param>
        /// <returns></returns>
        public bool UserReg(UserRegData UserRegDto)
        {


            //业务性判断, 用户是否存在 ,......

           

            throw new System.NotImplementedException();
        }





        //System.Collections.Generic.IEnumerable<Dev.Demo.ViewModels.TestDto.AdminDto> ITestService.GetAdminList(int page, int pageSize)
        //{
        //    throw new System.NotImplementedException();
        //}

        string[] ITestService.GetData()
        {
            throw new System.NotImplementedException();
        }

        bool ITestService.UserReg(UserRegData UserRegDto)
        {
            throw new System.NotImplementedException();
        }

        List<AdminDto> ITestService.GetAdminList(int page, int pageSize)
        {
            List<Admin> datalist = this._testRepository.GetAdminList(page, pageSize);
            return datalist.ProjectedAsCollection<AdminDto>();
        }
        AdminDto ITestService.GetAdminByAdminId(int AdminId)
        {
            return this._testRepository.GetAdminByAdminId(AdminId).ProjectedAs<AdminDto>();
        }

        bool ITestService.AddAdmin(string UName, string Pwd, int TypeId)
        {
            return this._testRepository.AddAdmin(UName,Pwd,TypeId);
        }

        bool ITestService.UpdatePwd(int AdminId, string NewPwd)
        {
            return this._testRepository.UpdatePwd(AdminId,NewPwd);
        }
    }
}