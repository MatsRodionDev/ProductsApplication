using AutoMapper;
using OrderService.Application.Common.Models;
using ProductsService.API.Protos;

namespace OrderService.Infrastructure.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ProductImageResponse, Image>();
        }
    }
}
