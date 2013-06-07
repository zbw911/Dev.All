using Dev.Data;
using Dev.Demo.Entities2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Dev.Demo.Style2.Repository.Test.Repository
{
    public class TestRepository : GenericRepository<Admin>, ITestRepository
    {
                /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public TestRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TestRepository(DbContext context)
            : base(context)
        {
        }


        public bool Add(Admin admin) 
        {
            return this.Add(admin)!=null;
        }

        List<Admin> ITestRepository.GetAdminList(int pageIndex, int pageSize)
        {
            return this.Get(m => m.AdminId, pageIndex, pageSize, Data.Infras.SortOrder.Descending).ToList();
        }

        Admin ITestRepository.GetAdminByAdminId(int AdminId)
        {
            return this.GetQuery().Where(m => m.AdminId == AdminId).FirstOrDefault();
        }

        bool ITestRepository.AddAdmin(string UName, string Pwd, int TypeId)
        {
            return null!=this.Add(new Admin {  Admintype=new Entities2.Models.Admintype(){ Admins=null, Purviews="", System=false, Typeid=1, Typename=""}, Email="", Loginip="", Logintime=DateTime.Now, Pwd=Pwd, Uname=UName, Tname="", Typeid=TypeId});
        }

        bool ITestRepository.UpdatePwd(int AdminId, string NewPwd)
        {
            var adminInfo = this.GetQuery(m => m.AdminId == AdminId).FirstOrDefault();
            adminInfo.Pwd = NewPwd;
            this.Update(adminInfo);
            return true;
        }
    }
}
