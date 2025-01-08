using ChatsService.BLL.Models;
using ChatsService.BLL.Dtos;

namespace ChatsService.BLL.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDto> CreateChatAsync(Guid productId, string buyerName, Guid buyerId, CancellationToken cancellationToken = default);
        Task<List<ChatResponseDto>> GetAllBuyersChatsAsync(Guid buyerId, CancellationToken cancellationToken = default);
        Task<List<ChatResponseDto>> GetAllSellersChatsAsync(Guid sellerId, CancellationToken cancellationToken = default);
        Task<Chat> GetChatByIdAsync(Guid chatId, CancellationToken cancellationToken = default);
        Task<Message> SendMessageAsync(Guid senderId, string text, Guid chatId, CancellationToken cancellationToken = default);
        Task MarkMessagesAsReadAsync(Guid userId, Guid chatId, CancellationToken cancellationToken = default);
    }
}