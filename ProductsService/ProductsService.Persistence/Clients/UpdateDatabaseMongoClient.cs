using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ProductsService.Persistence.Clients
{
    internal class UpdateDatabaseMongoClient
    {
        private readonly IMongoDatabase Database;
        private readonly MongoClient MongoClient;

        public UpdateDatabaseMongoClient(IConfiguration configuration)
        {
            MongoClient = new MongoClient(configuration["UpdateMongoSettings:Connection"]);

            Database = MongoClient.GetDatabase(configuration["UpdateMongoSettings:DatabaseName"]);
        }

        public IMongoDatabase GetDatabase() 
            => Database;
    }
}
