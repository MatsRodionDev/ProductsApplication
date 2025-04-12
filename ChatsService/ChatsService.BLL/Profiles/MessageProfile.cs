using AutoMapper;
using ChatsService.BLL.Models;
using ChatsService.DAL.Entities;

namespace ChatsService.BLL.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageEntity, Message>();
        }
    }
}
