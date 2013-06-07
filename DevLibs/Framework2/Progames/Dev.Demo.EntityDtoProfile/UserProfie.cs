namespace Dev.Demo.EntityDtoProfile
{
    using System;

    using AutoMapper;

    using Dev.Demo.Entities2.Models;
    using Dev.Demo.ViewModels;

    /// <summary>
    ///     配置 User 模块的映射
    /// </summary>
    public class UserProfie : Profile
    {
        #region Methods

        protected override void Configure()
        {
            //Admin => AdminInfoDto
            var map = Mapper.CreateMap<Admin, AdminInfoDto>();
            map.ForMember(
                dto => dto.PurviewsKeys,
                mc =>
                mc.MapFrom(e => e.Admintype.Purviews.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));


            //Admin => AdminDto
            var maptest = Mapper.CreateMap<Admin,Demo.ViewModels.TestDto.AdminDto>();
            map.ForMember(
                dto => dto.PurviewsKeys,
                mc =>
                mc.MapFrom(e => e.Admintype.Purviews.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));

        }

        #endregion
    }
}