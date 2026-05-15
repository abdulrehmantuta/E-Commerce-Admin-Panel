namespace ECommerceAdminPanel.DTOs.Response;

/// <summary>
/// Tenant Response DTO
/// </summary>
public class TenantResponseDto
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
/// User Response DTO
/// </summary>
public class UserResponseDto
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }
}

/// <summary>
/// Category Response DTO
/// </summary>
public class CategoryResponseDto
{
    public int CategoryId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public bool Status { get; set; }

    public string? ImageUrl { get; set; }

}

/// <summary>
/// Product Response DTO
/// </summary>
public class ProductResponseDto
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
    public List<string> Sizes { get; set; } = new();
    public List<string> Colors { get; set; } = new();
    public string? SKU { get; set; }
    public string? Brand { get; set; }
}

/// <summary>
/// Order Response DTO
/// </summary>
public class OrderResponseDto
{
    public int OrderId { get; set; }
    public int TenantId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public List<OrderDetailResponseDto>? OrderDetails { get; set; }
}

/// <summary>
/// OrderDetail Response DTO
/// </summary>
public class OrderDetailResponseDto
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

/// <summary>
/// Page Response DTO
/// </summary>
public class PageResponseDto
{
    public int PageId { get; set; }
    public int TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Section Response DTO
/// </summary>
public class SectionResponseDto
{
    public int SectionId { get; set; }
    public int PageId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int OrderNo { get; set; }
    public bool Status { get; set; }
}

/// <summary>
/// SectionData Response DTO
/// </summary>
public class SectionDataResponseDto
{
    public int DataId { get; set; }
    public int SectionId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
}

/// <summary>
/// Generic API Response Wrapper
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static ApiResponse<T> ErrorResponse(string message = "Error", int statusCode = 500)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            StatusCode = statusCode
        };
    }
}

/// <summary>
/// Paginated Response
/// </summary>
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// Customer Response DTO
/// </summary>
public class CustomerResponseDto
{
    public int CustomerId { get; set; }
    public int TenantId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
}





public class CustomerLoginResponseDto
{
    public int CustomerId { get; set; }
    public int TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}








/// <summary>
/// TenantSettingsResponseDto DTO
/// </summary>
/// 

public class TenantSettingsResponseDto
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


    public string PromoBannerBg { get; set; } = "#1e1a14";
    public string PromoBannerText { get; set; } = "#f5ede0";
    public string CardBg { get; set; } = "#ffffff";
    public string CardText { get; set; } = "#2a1f14";
    // ✅ Naye
    public string CardStyle { get; set; } = "minimal";
    public string CardRadius { get; set; } = "12px";
    public string FontHeading { get; set; } = "Cormorant Garamond";
    public string FontBody { get; set; } = "Jost";
    public string ButtonRadius { get; set; } = "8px";
    public string ImageAspectRatio { get; set; } = "4/5";
}



/// <summary>
/// TenantSliderResponseDto DTO
/// </summary>
/// 

public class TenantSliderResponseDto
{
    public int SliderId { get; set; }
    public int TenantId { get; set; }
    public string ImageUrl { get; set; } 
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonLink { get; set; }
    public int OrderNo { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }

    // NEW
    public string LayoutType { get; set; } = "full-image";
    public string BgColor { get; set; } = "#1a1a2e";
    public string TextColor { get; set; } = "#ffffff";
    public int OverlayOpacity { get; set; } = 50;
    public bool IsPresetImage { get; set; }  // ← YEH ADD KARO

}




// =============================================
// ✅ NEW — TenantIntegration Response DTOs
// =============================================

/// <summary>
/// TenantIntegration Response DTO
/// </summary>
public class TenantIntegrationResponseDto
{
    public int IntegrationId { get; set; }
    public int TenantId { get; set; }

    // Email
    public bool IsEmailEnabled { get; set; }
    public string? EmailProvider { get; set; }
    public string? EmailApiKey { get; set; }        // Masked (****) from API
    public string? EmailSenderAddress { get; set; }
    public string? EmailSenderName { get; set; }

    // WhatsApp
    public bool IsWhatsAppEnabled { get; set; }
    public string? WhatsAppProvider { get; set; }
    public string? WhatsAppToken { get; set; }      // Masked (****) from API
    public string? WhatsAppPhoneNumberId { get; set; }
    public string? WhatsAppBusinessId { get; set; }

    public DateTime UpdatedDate { get; set; }
}

/// <summary>
/// NotificationLog Response DTO
/// </summary>
public class NotificationLogResponseDto
{
    public int LogId { get; set; }
    public int TenantId { get; set; }
    public int? OrderId { get; set; }
    public string Channel { get; set; } = "";       // "Email" / "WhatsApp"
    public string EventType { get; set; } = "";     // "OrderCreated" etc
    public string? RecipientContact { get; set; }
    public string Status { get; set; } = "";        // "Sent" / "Failed"
    public string? ErrorMessage { get; set; }
    public DateTime SentAt { get; set; }
}
