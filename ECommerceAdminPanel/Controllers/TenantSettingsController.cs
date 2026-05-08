using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.Services.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TenantSettingsController : ControllerBase
{
    private readonly ITenantSettingsService _service;

    public TenantSettingsController(ITenantSettingsService service)
    {
        _service = service;
    }

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> Get(int tenantId)
    {
        var result = await _service.GetByTenantAsync(tenantId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] TenantSettingsRequestDto dto)
    {
        var result = await _service.UpsertAsync(dto);
        return StatusCode(result.StatusCode, result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class TenantSliderController : ControllerBase
{
    private readonly ITenantSliderService _service;

    public TenantSliderController(ITenantSliderService service)
    {
        _service = service;
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<IActionResult> GetActive(int tenantId)
    {
        var result = await _service.GetActiveAsync(tenantId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("admin/{tenantId}")]
    public async Task<IActionResult> GetAll(int tenantId)
    {
        var result = await _service.GetAllAsync(tenantId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] TenantSliderRequestDto dto)
    {
        var result = await _service.AddAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{sliderId}")]
    public async Task<IActionResult> Update(int sliderId, [FromBody] UpdateSliderRequestDto dto)
    {
        var result = await _service.UpdateAsync(sliderId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{sliderId}")]
    public async Task<IActionResult> Delete(int sliderId)
    {
        var result = await _service.DeleteAsync(sliderId);
        return StatusCode(result.StatusCode, result);
    }
}