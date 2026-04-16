using AutoMapper;
using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Models;

namespace ECommerceAdminPanel.Config;

/// <summary>
/// AutoMapper configuration profile
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Tenant mappings
        CreateMap<Tenant, TenantResponseDto>().ReverseMap();
        CreateMap<Tenant, TenantRequestDto>().ReverseMap();

        // User mappings
        CreateMap<User, UserResponseDto>().ReverseMap();
        CreateMap<User, UserCreateRequestDto>().ReverseMap();
        CreateMap<User, UserUpdateRequestDto>().ReverseMap();

        // Category mappings
        CreateMap<Category, CategoryResponseDto>().ReverseMap();
        CreateMap<Category, CategoryRequestDto>().ReverseMap();

        // Product mappings
        CreateMap<Product, ProductResponseDto>().ReverseMap();
        CreateMap<Product, ProductCreateRequestDto>().ReverseMap();
        CreateMap<Product, ProductUpdateRequestDto>().ReverseMap();

        // Order mappings
        CreateMap<Order, OrderResponseDto>().ReverseMap();
        CreateMap<Order, OrderCreateRequestDto>().ReverseMap();
        CreateMap<Order, OrderUpdateRequestDto>().ReverseMap();

        // OrderDetail mappings
        CreateMap<OrderDetail, OrderDetailResponseDto>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailRequestDto>().ReverseMap();

        // Page mappings
        CreateMap<Page, PageResponseDto>().ReverseMap();
        CreateMap<Page, PageRequestDto>().ReverseMap();

        // Section mappings
        CreateMap<Section, SectionResponseDto>().ReverseMap();
        CreateMap<Section, SectionRequestDto>().ReverseMap();

        // SectionData mappings
        CreateMap<SectionData, SectionDataResponseDto>().ReverseMap();
        CreateMap<SectionData, SectionDataRequestDto>().ReverseMap();



        // Customer mappings
        CreateMap<Customer, CustomerResponseDto>().ReverseMap();
        CreateMap<Customer, CustomerCreateDto>().ReverseMap();
        CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
    }
}
