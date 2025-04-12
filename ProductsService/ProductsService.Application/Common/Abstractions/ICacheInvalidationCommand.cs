namespace ProductsService.Application.Common.Abstractions
{
    public interface ICacheInvalidationCommand : ICommand
    {
        string Key { get; }
    }
}
