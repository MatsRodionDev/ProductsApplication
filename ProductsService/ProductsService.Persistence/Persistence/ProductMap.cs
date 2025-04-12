using MongoDB.Bson.Serialization;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Persistence
{
    public static class ProductMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Product>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true);
                map.MapMember(x => x.Description).SetIsRequired(true);
                map.MapMember(x => x.UserId).SetIsRequired(true);
            });
        }
    }
}
