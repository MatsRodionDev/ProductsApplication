namespace OrderService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBasketRepository BasketRepository { get; }
        IBasketItemRepository BasketItemRepository { get; }
        IOrderRepository OrderRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void DataBaseMigrate();
    }
}
