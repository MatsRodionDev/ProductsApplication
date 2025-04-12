using MongoDB.Bson.Serialization;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Persistence
{
    public static class CategoryMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Category>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true);
            });
        }
    }
}
