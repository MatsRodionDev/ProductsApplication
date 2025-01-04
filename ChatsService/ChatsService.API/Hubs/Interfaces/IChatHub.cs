using ChatsService.BLL.Models;
using ChatsService.BLL.Dtos;

namespace ChatsService.API.Hubs.Interfaces
{
    public interface IChatHub
    {
        public Task ReceiveMessage(Message message);
        public Task ReadMessages(Guid chatId);
        public Task ReceiveChat(Chat chat);
        public Task ReceiveNewChat(ChatResponseDto chat);
        public Task RecieveChats(List<ChatResponseDto> chat);
    }
}
