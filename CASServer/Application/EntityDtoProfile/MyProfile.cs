namespace Application.EntityDtoProfile
{
    using Application.Dto;
    using Application.Dto.User;
    using AutoMapper;
    using Domain.Entities.Models;

    public class MyProfile : Profile
    {


        #region Methods

        protected override void Configure()
        {
            //UserProfile实体类
            var userView = Mapper.CreateMap<UserProfile,UserProfileModel>();
            userView.ReverseMap();
             
            var map = Mapper.CreateMap<UserProfile, MyInfo>();
            //map.ForMember(dto => dto.ShortContent, mc => mc.MapFrom(e => "这是短的内容1:"));
            ////map.ReverseMap();

            ////帖子实体类
            //var mapPost = Mapper.CreateMap<Group_BBS, PostModel>();
            //mapPost.ReverseMap();

            ////帖子展示类
            //var mapView = Mapper.CreateMap<Group_BBS, ViewModel>();
            //mapView.ReverseMap();




            //Admin => AdminInfoDto
            //var map = Mapper.CreateMap<Admin, AdminInfoDto>();
            //map.ForMember(
            //    dto => dto.PurviewsKeys,
            //    mc =>
            //    mc.MapFrom(e => e.Admintype.Purviews.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));


            ////Admin => AdminDto
            //var maptest = Mapper.CreateMap<Admin, Demo.ViewModels.TestDto.AdminDto>();
            //map.ForMember(
            //    dto => dto.PurviewsKeys,
            //    mc =>
            //    mc.MapFrom(e => e.Admintype.Purviews.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));

        }

        #endregion
    }
}