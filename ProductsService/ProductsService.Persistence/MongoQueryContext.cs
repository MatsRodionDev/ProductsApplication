using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProductsService.Persistence.Interfaces;

namespace ProductsService.Persistence
{
    public class MongoQueryContext(IConfiguration configuration) : IMongoQueryContext
    {
        private IMongoDatabase? Database { get; set; }
        public MongoClient? MongoClient { get; set; }

        private void ConfigureMongo()
        {
            if (MongoClient is not null)
            {
                return;
            }

            MongoClient = new MongoClient(configuration["ReadMongoSettings:Connection"]);

            Database = MongoClient.GetDatabase(configuration["ReadMongoSettings:DatabaseName"]);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();

            return Database!.GetCollection<T>(name);
        }
    }
}
