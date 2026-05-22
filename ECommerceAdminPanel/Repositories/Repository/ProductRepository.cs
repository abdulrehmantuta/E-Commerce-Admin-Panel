namespace ECommerceAdminPanel.Repositories.Repository;

using Dapper;
using ECommerceAdminPanel.Helper;
using ECommerceAdminPanel.Models;
using ECommerceAdminPanel.Repositories.IRepository;
using global::ECommerceAdminPanel.Models;
using global::ECommerceAdminPanel.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;

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







/// <summary>
/// ProductImageRepository
/// </summary>
public class ProductImageRepository : IProductImageRepository
{
    private readonly string _connectionString;

    public ProductImageRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    // Sari images by productId
    public async Task<List<ProductImage>> GetByProductAsync(int productId)
    {
        var images = new List<ProductImage>();

        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_ProductImage_GetByProduct", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductId", productId);

        await con.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            images.Add(new ProductImage
            {
                ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                ColorName = reader.IsDBNull(reader.GetOrdinal("ColorName"))
                              ? null
                              : reader.GetString(reader.GetOrdinal("ColorName")),
                IsPrimary = reader.GetBoolean(reader.GetOrdinal("IsPrimary")),
                OrderNo = reader.GetInt32(reader.GetOrdinal("OrderNo")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            });
        }

        return images;
    }

    // Image add karo
    public async Task<int> AddImageAsync(ProductImage image)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_ProductImage_Add", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductId", image.ProductId);
        cmd.Parameters.AddWithValue("@ImageUrl", image.ImageUrl);
        cmd.Parameters.AddWithValue("@ColorName", (object?)image.ColorName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsPrimary", image.IsPrimary);
        cmd.Parameters.AddWithValue("@OrderNo", image.OrderNo);

        await con.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    // Image delete karo
    public async Task<bool> DeleteImageAsync(int imageId)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_ProductImage_Delete", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ImageId", imageId);

        await con.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    // Primary image set karo
    public async Task<bool> SetPrimaryAsync(int imageId, int productId)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_ProductImage_SetPrimary", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ImageId", imageId);
        cmd.Parameters.AddWithValue("@ProductId", productId);

        await con.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}
