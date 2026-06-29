using AutoMapper;
using ShoeStore.Data;
using ShoeStore.Shared.DTOs.Categories;
using ShoeStore.Shared.DTOs.Manufacturers;
using ShoeStore.Shared.DTOs.Orders;
using ShoeStore.Shared.DTOs.Products;
using ShoeStore.Shared.DTOs.Suppliers;

namespace ShoeStore.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Category, CategoryDto>();
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Supplier, SupplierDto>();

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        CreateMap<CreateOrderDto, Order>();
        CreateMap<CreateOrderItemDto, OrderItem>();
    }
}