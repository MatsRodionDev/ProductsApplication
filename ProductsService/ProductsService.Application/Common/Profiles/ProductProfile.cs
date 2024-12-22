using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.UseCases.ProductUseCases.Commands.Create;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByFilters;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetByUserId;
using ProductsService.Domain.Enums;
using ProductsService.Domain.Filters;
using ProductsService.Domain.Models;

namespace ProductsService.Application.Common.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => MapCategories(src.Categories)))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => MapImageFiles(src.ImageFiles)))
                .ReverseMap();

            CreateMap<GetProductsByFiltersRequest, GetProductsFilters>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

            CreateMap<GetProductsByUserIdRequest, GetUsersProductsFilters>();
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
