using MongoDB.Bson.Serialization;
using ProductsService.Domain.Models;

namespace ProductsService.Persistence.Persistence
{
    public static class ImageMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Image>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.ImageName).SetIsRequired(true);
            });
        }
    }
}
