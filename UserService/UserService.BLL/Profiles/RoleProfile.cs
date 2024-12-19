using AutoMapper;
using UserService.BLL.Models;
using UserService.DAL.Entities;

namespace UserService.BLL.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleEntity, Role>()
               .ReverseMap();
        }
    }
}   
