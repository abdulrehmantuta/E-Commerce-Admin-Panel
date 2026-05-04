namespace ECommerceAdminPanel.Services.Services;

using AutoMapper;
using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Models;
using ECommerceAdminPanel.Repositories.IRepository;
using ECommerceAdminPanel.Services.IServices;

/// <summary>
/// Product Service Implementation
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // public async Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateRequestDto request)
    // {
    //     try
    //     {
    //         var product = _mapper.Map<Product>(request);
    //         product.CreatedDate = DateTime.Now;
    //         product.Status = true;

    //         var id = await _repository.CreateAsync(product);
    //         if (id <= 0)
    //             return ApiResponse<ProductResponseDto>.ErrorResponse("Failed to create product", 400);

    //         // Fetch the created product
    //         var created = await _repository.GetByIdAsync(id);
    //         var response = _mapper.Map<ProductResponseDto>(created);

    //         return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product created successfully", 201);
    //     }
    //     catch (Exception ex)
    //     {
    //         return ApiResponse<ProductResponseDto>.ErrorResponse($"Error creating product: {ex.Message}", 500);
    //     }
    // }

    public async Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateRequestDto request)
{
    try
    {
        var product = _mapper.Map<Product>(request);
        product.CreatedDate = DateTime.Now;
        product.Status = true;

        // ✅ IMAGE SAVE LOGIC
        if (request.Image != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            product.ImageUrl = "/uploads/products/" + fileName;
        }

        var id = await _repository.CreateAsync(product);

        var created = await _repository.GetByIdAsync(id);
        var response = _mapper.Map<ProductResponseDto>(created);

        return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product created successfully", 201);
    }
    catch (Exception ex)
    {
        return ApiResponse<ProductResponseDto>.ErrorResponse(ex.Message, 500);
    }
}

    public async Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int productId)
    {
        try
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
                return ApiResponse<ProductResponseDto>.ErrorResponse("Product not found", 404);

            var response = _mapper.Map<ProductResponseDto>(product);
            return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProductResponseDto>.ErrorResponse($"Error retrieving product: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetProductsByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var products = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);
            var response = new PaginatedResponse<ProductResponseDto>
            {
                Items = _mapper.Map<List<ProductResponseDto>>(products),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = products.Count
            };

            return ApiResponse<PaginatedResponse<ProductResponseDto>>.SuccessResponse(response, "Products retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<ProductResponseDto>>.ErrorResponse($"Error retrieving products: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetProductsByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var products = await _repository.GetByCategoryAsync(categoryId, pageNumber, pageSize);
            var response = new PaginatedResponse<ProductResponseDto>
            {
                Items = _mapper.Map<List<ProductResponseDto>>(products),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = products.Count
            };

            return ApiResponse<PaginatedResponse<ProductResponseDto>>.SuccessResponse(response, "Products retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<ProductResponseDto>>.ErrorResponse($"Error retrieving products: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateProductAsync(int productId, ProductUpdateRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(productId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Product not found", 404);

            _mapper.Map(request, existing);
            var result = await _repository.UpdateAsync(productId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update product", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Product updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating product: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(int productId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(productId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Product not found", 404);

            var result = await _repository.DeleteAsync(productId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete product", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Product deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting product: {ex.Message}", 500);
        }
    }
}

/// <summary>
/// Category Service Implementation
/// </summary>
// public class CategoryService : ICategoryService
// {
//     private readonly ICategoryRepository _repository;
//     private readonly IMapper _mapper;

//     public CategoryService(ICategoryRepository repository, IMapper mapper)
//     {
//         _repository = repository;
//         _mapper = mapper;
//     }

//     public async Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CategoryRequestDto request)
//     {
//         try
//         {
//             var category = _mapper.Map<Category>(request);
//             category.Status = true;

//             var id = await _repository.CreateAsync(category);
//             if (id <= 0)
//                 return ApiResponse<CategoryResponseDto>.ErrorResponse("Failed to create category", 400);

//             var created = await _repository.GetByIdAsync(id);
//             var response = _mapper.Map<CategoryResponseDto>(created);

//             return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category created successfully", 201);
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<CategoryResponseDto>.ErrorResponse($"Error creating category: {ex.Message}", 500);
//         }
//     }

//     public async Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int categoryId)
//     {
//         try
//         {
//             var category = await _repository.GetByIdAsync(categoryId);
//             if (category == null)
//                 return ApiResponse<CategoryResponseDto>.ErrorResponse("Category not found", 404);

//             var response = _mapper.Map<CategoryResponseDto>(category);
//             return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category retrieved successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<CategoryResponseDto>.ErrorResponse($"Error retrieving category: {ex.Message}", 500);
//         }
//     }

//     public async Task<ApiResponse<PaginatedResponse<CategoryResponseDto>>> GetCategoriesByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
//     {
//         try
//         {
//             var categories = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);
//             var response = new PaginatedResponse<CategoryResponseDto>
//             {
//                 Items = _mapper.Map<List<CategoryResponseDto>>(categories),
//                 PageNumber = pageNumber,
//                 PageSize = pageSize,
//                 TotalCount = categories.Count
//             };

//             return ApiResponse<PaginatedResponse<CategoryResponseDto>>.SuccessResponse(response, "Categories retrieved successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<PaginatedResponse<CategoryResponseDto>>.ErrorResponse($"Error retrieving categories: {ex.Message}", 500);
//         }
//     }

//     public async Task<ApiResponse<bool>> UpdateCategoryAsync(int categoryId, CategoryRequestDto request)
//     {
//         try
//         {
//             var existing = await _repository.GetByIdAsync(categoryId);
//             if (existing == null)
//                 return ApiResponse<bool>.ErrorResponse("Category not found", 404);

//             _mapper.Map(request, existing);
//             var result = await _repository.UpdateAsync(categoryId, existing);

//             if (result <= 0)
//                 return ApiResponse<bool>.ErrorResponse("Failed to update category", 400);

//             return ApiResponse<bool>.SuccessResponse(true, "Category updated successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<bool>.ErrorResponse($"Error updating category: {ex.Message}", 500);
//         }
//     }

//     public async Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId)
//     {
//         try
//         {
//             var existing = await _repository.GetByIdAsync(categoryId);
//             if (existing == null)
//                 return ApiResponse<bool>.ErrorResponse("Category not found", 404);

//             var result = await _repository.DeleteAsync(categoryId);
//             if (result <= 0)
//                 return ApiResponse<bool>.ErrorResponse("Failed to delete category", 400);

//             return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<bool>.ErrorResponse($"Error deleting category: {ex.Message}", 500);
//         }
//     }
// }


public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public CategoryService(ICategoryRepository repository, IMapper mapper, IWebHostEnvironment env)
    {
        _repository = repository;
        _mapper = mapper;
        _env = env;
    }

    // ✅ COMMON IMAGE SAVE METHOD
   private async Task<string?> SaveImageAsync(IFormFile? file)
{
    if (file == null) return null;

    var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    var uploadsFolder = Path.Combine(webRoot, "uploads", "categories");

    if (!Directory.Exists(uploadsFolder))
        Directory.CreateDirectory(uploadsFolder);

    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    return "/uploads/categories/" + fileName;
}

    // ✅ CREATE
    public async Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CategoryRequestDto request)
    {
        try
        {
            var category = _mapper.Map<Category>(request);

            // image save
            var imageUrl = await SaveImageAsync(request.Image);
            if (imageUrl != null)
                category.ImageUrl = imageUrl;

            category.Status = true;

            var id = await _repository.CreateAsync(category);
            if (id <= 0)
                return ApiResponse<CategoryResponseDto>.ErrorResponse("Failed to create category", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<CategoryResponseDto>(created);

            return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoryResponseDto>.ErrorResponse($"Error creating category: {ex.Message}", 500);
        }
    }

    // ✅ GET BY ID
    public async Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int categoryId)
    {
        try
        {
            var category = await _repository.GetByIdAsync(categoryId);
            if (category == null)
                return ApiResponse<CategoryResponseDto>.ErrorResponse("Category not found", 404);

            var response = _mapper.Map<CategoryResponseDto>(category);
            return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoryResponseDto>.ErrorResponse($"Error retrieving category: {ex.Message}", 500);
        }
    }

    // ✅ GET ALL
    public async Task<ApiResponse<PaginatedResponse<CategoryResponseDto>>> GetCategoriesByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var categories = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);

            var response = new PaginatedResponse<CategoryResponseDto>
            {
                Items = _mapper.Map<List<CategoryResponseDto>>(categories),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = categories.Count
            };

            return ApiResponse<PaginatedResponse<CategoryResponseDto>>.SuccessResponse(response, "Categories retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<CategoryResponseDto>>.ErrorResponse($"Error retrieving categories: {ex.Message}", 500);
        }
    }

    // ✅ UPDATE (WITH IMAGE REPLACE)
    public async Task<ApiResponse<bool>> UpdateCategoryAsync(int categoryId, CategoryRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(categoryId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Category not found", 404);

            _mapper.Map(request, existing);

            // new image upload
            if (request.Image != null)
            {
                // delete old image (optional but recommended)
                if (!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                var newImage = await SaveImageAsync(request.Image);
                existing.ImageUrl = newImage;
            }

            var result = await _repository.UpdateAsync(categoryId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update category", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Category updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating category: {ex.Message}", 500);
        }
    }

    // ✅ DELETE
    public async Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(categoryId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Category not found", 404);

            // delete image
            if (!string.IsNullOrEmpty(existing.ImageUrl))
            {
                var path = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                if (File.Exists(path))
                    File.Delete(path);
            }

            var result = await _repository.DeleteAsync(categoryId);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete category", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting category: {ex.Message}", 500);
        }
    }
}


