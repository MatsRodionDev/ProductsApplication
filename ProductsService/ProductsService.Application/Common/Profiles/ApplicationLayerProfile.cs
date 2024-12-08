using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.Common.Dto.Responses;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Create;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;
using ProductsService.Domain.Enums;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;

namespace ProductsService.Application.Common.Profiles
{
    public class ApplicationLayerProfile : Profile
    {
        public ApplicationLayerProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => MapCategories(src.Categories)))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => MapImageFiles(src.ImageFiles)))
                .ReverseMap();

            CreateMap<GetProductsByFiltersRequest, GetProductsFilters>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

            CreateMap<GetProductsByUserIdRequest, GetUsersProductsFilters>();

            CreateMap<Product, ProductResponseDto>()
                .ReverseMap();
            CreateMap<Category, CategoryResponseDto>()
                .ReverseMap();
            CreateMap<Image, ImageResponseDto>()
                .ReverseMap();
        }

        private List<IFormFile> MapImageFiles(List<IFormFile>? images)
        {
            if (images is null || images.Count == 0)
            {
                return [];
            }

            return images;
        }

        private List<Category> MapCategories(List<Categories>? categories)
        {
            if(categories is null || categories.Count == 0)
            {
                return [];
            }

            var mappedCategories = new List<Category>();

            foreach (var category in categories)
            {
                mappedCategories.Add(new Category
                {
                    Name = category.ToString() 
                });
            }

            return mappedCategories;
        }
    }
}
