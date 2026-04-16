namespace ECommerceAdminPanel.Repositories.IRepository;
using ECommerceAdminPanel.DTOs.Request; 
using ECommerceAdminPanel.Models;
using ECommerceAdminPanel.DTOs.Response;

/// <summary>
/// Base repository interface
/// </summary>
public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<int> CreateAsync(T entity);
    Task<int> UpdateAsync(int id, T entity);
    Task<int> DeleteAsync(int id);
}

/// <summary>
/// Product Repository Interface
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<List<Product>> GetByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
}

/// <summary>
/// Category Repository Interface
/// </summary>
public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<List<Category>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
}

/// <summary>
/// Order Repository Interface
/// </summary>
public interface IOrderRepository : IBaseRepository<Order>
{
    Task<List<Order>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
}

/// <summary>
/// OrderDetail Repository Interface
/// </summary>
public interface IOrderDetailRepository : IBaseRepository<OrderDetail>
{
    Task<List<OrderDetail>> GetByOrderAsync(int orderId);
}

/// <summary>
/// Tenant Repository Interface
/// </summary>
public interface ITenantRepository : IBaseRepository<Tenant>
{
}

/// <summary>
/// User Repository Interface
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    Task<List<User>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
        Task<User?> GetByEmailAsync(string email); // ← yeh add karo

}

/// <summary>
/// Page Repository Interface
/// </summary>
public interface IPageRepository : IBaseRepository<Page>
{
    Task<List<Page>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
}

/// <summary>
/// Section Repository Interface
/// </summary>
public interface ISectionRepository : IBaseRepository<Section>
{
    // Task<List<Section>> GetByPageAsync(int pageId);
    Task<List<Section>> GetByPageAsync(int pageId);
}

/// <summary>
/// SectionData Repository Interface
/// </summary>
public interface ISectionDataRepository : IBaseRepository<SectionData>
{
    Task<List<SectionData>> GetBySectionAsync(int sectionId);
}



/// <summary>
/// Customer Repository Interface
/// </summary>
