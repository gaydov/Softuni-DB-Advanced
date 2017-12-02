using System.Linq;
using AutoMapper;
using ProductsShop.App.ModelsDTOs;
using ProductsShop.Models;

namespace ProductsShop.App.Core
{
    public class ProductsShopMappingProfile : Profile
    {
        public ProductsShopMappingProfile()
        {
            this.CreateMap<Product, ProductWithSellerDto>()
                .ForMember(dto => dto.Seller,
                opt => opt.MapFrom(src => src.Seller.FirstName != null ? $"{src.Seller.FirstName} {src.Seller.LastName}" : $"{src.Seller.LastName}"));

            this.CreateMap<Product, ProductWithBuyerDto>()
                .ForMember(dto => dto.Buyer,
                    opt => opt.MapFrom(src => src.Buyer.FirstName != null ? $"{src.Buyer.FirstName} {src.Buyer.LastName}" : $"{src.Buyer.LastName}"));

            this.CreateMap<Product, SoldProductDto>()
                .ForMember(dto => dto.BuyerFirstName,
                opt => opt.MapFrom(src => src.Buyer.FirstName))
                .ForMember(dto => dto.BuyerLastName,
                opt => opt.MapFrom(src => src.Buyer.LastName));

            this.CreateMap<User, UserWithSoldProductsDto>()
                .ForMember(dto => dto.SoldProducts,
                opt => opt.MapFrom(src => src.ProductsSold));

            this.CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Category,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(dto => dto.ProductsCount,
                opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dto => dto.AveragePrice,
                opt => opt.MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(dto => dto.TotalRevenue,
                opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}