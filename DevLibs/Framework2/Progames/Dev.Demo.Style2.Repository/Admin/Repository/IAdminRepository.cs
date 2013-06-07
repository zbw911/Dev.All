namespace Dev.Demo.Style2.Repository.Repository
{
    using System.Collections.Generic;

    using Dev.Data.Infras;
    using Dev.Demo.Entities2.Models;

    public interface IAdminRepository : IRepository<Admin>
    {
        List<Admin> GetSystemAdmin();
        List<Admin> GetSystemActiveAdmin();
    }
}
