using MongoDB.Driver;
using ProductsService.Persistence.Clients;
using ProductsService.Persistence.Interfaces;

namespace ProductsService.Persistence
{
    internal class MongoCommandContext(UpdateDatabaseMongoClient client) : IMongoCommandContext
    {
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return client.GetDatabase().GetCollection<T>(name);
        }
    }
}
