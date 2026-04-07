# Configuration & Setup Templates

## 1. appsettings.json - Different Environments

### Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=db47144;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "ECommerceAdminPanel": "Debug"
    }
  },
  "Jwt": {
    "Key": "your-super-secret-key-here-minimum-32-characters-required",
    "Issuer": "localhost:5000",
    "Audience": "localhost:3000",
    "ExpiryMinutes": 60
  },
  "AllowedHosts": "*",
  "CorsOrigins": ["http://localhost:3000", "http://localhost:3001"]
}
```

### Production
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=db47144;User Id=prod_user;Password=SecurePassword!@#;Encrypt=true;TrustServerCertificate=False;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "Jwt": {
    "Key": "your-production-secret-key-here-minimum-32-characters-required",
    "Issuer": "yourdomain.com",
    "Audience": "yourdomain.com",
    "ExpiryMinutes": 120
  },
  "AllowedHosts": "yourdomain.com",
  "CorsOrigins": ["https://yourdomain.com", "https://www.yourdomain.com"]
}
```

## 2. Program.cs - Extended Configuration Examples

### With Authentication
```csharp
// Add Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]!)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin", "Superadmin"));
    
    options.AddPolicy("Manager", policy =>
        policy.RequireRole("Manager", "Admin", "Superadmin"));
});
```

### With Serilog Logging
```csharp
// Add Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", 
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", "ECommerceAdminPanel"));
```

### With FluentValidation
```csharp
// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// In pipeline
app.UseMiddleware<ValidationExceptionMiddleware>();
```

### With Rate Limiting
```csharp
// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
    options
        .AddFixedWindowLimiter(policyName: "fixed", configure: o =>
        {
            o.PermitLimit = 100;
            o.Window = TimeSpan.FromSeconds(60);
        })
        .AddSlidingWindowLimiter(policyName: "sliding", configure: o =>
        {
            o.PermitLimit = 50;
            o.Window = TimeSpan.FromSeconds(60);
            o.SegmentsPerWindow = 2;
        }));

// In pipeline
app.UseRateLimiter();

// On specific endpoints
[Authorize]
[RequireRateLimiting("fixed")]
[HttpGet("products")]
public async Task<IActionResult> GetProducts() { ... }
```

### With Caching
```csharp
// Add Caching
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
```

## 3. Middleware Examples

### Custom Error Handling Middleware
```csharp
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred",
                StatusCode = 500
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
```

### Validation Exception Middleware
```csharp
public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = ex.Errors.GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(v => v.ErrorMessage).ToArray()),
                StatusCode = 400
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
```

## 4. FluentValidation Examples

### Product Validator
```csharp
public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequestDto>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(150).WithMessage("Product name cannot exceed 150 characters");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("Tenant ID is required")
            .GreaterThan(0).WithMessage("Invalid Tenant ID");

        RuleFor(x => x.StockQty)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");
    }
}

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequestDto>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(150).WithMessage("Product name cannot exceed 150 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.StockQty)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");
    }
}
```

## 5. AutoMapper Advanced Configuration

### Complex Mapping with Custom Resolvers
```csharp
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Simple mappings
        CreateMap<Product, ProductResponseDto>()
            .ReverseMap()
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

        // Complex mappings with custom logic
        CreateMap<Product, ProductDetailResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => 
                opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.FormattedPrice, opt => 
                opt.MapFrom(src => $"${src.Price:F2}"));

        // Nested mappings
        CreateMap<Order, OrderDetailedResponseDto>()
            .ForMember(dest => dest.Items, opt => 
                opt.MapFrom(src => src.OrderDetails))
            .ForMember(dest => dest.ItemCount, opt => 
                opt.MapFrom(src => src.OrderDetails.Count));
    }
}
```

## 6. Dependency Injection Advanced Setup

### Factory Pattern for Repositories
```csharp
// Register repositories dynamically
builder.Services.AddScoped<IProductRepository>(provider => 
    new ProductRepository(provider.GetRequiredService<DapperHelper>()));

// Or using factory
builder.Services.AddScoped<IRepositoryFactory>(provider =>
    new RepositoryFactory(provider.GetRequiredService<DapperHelper>()));

public interface IRepositoryFactory
{
    IProductRepository CreateProductRepository();
    ICategoryRepository CreateCategoryRepository();
}

public class RepositoryFactory : IRepositoryFactory
{
    private readonly DapperHelper _dapperHelper;

    public RepositoryFactory(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public IProductRepository CreateProductRepository() => 
        new ProductRepository(_dapperHelper);

    public ICategoryRepository CreateCategoryRepository() => 
        new CategoryRepository(_dapperHelper);
}
```

