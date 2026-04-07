using Microsoft.AspNetCore.Mvc;
using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Services.IServices;

namespace ECommerceAdminPanel.Controllers;

/// <summary>
/// Product Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService service, ILogger<ProductController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct([FromBody] ProductCreateRequestDto request)
    {
        _logger.LogInformation("Creating product: {ProductName}", request.Name);
        var result = await _service.CreateProductAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductById(int id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        var result = await _service.GetProductByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get all products for a tenant with pagination
    /// </summary>
    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponseDto>>>> GetProductsByTenant(
        int tenantId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting products for tenant: {TenantId}", tenantId);
        var result = await _service.GetProductsByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponseDto>>>> GetProductsByCategory(
        int categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting products for category: {CategoryId}", categoryId);
        var result = await _service.GetProductsByCategoryAsync(categoryId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateProduct(int id, [FromBody] ProductUpdateRequestDto request)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        var result = await _service.UpdateProductAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Delete product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        var result = await _service.DeleteProductAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
