using AutoMapper;
using ProductsService.Application.Common.Dto.Responses;
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
                .ReverseMap();

            CreateMap<Product, ProductResponseDto>()
                .ReverseMap();
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
