using AutoMapper;
using UserService.BLL.Models;
using UserService.DAL.Entities;

namespace UserService.BLL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>()
                .ReverseMap();

        }
    }
}
