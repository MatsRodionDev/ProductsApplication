namespace ProductsService.Application.Common.Abstractions
{
    public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
    { 

    }


    public interface ICachedQuery 
    {
        string Key { get; }

        TimeSpan? AbsoluteExpiration { get; }

        TimeSpan? SlidingExpiration { get; }
    }
}