## 7. Response Caching

### Add Caching Layer to Services
```csharp
public class CachedProductService : IProductService
{
    private readonly IProductService _innerService;
    private readonly IMemoryCache _cache;
    private const string CachePrefix = "product_";
    private const int CacheDurationMinutes = 30;

    public CachedProductService(IProductService innerService, IMemoryCache cache)
    {
        _innerService = innerService;
        _cache = cache;
    }

    public async Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int productId)
    {
        var cacheKey = $"{CachePrefix}{productId}";

        if (_cache.TryGetValue(cacheKey, out ApiResponse<ProductResponseDto> cachedResult))
        {
            return cachedResult;
        }

        var result = await _innerService.GetProductByIdAsync(productId);

        if (result.Success)
        {
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(CacheDurationMinutes));
        }

        return result;
    }
}

// Register in Program.cs
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.Decorate<IProductService, CachedProductService>();
```

## 8. Extension Methods

### Add Extension for Pagination
```csharp
public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this List<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PaginatedResponse<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
```

### Add Extension for API Response
```csharp
public static class ApiResponseExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Exception ex, T? data = default)
    {
        return ApiResponse<T>.ErrorResponse(
            $"{ex.GetType().Name}: {ex.Message}",
            StatusCodes.Status500InternalServerError);
    }

    public static ApiResponse<T> ToSuccessResponse<T>(this T data, string message = "Success")
    {
        return ApiResponse<T>.SuccessResponse(data, message, 200);
    }
}
```

## 9. Testing Configuration

### Mock Setup Example
```csharp
[TestClass]
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new ProductService(_mockRepository.Object, _mockMapper.Object);
    }

    [TestMethod]
    public async Task CreateProductAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new ProductCreateRequestDto
        {
            Name = "Test Product",
            Price = 99.99m,
            TenantId = 1
        };

        var product = new Product { ProductId = 1, Name = request.Name };
        _mockMapper.Setup(m => m.Map<Product>(request)).Returns(product);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(1);
        _mockRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _service.CreateProductAsync(request);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual(201, result.StatusCode);
    }
}
```

## 10. Performance Tuning

### Dapper Query Optimization
```csharp
// Buffered vs Unbuffered
// Buffered (default) - returns materialized list
public async Task<List<Product>> GetProductsBufferedAsync(int tenantId)
{
    var parameters = new DynamicParameters();
    parameters.Add("@TenantId", tenantId);
    
    using (var connection = new SqlConnection(_connectionString))
    {
        return (await connection.QueryAsync<Product>(
            "sp_Product_GetByTenant",
            parameters,
            commandType: CommandType.StoredProcedure,
            buffered: true // Default
        )).ToList();
    }
}

// Unbuffered - returns streaming results
public async Task<IEnumerable<Product>> GetProductsUnbufferedAsync(int tenantId)
{
    var parameters = new DynamicParameters();
    parameters.Add("@TenantId", tenantId);
    
    using (var connection = new SqlConnection(_connectionString))
    {
        return await connection.QueryAsync<Product>(
            "sp_Product_GetByTenant",
            parameters,
            commandType: CommandType.StoredProcedure,
            buffered: false  // Stream results
        );
    }
}
```

## 11. Docker Configuration

### Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ECommerceAdminPanel.csproj", "."]
RUN dotnet restore "ECommerceAdminPanel.csproj"
COPY . .
RUN dotnet build "ECommerceAdminPanel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceAdminPanel.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceAdminPanel.dll"]
```

### Docker Compose
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: YourPassword123!
      ACCEPT_EULA: Y
    ports:
      - "1433:1433"

  api:
    build: .
    depends_on:
      - sqlserver
    environment:
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=db47144;User Id=sa;Password=YourPassword123!;"
    ports:
      - "5000:80"
```

---

These templates provide production-ready configurations for common scenarios.
