using AutoMapper;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Domain.Models;

namespace ProductsService.Application.Common.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageResponseDto>()
                .ReverseMap();
        }
    }
}
