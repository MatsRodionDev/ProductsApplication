using AutoMapper;
using UserService.BLL.Models;
using UserService.DAL.Entities;

namespace UserService.BLL.Profiles
{
    public class BusinessLogicLayerProfile : Profile
    {
        public BusinessLogicLayerProfile()
        {
            CreateMap<UserEntity, User>()
                .ReverseMap();

            CreateMap<RoleEntity, Role>()
                .ReverseMap();

        }
    }
}
