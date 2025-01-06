using ChatsService.API.Hubs.Interfaces;
using ChatsService.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Consts;

namespace ChatsService.API.Hubs
{
    [Authorize]
    public class ChatHub(
        IChatService chatService,
        ICacheService cacheService) : Hub<IChatHub>
    {
        public override async Task OnConnectedAsync()
        { 
            await cacheService.SetAsync(
                Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value,
                Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task GetAllSellersChats()
        {
            var userId = Guid.Parse(Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var chats = await chatService.GetAllSellersChatsAsync(userId);

            foreach (var chat in chats) 
            {
                await Groups
                    .AddToGroupAsync(
                        Context.ConnectionId,
                        chat.Id.ToString());
            }

            await Clients
                .Caller
                .RecieveChats(chats);
        }

        public async Task GetAllBuyersChats()
        {
            var userId = Guid.Parse(Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var chats = await chatService.GetAllBuyersChatsAsync(userId);

            foreach (var chat in chats)
            {
                await Groups
                    .AddToGroupAsync(
                        Context.ConnectionId,
                        chat.Id.ToString());
            }

            await Clients
                .Caller
                .RecieveChats(chats);
        }

        public async Task JoinChat(Guid chatId)
        {
            var chat = await chatService.GetChatByIdAsync(chatId);

            await Groups
                .AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients
                .Caller
                .ReceiveChat(chat);
        }

        public async Task SendMessage(string text, Guid chatId)
        {
            var senderId = Guid.Parse(Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var message = await chatService.SendMessageAsync(senderId, text, chatId);

            await Clients
                .Group(chatId.ToString())
                .ReceiveMessage(message);
        }

        public async Task MarkMessagesAsReaded(Guid chatId)
        {
            var userId = Guid.Parse(Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            await chatService.MarkMessaesAsRedadedAsync(userId, chatId);

            await Clients
                .OthersInGroup(chatId.ToString())
                .ReadMessages(chatId);
        }

        public async Task CreateChat(Guid productId, string buyerName)
        {
            var userId = Guid.Parse(Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            var chat = await chatService.CreateChatAsync(productId, buyerName, userId);

            var connectionId = await cacheService.GetAsync<string>(chat.SellerId.ToString());

            if(string.IsNullOrEmpty(connectionId))
            {
                return;
            }

            await Clients.User(connectionId)
                .ReceiveNewChat(chat);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await cacheService.RemoveAsync(
                Context.User!.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
