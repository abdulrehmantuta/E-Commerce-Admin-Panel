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

    public string? Sizes { get; set; }
    public string? Colors { get; set; }
    public string? SKU { get; set; }
    public string? Brand { get; set; }
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


    // Existing fields ke baad add karo:
    public string PromoBannerBg { get; set; } = "#1e1a14";
    public string PromoBannerText { get; set; } = "#f5ede0";
    public string CardBg { get; set; } = "#ffffff";
    public string CardText { get; set; } = "#2a1f14";
    // ✅ Naye card style fields
    public string CardStyle { get; set; } = "minimal";
    public string CardRadius { get; set; } = "12px";
    public string FontHeading { get; set; } = "Cormorant Garamond";
    public string FontBody { get; set; } = "Jost";
    public string ButtonRadius { get; set; } = "8px";
    public string ImageAspectRatio { get; set; } = "4/5";
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
    public bool IsPresetImage { get; set; } = false;  // ← NEW


}



/// <summary>
/// TenantIntegration entity - represents customer table in database
/// </summary>


public class TenantIntegration
    {
        public int IntegrationId { get; set; }
        public int TenantId { get; set; }

        // Email
        public bool IsEmailEnabled { get; set; }
        public string? EmailProvider { get; set; }       // "SendGrid" / "SMTP"
        public string? EmailApiKey { get; set; }
        public string? EmailSenderAddress { get; set; }
        public string? EmailSenderName { get; set; }

        // WhatsApp
        public bool IsWhatsAppEnabled { get; set; }
        public string? WhatsAppProvider { get; set; }    // "Meta" / "Twilio"
        public string? WhatsAppToken { get; set; }
        public string? WhatsAppPhoneNumberId { get; set; }
        public string? WhatsAppBusinessId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }




// =============================================
// YE APNI Models.cs FILE MEIN ADD KARO
// Existing code ke NEECHE paste karo
// =============================================

/// <summary>
/// NotificationLog entity - tracks all sent notifications
/// </summary>
public class NotificationLog
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

/// <summary>
/// Notification Event Type Constants
/// </summary>
public static class NotificationEvents
{
    public const string OrderCreated = "OrderCreated";
    public const string OrderConfirmed = "OrderConfirmed";
    public const string OrderCancelled = "OrderCancelled";
    public const string OrderShipped = "OrderShipped";
    public const string PaymentReceived = "PaymentReceived";
}

/// <summary>
/// Notification Channel Constants
/// </summary>
public static class NotificationChannels
{
    public const string Email = "Email";
    public const string WhatsApp = "WhatsApp";
}

/// <summary>
/// Notification Status Constants
/// </summary>
public static class NotificationStatus
{
    public const string Sent = "Sent";
    public const string Failed = "Failed";
    public const string Pending = "Pending";
}