// /// <summary>
/// Order Service Implementation
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _detailRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IOrderDetailRepository detailRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _detailRepository = detailRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateRequestDto request)
    {
        try
        {
            var order = _mapper.Map<Order>(request);
            order.CreatedDate = DateTime.Now;

            var id = await _orderRepository.CreateAsync(order);
            if (id <= 0)
                return ApiResponse<OrderResponseDto>.ErrorResponse("Failed to create order", 400);

            var created = await _orderRepository.GetByIdAsync(id);
            var response = _mapper.Map<OrderResponseDto>(created);

            return ApiResponse<OrderResponseDto>.SuccessResponse(response, "Order created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<OrderResponseDto>.ErrorResponse($"Error creating order: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return ApiResponse<OrderResponseDto>.ErrorResponse("Order not found", 404);

            var details = await _detailRepository.GetByOrderAsync(orderId);
            var response = _mapper.Map<OrderResponseDto>(order);
            response.OrderDetails = _mapper.Map<List<OrderDetailResponseDto>>(details);

            return ApiResponse<OrderResponseDto>.SuccessResponse(response, "Order retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<OrderResponseDto>.ErrorResponse($"Error retrieving order: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<OrderResponseDto>>> GetOrdersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var orders = await _orderRepository.GetByTenantAsync(tenantId, pageNumber, pageSize);
            var response = new PaginatedResponse<OrderResponseDto>
            {
                Items = _mapper.Map<List<OrderResponseDto>>(orders),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = orders.Count
            };

            return ApiResponse<PaginatedResponse<OrderResponseDto>>.SuccessResponse(response, "Orders retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<OrderResponseDto>>.ErrorResponse($"Error retrieving orders: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateOrderAsync(int orderId, OrderUpdateRequestDto request)
    {
        try
        {
            var existing = await _orderRepository.GetByIdAsync(orderId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Order not found", 404);

            _mapper.Map(request, existing);
            var result = await _orderRepository.UpdateAsync(orderId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update order", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Order updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating order: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteOrderAsync(int orderId)
    {
        try
        {
            var existing = await _orderRepository.GetByIdAsync(orderId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Order not found", 404);

            var result = await _orderRepository.DeleteAsync(orderId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete order", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Order deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting order: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<OrderDetailResponseDto>> AddOrderDetailAsync(OrderDetailRequestDto request)
    {
        try
        {
            var detail = _mapper.Map<OrderDetail>(request);
            var id = await _detailRepository.CreateAsync(detail);

            if (id <= 0)
                return ApiResponse<OrderDetailResponseDto>.ErrorResponse("Failed to add order detail", 400);

            var created = await _detailRepository.GetByIdAsync(id);
            var response = _mapper.Map<OrderDetailResponseDto>(created);

            return ApiResponse<OrderDetailResponseDto>.SuccessResponse(response, "Order detail added successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<OrderDetailResponseDto>.ErrorResponse($"Error adding order detail: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<List<OrderDetailResponseDto>>> GetOrderDetailsAsync(int orderId)
    {
        try
        {
            var details = await _detailRepository.GetByOrderAsync(orderId);
            var response = _mapper.Map<List<OrderDetailResponseDto>>(details);

            return ApiResponse<List<OrderDetailResponseDto>>.SuccessResponse(response, "Order details retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<OrderDetailResponseDto>>.ErrorResponse($"Error retrieving order details: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteOrderDetailAsync(int orderDetailId)
    {
        try
        {
            var result = await _detailRepository.DeleteAsync(orderDetailId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete order detail", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Order detail deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting order detail: {ex.Message}", 500);
        }
    }
}

/// <summary>
/// Tenant Service Implementation
/// </summary>
public class TenantService : ITenantService
{
    private readonly ITenantRepository _repository;
    private readonly IMapper _mapper;

    public TenantService(ITenantRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<TenantResponseDto>> CreateTenantAsync(TenantRequestDto request)
    {
        try
        {
            var tenant = _mapper.Map<Tenant>(request);
            tenant.CreatedDate = DateTime.Now;
            tenant.Status = true;

            var id = await _repository.CreateAsync(tenant);
            if (id <= 0)
                return ApiResponse<TenantResponseDto>.ErrorResponse("Failed to create tenant", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<TenantResponseDto>(created);

            return ApiResponse<TenantResponseDto>.SuccessResponse(response, "Tenant created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantResponseDto>.ErrorResponse($"Error creating tenant: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<TenantResponseDto>> GetTenantByIdAsync(int tenantId)
    {
        try
        {
            var tenant = await _repository.GetByIdAsync(tenantId);
            if (tenant == null)
                return ApiResponse<TenantResponseDto>.ErrorResponse("Tenant not found", 404);

            var response = _mapper.Map<TenantResponseDto>(tenant);
            return ApiResponse<TenantResponseDto>.SuccessResponse(response, "Tenant retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantResponseDto>.ErrorResponse($"Error retrieving tenant: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<TenantResponseDto>>> GetAllTenantsAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var tenants = await _repository.GetAllAsync(pageNumber, pageSize);
            var response = new PaginatedResponse<TenantResponseDto>
            {
                Items = _mapper.Map<List<TenantResponseDto>>(tenants),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = tenants.Count
            };

            return ApiResponse<PaginatedResponse<TenantResponseDto>>.SuccessResponse(response, "Tenants retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<TenantResponseDto>>.ErrorResponse($"Error retrieving tenants: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateTenantAsync(int tenantId, TenantRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(tenantId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Tenant not found", 404);

            _mapper.Map(request, existing);
            var result = await _repository.UpdateAsync(tenantId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update tenant", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Tenant updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating tenant: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteTenantAsync(int tenantId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(tenantId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Tenant not found", 404);

            var result = await _repository.DeleteAsync(tenantId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete tenant", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Tenant deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting tenant: {ex.Message}", 500);
        }
    }

    //new
    public async Task<ApiResponse<TenantResponseDto>> GetTenantByDomainAsync(string domain)
    {
        try
        {
            var tenant = await _repository.GetByDomainAsync(domain);
            if (tenant == null)
                return ApiResponse<TenantResponseDto>.ErrorResponse("Tenant not found", 404);

            var response = _mapper.Map<TenantResponseDto>(tenant);
            return ApiResponse<TenantResponseDto>.SuccessResponse(response, "Tenant resolved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantResponseDto>.ErrorResponse($"Error resolving tenant: {ex.Message}", 500);
        }
    }
}


/// <summary>
/// User Service Implementation
/// </summary>

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<UserResponseDto>> CreateUserAsync(UserCreateRequestDto request)
    {
        try
        {
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.CreatedDate = DateTime.Now;
            user.Status = true;

            var id = await _repository.CreateAsync(user);
            if (id <= 0)
                return ApiResponse<UserResponseDto>.ErrorResponse("Failed to create user", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<UserResponseDto>(created);

            return ApiResponse<UserResponseDto>.SuccessResponse(response, "User created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error creating user: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found", 404);

            var response = _mapper.Map<UserResponseDto>(user);
            return ApiResponse<UserResponseDto>.SuccessResponse(response, "User retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error retrieving user: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<UserResponseDto>>> GetUsersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var users = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);
            var response = new PaginatedResponse<UserResponseDto>
            {
                Items = _mapper.Map<List<UserResponseDto>>(users),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = users.Count
            };

            return ApiResponse<PaginatedResponse<UserResponseDto>>.SuccessResponse(response, "Users retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<UserResponseDto>>.ErrorResponse($"Error retrieving users: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateUserAsync(int userId, UserUpdateRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(userId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("User not found", 404);

            _mapper.Map(request, existing);
            var result = await _repository.UpdateAsync(userId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update user", 400);

            return ApiResponse<bool>.SuccessResponse(true, "User updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating user: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(int userId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(userId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("User not found", 404);

            var result = await _repository.DeleteAsync(userId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete user", 400);

            return ApiResponse<bool>.SuccessResponse(true, "User deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting user: {ex.Message}", 500);
        }
    }
}



/// <summary>
/// Page Service Implementation
/// </summary>
public class PageService : IPageService
{
    private readonly IPageRepository _repository;
    private readonly IMapper _mapper;

    public PageService(IPageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PageResponseDto>> CreatePageAsync(PageRequestDto request)
    {
        try
        {
            var page = _mapper.Map<Page>(request);
            page.CreatedDate = DateTime.Now;

            var id = await _repository.CreateAsync(page);
            if (id <= 0)
                return ApiResponse<PageResponseDto>.ErrorResponse("Failed to create page", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<PageResponseDto>(created);

            return ApiResponse<PageResponseDto>.SuccessResponse(response, "Page created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<PageResponseDto>.ErrorResponse($"Error creating page: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PageResponseDto>> GetPageByIdAsync(int pageId)
    {
        try
        {
            var page = await _repository.GetByIdAsync(pageId);
            if (page == null)
                return ApiResponse<PageResponseDto>.ErrorResponse("Page not found", 404);

            var response = _mapper.Map<PageResponseDto>(page);
            return ApiResponse<PageResponseDto>.SuccessResponse(response, "Page retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PageResponseDto>.ErrorResponse($"Error retrieving page: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<PageResponseDto>>> GetPagesByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var pages = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);
            var response = new PaginatedResponse<PageResponseDto>
            {
                Items = _mapper.Map<List<PageResponseDto>>(pages),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = pages.Count
            };

            return ApiResponse<PaginatedResponse<PageResponseDto>>.SuccessResponse(response, "Pages retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<PageResponseDto>>.ErrorResponse($"Error retrieving pages: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdatePageAsync(int pageId, PageRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(pageId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Page not found", 404);

            _mapper.Map(request, existing);
            var result = await _repository.UpdateAsync(pageId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update page", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Page updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating page: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeletePageAsync(int pageId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(pageId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Page not found", 404);

            var result = await _repository.DeleteAsync(pageId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete page", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Page deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting page: {ex.Message}", 500);
        }
    }
}

/// <summary>
/// Section Service Implementation
/// </summary>
public class SectionService : ISectionService
{
    private readonly ISectionRepository _repository;
    private readonly IMapper _mapper;

    public SectionService(ISectionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<SectionResponseDto>> CreateSectionAsync(SectionRequestDto request)
    {
        try
        {
            var section = _mapper.Map<Section>(request);
            section.Status = true;

            var id = await _repository.CreateAsync(section);
            if (id <= 0)
                return ApiResponse<SectionResponseDto>.ErrorResponse("Failed to create section", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<SectionResponseDto>(created);

            return ApiResponse<SectionResponseDto>.SuccessResponse(response, "Section created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<SectionResponseDto>.ErrorResponse($"Error creating section: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<SectionResponseDto>> GetSectionByIdAsync(int sectionId)
    {
        try
        {
            var section = await _repository.GetByIdAsync(sectionId);
            if (section == null)
                return ApiResponse<SectionResponseDto>.ErrorResponse("Section not found", 404);

            var response = _mapper.Map<SectionResponseDto>(section);
            return ApiResponse<SectionResponseDto>.SuccessResponse(response, "Section retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<SectionResponseDto>.ErrorResponse($"Error retrieving section: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<List<SectionResponseDto>>> GetSectionsByPageAsync(int pageId)
    {
        try
        {
            var sections = await _repository.GetByPageAsync(pageId);
            var response = _mapper.Map<List<SectionResponseDto>>(sections);

            return ApiResponse<List<SectionResponseDto>>.SuccessResponse(response, "Sections retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<SectionResponseDto>>.ErrorResponse($"Error retrieving sections: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateSectionAsync(int sectionId, SectionRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(sectionId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Section not found", 404);

            _mapper.Map(request, existing);
            var result = await _repository.UpdateAsync(sectionId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update section", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Section updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating section: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteSectionAsync(int sectionId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(sectionId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Section not found", 404);

            var result = await _repository.DeleteAsync(sectionId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete section", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Section deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting section: {ex.Message}", 500);
        }
    }
}

/// <summary>
/// SectionData Service Implementation
/// </summary>
public class SectionDataService : ISectionDataService
{
    private readonly ISectionDataRepository _repository;
    private readonly IMapper _mapper;

    public SectionDataService(ISectionDataRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<SectionDataResponseDto>> CreateSectionDataAsync(SectionDataRequestDto request)
    {
        try
        {
            var data = _mapper.Map<SectionData>(request);

            var id = await _repository.CreateAsync(data);
            if (id <= 0)
                return ApiResponse<SectionDataResponseDto>.ErrorResponse("Failed to create section data", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<SectionDataResponseDto>(created);

            return ApiResponse<SectionDataResponseDto>.SuccessResponse(response, "Section data created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<SectionDataResponseDto>.ErrorResponse($"Error creating section data: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<SectionDataResponseDto>> GetSectionDataByIdAsync(int dataId)
    {
        try
        {
            var data = await _repository.GetByIdAsync(dataId);
            if (data == null)
                return ApiResponse<SectionDataResponseDto>.ErrorResponse("Section data not found", 404);

            var response = _mapper.Map<SectionDataResponseDto>(data);
            return ApiResponse<SectionDataResponseDto>.SuccessResponse(response, "Section data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<SectionDataResponseDto>.ErrorResponse($"Error retrieving section data: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<List<SectionDataResponseDto>>> GetSectionDataBySectionAsync(int sectionId)
    {
        try
        {
            var dataList = await _repository.GetBySectionAsync(sectionId);
            var response = _mapper.Map<List<SectionDataResponseDto>>(dataList);

            return ApiResponse<List<SectionDataResponseDto>>.SuccessResponse(response, "Section data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<SectionDataResponseDto>>.ErrorResponse($"Error retrieving section data: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateSectionDataAsync(int dataId, SectionDataRequestDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(dataId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Section data not found", 404);

            _mapper.Map(request, existing);
            var result = await _repository.UpdateAsync(dataId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update section data", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Section data updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating section data: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteSectionDataAsync(int dataId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(dataId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Section data not found", 404);

            var result = await _repository.DeleteAsync(dataId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete section data", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Section data deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting section data: {ex.Message}", 500);
        }
    }
}




/// <summary>
/// Customer Service Implementation
/// </summary>




public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CustomerResponseDto>> CreateCustomerAsync(CustomerCreateDto request)
    {
        try
        {
            var customer = _mapper.Map<Customer>(request);
            customer.Status = true;

            var id = await _repository.CreateAsync(customer);
            if (id <= 0)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Failed to create customer", 400);

            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<CustomerResponseDto>(created);

            return ApiResponse<CustomerResponseDto>.SuccessResponse(response, "Customer created successfully", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<CustomerResponseDto>.ErrorResponse($"Error creating customer: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(int customerId)
    {
        try
        {
            var customer = await _repository.GetByIdAsync(customerId);
            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer not found", 404);

            var response = _mapper.Map<CustomerResponseDto>(customer);
            return ApiResponse<CustomerResponseDto>.SuccessResponse(response, "Customer retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<CustomerResponseDto>.ErrorResponse($"Error retrieving customer: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<PaginatedResponse<CustomerResponseDto>>> GetCustomersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var customers = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);

            var response = new PaginatedResponse<CustomerResponseDto>
            {
                Items = _mapper.Map<List<CustomerResponseDto>>(customers),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = customers.Count
            };

            return ApiResponse<PaginatedResponse<CustomerResponseDto>>.SuccessResponse(response, "Customers retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<CustomerResponseDto>>.ErrorResponse($"Error retrieving customers: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateCustomerAsync(int customerId, CustomerUpdateDto request)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(customerId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Customer not found", 404);

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(customerId, existing);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update customer", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Customer updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating customer: {ex.Message}", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteCustomerAsync(int customerId)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(customerId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Customer not found", 404);

            var result = await _repository.DeleteAsync(customerId);
            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to delete customer", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Customer deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting customer: {ex.Message}", 500);
        }
    }
}

