using AutoMapper;
using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Helper;
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

        //// Product mappings
        //CreateMap<Product, ProductResponseDto>().ReverseMap();
        //CreateMap<Product, ProductCreateRequestDto>().ReverseMap();
        //CreateMap<Product, ProductUpdateRequestDto>().ReverseMap();

        // Product mappings — ReverseMap() nahi, alag alag map karo
        CreateMap<Product, ProductResponseDto>()
            .ForMember(d => d.Sizes, o => o.MapFrom(s => JsonHelper.ToList(s.Sizes)))
            .ForMember(d => d.Colors, o => o.MapFrom(s => JsonHelper.ToList(s.Colors)));

        CreateMap<ProductCreateRequestDto, Product>()
            .ForMember(d => d.Sizes, o => o.MapFrom(s => JsonHelper.ToJson(s.Sizes)))
            .ForMember(d => d.Colors, o => o.MapFrom(s => JsonHelper.ToJson(s.Colors)));

        CreateMap<ProductUpdateRequestDto, Product>()
            .ForMember(d => d.Sizes, o => o.MapFrom(s => JsonHelper.ToJson(s.Sizes)))
            .ForMember(d => d.Colors, o => o.MapFrom(s => JsonHelper.ToJson(s.Colors)));

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


        // TenantSettings mappings
        CreateMap<TenantSettings, TenantSettingsResponseDto>().ReverseMap();
        CreateMap<TenantSettingsRequestDto, TenantSettings>();

        // TenantSlider mappings
        //CreateMap<TenantSlider, TenantSliderResponseDto>().ReverseMap();
        //CreateMap<TenantSliderRequestDto, TenantSlider>();
        //CreateMap<UpdateSliderRequestDto, TenantSlider>();


        // TenantSlider mappings
        CreateMap<TenantSlider, TenantSliderResponseDto>().ReverseMap();

        CreateMap<TenantSliderRequestDto, TenantSlider>()
            .ForMember(d => d.IsPresetImage, o => o.MapFrom(s => s.IsPresetImage));  // ← ADD

        CreateMap<UpdateSliderRequestDto, TenantSlider>()
            .ForMember(d => d.IsPresetImage, o => o.MapFrom(s => s.IsPresetImage));  // ← ADD

        // ✅ NEW — TenantIntegration mappings
        CreateMap<TenantIntegration, TenantIntegrationResponseDto>().ReverseMap();
        CreateMap<TenantIntegrationRequestDto, TenantIntegration>();

        // ✅ NEW — NotificationLog mappings
        CreateMap<NotificationLog, NotificationLogResponseDto>().ReverseMap();
    }
}

