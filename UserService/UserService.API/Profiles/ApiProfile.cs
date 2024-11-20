using AutoMapper;
using UserService.API.Dtos.Requests;
using UserService.API.Dtos.Responses;
using UserService.BLL.Models;

namespace UserService.API.Profiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<RegisterUserRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(er => er.Password));

            CreateMap<User, UserResponse>();
        }
    }
}
