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
    // [HttpPost("create")]
    // public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct([FromBody] ProductCreateRequestDto request)
    // {
    //     _logger.LogInformation("Creating product: {ProductName}", request.Name);
    //     var result = await _service.CreateProductAsync(request);
    //     return StatusCode(result.StatusCode, result);
    // }

    [HttpPost("create")]
public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct([FromForm] ProductCreateRequestDto request)
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





/// <summary>
/// ProductImageController
/// /// </summary>

[ApiController]
[Route("api/[controller]")]
public class ProductImageController : ControllerBase
{
    private readonly IProductImageService _service;
    private readonly ILogger<ProductImageController> _logger;

    public ProductImageController(
        IProductImageService service,
        ILogger<ProductImageController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: api/productimage/5
    // Customer detail page → sari images lao
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetImages(int productId)
    {
        _logger.LogInformation("Getting images for product: {ProductId}", productId);
        var result = await _service.GetImagesByProductAsync(productId);
        return StatusCode(result.StatusCode, result);
    }

    // POST: api/productimage/add
    // Admin → image upload karo
    [HttpPost("add")]
    public async Task<IActionResult> AddImage([FromForm] ProductImageAddDto request)
    {
        _logger.LogInformation("Adding image for product: {ProductId}", request.ProductId);
        var result = await _service.AddImageAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    // DELETE: api/productimage/8
    // Admin → image delete karo
    [HttpDelete("{imageId}")]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        _logger.LogInformation("Deleting image: {ImageId}", imageId);
        var result = await _service.DeleteImageAsync(imageId);
        return StatusCode(result.StatusCode, result);
    }

    // PUT: api/productimage/set-primary
    // Admin → primary image set karo
    [HttpPut("set-primary")]
    public async Task<IActionResult> SetPrimary([FromBody] SetPrimaryImageDto request)
    {
        _logger.LogInformation("Setting primary: {ImageId}", request.ImageId);
        var result = await _service.SetPrimaryAsync(request.ImageId, request.ProductId);
        return StatusCode(result.StatusCode, result);
    }
}