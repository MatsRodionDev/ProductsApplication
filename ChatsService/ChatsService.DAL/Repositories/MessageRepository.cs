using ChatsService.DAL.Entities;
using ChatsService.DAL.Interfaces;

namespace ChatsService.DAL.Repositories
{
    public class MessageRepository(ApplicationDbContext context) : GenericRepository<MessageEntity>(context), IMessageRepository
    {

    }
}
