using AutoMapper;
using BenchmarkItemAPI.Dtos;
using Domain.Entities;

namespace BenchmarkItemAPI.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreationProduct, Product>();
            CreateMap<Product, CreationProduct>();
        }
    }
}
