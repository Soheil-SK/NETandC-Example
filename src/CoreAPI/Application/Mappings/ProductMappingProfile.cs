using AutoMapper;
using CoreAPI.Application.DTOs;
using CoreAPI.Domain.Entities;

namespace CoreAPI.Application.Mappings;

/// <summary>
/// Profile AutoMapper pour mapper entre entités et DTOs
/// Automatisation du mapping objet-à-objet
/// </summary>
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.IsInStock()));
    }
}

