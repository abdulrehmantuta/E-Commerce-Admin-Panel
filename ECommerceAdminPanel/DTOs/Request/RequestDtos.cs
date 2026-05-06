namespace ECommerceAdminPanel.DTOs.Request;

/// <summary>
/// Tenant Create/Update Request
/// </summary>
public class TenantRequestDto
{
    public string Name { get; set; } = string.Empty;    
    public string Domain { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? ThemeColor { get; set; }
}

/// <summary>
/// User Create Request
/// </summary>
public class UserCreateRequestDto
{
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
}

/// <summary>
/// User Update Request
/// </summary>
public class UserUpdateRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool Status { get; set; }
}

/// <summary>
/// Category Create/Update Request
/// </summary>
public class CategoryRequestDto
{
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public bool Status { get; set; } = true;

    public IFormFile? Image { get; set; }

}

/// <summary>
/// Product Create Request
/// </summary>
public class ProductCreateRequestDto
{
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }   // ✅ NEW
    public int? CategoryId { get; set; }
    public int StockQty { get; set; }
}

/// <summary>
/// Product Update Request
/// </summary>
public class ProductUpdateRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }   // ✅ NEW
    public int? CategoryId { get; set; }
    public int StockQty { get; set; }
    public bool Status { get; set; }
}

/// <summary>
/// Order Create Request
/// </summary>
public class OrderCreateRequestDto
{
    public int TenantId { get; set; }
    public int? UserId { get; set; }  // ✅ add karo

    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
}

/// <summary>
/// Order Update Request
/// </summary>
public class OrderUpdateRequestDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// OrderDetail Create/Update Request
/// </summary>
public class OrderDetailRequestDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

/// <summary>
/// Page Create/Update Request
/// </summary>
public class PageRequestDto
{
    public int TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
}

/// <summary>
/// Section Create/Update Request
/// </summary>
public class SectionRequestDto
{
    public int PageId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int OrderNo { get; set; }
    public bool Status { get; set; } = true;
}

/// <summary>
/// SectionData Create/Update Request
/// </summary>
public class SectionDataRequestDto
{
    public int SectionId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
}
