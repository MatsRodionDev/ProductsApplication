using AutoMapper;
using ChatsService.BLL.Exceptions;
using ChatsService.BLL.Interfaces;
using ChatsService.BLL.Models;
using ChatsService.BLL.Dtos;
using ChatsService.DAL.Entities;
using ChatsService.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace ChatsService.BLL.Services
{
    public class ChatService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IProductService productService,
        ILogger<ChatService> logger) : IChatService
    {
        public async Task<List<ChatResponseDto>> GetAllSellersChatsAsync(Guid sellerId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching all chats for seller with ID {SellerId}", sellerId);

            var chats = await unitOfWork.ChatRepository.GetBySellerIdAsync(sellerId, cancellationToken);

            if (chats is null)
            {
                logger.LogWarning("No chats found for seller with ID {SellerId}", sellerId);
                return new List<ChatResponseDto>();
            }

            var responses = chats.Select(chat => CreateChatResponse(chat, sellerId)).ToList();

            logger.LogInformation("Found {ChatCount} chats for seller with ID {SellerId}", responses.Count, sellerId);

            return responses;
        }

        public async Task<List<ChatResponseDto>> GetAllBuyersChatsAsync(Guid buyerId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching all chats for buyer with ID {BuyerId}", buyerId);

            var chats = await unitOfWork.ChatRepository.GetByBuyerIdAsync(buyerId, cancellationToken);

            if (chats is null)
            {
                logger.LogWarning("No chats found for buyer with ID {BuyerId}", buyerId);
                return new List<ChatResponseDto>();
            }

            var responses = chats.Select(chat => CreateChatResponse(chat, buyerId)).ToList();

            logger.LogInformation("Found {ChatCount} chats for buyer with ID {BuyerId}", responses.Count, buyerId);

            return responses;
        }

        public async Task<Chat> GetChatByIdAsync(Guid chatId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching chat with ID {ChatId}", chatId);

            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId, cancellationToken);

            if (chat is null)
            {
                logger.LogWarning("Chat with ID {ChatId} does not exist", chatId);
                throw new NotFoundException("There is no chat with this id.");
            }

            logger.LogInformation("Chat with ID {ChatId} successfully retrieved", chatId);

            return mapper.Map<Chat>(chat);
        }

        public async Task<ChatResponseDto> CreateChatAsync(Guid productId, string buyerName, Guid buyerId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating chat for product {ProductId} with buyer {BuyerId}", productId, buyerId);

            var product = await productService.GetByIdAsync(productId);

            if (product is null)
            {
                logger.LogWarning("Product with ID {ProductId} does not exist", productId);
                throw new NotFoundException("Product with this ID does not exist");
            }

            var existingChat = await unitOfWork.ChatRepository
                .GetByProductAndBuyerIdsAsync(productId, buyerId, cancellationToken);

            if (existingChat is not null)
            {
                logger.LogWarning("Chat for product {ProductId} with buyer {BuyerId} already exists", productId, buyerId);
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

            logger.LogInformation("Chat created successfully with ID {ChatId}", newChat.Id);

            return mapper.Map<ChatResponseDto>(newChat);
        }

        public async Task<Message> SendMessageAsync(Guid senderId, string text, Guid chatId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending message in chat {ChatId} by user {SenderId}", chatId, senderId);

            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId, cancellationToken);

            if (chat is null)
            {
                logger.LogWarning("Chat with ID {ChatId} does not exist", chatId);
                throw new NotFoundException("There is no chat with this id.");
            }

            if (chat.SellerId != senderId && chat.BuyerId != senderId)
            {
                logger.LogWarning("Unauthorized attempt to send message in chat {ChatId} by user {SenderId}", chatId, senderId);
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

            logger.LogInformation("Message sent successfully in chat {ChatId} by user {SenderId}", chatId, senderId);

            return mapper.Map<Message>(message);
        }

        public async Task MarkMessagesAsReadAsync(Guid userId, Guid chatId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Marking messages as read in chat {ChatId} by user {UserId}", chatId, userId);

            var chat = await unitOfWork.ChatRepository.GetByIdAsync(chatId, cancellationToken);

            if (chat is null)
            {
                logger.LogWarning("Chat with ID {ChatId} does not exist", chatId);
                throw new NotFoundException("There is no chat with this id.");
            }

            if (chat.SellerId != userId && chat.BuyerId != userId)
            {
                logger.LogWarning("Unauthorized attempt to mark messages as read in chat {ChatId} by user {UserId}", chatId, userId);
                throw new UnauthorizedException("You cannot mark messages as read in this chat");
            }

            ReadMessages(chat.Messages, userId);

            unitOfWork.ChatRepository.Update(chat);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Messages marked as read in chat {ChatId} by user {UserId}", chatId, userId);
        }

        private void ReadMessages(List<MessageEntity> messages, Guid userId)
        {
            var unreadMessages = messages
                .Where(m => m.SenderId != userId && !m.IsRead)
                .ToList();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }
        }

        private ChatResponseDto CreateChatResponse(ChatEntity chat, Guid userId)
        {
            var response = mapper.Map<ChatResponseDto>(chat);
            response.UnreadMessagesQuantity = chat.Messages
                .Count(m => m.SenderId != userId && !m.IsRead);
            var lastMessage = chat.Messages
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefault();
            response.LastMessage = mapper.Map<Message>(lastMessage);

            return response;
        }
    }
}
