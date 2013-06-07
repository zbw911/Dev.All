namespace Dev.Demo.Application.MainBoundedContext.AdminModule
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using Dev.Comm;
    using Dev.Crosscutting.Adapter;
    using Dev.Data.Infras.Extensions;
    using Dev.Demo.Entities2.Models;
    using Dev.Demo.Style2.Repository.Repository;
    using Dev.Demo.ViewModels;

    using SysManager.Service;

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAdmintypeRepository _admintypeRepository;

        public AdminService(IAdminRepository adminRepository,
                            IAdmintypeRepository admintypeRepository)
        {
            this._adminRepository = adminRepository;
            this._admintypeRepository = admintypeRepository;
        }


        public IEnumerable<AdminInfoDto> GetAdminList(int typeid, string keyword, int pageNum, int numperPage, out int count)
        {
            var predicate = PredicateBuilder.True<Admin>();

            if (typeid > 0)
            {
                predicate = predicate.And(x => x.Typeid == typeid);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(x => x.Tname.IndexOf(keyword) >= 0
                      || x.Uname.IndexOf(keyword) >= 0
                      || x.Userid.IndexOf(keyword) >= 0);
            }





            _adminRepository.Get(predicate, x => x.Userid, pageNum, numperPage);

            count = this._adminRepository.Count(predicate);
            var dataList = this._adminRepository.GetQuery(predicate).Include(x => x.Admintype)
                .OrderByDescending(x => x.Userid).Skip((pageNum - 1) * numperPage)
                .Take(numperPage).ToList();


            return dataList.ProjectedAsCollection<AdminInfoDto>();
        }


        public AdminInfoDto GetAdmin(int id)
        {
            var admin = this._adminRepository.FindOne(x => x.AdminId == id);

            return admin.ProjectedAs<AdminInfoDto>();
        }

        public List<Admintype> GetAdminTypeList()
        {
            return this._admintypeRepository.GetAll().ToList();
        }

        public bool ExistUserid(string userid)
        {
            return this._adminRepository.GetQuery(x => x.Userid == userid).Any();
        }

        public bool ExistUName(string uname)
        {
            return this._adminRepository.GetQuery(x => x.Uname == uname).Any();
        }

        public BaseState AddAdmin(Admin collection)
        {
            if (this.ExistUserid(collection.Userid))
            {

                return new BaseState
                           {
                               ErrorCode = -1,
                               ErrorMessage = "已经存在此用户名"
                           };

            }

            if (this.ExistUName(collection.Uname))
            {

                return new BaseState
                {
                    ErrorCode = -1,
                    ErrorMessage = "已经存在此笔名"
                };
            }

            var password = Dev.Comm.Security.GetMD5(collection.Pwd);
            collection.Pwd = password;

            this._adminRepository.Add(collection);

            this._adminRepository.UnitOfWork.SaveChanges();


            if (collection.AdminId > 0)
            {
                return new BaseState();
            }

            return new BaseState(-1, "未知错误");
        }

        public BaseState EditAdmin(Admin collection)
        {
            var model = this._adminRepository.GetByKey(collection.AdminId);


            if (model.Uname != collection.Uname && this.ExistUName(collection.Uname))
            {

                return new BaseState
                {
                    ErrorCode = -1,
                    ErrorMessage = "已经存在此笔名"
                };
            }

            model.Uname = collection.Uname;

            if (!string.IsNullOrWhiteSpace(collection.Pwd))
            {
                var password = Dev.Comm.Security.GetMD5(collection.Pwd);
                model.Pwd = password;
            }

            model.Tname = collection.Tname;
            model.Email = collection.Email;
            model.Typeid = collection.Typeid;

            this._adminRepository.Update(model);

            this._adminRepository.UnitOfWork.SaveChanges();

            if (model.AdminId > 0)
            {
                return new BaseState();
            }

            return new BaseState(-1, "未知错误");
        }



        public void DeleteAdmin(int id)
        {
            this._adminRepository.Delete(x => x.AdminId == id);
            this._adminRepository.UnitOfWork.SaveChanges();
        }

        public void DeleteAdmin(int[] AdminIds)
        {
            foreach (var adminId in AdminIds)
            {
                this._adminRepository.Delete(x => x.AdminId == adminId);
            }
            this._adminRepository.UnitOfWork.SaveChanges();

        }

        public List<Admintype> GetAdminGroupList()
        {
            return this._admintypeRepository.GetAll().ToList();
        }

        public Admintype GetAdminGroup(int id)
        {
            return this._admintypeRepository.GetByKey(id);
        }


        public BaseState AddAdminGroup(string typename, string[] purviewKey)
        {
            if (purviewKey == null || purviewKey.Length == 0)
            {
                purviewKey = new string[] { };
            }

            if (this._admintypeRepository.GetQuery(x => x.Typename == typename).Any())
                return new BaseState(-1, "已经存在此组名");

            this._admintypeRepository.Add(new Admintype
                                                           {
                                                               System = false,
                                                               Typename = typename,
                                                               Purviews = "," + string.Join(",", purviewKey) + ","
                                                           });

            this._admintypeRepository.UnitOfWork.SaveChanges();

            if (this._admintypeRepository.Count(x => x.Typename == typename) == 0)
                return new BaseState();

            return new BaseState
                       {
                           ErrorCode = -1,
                           ErrorMessage = "添加组失败"
                       };
        }

        public BaseState DeleteAdminGroup(int typeid)
        {
            return this.DeleteAdminGroup(new[] { typeid });
        }

        public BaseState DeleteAdminGroup(int[] typeids)
        {
            if (this._adminRepository.GetQuery().Any(x => typeids.Contains(x.Typeid)))
            {
                return new BaseState
                           {
                               ErrorCode = -1,
                               ErrorMessage = "所删除组下用户不为空"
                           };
            }


            foreach (int i in typeids)
            {
                this._admintypeRepository.Delete(x => x.Typeid == i);
            }

            this._adminRepository.UnitOfWork.SaveChanges();

            return new BaseState { ErrorCode = 0 };
        }



        public BaseState EditAdminGroup(int id, string typename, string[] purviewKey)
        {

            if (purviewKey == null || purviewKey.Length == 0)
            {
                purviewKey = new string[] { };
            }

            var agroup = this._admintypeRepository.GetByKey(id);

            if (agroup == null)
            {
                return new BaseState
                {
                    ErrorCode = -1,
                    ErrorMessage = "不存在的用户"
                };
            }
            //与原名相同
            if (typename != agroup.Typename && this._admintypeRepository.GetQuery(x => x.Typename == typename).Any())
            {
                return new BaseState
                {
                    ErrorCode = -1,
                    ErrorMessage = "已经存在的用户组名"
                };
            }

            agroup.Purviews = "," + string.Join(",", purviewKey) + ",";

            this._admintypeRepository.Update(agroup);

            return new BaseState();
        }

        public List<Admintype> GetAdminGroupList(string purviewName, int pageNum, int numperPage, out int count)
        {

            var predicate = PredicateBuilder.True<Admintype>();


            if (!string.IsNullOrWhiteSpace(purviewName))
            {
                //与名称相近的Keys
                List<KeyValuePair<string, string>> purviews = AdminPurview.AllPurviewKeyValues;

                var keys = purviews.Where(x => x.Value.IndexOf(purviewName) >= 0).Select(x => "," + x.Key + ",");
                //根据Keys筛选权限名
                //predicate = predicate.And(
                //    x => keys.Any(y => x.Purviews.IndexOf(y) >= 0));

                var Or = PredicateBuilder.False<Admintype>();
                foreach (var key in keys)
                {

                    Or = Or.Or(x => x.Purviews.IndexOf(key) >= 0);
                }

                predicate = predicate.And(Or);

            }


            count = this._admintypeRepository.Count(predicate);


            return this._admintypeRepository.GetQuery(predicate)
                .OrderByDescending(x => x.Typeid)
                .Skip(numperPage * (pageNum - 1))
                .Take(numperPage).ToList();
        }
        /// <summary>
        /// 根据关键字查找
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetPurviewByKeyWord(string keyword, int top = 10)
        {


            List<KeyValuePair<string, string>> purviews = AdminPurview.AllPurviewKeyValues;

            var reslut = purviews.Where(x => x.Key.ToUpper().IndexOf(keyword.ToUpper()) == 0
                || x.Value.IndexOf(keyword) >= 0
                || Dev.Comm.ChineseCode.GetChineseSpell(x.Value).IndexOf(keyword.ToUpper()) == 0).Take(10);

            return reslut.ToList();

        }





        public BaseState Login(string username, string password, string loginip, out AdminInfoDto adminifo)
        {
            adminifo = null;

            var admin = this._adminRepository.GetQuery().Include(x => x.Admintype).First(x => x.Userid == username);

            if (admin == null) return new BaseState(-1, "不存在的用户");

            if (admin.Pwd != Security.GetMD5(password)) return new BaseState(-1, "密码不正确");




            //adminifo = new AdminInfoDto
            //               {
            //                   AdminId = admin.AdminId,
            //                   Email = admin.Email,
            //                   Loginip = admin.Loginip,
            //                   Logintime = admin.Logintime,
            //                   Tname = admin.Tname,
            //                   Typeid = admin.Typeid,
            //                   Typename = admin.Admintype.Typename,
            //                   Uname = admin.Uname,
            //                   Userid = admin.Userid,
            //                   PurviewsKeys = admin.Admintype.Purviews.Split(",".ToCharArray(),
            //                                                                 System.StringSplitOptions
            //                                                                       .RemoveEmptyEntries)
            //               };



            adminifo = admin.ProjectedAs<AdminInfoDto>();

            //更新登录信息
            admin.Logintime = System.DateTime.Now;
            admin.Loginip = loginip;

            this._adminRepository.Update(admin);

            return new BaseState();

        }

        public BaseState ChangePwd(int adminid, string oldpwd, string newpwd)
        {
            var admin = this._adminRepository.GetByKey(adminid);

            oldpwd = Dev.Comm.Security.GetMD5(oldpwd);
            if (oldpwd != admin.Pwd)
                return new BaseState(-1, "旧密码不正确");

            admin.Pwd = Dev.Comm.Security.GetMD5(newpwd);

            this._adminRepository.Update(admin);

            return new BaseState();
        }

        public bool DeleteAdminByUserId(string userid)
        {
            this._adminRepository.Delete(x => x.Userid == userid);
            this._adminRepository.UnitOfWork.SaveChanges();
            return true;
        }
    }
}