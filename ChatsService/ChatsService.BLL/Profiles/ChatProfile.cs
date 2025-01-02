using AutoMapper;
using ChatsService.BLL.Models;
using ChatsService.DAL.Entities;

namespace ChatsService.BLL.Profiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatEntity, Chat>();
        }
    }
}
