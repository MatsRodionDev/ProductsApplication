using MongoDB.Driver;

namespace ProductsService.Persistence.Interfaces
{
    public interface IMongoContext 
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
