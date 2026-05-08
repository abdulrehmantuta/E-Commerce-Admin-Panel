namespace ECommerceAdminPanel.Services.IServices;

using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Models;

/// <summary>
/// Base service interface
/// </summary>
public interface IBaseService<T> where T : class
{
    Task<ApiResponse<T>> GetByIdAsync(int id);
}

/// <summary>
/// Product Service Interface
/// </summary>
/// 
public interface IProductService
{
    Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateRequestDto request);
    Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int productId);
    Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetProductsByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetProductsByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateProductAsync(int productId, ProductUpdateRequestDto request);
    Task<ApiResponse<bool>> DeleteProductAsync(int productId);
}

/// <summary>
/// Category Service Interface
/// </summary>
public interface ICategoryService
{
    Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CategoryRequestDto request);
    Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int categoryId);
    Task<ApiResponse<PaginatedResponse<CategoryResponseDto>>> GetCategoriesByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateCategoryAsync(int categoryId, CategoryRequestDto request);
    Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId);
}

/// <summary>
/// Order Service Interface
/// </summary>
public interface IOrderService
{
    Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateRequestDto request);
    Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId);
    Task<ApiResponse<PaginatedResponse<OrderResponseDto>>> GetOrdersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateOrderAsync(int orderId, OrderUpdateRequestDto request);
    Task<ApiResponse<bool>> DeleteOrderAsync(int orderId);
    Task<ApiResponse<OrderDetailResponseDto>> AddOrderDetailAsync(OrderDetailRequestDto request);
    Task<ApiResponse<List<OrderDetailResponseDto>>> GetOrderDetailsAsync(int orderId);
    Task<ApiResponse<bool>> DeleteOrderDetailAsync(int orderDetailId);
}

/// <summary>
/// Tenant Service Interface
/// </summary>
public interface ITenantService
{
    Task<ApiResponse<TenantResponseDto>> CreateTenantAsync(TenantRequestDto request);
    Task<ApiResponse<TenantResponseDto>> GetTenantByIdAsync(int tenantId);
    Task<ApiResponse<PaginatedResponse<TenantResponseDto>>> GetAllTenantsAsync(int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateTenantAsync(int tenantId, TenantRequestDto request);
    Task<ApiResponse<bool>> DeleteTenantAsync(int tenantId);
    //new
    Task<ApiResponse<TenantResponseDto>> GetTenantByDomainAsync(string domain); // ✅ ADD

}

/// <summary>
/// User Service Interface
/// </summary>
public interface IUserService
{
    Task<ApiResponse<UserResponseDto>> CreateUserAsync(UserCreateRequestDto request);
    Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int userId);
    Task<ApiResponse<PaginatedResponse<UserResponseDto>>> GetUsersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateUserAsync(int userId, UserUpdateRequestDto request);
    Task<ApiResponse<bool>> DeleteUserAsync(int userId);
}

/// <summary>
/// Page Service Interface
/// </summary>
public interface IPageService
{
    Task<ApiResponse<PageResponseDto>> CreatePageAsync(PageRequestDto request);
    Task<ApiResponse<PageResponseDto>> GetPageByIdAsync(int pageId);
    Task<ApiResponse<PaginatedResponse<PageResponseDto>>> GetPagesByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdatePageAsync(int pageId, PageRequestDto request);
    Task<ApiResponse<bool>> DeletePageAsync(int pageId);
}

/// <summary>
/// Section Service Interface
/// </summary>
public interface ISectionService
{
    Task<ApiResponse<SectionResponseDto>> CreateSectionAsync(SectionRequestDto request);
    Task<ApiResponse<SectionResponseDto>> GetSectionByIdAsync(int sectionId);
    Task<ApiResponse<List<SectionResponseDto>>> GetSectionsByPageAsync(int pageId);
    Task<ApiResponse<bool>> UpdateSectionAsync(int sectionId, SectionRequestDto request);
    Task<ApiResponse<bool>> DeleteSectionAsync(int sectionId);
}

/// <summary>
/// SectionData Service Interface
/// </summary>
public interface ISectionDataService
{
    Task<ApiResponse<SectionDataResponseDto>> CreateSectionDataAsync(SectionDataRequestDto request);
    Task<ApiResponse<SectionDataResponseDto>> GetSectionDataByIdAsync(int dataId);
    Task<ApiResponse<List<SectionDataResponseDto>>> GetSectionDataBySectionAsync(int sectionId);
    Task<ApiResponse<bool>> UpdateSectionDataAsync(int dataId, SectionDataRequestDto request);
    Task<ApiResponse<bool>> DeleteSectionDataAsync(int dataId);
}



/// <summary>
/// SectionData Service Interface
/// </summary>

public interface ICustomerService
{
    Task<ApiResponse<CustomerResponseDto>> CreateCustomerAsync(CustomerCreateDto request);
    Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(int customerId);
    //new
    Task<ApiResponse<CustomerLoginResponseDto>> RegisterAsync(CustomerRegisterDto request);
    Task<ApiResponse<CustomerLoginResponseDto>> LoginAsync(CustomerLoginDto request);
    Task<ApiResponse<List<OrderResponseDto>>> GetMyOrdersAsync(int customerId, int tenantId);

    Task<ApiResponse<PaginatedResponse<CustomerResponseDto>>> GetCustomersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateCustomerAsync(int customerId, CustomerUpdateDto request);
    Task<ApiResponse<bool>> DeleteCustomerAsync(int customerId);
}








/// <summary>
/// ITenantSettingsService Interface
/// </summary>

public interface ITenantSettingsService
{
    Task<ApiResponse<TenantSettingsResponseDto>> GetByTenantAsync(int tenantId);
    Task<ApiResponse<TenantSettingsResponseDto>> UpsertAsync(TenantSettingsRequestDto request);
}


/// <summary>
/// ITenantSliderService Interface
/// </summary>


public interface ITenantSliderService
{
    Task<ApiResponse<List<TenantSliderResponseDto>>> GetActiveAsync(int tenantId);
    Task<ApiResponse<List<TenantSliderResponseDto>>> GetAllAsync(int tenantId);
    Task<ApiResponse<int>> AddAsync(TenantSliderRequestDto request);
    Task<ApiResponse<bool>> UpdateAsync(int sliderId, UpdateSliderRequestDto request);
    Task<ApiResponse<bool>> DeleteAsync(int sliderId);
}