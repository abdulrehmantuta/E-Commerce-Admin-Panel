# E-Commerce Admin Panel - Complete Implementation Guide

## Project Overview

This is a single-layer .NET 9 E-Commerce Admin Panel backend using Dapper ORM with SQL Server. The architecture follows clean code principles with separation of concerns across Controllers, Services, Repositories, and DTOs.

## Architecture

```
Controllers (API Endpoints)
       ↓
Services (Business Logic)
       ↓
Repositories (Data Access with Dapper)
       ↓
SQL Server (Stored Procedures)
```

## Project Structure

```
ECommerceAdminPanel/
├── Controllers/          # API Endpoints
├── Services/
│   ├── IServices/       # Service Interfaces
│   └── Services/        # Service Implementations
├── Repositories/
│   ├── IRepository/     # Repository Interfaces
│   └── Repository/      # Repository Implementations
├── Models/              # Entity Models
├── DTOs/
│   ├── Request/        # Request DTOs
│   └── Response/       # Response DTOs & Wrappers
├── Enums/              # System Enums
├── Helper/             # Dapper Helper, Utilities
├── Config/             # AutoMapper Configuration
├── Data/               # Migrations, Seed Scripts
├── Interfaces/         # General Interfaces
├── Properties/         # Project Settings
└── SQL_Scripts/        # Database Scripts
```

## Database Setup

### 1. Create Database Tables
Run the SQL script: `SQL_Scripts/01_CreateTables.sql`

```sql
USE db47144;
-- Execute the table creation script
```

### 2. Create Stored Procedures
Run the SQL script: `SQL_Scripts/02_CreateStoredProcedures.sql`

This creates CRUD stored procedures for all entities:
- `sp_Product_Create`, `sp_Product_GetById`, `sp_Product_GetByTenant`, `sp_Product_Update`, `sp_Product_Delete`
- Similar SPs for all other entities

### 3. Update Connection String
Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=db47144;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

## Key Features

### 1. Multi-Tenant Support
All entities are tenant-aware with `TenantId`:
- Tenants: Organization/Company definitions
- Users: User accounts per tenant
- Categories: Product categories per tenant
- Products: Products per tenant
- Orders: Orders per tenant
- Pages: CMS pages per tenant

### 2. Complete CRUD Operations
Each entity has:
- **Create**: Add new records
- **Read**: Get by ID, Get by Tenant/Parent
- **Update**: Update existing records
- **Delete**: Delete records with cascading

### 3. Dapper ORM
- Lightweight and performant
- Uses stored procedures for all DB operations
- Async/await support
- Type-safe query results

### 4. Dependency Injection
All services and repositories are registered in `Program.cs` for easy testing and modularity.

### 5. AutoMapper
Automatic mapping between entities and DTOs.

## API Endpoints - Product Example

### Create Product
```
POST /api/product/create
Body: {
  "tenantId": 1,
  "name": "Laptop",
  "description": "High-end laptop",
  "price": 1299.99,
  "imageUrl": "laptop.jpg",
  "categoryId": 1,
  "stockQty": 50
}
```

### Get Product by ID
```
GET /api/product/1
```

### Get Products by Tenant
```
GET /api/product/tenant/1?pageNumber=1&pageSize=10
```

### Get Products by Category
```
GET /api/product/category/1?pageNumber=1&pageSize=10
```

### Update Product
```
PUT /api/product/1
Body: {
  "name": "Laptop Pro",
  "description": "Updated description",
  "price": 1499.99,
  "imageUrl": "laptop-pro.jpg",
  "categoryId": 1,
  "stockQty": 45,
  "status": true
}
```

### Delete Product
```
DELETE /api/product/1
```

## Response Format

All API responses follow a standard format:

### Success Response (201/200)
```json
{
  "success": true,
  "message": "Product created successfully",
  "data": {
    "productId": 1,
    "tenantId": 1,
    "name": "Laptop",
    "price": 1299.99,
    "stockQty": 50,
    "status": true,
    "createdDate": "2024-12-19T10:30:00Z"
  },
  "statusCode": 201,
  "timestamp": "2024-12-19T10:30:00Z"
}
```

### Error Response (400/404/500)
```json
{
  "success": false,
  "message": "Product not found",
  "data": null,
  "statusCode": 404,
  "timestamp": "2024-12-19T10:30:00Z"
}
```

### Paginated Response
```json
{
  "success": true,
  "message": "Products retrieved successfully",
  "data": {
    "items": [...],
    "totalCount": 45,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "statusCode": 200,
  "timestamp": "2024-12-19T10:30:00Z"
}
```

## Implementation Example - Product Entity

### 1. Model/Entity (`Models/Entities.cs`)
```csharp
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
```

### 2. DTOs (`DTOs/Request/RequestDtos.cs` & `DTOs/Response/ResponseDtos.cs`)
```csharp
// Request
public class ProductCreateRequestDto
{
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public int StockQty { get; set; }
}

// Response
public class ProductResponseDto
{
    public int ProductId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

### 3. Repository Interface (`Repositories/IRepository/IRepositories.cs`)
```csharp
public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<List<Product>> GetByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
}
```

### 4. Repository Implementation (`Repositories/Repository/ProductRepository.cs`)
```csharp
public class ProductRepository : IProductRepository
{
    private readonly DapperHelper _dapperHelper;

    public ProductRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(Product entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Price", entity.Price);
        parameters.Add("@StockQty", entity.StockQty);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Product_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ProductId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Product>("sp_Product_GetById", parameters);
    }

