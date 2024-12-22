using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ProductsService.Persistence.Clients
{
    internal class ReadDatabaseMongoClient
    {
        private readonly IMongoDatabase Database;
        private readonly MongoClient MongoClient;

        public ReadDatabaseMongoClient(IConfiguration configuration)
        {
            MongoClient = new MongoClient(configuration["ReadMongoSettings:Connection"]);

            Database = MongoClient.GetDatabase(configuration["ReadMongoSettings:DatabaseName"]);
        }

        public IMongoDatabase GetDatabase()
            => Database;
    }
}
