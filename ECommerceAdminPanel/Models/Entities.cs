namespace ECommerceAdminPanel.Models;

/// <summary>
/// Tenant entity - represents a tenant/company in multi-tenant system
/// </summary>
public class Tenant
{
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? ThemeColor { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }
}

/// <summary>
/// User entity
/// </summary>
public class User
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }
}

/// <summary>
/// Category entity for products
/// </summary>
public class Category
{
    public int CategoryId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public bool Status { get; set; }

    public string? ImageUrl { get; set; }

}

/// <summary>
/// Product entity
/// </summary>
public class Product
{
    public int ProductId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public int StockQty { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Order entity
/// </summary>
public class Order
{
    public int OrderId { get; set; }
    public int TenantId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// OrderDetail entity - line items in an order
/// </summary>
public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

/// <summary>
/// Page entity for CMS functionality
/// </summary>
public class Page
{
    public int PageId { get; set; }
    public int TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Section entity - sections within a page
/// </summary>
public class Section
{
    public int SectionId { get; set; }
    public int PageId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int OrderNo { get; set; }
    public bool Status { get; set; }
}

/// <summary>
/// SectionData entity - key-value data for sections
/// </summary>
public class SectionData
{
    public int DataId { get; set; }
    public int SectionId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
}



/// <summary>
/// Customer entity - represents customer table in database
/// </summary>
public class Customer
{
    public int CustomerId { get; set; }
    public int TenantId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string Password { get; set; } = string.Empty;

    public bool Status { get; set; } = true;
    public DateTime CreatedDate { get; set; }
}