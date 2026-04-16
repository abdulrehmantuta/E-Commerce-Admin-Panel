using Microsoft.AspNetCore.Mvc;
using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Services.IServices;

namespace ECommerceAdminPanel.Controllers;

/// <summary>
/// Category Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService service, ILogger<CategoryController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> CreateCategory([FromBody] CategoryRequestDto request)
    {
        _logger.LogInformation("Creating category: {CategoryName}", request.Name);
        var result = await _service.CreateCategoryAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> GetCategoryById(int id)
    {
        _logger.LogInformation("Getting category with ID: {CategoryId}", id);
        var result = await _service.GetCategoryByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<CategoryResponseDto>>>> GetCategoriesByTenant(
        int tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting categories for tenant: {TenantId}", tenantId);
        var result = await _service.GetCategoriesByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateCategory(int id, [FromBody] CategoryRequestDto request)
    {
        _logger.LogInformation("Updating category with ID: {CategoryId}", id);
        var result = await _service.UpdateCategoryAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
    {
        _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
        var result = await _service.DeleteCategoryAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// Tenant Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly ITenantService _service;
    private readonly ILogger<TenantController> _logger;

    public TenantController(ITenantService service, ILogger<TenantController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<TenantResponseDto>>> CreateTenant([FromBody] TenantRequestDto request)
    {
        _logger.LogInformation("Creating tenant: {TenantName}", request.Name);
        var result = await _service.CreateTenantAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TenantResponseDto>>> GetTenantById(int id)
    {
        _logger.LogInformation("Getting tenant with ID: {TenantId}", id);
        var result = await _service.GetTenantByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<TenantResponseDto>>>> GetAllTenants(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all tenants");
        var result = await _service.GetAllTenantsAsync(pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateTenant(int id, [FromBody] TenantRequestDto request)
    {
        _logger.LogInformation("Updating tenant with ID: {TenantId}", id);
        var result = await _service.UpdateTenantAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteTenant(int id)
    {
        _logger.LogInformation("Deleting tenant with ID: {TenantId}", id);
        var result = await _service.DeleteTenantAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// User Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService service, ILogger<UserController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> CreateUser([FromBody] UserCreateRequestDto request)
    {
        _logger.LogInformation("Creating user: {UserEmail}", request.Email);
        var result = await _service.CreateUserAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUserById(int id)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);
        var result = await _service.GetUserByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<UserResponseDto>>>> GetUsersByTenant(
        int tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting users for tenant: {TenantId}", tenantId);
        var result = await _service.GetUsersByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(int id, [FromBody] UserUpdateRequestDto request)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);
        var result = await _service.UpdateUserAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);
        var result = await _service.DeleteUserAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// Order Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService service, ILogger<OrderController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> CreateOrder([FromBody] OrderCreateRequestDto request)
    {
        _logger.LogInformation("Creating order for customer: {CustomerName}", request.CustomerName);
        var result = await _service.CreateOrderAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> GetOrderById(int id)
    {
        _logger.LogInformation("Getting order with ID: {OrderId}", id);
        var result = await _service.GetOrderByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<OrderResponseDto>>>> GetOrdersByTenant(
        int tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting orders for tenant: {TenantId}", tenantId);
        var result = await _service.GetOrdersByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateOrder(int id, [FromBody] OrderUpdateRequestDto request)
    {
        _logger.LogInformation("Updating order with ID: {OrderId}", id);
        var result = await _service.UpdateOrderAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteOrder(int id)
    {
        _logger.LogInformation("Deleting order with ID: {OrderId}", id);
        var result = await _service.DeleteOrderAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("detail/add")]
    public async Task<ActionResult<ApiResponse<OrderDetailResponseDto>>> AddOrderDetail([FromBody] OrderDetailRequestDto request)
    {
        _logger.LogInformation("Adding detail to order: {OrderId}", request.OrderId);
        var result = await _service.AddOrderDetailAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{orderId}/details")]
    public async Task<ActionResult<ApiResponse<List<OrderDetailResponseDto>>>> GetOrderDetails(int orderId)
    {
        _logger.LogInformation("Getting details for order: {OrderId}", orderId);
        var result = await _service.GetOrderDetailsAsync(orderId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("detail/{detailId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteOrderDetail(int detailId)
    {
        _logger.LogInformation("Deleting order detail with ID: {DetailId}", detailId);
        var result = await _service.DeleteOrderDetailAsync(detailId);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// Page Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PageController : ControllerBase
{
    private readonly IPageService _service;
    private readonly ILogger<PageController> _logger;

    public PageController(IPageService service, ILogger<PageController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<PageResponseDto>>> CreatePage([FromBody] PageRequestDto request)
    {
        _logger.LogInformation("Creating page: {PageTitle}", request.Title);
        var result = await _service.CreatePageAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PageResponseDto>>> GetPageById(int id)
    {
        _logger.LogInformation("Getting page with ID: {PageId}", id);
        var result = await _service.GetPageByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<PageResponseDto>>>> GetPagesByTenant(
        int tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting pages for tenant: {TenantId}", tenantId);
        var result = await _service.GetPagesByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdatePage(int id, [FromBody] PageRequestDto request)
    {
        _logger.LogInformation("Updating page with ID: {PageId}", id);
        var result = await _service.UpdatePageAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePage(int id)
    {
        _logger.LogInformation("Deleting page with ID: {PageId}", id);
        var result = await _service.DeletePageAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// Section Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ISectionService _service;
    private readonly ILogger<SectionController> _logger;

    public SectionController(ISectionService service, ILogger<SectionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<SectionResponseDto>>> CreateSection([FromBody] SectionRequestDto request)
    {
        _logger.LogInformation("Creating section for page: {PageId}", request.PageId);
        var result = await _service.CreateSectionAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SectionResponseDto>>> GetSectionById(int id)
    {
        _logger.LogInformation("Getting section with ID: {SectionId}", id);
        var result = await _service.GetSectionByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("page/{pageId}")]
    public async Task<ActionResult<ApiResponse<List<SectionResponseDto>>>> GetSectionsByPage(int pageId)
    {
        _logger.LogInformation("Getting sections for page: {PageId}", pageId);
        var result = await _service.GetSectionsByPageAsync(pageId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateSection(int id, [FromBody] SectionRequestDto request)
    {
        _logger.LogInformation("Updating section with ID: {SectionId}", id);
        var result = await _service.UpdateSectionAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSection(int id)
    {
        _logger.LogInformation("Deleting section with ID: {SectionId}", id);
        var result = await _service.DeleteSectionAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// SectionData Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SectionDataController : ControllerBase
{
    private readonly ISectionDataService _service;
    private readonly ILogger<SectionDataController> _logger;

    public SectionDataController(ISectionDataService service, ILogger<SectionDataController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<SectionDataResponseDto>>> CreateSectionData([FromBody] SectionDataRequestDto request)
    {
        _logger.LogInformation("Creating section data for section: {SectionId}", request.SectionId);
        var result = await _service.CreateSectionDataAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SectionDataResponseDto>>> GetSectionDataById(int id)
    {
        _logger.LogInformation("Getting section data with ID: {DataId}", id);
        var result = await _service.GetSectionDataByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("section/{sectionId}")]
    public async Task<ActionResult<ApiResponse<List<SectionDataResponseDto>>>> GetSectionDataBySection(int sectionId)
    {
        _logger.LogInformation("Getting data for section: {SectionId}", sectionId);
        var result = await _service.GetSectionDataBySectionAsync(sectionId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateSectionData(int id, [FromBody] SectionDataRequestDto request)
    {
        _logger.LogInformation("Updating section data with ID: {DataId}", id);
        var result = await _service.UpdateSectionDataAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSectionData(int id)
    {
        _logger.LogInformation("Deleting section data with ID: {DataId}", id);
        var result = await _service.DeleteSectionDataAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}


/// <summary>
/// Customer Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ICustomerService service, ILogger<CustomerController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> CreateCustomer(
        [FromBody] CustomerCreateDto request)
    {
        if (request == null)
            return BadRequest("Request cannot be null");

        if (string.IsNullOrWhiteSpace(request.FirstName))
            return BadRequest("FirstName is required");

        _logger.LogInformation(
            "Creating customer: {FirstName} {LastName}",
            request.FirstName,
            request.LastName
        );

        var result = await _service.CreateCustomerAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> GetCustomerById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid Customer Id");

        _logger.LogInformation("Getting customer with ID: {CustomerId}", id);

        var result = await _service.GetCustomerByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<CustomerResponseDto>>>> GetCustomersByTenant(
        int tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (tenantId <= 0)
            return BadRequest("Invalid Tenant Id");

        _logger.LogInformation("Getting customers for tenant: {TenantId}", tenantId);

        var result = await _service.GetCustomersByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateCustomer(
        int id,
        [FromBody] CustomerUpdateDto request)
    {
        if (id <= 0)
            return BadRequest("Invalid Customer Id");

        if (request == null)
            return BadRequest("Request cannot be null");

        _logger.LogInformation("Updating customer with ID: {CustomerId}", id);

        var result = await _service.UpdateCustomerAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid Customer Id");

        _logger.LogInformation("Deleting customer with ID: {CustomerId}", id);

        var result = await _service.DeleteCustomerAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}