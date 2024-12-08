using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProductsService.Persistence.Interfaces;

namespace ProductsService.Persistence
{
    public class MongoCommandContext(IConfiguration configuration) : IMongoCommandContext
    {
        private IMongoDatabase? Database { get; set; }
        public MongoClient? MongoClient { get; set; }

        private void ConfigureMongo()
        {
            if (MongoClient is not null)
            {
                return;
            }

            MongoClient = new MongoClient(configuration["UpdateMongoSettings:Connection"]);

            Database = MongoClient.GetDatabase(configuration["UpdateMongoSettings:DatabaseName"]);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();

            return Database!.GetCollection<T>(name);
        }
    }
}
