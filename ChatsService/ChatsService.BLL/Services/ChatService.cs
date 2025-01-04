using AutoMapper;
using ChatsService.BLL.Exceptions;
using ChatsService.BLL.Interfaces;
using ChatsService.BLL.Models;
using ChatsService.BLL.Dtos;
using ChatsService.DAL.Entities;
using ChatsService.DAL.Interfaces;

namespace ChatsService.BLL.Services
{
    public class ChatService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IProductService productService) : IChatService
    {
        public async Task<List<ChatResponseDto>> GetAllSellersChatsAsync(Guid sellerId, CancellationToken cancellationToken)
        {
            var chats = await unitOfWork.ChatRepository.GetBySellerIdAsync(sellerId, cancellationToken);

            var responses = new List<ChatResponseDto>();

            foreach (var chat in chats)
            {
                var response = CreateChatResponse(chat, sellerId);
                responses.Add(response);
            }

            return responses;
        }

        public async Task<List<ChatResponseDto>> GetAllBuyersChatsAsync(Guid buyerId, CancellationToken cancellationToken)
        {
            var chats = await unitOfWork.ChatRepository.GetByBuyerIdAsync(buyerId, cancellationToken);

            var responses = new List<ChatResponseDto>();

            foreach (var chat in chats)
            {
                var response = CreateChatResponse(chat, buyerId);
                responses.Add(response);
            }

            return responses;
        }

        public async Task<Chat> GetChatByIdAsync(Guid chatId, CancellationToken cancellationToken)
        {
            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId, cancellationToken)
                ?? throw new NotFoundException("There is no chat with this id.");

            return mapper.Map<Chat>(chat);
        }

        public async Task<ChatResponseDto> CreateChatAsync(Guid productId, string buyerName, Guid buyerId, CancellationToken cancellationToken)
        {
            var product = await productService.GetByIdAsync(productId);

            var chat = await unitOfWork.ChatRepository
                .GetByProductAndBuyerIdsAsync(productId, buyerId, cancellationToken);

            if (chat is not null)
            {
                throw new BadRequestException("You already have a chat with the seller for this product");
            }

            var newChat = new ChatEntity
            {
                Id = Guid.NewGuid(),
                SellerId = product.UserId,
                BuyerId = buyerId,
                BuyerName = buyerName,
                ProductId = productId
            };

            await unitOfWork.ChatRepository.CreateAsync(newChat, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ChatResponseDto>(newChat);
        }

        public async Task<Message> SendMessageAsync(Guid senderId, string text, Guid chatId, CancellationToken cancellationToken)
        {
            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId, cancellationToken)
                ?? throw new NotFoundException("There is no chat with this id."); 

            if (chat.SellerId != senderId || chat.BuyerId != senderId)
            {
                throw new UnauthorizedException("You cannot send messages in this chat");
            }

            var message = new MessageEntity
            {
                Id = Guid.NewGuid(),
                Text = text,
                ChatId = chatId,
                SenderId = senderId
            };
            chat.Messages.Add(message);

            unitOfWork.ChatRepository.Update(chat);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<Message>(message);
        }

        public async Task MarkMessaesAsRedadedAsync(Guid userId, Guid chatId, CancellationToken cancellationToken)
        {
            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId, cancellationToken)
                ?? throw new NotFoundException("There is no chat with this id.");

            if (chat.SellerId != userId || chat.BuyerId != userId)
            {
                throw new UnauthorizedException("You cannot send messages in this chat");
            }

            ReadMessages(chat.Messages, userId);
                 
            unitOfWork.ChatRepository.Update(chat);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private void ReadMessages(List<MessageEntity> messages, Guid userId)
        {
            var unreadedMessages = messages
                .Where(m => m.SenderId != userId && !m.IsReaded)
                .ToList();

            foreach(var message in unreadedMessages)
            {
                message.IsReaded = true;
            }
        }

        private ChatResponseDto CreateChatResponse(ChatEntity chat, Guid userId)
        {
            var response = mapper.Map<ChatResponseDto>(chat);
            response.UnreadMessagesQuantity = chat.Messages
                .Where(m => m.SenderId != userId)
                .Where(m => m.IsReaded == false)
                .Count();
            var messageEntity = chat.Messages
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault();
            response.LastMessage = mapper.Map<Message>(messageEntity);

            return response;
        }
    }
}
