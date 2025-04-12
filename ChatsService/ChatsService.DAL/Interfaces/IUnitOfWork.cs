namespace ChatsService.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IChatRepository ChatRepository { get; }
        IMessageRepository MessageRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void MigrateDatabase();
    }
}
