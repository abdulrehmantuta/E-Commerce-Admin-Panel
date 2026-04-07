# Quick Start Guide - Adding New Entities

This guide shows how to add a new entity to the system using the same architecture pattern used for Products, Categories, etc.

## Example: Adding a NEW ENTITY (BlogPost)

### Step 1: Create the Entity Model

**File: `Models/Entities.cs`**

Add your entity class:

```csharp
/// <summary>
/// BlogPost entity
/// </summary>
public class BlogPost
{
    public int BlogPostId { get; set; }
    public int TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public int ViewCount { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
```

### Step 2: Create Request DTOs

**File: `DTOs/Request/RequestDtos.cs`**

Add the request classes:

```csharp
/// <summary>
/// BlogPost Create Request
/// </summary>
public class BlogPostCreateRequestDto
{
    public int TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
}

/// <summary>
/// BlogPost Update Request
/// </summary>
public class BlogPostUpdateRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public bool Status { get; set; }
}
```

### Step 3: Create Response DTOs

**File: `DTOs/Response/ResponseDtos.cs`**

Add the response class:

```csharp
/// <summary>
/// BlogPost Response DTO
/// </summary>
public class BlogPostResponseDto
{
    public int BlogPostId { get; set; }
    public int TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public int ViewCount { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
```

### Step 4: Create Repository Interface

**File: `Repositories/IRepository/IRepositories.cs`**

Add the interface:

```csharp
/// <summary>
/// BlogPost Repository Interface
/// </summary>
public interface IBlogPostRepository : IBaseRepository<BlogPost>
{
    Task<List<BlogPost>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<List<BlogPost>> GetByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
    Task<BlogPost?> GetBySlugAsync(string slug);
}
```

### Step 5: Create Repository Implementation

**File: `Repositories/Repository/AllRepositories.cs` (add to existing file)**

Add the implementation class:

```csharp
/// <summary>
/// BlogPost Repository Implementation
/// </summary>
public class BlogPostRepository : IBlogPostRepository
{
    private readonly DapperHelper _dapperHelper;

    public BlogPostRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(BlogPost entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@Title", entity.Title);
        parameters.Add("@Slug", entity.Slug);
        parameters.Add("@Content", entity.Content);
        parameters.Add("@Author", entity.Author);
        parameters.Add("@ImageUrl", entity.ImageUrl);
        parameters.Add("@CategoryId", entity.CategoryId);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_BlogPost_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<BlogPost?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@BlogPostId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<BlogPost>("sp_BlogPost_GetById", parameters);
    }

    public async Task<List<BlogPost>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<BlogPost>("sp_BlogPost_GetAll", parameters);
    }

    public async Task<List<BlogPost>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<BlogPost>("sp_BlogPost_GetByTenant", parameters);
    }

    public async Task<List<BlogPost>> GetByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CategoryId", categoryId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<BlogPost>("sp_BlogPost_GetByCategory", parameters);
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Slug", slug);
        return await _dapperHelper.QuerySingleOrDefaultAsync<BlogPost>("sp_BlogPost_GetBySlug", parameters);
    }

    public async Task<int> UpdateAsync(int id, BlogPost entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@BlogPostId", id);
        parameters.Add("@Title", entity.Title);
        parameters.Add("@Slug", entity.Slug);
        parameters.Add("@Content", entity.Content);
        parameters.Add("@Author", entity.Author);
        parameters.Add("@ImageUrl", entity.ImageUrl);
        parameters.Add("@CategoryId", entity.CategoryId);
        parameters.Add("@Status", entity.Status);
        parameters.Add("@UpdatedDate", DateTime.Now);

        return await _dapperHelper.ExecuteAsync("sp_BlogPost_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@BlogPostId", id);
        return await _dapperHelper.ExecuteAsync("sp_BlogPost_Delete", parameters);
    }
}
```

### Step 6: Create Service Interface

**File: `Services/IServices/IServices.cs`**

Add the interface:

