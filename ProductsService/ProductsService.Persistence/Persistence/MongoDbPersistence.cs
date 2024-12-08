using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Persistence
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            ProductMap.Configure();
            ImageMap.Configure();
            CategoryMap.Configure();

            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }

        
    }
}
