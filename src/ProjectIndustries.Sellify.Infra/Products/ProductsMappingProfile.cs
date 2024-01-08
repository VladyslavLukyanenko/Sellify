using AutoMapper;
using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.App.Products.Services;
using ProjectIndustries.Sellify.Core.Products;

namespace ProjectIndustries.Sellify.Infra.Products
{
  public class ProductsMappingProfile : Profile
  {
    public ProductsMappingProfile()
    {
      CreateMap<Product, ProductData>()
        .IgnoreAllPropertiesWithAnInaccessibleSetter()
        .ReverseMap();

      CreateMap<Product, SaveProductCommand>()
        .IncludeBase<Product, ProductData>()
        .ForMember(_ => _.UploadedPicture, _ => _.Ignore())
        .ReverseMap();
    }
  }
}