```csharp
/// <summary>
/// BlogPost Service Interface
/// </summary>
public interface IBlogPostService
{
    Task<ApiResponse<BlogPostResponseDto>> CreateBlogPostAsync(BlogPostCreateRequestDto request);
    Task<ApiResponse<BlogPostResponseDto>> GetBlogPostByIdAsync(int blogPostId);
    Task<ApiResponse<PaginatedResponse<BlogPostResponseDto>>> GetBlogPostsByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<PaginatedResponse<BlogPostResponseDto>>> GetBlogPostsByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
    Task<ApiResponse<BlogPostResponseDto>> GetBlogPostBySlugAsync(string slug);
    Task<ApiResponse<bool>> UpdateBlogPostAsync(int blogPostId, BlogPostUpdateRequestDto request);
    Task<ApiResponse<bool>> DeleteBlogPostAsync(int blogPostId);
}
```

### Step 7: Create Service Implementation

**File: `Services/Services/AllServices.cs` (add to existing file)**

Add the service class:

```csharp
/// <summary>
/// BlogPost Service Implementation
/// </summary>
public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _repository;
    private readonly IMapper _mapper;

    public BlogPostService(IBlogPostRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<BlogPostResponseDto>> CreateBlogPostAsync(BlogPostCreateRequestDto request)
    {
        try
        {
            var blogPost = _mapper.Map<BlogPost>(request);
            blogPost.CreatedDate = DateTime.Now;
            blogPost.Status = true;
            blogPost.ViewCount = 0;

            var id = await _repository.CreateAsync(blogPost);
            if (id <= 0)
                return ApiResponse<BlogPostResponseDto>.ErrorResponse("Failed to create blog post", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<BlogPostResponseDto>(created);
            return ApiResponse<BlogPostResponseDto>.SuccessResponse(response, "Blog post created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<BlogPostResponseDto>.ErrorResponse($"Error creating blog post: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<BlogPostResponseDto>> GetBlogPostByIdAsync(int blogPostId)
    {
        try
        {
            var blogPost = await _repository.GetByIdAsync(blogPostId);
            if (blogPost == null)
                return ApiResponse<BlogPostResponseDto>.ErrorResponse("Blog post not found", 404);

            var response = _mapper.Map<BlogPostResponseDto>(blogPost);
            return ApiResponse<BlogPostResponseDto>.SuccessResponse(response, "Blog post retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<BlogPostResponseDto>.ErrorResponse($"Error retrieving blog post: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<BlogPostResponseDto>>> GetBlogPostsByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var blogPosts = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);
            var response = new PaginatedResponse<BlogPostResponseDto>
            {
                Items = _mapper.Map<List<BlogPostResponseDto>>(blogPosts),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = blogPosts.Count
            };

            return ApiResponse<PaginatedResponse<BlogPostResponseDto>>.SuccessResponse(response, "Blog posts retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<BlogPostResponseDto>>.ErrorResponse($"Error retrieving blog posts: {ex.Message}", 500);
        }
    }

    // ... Similarly implement other methods
}
```

### Step 8: Create Controller

**File: `Controllers/AllControllers.cs` (add to existing file)**

Add the controller class:

```csharp
/// <summary>
/// BlogPost Management API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BlogPostController : ControllerBase
{
    private readonly IBlogPostService _service;
    private readonly ILogger<BlogPostController> _logger;

    public BlogPostController(IBlogPostService service, ILogger<BlogPostController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<BlogPostResponseDto>>> CreateBlogPost([FromBody] BlogPostCreateRequestDto request)
    {
        _logger.LogInformation("Creating blog post: {Title}", request.Title);
        var result = await _service.CreateBlogPostAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BlogPostResponseDto>>> GetBlogPostById(int id)
    {
        var result = await _service.GetBlogPostByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ApiResponse<BlogPostResponseDto>>> GetBlogPostBySlug(string slug)
    {
        var result = await _service.GetBlogPostBySlugAsync(slug);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<BlogPostResponseDto>>>> GetBlogPostsByTenant(
        int tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetBlogPostsByTenantAsync(tenantId, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateBlogPost(int id, [FromBody] BlogPostUpdateRequestDto request)
    {
        var result = await _service.UpdateBlogPostAsync(id, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBlogPost(int id)
    {
        var result = await _service.DeleteBlogPostAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
```

