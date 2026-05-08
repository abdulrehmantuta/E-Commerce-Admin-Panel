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
    public int? UserId { get; set; }  // ✅ add karo

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




/// <summary>
/// TenantSettings entity - represents customer table in database
/// </summary>


public class TenantSettings
{
    public int SettingId { get; set; }
    public int TenantId { get; set; }
    public string? StoreName { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string PrimaryColor { get; set; } = "#ea6c2d";
    public string SecondaryColor { get; set; } = "#1a1a2e";
    public string AccentColor { get; set; } = "#ffffff";
    public string BackgroundColor { get; set; } = "#ffffff";
    public string TextColor { get; set; } = "#1a1a1a";
    public string NavbarBgColor { get; set; } = "#ffffff";
    public string NavbarTextColor { get; set; } = "#1a1a1a";
    public string FooterBgColor { get; set; } = "#0f172a";
    public string FooterTextColor { get; set; } = "#ffffff";
    public string ButtonColor { get; set; } = "#ea6c2d";
    public string ButtonTextColor { get; set; } = "#ffffff";
    public string FontFamily { get; set; } = "Poppins";
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? FooterTagline { get; set; }
    public string HeroBgColor { get; set; } = "#ffffff"; // ✅ naya

    public DateTime UpdatedDate { get; set; }
}



/// <summary>
/// TenantSlider entity - represents customer table in database
/// </summary>




public class TenantSlider
{
    public int SliderId { get; set; }
    public int TenantId { get; set; }
    public string ImageUrl { get; set; } = "";
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonLink { get; set; }
    public int OrderNo { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }

    // NEW
    public string LayoutType { get; set; } = "full-image";
    public string BgColor { get; set; } = "#1a1a2e";
    public string TextColor { get; set; } = "#ffffff";
    public int OverlayOpacity { get; set; } = 50;

}