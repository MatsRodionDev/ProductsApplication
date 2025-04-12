using AutoMapper;
using ChatsService.BLL.Models;
using ChatsService.BLL.Dtos;
using ChatsService.DAL.Entities;

namespace ChatsService.BLL.Profiles
{
    public class ChatResponseProfile : Profile
    {
        public ChatResponseProfile()
        {
            CreateMap<ChatEntity, ChatResponseDto>();
            CreateMap<Chat, ChatResponseDto>();
        }
    }
}