### Step 9: Create Stored Procedures

**File: `SQL_Scripts/02_CreateStoredProcedures.sql` (add to existing file)**

Add the table and stored procedures:

```sql
-- Create BlogPosts table
CREATE TABLE BlogPosts (
    BlogPostId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Author NVARCHAR(100) NULL,
    ImageUrl NVARCHAR(255) NULL,
    CategoryId INT NULL,
    ViewCount INT DEFAULT 0,
    Status BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    CONSTRAINT FK_BlogPosts_Tenant FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE,
    CONSTRAINT FK_BlogPosts_Category FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE SET NULL
);

-- Create BlogPost procedures
CREATE OR ALTER PROCEDURE sp_BlogPost_Create
    @TenantId INT,
    @Title NVARCHAR(200),
    @Slug NVARCHAR(200),
    @Content NVARCHAR(MAX),
    @Author NVARCHAR(100) = NULL,
    @ImageUrl NVARCHAR(255) = NULL,
    @CategoryId INT = NULL
AS
BEGIN
    INSERT INTO BlogPosts (TenantId, Title, Slug, Content, Author, ImageUrl, CategoryId, CreatedDate, Status)
    VALUES (@TenantId, @Title, @Slug, @Content, @Author, @ImageUrl, @CategoryId, GETDATE(), 1);
    
    SELECT SCOPE_IDENTITY() AS BlogPostId;
END;

CREATE OR ALTER PROCEDURE sp_BlogPost_GetById
    @BlogPostId INT
AS
BEGIN
    SELECT BlogPostId, TenantId, Title, Slug, Content, Author, ImageUrl, CategoryId, ViewCount, Status, CreatedDate, UpdatedDate
    FROM BlogPosts
    WHERE BlogPostId = @BlogPostId;
END;

-- ... Continue with other CRUD procedures
```

### Step 10: Add AutoMapper Mappings

**File: `Config/AutoMapperProfile.cs`**

Add to the `AutoMapperProfile` class constructor:

```csharp
// BlogPost mappings
CreateMap<BlogPost, BlogPostResponseDto>().ReverseMap();
CreateMap<BlogPost, BlogPostCreateRequestDto>().ReverseMap();
CreateMap<BlogPost, BlogPostUpdateRequestDto>().ReverseMap();
```

### Step 11: Register in Program.cs

**File: `Program.cs`**

Add the DI registrations:

```csharp
// Add to repository registrations
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

// Add to service registrations
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
```

### Step 12: Build and Test

```bash
dotnet build
dotnet run
```

Test with Swagger: `https://localhost:7xxx`

---

## Checklist for New Entity

- [ ] Create Entity Model
- [ ] Create Request DTOs (Create, Update)
- [ ] Create Response DTO
- [ ] Create Repository Interface
- [ ] Create Repository Implementation
- [ ] Create Service Interface
- [ ] Create Service Implementation
- [ ] Create Controller
- [ ] Create/Update Table in DB
- [ ] Create Stored Procedures
- [ ] Add AutoMapper Mappings
- [ ] Register in Program.cs DI
- [ ] Test with Swagger/Postman

## Common Patterns Used

1. **IBaseRepository<T>**: Common CRUD operations
2. **Specific Repository Interface**: Additional custom methods
3. **DynamicParameters**: Type-safe SP parameter passing
4. **ApiResponse<T>**: Standard response wrapper
5. **PaginatedResponse<T>**: Pagination support
6. **Async/Await**: Non-blocking operations
7. **AutoMapper**: Entity-DTO mapping
8. **Dependency Injection**: Loose coupling

## Tips

- Keep stored procedure names consistent: `sp_EntityName_Action`
- Always include TenantId for tenant-scoped entities
- Add proper logging in controllers
- Handle exceptions in services with meaningful messages
- Use pagination for list endpoints
- Create indexes on commonly filtered columns
- Document all public methods with XML comments
