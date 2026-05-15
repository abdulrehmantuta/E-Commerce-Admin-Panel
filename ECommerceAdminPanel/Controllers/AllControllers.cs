using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

    // [HttpPost("create")]
    // public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> CreateCategory([FromBody] CategoryRequestDto request)
    // {
    //     _logger.LogInformation("Creating category: {CategoryName}", request.Name);
    //     var result = await _service.CreateCategoryAsync(request);
    //     return StatusCode(result.StatusCode, result);
    // }

    [HttpPost("create")]
public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> CreateCategory([FromForm] CategoryRequestDto request)
{
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

    // [HttpPut("{id}")]
    // public async Task<ActionResult<ApiResponse<bool>>> UpdateCategory(int id, [FromBody] CategoryRequestDto request)
    // {
    //     _logger.LogInformation("Updating category with ID: {CategoryId}", id);
    //     var result = await _service.UpdateCategoryAsync(id, request);
    //     return StatusCode(result.StatusCode, result);
    // }

[HttpPut("{id}")]
public async Task<ActionResult<ApiResponse<bool>>> UpdateCategory(int id, [FromForm] CategoryRequestDto request)
{
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

    [HttpGet("resolve")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<TenantResponseDto>>> ResolveTenant([FromQuery] string domain)
    {
        _logger.LogInformation("Resolving tenant for domain: {Domain}", domain);
        var result = await _service.GetTenantByDomainAsync(domain);
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


    //new

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CustomerLoginResponseDto>>> Register(
    [FromBody] CustomerRegisterDto request)
    {
        _logger.LogInformation("Customer register attempt: {Email}", request.Email);
        var result = await _service.RegisterAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CustomerLoginResponseDto>>> Login(
        [FromBody] CustomerLoginDto request)
    {
        _logger.LogInformation("Customer login attempt: {Email}", request.Email);
        var result = await _service.LoginAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("my-orders")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<List<OrderResponseDto>>>> GetMyOrders()
    {
        var customerIdClaim = User.FindFirst("CustomerId")?.Value;
        var tenantIdClaim = User.FindFirst("TenantId")?.Value;

        if (string.IsNullOrEmpty(customerIdClaim) || string.IsNullOrEmpty(tenantIdClaim))
            return Unauthorized("Invalid token");

        var customerId = int.Parse(customerIdClaim);
        var tenantId = int.Parse(tenantIdClaim);

        var result = await _service.GetMyOrdersAsync(customerId, tenantId);
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






// =============================================
// NAYE CONTROLLERS — SIRF YE ADD HUE HAIN
// =============================================

/// <summary>
/// Tenant Integration Settings Controller
/// Admin panel se Email + WhatsApp settings save/get karne ke liye
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantIntegrationsController : ControllerBase
{
    private readonly ITenantIntegrationService _service;
    private readonly ILogger<TenantIntegrationsController> _logger;

    public TenantIntegrationsController(ITenantIntegrationService service, ILogger<TenantIntegrationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Tenant ki integration settings fetch karo
    /// GET api/TenantIntegrations/{tenantId}
    /// </summary>
    [HttpGet("{tenantId}")]
    public async Task<ActionResult<ApiResponse<TenantIntegrationResponseDto>>> GetByTenant(int tenantId)
    {
        _logger.LogInformation("Getting integrations for tenant: {TenantId}", tenantId);
        var result = await _service.GetByTenantAsync(tenantId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Tenant ki integration settings save/update karo (Upsert)
    /// POST api/TenantIntegrations/save
    /// </summary>
    [HttpPost("save")]
    public async Task<ActionResult<ApiResponse<bool>>> Save([FromBody] TenantIntegrationRequestDto request)
    {
        _logger.LogInformation("Saving integrations for tenant: {TenantId}", request.TenantId);
        var result = await _service.UpsertAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Tenant ki integration settings delete karo
    /// DELETE api/TenantIntegrations/{tenantId}
    /// </summary>
    [HttpDelete("{tenantId}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int tenantId)
    {
        _logger.LogInformation("Deleting integrations for tenant: {TenantId}", tenantId);
        var result = await _service.DeleteAsync(tenantId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Test Email bhejo (Integration verify karne ke liye)
    /// POST api/TenantIntegrations/test-email/{tenantId}
    /// </summary>
    [HttpPost("test-email/{tenantId}")]
    public async Task<ActionResult<ApiResponse<bool>>> TestEmail(int tenantId, [FromQuery] string toEmail)
    {
        _logger.LogInformation("Testing email for tenant: {TenantId}", tenantId);
        var result = await _service.TestEmailAsync(tenantId, toEmail);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Test WhatsApp bhejo (Integration verify karne ke liye)
    /// POST api/TenantIntegrations/test-whatsapp/{tenantId}
    /// </summary>
    [HttpPost("test-whatsapp/{tenantId}")]
    public async Task<ActionResult<ApiResponse<bool>>> TestWhatsApp(int tenantId, [FromQuery] string toPhone)
    {
        _logger.LogInformation("Testing WhatsApp for tenant: {TenantId}", tenantId);
        var result = await _service.TestWhatsAppAsync(tenantId, toPhone);
        return StatusCode(result.StatusCode, result);
    }
}

/// <summary>
/// Notification Logs Controller
/// Sent/Failed notifications ki history dekhne ke liye
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationLogsController : ControllerBase
{
    private readonly INotificationLogService _service;
    private readonly ILogger<NotificationLogsController> _logger;

    public NotificationLogsController(INotificationLogService service, ILogger<NotificationLogsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Tenant ki saari notification history
    /// GET api/NotificationLogs/tenant/{tenantId}
    /// </summary>
    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<List<NotificationLogResponseDto>>>> GetByTenant(
        int tenantId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("Getting notification logs for tenant: {TenantId}", tenantId);
        var result = await _service.GetByTenantAsync(tenantId, page, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Specific order ki notifications
    /// GET api/NotificationLogs/order/{orderId}?tenantId=1
    /// </summary>
    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<ApiResponse<List<NotificationLogResponseDto>>>> GetByOrder(
        int orderId,
        [FromQuery] int tenantId)
    {
        _logger.LogInformation("Getting notification logs for order: {OrderId}", orderId);
        var result = await _service.GetByOrderAsync(tenantId, orderId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Sirf failed notifications
    /// GET api/NotificationLogs/failed/{tenantId}
    /// </summary>
    [HttpGet("failed/{tenantId}")]
    public async Task<ActionResult<ApiResponse<List<NotificationLogResponseDto>>>> GetFailed(int tenantId)
    {
        _logger.LogInformation("Getting failed notification logs for tenant: {TenantId}", tenantId);
        var result = await _service.GetFailedAsync(tenantId);
        return StatusCode(result.StatusCode, result);
    }
}














/// <summary>
//WhatsAppWebhookController
/// </summary>


[ApiController]
[Route("api/[controller]")]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly INotificationService _notificationService;
    private readonly ILogger<WhatsAppWebhookController> _logger;

    private const string WEBHOOK_VERIFY_TOKEN = "YOUR_VERIFY_TOKEN_HERE";

    public WhatsAppWebhookController(
        IOrderService service,
        INotificationService notificationService,
        ILogger<WhatsAppWebhookController> logger)
    {
        _service = service;
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Verify(
        [FromQuery(Name = "hub.mode")] string? hubMode,
        [FromQuery(Name = "hub.verify_token")] string? hubVerifyToken,
        [FromQuery(Name = "hub.challenge")] string? hubChallenge)
    {
        if (hubMode == "subscribe" && hubVerifyToken == WEBHOOK_VERIFY_TOKEN)
            return Ok(hubChallenge);

        return Forbid();
    }

    [HttpPost]
    public async Task<IActionResult> Receive([FromBody] JsonElement body)
    {
        try
        {
            var entry = body
                .GetProperty("entry")[0]
                .GetProperty("changes")[0]
                .GetProperty("value");

            if (!entry.TryGetProperty("messages", out var messages)) return Ok();

            var message = messages[0];
            if (message.GetProperty("type").GetString() == "interactive")
            {
                var interactive = message.GetProperty("interactive");
                if (interactive.GetProperty("type").GetString() == "button_reply")
                {
                    var buttonId = interactive.GetProperty("button_reply").GetProperty("id").GetString();
                    var fromPhone = message.GetProperty("from").GetString();
                    await HandleButtonReply(buttonId!, fromPhone!);
                }
            }
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Webhook error");
            return Ok();
        }
    }

    private async Task HandleButtonReply(string buttonId, string fromPhone)
    {
        if (buttonId.StartsWith("CONFIRM_ORDER_"))
        {
            var orderId = int.Parse(buttonId.Replace("CONFIRM_ORDER_", ""));
            await ProcessOrderAction(orderId, "Confirmed");
        }
        else if (buttonId.StartsWith("CANCEL_ORDER_"))
        {
            var orderId = int.Parse(buttonId.Replace("CANCEL_ORDER_", ""));
            await ProcessOrderAction(orderId, "Cancelled");
        }
    }

    private async Task ProcessOrderAction(int orderId, string newStatus)
    {
        // ✅ Aapka existing GetOrderById use kar rahe hain
        var response = await _service.GetOrderByIdAsync(orderId);
        if (!response.Success || response.Data == null) return;

        var oldStatus = response.Data.Status;

        // ✅ Aapka existing UpdateOrder use kar rahe hain
        await _service.UpdateOrderAsync(orderId, new OrderUpdateRequestDto
        {
            Status = newStatus
        });

        _logger.LogInformation("✅ Order {OrderId} → {Status}", orderId, newStatus);
    }
}