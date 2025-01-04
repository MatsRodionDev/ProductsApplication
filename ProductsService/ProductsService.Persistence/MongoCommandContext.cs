using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductsService.Persistence.Settings;
using ProductsService.Persistence.Interfaces;

namespace ProductsService.Persistence
{
    internal class MongoCommandContext : IMongoCommandContext
    {
        private readonly IMongoDatabase _database;

        public MongoCommandContext(IOptions<CommandMongoDbSettings> settings)
        {
            _database = new MongoClient(settings.Value.ConnectionString)
                .GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
