namespace ECommerceAdminPanel.Enums;

/// <summary>
/// Entity status enumeration (Active/Inactive)
/// </summary>
public enum EntityStatus
{
    Inactive = 0,
    Active = 1
}

/// <summary>
/// User roles in the system
/// </summary>
public enum UserRole
{
    User,
    Admin,
    Manager,
    Superadmin
}

/// <summary>
/// Order status enumeration
/// </summary>
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned,
    Failed
}

/// <summary>
/// HTTP Response status codes
/// </summary>
public enum ResponseStatus
{
    Success = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500
}
