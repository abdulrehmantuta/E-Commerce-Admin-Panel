namespace ECommerceAdminPanel.Repositories.Repository;

using Dapper;
using ECommerceAdminPanel.Helper;
using ECommerceAdminPanel.Models;
using ECommerceAdminPanel.Repositories.IRepository;

/// <summary>
/// Product Repository Implementation using Dapper
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DapperHelper _dapperHelper;

    public ProductRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    public async Task<int> CreateAsync(Product entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Description", entity.Description);
        parameters.Add("@Price", entity.Price);
        parameters.Add("@ImageUrl", entity.ImageUrl);
        parameters.Add("@CategoryId", entity.CategoryId);
        parameters.Add("@StockQty", entity.StockQty);
        parameters.Add("@Sizes", entity.Sizes);
        parameters.Add("@Colors", entity.Colors);
        parameters.Add("@SKU", entity.SKU);
        parameters.Add("@Brand", entity.Brand);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Product_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    public async Task<Product?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ProductId", id);

        return await _dapperHelper.QuerySingleOrDefaultAsync<Product>("sp_Product_GetById", parameters);
    }

    /// <summary>
    /// Get all products with pagination
    /// </summary>
    public async Task<List<Product>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);

        return await _dapperHelper.QueryAsync<Product>("sp_Product_GetAll", parameters);
    }

    /// <summary>
    /// Get products by tenant
    /// </summary>
    public async Task<List<Product>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);

        return await _dapperHelper.QueryAsync<Product>("sp_Product_GetByTenant", parameters);
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    public async Task<List<Product>> GetByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CategoryId", categoryId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);

        return await _dapperHelper.QueryAsync<Product>("sp_Product_GetByCategory", parameters);
    }

    /// <summary>
    /// Update product
    /// </summary>
    public async Task<int> UpdateAsync(int id, Product entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ProductId", id);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Description", entity.Description);
        parameters.Add("@Price", entity.Price);
        parameters.Add("@ImageUrl", entity.ImageUrl);
        parameters.Add("@CategoryId", entity.CategoryId);
        parameters.Add("@StockQty", entity.StockQty);
        parameters.Add("@Status", entity.Status);
        parameters.Add("@Sizes", entity.Sizes);
        parameters.Add("@Colors", entity.Colors);
        parameters.Add("@SKU", entity.SKU);
        parameters.Add("@Brand", entity.Brand);

        return await _dapperHelper.ExecuteAsync("sp_Product_Update", parameters);
    }

    /// <summary>
    /// Delete product
    /// </summary>
    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ProductId", id);

        return await _dapperHelper.ExecuteAsync("sp_Product_Delete", parameters);
    }
}
