namespace ECommerceAdminPanel.DTOs.Request;

public class CustomerCreateDto
{
    public int TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string Password { get; set; } = string.Empty;

    public bool Status { get; set; }

}

public class CustomerUpdateDto
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public bool Status { get; set; }
}


//new
public class CustomerRegisterDto
{
    public int TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CustomerLoginDto
{
    public int TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
