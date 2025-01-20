namespace UserService.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void DatabaseMigrate();
    }
}