    public async Task<List<Product>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Product>("sp_Product_GetByTenant", parameters);
    }
}
```

### 5. Service Interface (`Services/IServices/IServices.cs`)
```csharp
public interface IProductService
{
    Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateRequestDto request);
    Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int productId);
    Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetProductsByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<bool>> UpdateProductAsync(int productId, ProductUpdateRequestDto request);
    Task<ApiResponse<bool>> DeleteProductAsync(int productId);
}
```

### 6. Service Implementation (`Services/Services/AllServices.cs`)
```csharp
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateRequestDto request)
    {
        try
        {
            var product = _mapper.Map<Product>(request);
            product.CreatedDate = DateTime.Now;
            product.Status = true;

            var id = await _repository.CreateAsync(product);
            if (id <= 0)
                return ApiResponse<ProductResponseDto>.ErrorResponse("Failed to create product", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<ProductResponseDto>(created);
            return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<ProductResponseDto>.ErrorResponse($"Error creating product: {ex.Message}", 500);
        }
    }
}
```

### 7. Controller (`Controllers/ProductController.cs`)
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService service, ILogger<ProductController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct([FromBody] ProductCreateRequestDto request)
    {
        _logger.LogInformation("Creating product: {ProductName}", request.Name);
        var result = await _service.CreateProductAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductById(int id)
    {
        var result = await _service.GetProductByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
```

### 8. Stored Procedure (`SQL_Scripts/02_CreateStoredProcedures.sql`)
```sql
CREATE OR ALTER PROCEDURE sp_Product_Create
    @TenantId INT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(MAX) = NULL,
    @Price DECIMAL(18,2),
    @ImageUrl NVARCHAR(255) = NULL,
    @CategoryId INT = NULL,
    @StockQty INT = 0
AS
BEGIN
    INSERT INTO Products (TenantId, Name, Description, Price, ImageUrl, CategoryId, StockQty, CreatedDate, Status)
    VALUES (@TenantId, @Name, @Description, @Price, @ImageUrl, @CategoryId, @StockQty, GETDATE(), 1);
    
    SELECT SCOPE_IDENTITY() AS ProductId;
END;
```

## Adding a New Entity - Step-by-Step

To replicate this for other entities:

1. **Create Entity Model** in `Models/Entities.cs`
2. **Create Request DTOs** in `DTOs/Request/RequestDtos.cs`
3. **Create Response DTO** in `DTOs/Response/ResponseDtos.cs`
4. **Create Repository Interface** in `Repositories/IRepository/IRepositories.cs`
5. **Create Repository Implementation** with Dapper queries
6. **Create Service Interface** in `Services/IServices/IServices.cs`
7. **Create Service Implementation** with business logic
8. **Create Controller** with API endpoints
9. **Create Stored Procedures** in SQL Scripts
10. **Add AutoMapper Mappings** in `Config/AutoMapperProfile.cs`
11. **Register in Program.cs** with DI

## Running the Application

### Development
```bash
dotnet run
```

### Build
```bash
dotnet build
```

### Generate Migrations (if using EF Core)
```bash
dotnet ef migrations add Initial
dotnet ef database update
```

## Entities Overview

### Tenants
Multi-tenant support - each tenant has their own data silo.

### Users
User accounts with roles (User, Admin, Manager, Superadmin).

### Categories
Product categories with parent-child hierarchy.

### Products
E-commerce products with pricing, inventory, images.

### Orders
Customer orders with order details/line items.

### OrderDetails
Line items in orders - product quantity and price.

### Pages
CMS pages for content management.

### Sections
Reusable sections within pages.

### SectionData
Key-value data storage for sections.

## Status Codes

- `200`: Success
- `201`: Created
- `400`: Bad Request
- `404`: Not Found
- `500`: Internal Server Error

## Environment Setup

### Prerequisites
- .NET 9 SDK
- SQL Server 2019+
- Visual Studio 2022 (recommended)

### Configuration
1. Update connection string in `appsettings.json`
2. Run database scripts
3. Update AutoMapper mappings if adding new entities
4. Register services in `Program.cs`

## Best Practices in This Implementation

1. **Async/Await**: All operations are asynchronous for better performance
2. **Error Handling**: Try-catch blocks in services with meaningful error messages
3. **Logging**: Structured logging in controllers
4. **Dependency Injection**: All dependencies injected via constructors
5. **DTOs**: Separate request and response models for API contracts
6. **Stored Procedures**: Database logic encapsulated in SPs
7. **Pagination**: Built-in pagination support
8. **Multi-tenant**: Tenant isolation in all queries
9. **AutoMapper**: Automatic entity-to-DTO mapping
10. **API Documentation**: Swagger/OpenAPI integration

## Common Operations

### Get Products with Pagination
```
GET /api/product/tenant/1?pageNumber=1&pageSize=20
```

### Create New Order with Details
1. POST /api/order/create (creates order)
2. POST /api/order/detail/add (add items to order) - repeat for each item
3. PUT /api/order/{id} (update order if needed)

### Nested CMS Structure
1. POST /api/page/create (create page)
2. POST /api/section/create (add sections to page)
3. POST /api/sectiondata/create (add data to sections)

## Testing with Postman

1. Import controllers in Postman
2. Set base URL: `https://localhost:7xxx/api`
3. Test each endpoint with sample data
4. Use pagina parameters for list endpoints

## Performance Considerations

1. Stored procedures are pre-compiled for better performance
2. Dapper provides lightweight data mapping
3. Indexing on commonly queried fields (TenantId, CategoryId)
4. Pagination prevents loading large datasets
5. Async operations prevent thread blocking

## Future Enhancements

- Add authentication/authorization
- Implement caching layer
- Add comprehensive input validation with FluentValidation
- Add audit logging
- Implement soft deletes
- Add transaction support
- Implement search/filter capabilities
- Add reporting functionality
