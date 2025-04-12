using AutoMapper;
using ProductsService.API.Protos;
using ProductsService.Application.Common.Dto.Responses;

namespace ProductsService.API.Profiles
{
    public class ImageGrpcProfile : Profile
    {
        public ImageGrpcProfile()
        {
            CreateMap<ImageResponseDto, ProductImageResponse>();
        }
    }
}
