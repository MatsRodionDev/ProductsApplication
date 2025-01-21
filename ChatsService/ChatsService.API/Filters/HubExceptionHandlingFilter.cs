using ChatsService.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatsService.API.Filters
{
    public class HubExceptionHandlingFilter(
        IHubContext<ChatHub> hubContext,
        ILogger<HubExceptionHandlingFilter> logger) : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, 
            ValueTask<object?>> next)
        {
            try
            {
                logger.LogInformation($"Processing ws request: {invocationContext.HubMethod.Name}");
                return await next(invocationContext);
            }
            catch (Exception ex)
            {
                await hubContext
                    .Clients
                    .Client(invocationContext.Context.ConnectionId)
                    .SendAsync("HandleError", ex.Message);

                logger.LogError(ex, ex.Message);

                throw new HubException(ex.Message);
            }
        }
    }
}
