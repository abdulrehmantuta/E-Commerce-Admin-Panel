    namespace ECommerceAdminPanel.Services.Services;

    using AutoMapper;
    using ECommerceAdminPanel.DTOs.Request;
    using ECommerceAdminPanel.DTOs.Response;
    using ECommerceAdminPanel.Helper;
    using ECommerceAdminPanel.Models;
    using ECommerceAdminPanel.Repositories.IRepository;
    using ECommerceAdminPanel.Services.IServices;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
using System.Text.Json;

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

    public async Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateRequestDto request)
    {
        try
        {
            var product = _mapper.Map<Product>(request);
            product.CreatedDate = DateTime.Now;
            product.Status = true;

            if (request.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await request.Image.CopyToAsync(stream);

                product.ImageUrl = "/uploads/products/" + fileName;
            }

            product.Sizes = JsonHelper.ToJson(request.Sizes);
            product.Colors = JsonHelper.ToJson(request.Colors);
            product.SKU = request.SKU?.Trim();
            product.Brand = request.Brand?.Trim();

            var id = await _repository.CreateAsync(product);
            var created = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<ProductResponseDto>(created);

            response.Sizes = JsonHelper.ToList(created?.Sizes);
            response.Colors = JsonHelper.ToList(created?.Colors);

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
            response.Sizes = JsonHelper.ToList(product.Sizes);
            response.Colors = JsonHelper.ToList(product.Colors);

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
            var items = _mapper.Map<List<ProductResponseDto>>(products);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Sizes = JsonHelper.ToList(products[i].Sizes);
                items[i].Colors = JsonHelper.ToList(products[i].Colors);
            }

            var response = new PaginatedResponse<ProductResponseDto>
            {
                Items = items,
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
            var items = _mapper.Map<List<ProductResponseDto>>(products);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Sizes = JsonHelper.ToList(products[i].Sizes);
                items[i].Colors = JsonHelper.ToList(products[i].Colors);
            }

            var response = new PaginatedResponse<ProductResponseDto>
            {
                Items = items,
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

            existing.Sizes = JsonHelper.ToJson(request.Sizes);
            existing.Colors = JsonHelper.ToJson(request.Colors);
            existing.SKU = request.SKU?.Trim();
            existing.Brand = request.Brand?.Trim();

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
//public class OrderService : IOrderService
//{
//    private readonly IOrderRepository _orderRepository;
//    private readonly IOrderDetailRepository _detailRepository;
//    private readonly IMapper _mapper;
//    private readonly INotificationService _notificationService; // ✅ NEW


//public OrderService(IOrderRepository orderRepository, IOrderDetailRepository detailRepository, IMapper mapper, INotificationService notificationService)
//    {
//        _orderRepository = orderRepository;
//        _detailRepository = detailRepository;
//        _mapper = mapper;
//    _notificationService = notificationService; // ✅ NEW

//}

//public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateRequestDto request)
//    {
//        try
//        {
//            var order = _mapper.Map<Order>(request);
//            order.CreatedDate = DateTime.Now;

//            var id = await _orderRepository.CreateAsync(order);
//            if (id <= 0)
//                return ApiResponse<OrderResponseDto>.ErrorResponse("Failed to create order", 400);

//            var created = await _orderRepository.GetByIdAsync(id);
//            var response = _mapper.Map<OrderResponseDto>(created);

//            return ApiResponse<OrderResponseDto>.SuccessResponse(response, "Order created successfully", 201);
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<OrderResponseDto>.ErrorResponse($"Error creating order: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId)
//    {
//        try
//        {
//            var order = await _orderRepository.GetByIdAsync(orderId);
//            if (order == null)
//                return ApiResponse<OrderResponseDto>.ErrorResponse("Order not found", 404);

//            var details = await _detailRepository.GetByOrderAsync(orderId);
//            var response = _mapper.Map<OrderResponseDto>(order);
//            response.OrderDetails = _mapper.Map<List<OrderDetailResponseDto>>(details);

//            return ApiResponse<OrderResponseDto>.SuccessResponse(response, "Order retrieved successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<OrderResponseDto>.ErrorResponse($"Error retrieving order: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<PaginatedResponse<OrderResponseDto>>> GetOrdersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
//    {
//        try
//        {
//            var orders = await _orderRepository.GetByTenantAsync(tenantId, pageNumber, pageSize);
//            var response = new PaginatedResponse<OrderResponseDto>
//            {
//                Items = _mapper.Map<List<OrderResponseDto>>(orders),
//                PageNumber = pageNumber,
//                PageSize = pageSize,
//                TotalCount = orders.Count
//            };

//            return ApiResponse<PaginatedResponse<OrderResponseDto>>.SuccessResponse(response, "Orders retrieved successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<PaginatedResponse<OrderResponseDto>>.ErrorResponse($"Error retrieving orders: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<bool>> UpdateOrderAsync(int orderId, OrderUpdateRequestDto request)
//    {
//        try
//        {
//            var existing = await _orderRepository.GetByIdAsync(orderId);
//            if (existing == null)
//                return ApiResponse<bool>.ErrorResponse("Order not found", 404);

//            _mapper.Map(request, existing);
//            var result = await _orderRepository.UpdateAsync(orderId, existing);

//            if (result <= 0)
//                return ApiResponse<bool>.ErrorResponse("Failed to update order", 400);

//            return ApiResponse<bool>.SuccessResponse(true, "Order updated successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<bool>.ErrorResponse($"Error updating order: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<bool>> DeleteOrderAsync(int orderId)
//    {
//        try
//        {
//            var existing = await _orderRepository.GetByIdAsync(orderId);
//            if (existing == null)
//                return ApiResponse<bool>.ErrorResponse("Order not found", 404);

//            var result = await _orderRepository.DeleteAsync(orderId);
//            if (result <= 0)
//                return ApiResponse<bool>.ErrorResponse("Failed to delete order", 400);

//            return ApiResponse<bool>.SuccessResponse(true, "Order deleted successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<bool>.ErrorResponse($"Error deleting order: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<OrderDetailResponseDto>> AddOrderDetailAsync(OrderDetailRequestDto request)
//    {
//        try
//        {
//            var detail = _mapper.Map<OrderDetail>(request);
//            var id = await _detailRepository.CreateAsync(detail);

//            if (id <= 0)
//                return ApiResponse<OrderDetailResponseDto>.ErrorResponse("Failed to add order detail", 400);

//            var created = await _detailRepository.GetByIdAsync(id);
//            var response = _mapper.Map<OrderDetailResponseDto>(created);

//            return ApiResponse<OrderDetailResponseDto>.SuccessResponse(response, "Order detail added successfully", 201);
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<OrderDetailResponseDto>.ErrorResponse($"Error adding order detail: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<List<OrderDetailResponseDto>>> GetOrderDetailsAsync(int orderId)
//    {
//        try
//        {
//            var details = await _detailRepository.GetByOrderAsync(orderId);
//            var response = _mapper.Map<List<OrderDetailResponseDto>>(details);

//            return ApiResponse<List<OrderDetailResponseDto>>.SuccessResponse(response, "Order details retrieved successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<List<OrderDetailResponseDto>>.ErrorResponse($"Error retrieving order details: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<bool>> DeleteOrderDetailAsync(int orderDetailId)
//    {
//        try
//        {
//            var result = await _detailRepository.DeleteAsync(orderDetailId);
//            if (result <= 0)
//                return ApiResponse<bool>.ErrorResponse("Failed to delete order detail", 400);

//            return ApiResponse<bool>.SuccessResponse(true, "Order detail deleted successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<bool>.ErrorResponse($"Error deleting order detail: {ex.Message}", 500);
//        }
//    }
//}


public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _detailRepository;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService; // ✅ NEW

    public OrderService(
        IOrderRepository orderRepository,
        IOrderDetailRepository detailRepository,
        IMapper mapper,
        INotificationService notificationService) // ✅ NEW
    {
        _orderRepository = orderRepository;
        _detailRepository = detailRepository;
        _mapper = mapper;
        _notificationService = notificationService; // ✅ NEW
    }

    // ✅ UPDATED — notification call add hua
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

            // ✅ Order create hone ke baad notification bhejo — fire & forget
            //_ = Task.Run(() => _notificationService.SendOrderCreatedAsync(created!));

            await _notificationService.SendOrderCreatedAsync(created!); // ✅ direct await


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

    // ✅ UPDATED — old status save karke notification bhejo
    public async Task<ApiResponse<bool>> UpdateOrderAsync(int orderId, OrderUpdateRequestDto request)
    {
        try
        {
            var existing = await _orderRepository.GetByIdAsync(orderId);
            if (existing == null)
                return ApiResponse<bool>.ErrorResponse("Order not found", 404);

            var oldStatus = existing.Status; // ✅ purana status save karo

            _mapper.Map(request, existing);
            var result = await _orderRepository.UpdateAsync(orderId, existing);

            if (result <= 0)
                return ApiResponse<bool>.ErrorResponse("Failed to update order", 400);

            // ✅ Sirf tab bhejo jab status change hua ho
            if (oldStatus != existing.Status)
                //_ = Task.Run(() => _notificationService.SendOrderStatusUpdatedAsync(existing, oldStatus));
                await _notificationService.SendOrderStatusUpdatedAsync(existing, oldStatus); // ✅ direct await


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




//public class CustomerService : ICustomerService
//{
//    private readonly ICustomerRepository _repository;
//    private readonly IMapper _mapper;

//    public CustomerService(ICustomerRepository repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;
//    }

//    public async Task<ApiResponse<CustomerResponseDto>> CreateCustomerAsync(CustomerCreateDto request)
//    {
//        try
//        {
//            var customer = _mapper.Map<Customer>(request);
//            customer.Status = true;

//            var id = await _repository.CreateAsync(customer);
//            if (id <= 0)
//                return ApiResponse<CustomerResponseDto>.ErrorResponse("Failed to create customer", 400);

//            var created = await _repository.GetByIdAsync(id);
//            var response = _mapper.Map<CustomerResponseDto>(created);

//            return ApiResponse<CustomerResponseDto>.SuccessResponse(response, "Customer created successfully", 201);
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<CustomerResponseDto>.ErrorResponse($"Error creating customer: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(int customerId)
//    {
//        try
//        {
//            var customer = await _repository.GetByIdAsync(customerId);
//            if (customer == null)
//                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer not found", 404);

//            var response = _mapper.Map<CustomerResponseDto>(customer);
//            return ApiResponse<CustomerResponseDto>.SuccessResponse(response, "Customer retrieved successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<CustomerResponseDto>.ErrorResponse($"Error retrieving customer: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<PaginatedResponse<CustomerResponseDto>>> GetCustomersByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
//    {
//        try
//        {
//            var customers = await _repository.GetByTenantAsync(tenantId, pageNumber, pageSize);

//            var response = new PaginatedResponse<CustomerResponseDto>
//            {
//                Items = _mapper.Map<List<CustomerResponseDto>>(customers),
//                PageNumber = pageNumber,
//                PageSize = pageSize,
//                TotalCount = customers.Count
//            };

//            return ApiResponse<PaginatedResponse<CustomerResponseDto>>.SuccessResponse(response, "Customers retrieved successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<PaginatedResponse<CustomerResponseDto>>.ErrorResponse($"Error retrieving customers: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<bool>> UpdateCustomerAsync(int customerId, CustomerUpdateDto request)
//    {
//        try
//        {
//            var existing = await _repository.GetByIdAsync(customerId);
//            if (existing == null)
//                return ApiResponse<bool>.ErrorResponse("Customer not found", 404);

//            _mapper.Map(request, existing);

//            var result = await _repository.UpdateAsync(customerId, existing);
//            if (result <= 0)
//                return ApiResponse<bool>.ErrorResponse("Failed to update customer", 400);

//            return ApiResponse<bool>.SuccessResponse(true, "Customer updated successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<bool>.ErrorResponse($"Error updating customer: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<bool>> DeleteCustomerAsync(int customerId)
//    {
//        try
//        {
//            var existing = await _repository.GetByIdAsync(customerId);
//            if (existing == null)
//                return ApiResponse<bool>.ErrorResponse("Customer not found", 404);

//            var result = await _repository.DeleteAsync(customerId);
//            if (result <= 0)
//                return ApiResponse<bool>.ErrorResponse("Failed to delete customer", 400);

//            return ApiResponse<bool>.SuccessResponse(true, "Customer deleted successfully");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<bool>.ErrorResponse($"Error deleting customer: {ex.Message}", 500);
//        }
//    }




//    // Program.cs mein ye already hoga, confirm karo:
//    // using BCrypt.Net; -- NuGet: BCrypt.Net-Next

//    public async Task<ApiResponse<CustomerLoginResponseDto>> RegisterAsync(CustomerRegisterDto request)
//    {
//        try
//        {
//            // Email already exist karta hai?
//            var existing = await _repository.GetByEmailAsync(request.Email, request.TenantId);
//            if (existing != null)
//                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Email already registered", 400);

//            var customer = new Customer
//            {
//                TenantId = request.TenantId,
//                FirstName = request.FirstName,
//                LastName = request.LastName,
//                Email = request.Email,
//                Password = BCrypt.Net.BCrypt.HashPassword(request.Password), // hash karo
//                Status = true,
//                CreatedDate = DateTime.Now
//            };

//            var id = await _repository.CreateAsync(customer);
//            if (id <= 0)
//                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Registration failed", 400);

//            var created = await _repository.GetByIdAsync(id);
//            var token = _jwtHelper.GenerateCustomerToken(created!);

//            var response = new CustomerLoginResponseDto
//            {
//                CustomerId = created!.CustomerId,
//                TenantId = created.TenantId,
//                FirstName = created.FirstName,
//                LastName = created.LastName,
//                Email = created.Email ?? "",
//                Token = token
//            };

//            return ApiResponse<CustomerLoginResponseDto>.SuccessResponse(response, "Registered successfully", 201);
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<CustomerLoginResponseDto>.ErrorResponse($"Error: {ex.Message}", 500);
//        }
//    }

//    public async Task<ApiResponse<CustomerLoginResponseDto>> LoginAsync(CustomerLoginDto request)
//    {
//        try
//        {
//            var customer = await _repository.GetByEmailAsync(request.Email, request.TenantId);
//            if (customer == null)
//                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Invalid email or password", 401);

//            if (!BCrypt.Net.BCrypt.Verify(request.Password, customer.Password))
//                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Invalid email or password", 401);

//            if (!customer.Status)
//                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Account is inactive", 403);

//            var token = _jwtHelper.GenerateCustomerToken(customer);

//            var response = new CustomerLoginResponseDto
//            {
//                CustomerId = customer.CustomerId,
//                TenantId = customer.TenantId,
//                FirstName = customer.FirstName,
//                LastName = customer.LastName,
//                Email = customer.Email ?? "",
//                Token = token
//            };

//            return ApiResponse<CustomerLoginResponseDto>.SuccessResponse(response, "Login successful");
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<CustomerLoginResponseDto>.ErrorResponse($"Error: {ex.Message}", 500);
//        }
//    }
//}





/// <summary>
/// CustomerService Implementation
/// </summary>


public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IOrderRepository _orderRepository; // ✅ add
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration; // ✅ correctly declared

        public CustomerService(
            ICustomerRepository repository,
            IOrderRepository orderRepository,        // ✅ add
            IMapper mapper,
            IConfiguration configuration) // ✅ correctly injected
        {
            _repository = repository;
            _orderRepository = orderRepository;      // ✅ add
            _mapper = mapper;
            _configuration = configuration; // ✅ correctly assigned
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


        //new

        public async Task<ApiResponse<List<OrderResponseDto>>> GetMyOrdersAsync(int customerId, int tenantId)
        {
            try
            {
                var orders = await _orderRepository.GetByUserAsync(tenantId, customerId);
                var response = _mapper.Map<List<OrderResponseDto>>(orders);
                return ApiResponse<List<OrderResponseDto>>.SuccessResponse(response, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderResponseDto>>.ErrorResponse($"Error: {ex.Message}", 500);
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

        public async Task<ApiResponse<CustomerLoginResponseDto>> RegisterAsync(CustomerRegisterDto request)
        {
            try
            {
                var existing = await _repository.GetByEmailAsync(request.Email, request.TenantId);
                if (existing != null)
                    return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Email already registered", 400);

                var customer = new Customer
                {
                    TenantId = request.TenantId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Status = true,
                    CreatedDate = DateTime.Now
                };

                var id = await _repository.CreateAsync(customer);
                if (id <= 0)
                    return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Registration failed", 400);

                var created = await _repository.GetByIdAsync(id);
                var token = GenerateCustomerToken(created!); // ✅ _jwtHelper nahi, private method

                var response = new CustomerLoginResponseDto
                {
                    CustomerId = created!.CustomerId,
                    TenantId = created.TenantId,
                    FirstName = created.FirstName,
                    LastName = created.LastName,
                    Email = created.Email ?? "",
                    Token = token
                };

                return ApiResponse<CustomerLoginResponseDto>.SuccessResponse(response, "Registered successfully", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse($"Error: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<CustomerLoginResponseDto>> LoginAsync(CustomerLoginDto request)
        {
            try
            {
                var customer = await _repository.GetByEmailAsync(request.Email, request.TenantId);
                if (customer == null)
                    return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Invalid email or password", 401);

                if (!BCrypt.Net.BCrypt.Verify(request.Password, customer.Password))
                    return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Invalid email or password", 401);

                if (!customer.Status)
                    return ApiResponse<CustomerLoginResponseDto>.ErrorResponse("Account is inactive", 403);

                var token = GenerateCustomerToken(customer); // ✅ private method call

                var response = new CustomerLoginResponseDto
                {
                    CustomerId = customer.CustomerId,
                    TenantId = customer.TenantId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email ?? "",
                    Token = token
                };

                return ApiResponse<CustomerLoginResponseDto>.SuccessResponse(response, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerLoginResponseDto>.ErrorResponse($"Error: {ex.Message}", 500);
            }
        }

        // ✅ Private JWT token generator — _jwtHelper ki zaroorat nahi
        private string GenerateCustomerToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("CustomerId", customer.CustomerId.ToString()),
                new Claim("TenantId", customer.TenantId.ToString()),
                new Claim(ClaimTypes.Email, customer.Email ?? ""),
                new Claim(ClaimTypes.GivenName, customer.FirstName),
                new Claim("Role", "Customer")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }




/// <summary>
/// TenantSettingsService Implementation
/// </summary>


public class TenantSettingsService : ITenantSettingsService
{
    private readonly ITenantSettingsRepository _repository;
    private readonly IMapper _mapper;

    public TenantSettingsService(ITenantSettingsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<TenantSettingsResponseDto>> GetByTenantAsync(int tenantId)
    {
        try
        {
            var settings = await _repository.GetByTenantAsync(tenantId);

            // Return defaults if not configured yet
            if (settings == null)
                settings = new TenantSettings { TenantId = tenantId };

            var response = _mapper.Map<TenantSettingsResponseDto>(settings);
            return ApiResponse<TenantSettingsResponseDto>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantSettingsResponseDto>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<TenantSettingsResponseDto>> UpsertAsync(TenantSettingsRequestDto request)
    {
        try
        {
            var entity = _mapper.Map<TenantSettings>(request);
            var result = await _repository.UpsertAsync(entity);

            var response = _mapper.Map<TenantSettingsResponseDto>(result);
            return ApiResponse<TenantSettingsResponseDto>.SuccessResponse(response, "Settings saved!");
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantSettingsResponseDto>.ErrorResponse(ex.Message, 500);
        }
    }
}



/// <summary>
/// TenantSliderService Implementation
/// </summary>
/// 
public class TenantSliderService : ITenantSliderService
{
    private readonly ITenantSliderRepository _repository;
    private readonly IMapper _mapper;

    public TenantSliderService(ITenantSliderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<TenantSliderResponseDto>>> GetActiveAsync(int tenantId)
    {
        try
        {
            var sliders = await _repository.GetActiveByTenantAsync(tenantId);
            return ApiResponse<List<TenantSliderResponseDto>>.SuccessResponse(
                _mapper.Map<List<TenantSliderResponseDto>>(sliders));
        }
        catch (Exception ex)
        {
            return ApiResponse<List<TenantSliderResponseDto>>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<List<TenantSliderResponseDto>>> GetAllAsync(int tenantId)
    {
        try
        {
            var sliders = await _repository.GetAllByTenantAsync(tenantId);
            return ApiResponse<List<TenantSliderResponseDto>>.SuccessResponse(
                _mapper.Map<List<TenantSliderResponseDto>>(sliders));
        }
        catch (Exception ex)
        {
            return ApiResponse<List<TenantSliderResponseDto>>.ErrorResponse(ex.Message, 500);
        }
    }

    //public async Task<ApiResponse<int>> AddAsync(TenantSliderRequestDto request)
    //{
    //    try
    //    {
    //        var entity = _mapper.Map<TenantSlider>(request);
    //        var id = await _repository.AddAsync(entity);
    //        return ApiResponse<int>.SuccessResponse(id, "Slider added!", 201);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ApiResponse<int>.ErrorResponse(ex.Message, 500);
    //    }
    //}


    public async Task<ApiResponse<int>> AddAsync(TenantSliderRequestDto request)
    {
        try
        {
            var entity = new TenantSlider
            {
                TenantId = request.TenantId,
                ImageUrl = request.ImageUrl,
                Title = request.Title,
                Subtitle = request.Subtitle,
                ButtonText = request.ButtonText,
                ButtonLink = request.ButtonLink,
                OrderNo = request.OrderNo,
                IsActive = request.IsActive,
                LayoutType = request.LayoutType,
                BgColor = request.BgColor,
                TextColor = request.TextColor,
                OverlayOpacity = request.OverlayOpacity,
                IsPresetImage = request.IsPresetImage
            };
            var id = await _repository.AddAsync(entity);
            return ApiResponse<int>.SuccessResponse(id, "Slider added!", 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<int>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateAsync(int sliderId, UpdateSliderRequestDto request)
    {
        try
        {
            var entity = _mapper.Map<TenantSlider>(request);
            await _repository.UpdateAsync(sliderId, entity);
            return ApiResponse<bool>.SuccessResponse(true, "Slider updated!");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<List<TenantSliderResponseDto>>> GetPresetImagesAsync(int tenantId)
    {
        try
        {
            var sliders = await _repository.GetPresetImagesAsync(tenantId);
            return ApiResponse<List<TenantSliderResponseDto>>.SuccessResponse(
                _mapper.Map<List<TenantSliderResponseDto>>(sliders));
        }
        catch (Exception ex)
        {
            return ApiResponse<List<TenantSliderResponseDto>>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int sliderId)
    {
        try
        {
            await _repository.DeleteAsync(sliderId);
            return ApiResponse<bool>.SuccessResponse(true, "Slider deleted!");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse(ex.Message, 500);
        }
    }
}




// =============================================
// ✅ NAYE — SIRF YE 2 ADD HUE HAIN
// =============================================

/// <summary>
/// TenantIntegrationService Implementation
/// Email + WhatsApp settings save/get + test
/// </summary>
public class TenantIntegrationService : ITenantIntegrationService
{
    private readonly ITenantIntegrationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;

    public TenantIntegrationService(
        ITenantIntegrationRepository repository,
        IMapper mapper,
        IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiResponse<TenantIntegrationResponseDto>> GetByTenantAsync(int tenantId)
    {
        try
        {
            var integration = await _repository.GetByTenantAsync(tenantId);

            // Agar koi settings nahi hain to empty defaults return karo
            if (integration == null)
                integration = new TenantIntegration { TenantId = tenantId };

            var response = _mapper.Map<TenantIntegrationResponseDto>(integration);

            // Sensitive keys mask karo — frontend pe *** dikhao
            if (!string.IsNullOrEmpty(response.EmailApiKey))
                response.EmailApiKey = "****";
            if (!string.IsNullOrEmpty(response.WhatsAppToken))
                response.WhatsAppToken = "****";

            return ApiResponse<TenantIntegrationResponseDto>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantIntegrationResponseDto>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<bool>> UpsertAsync(TenantIntegrationRequestDto request)
    {
        try
        {
            var result = await _repository.UpsertAsync(request.TenantId, request);
            if (result == null)
                return ApiResponse<bool>.ErrorResponse("Failed to save integrations", 400);

            return ApiResponse<bool>.SuccessResponse(true, "Integration settings saved!");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int tenantId)
    {
        try
        {
            var deleted = await _repository.DeleteAsync(tenantId);
            if (!deleted)
                return ApiResponse<bool>.ErrorResponse("No integration found to delete", 404);

            return ApiResponse<bool>.SuccessResponse(true, "Integration settings deleted!");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse(ex.Message, 500);
        }
    }

    /// <summary>
    /// Test Email — SendGrid API se real email bhejo
    /// </summary>
    public async Task<ApiResponse<bool>> TestEmailAsync(int tenantId, string toEmail)
    {
        try
        {
            var integration = await _repository.GetByTenantAsync(tenantId);

            if (integration == null || !integration.IsEmailEnabled)
                return ApiResponse<bool>.ErrorResponse("Email integration not configured or disabled", 400);

            if (string.IsNullOrEmpty(integration.EmailApiKey))
                return ApiResponse<bool>.ErrorResponse("Email API Key missing", 400);

            // SendGrid API call
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", integration.EmailApiKey);

            var payload = new
            {
                personalizations = new[]
                {
                    new { to = new[] { new { email = toEmail } } }
                },
                from = new
                {
                    email = integration.EmailSenderAddress ?? "noreply@test.com",
                    name = integration.EmailSenderName ?? "Test"
                },
                subject = "Test Email from your Store",
                content = new[]
                {
                    new { type = "text/plain", value = "Yeh ek test email hai. Aapka email integration kaam kar raha hai!" }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.sendgrid.com/v3/mail/send", content);

            if (response.IsSuccessStatusCode)
                return ApiResponse<bool>.SuccessResponse(true, $"Test email sent to {toEmail}!");

            var error = await response.Content.ReadAsStringAsync();
            return ApiResponse<bool>.ErrorResponse($"SendGrid error: {error}", 400);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error sending test email: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Test WhatsApp — Meta Cloud API se real message bhejo
    /// </summary>
    public async Task<ApiResponse<bool>> TestWhatsAppAsync(int tenantId, string toPhone)
    {
        try
        {
            var integration = await _repository.GetByTenantAsync(tenantId);

            if (integration == null || !integration.IsWhatsAppEnabled)
                return ApiResponse<bool>.ErrorResponse("WhatsApp integration not configured or disabled", 400);

            if (string.IsNullOrEmpty(integration.WhatsAppToken))
                return ApiResponse<bool>.ErrorResponse("WhatsApp Token missing", 400);

            if (string.IsNullOrEmpty(integration.WhatsAppPhoneNumberId))
                return ApiResponse<bool>.ErrorResponse("WhatsApp Phone Number ID missing", 400);

            // Meta Cloud API call
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", integration.WhatsAppToken);

            var payload = new
            {
                messaging_product = "whatsapp",
                to = toPhone,
                type = "text",
                text = new { body = "Yeh ek test message hai. Aapka WhatsApp integration kaam kar raha hai!" }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var url = $"https://graph.facebook.com/v18.0/{integration.WhatsAppPhoneNumberId}/messages";
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return ApiResponse<bool>.SuccessResponse(true, $"Test WhatsApp sent to {toPhone}!");

            var error = await response.Content.ReadAsStringAsync();
            return ApiResponse<bool>.ErrorResponse($"Meta API error: {error}", 400);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error sending test WhatsApp: {ex.Message}", 500);
        }
    }
}

/// <summary>
/// NotificationLogService Implementation
/// Notification history fetch karne ke liye
/// </summary>
public class NotificationLogService : INotificationLogService
{
    private readonly INotificationLogRepository _repository;
    private readonly IMapper _mapper;

    public NotificationLogService(INotificationLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<NotificationLogResponseDto>>> GetByTenantAsync(int tenantId, int page = 1, int pageSize = 20)
    {
        try
        {
            var logs = await _repository.GetByTenantAsync(tenantId, page, pageSize);
            var response = _mapper.Map<List<NotificationLogResponseDto>>(logs);
            return ApiResponse<List<NotificationLogResponseDto>>.SuccessResponse(response, "Logs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<NotificationLogResponseDto>>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<List<NotificationLogResponseDto>>> GetByOrderAsync(int tenantId, int orderId)
    {
        try
        {
            var logs = await _repository.GetByOrderAsync(tenantId, orderId);
            var response = _mapper.Map<List<NotificationLogResponseDto>>(logs);
            return ApiResponse<List<NotificationLogResponseDto>>.SuccessResponse(response, "Order logs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<NotificationLogResponseDto>>.ErrorResponse(ex.Message, 500);
        }
    }

    public async Task<ApiResponse<List<NotificationLogResponseDto>>> GetFailedAsync(int tenantId)
    {
        try
        {
            var logs = await _repository.GetFailedAsync(tenantId);
            var response = _mapper.Map<List<NotificationLogResponseDto>>(logs);
            return ApiResponse<List<NotificationLogResponseDto>>.SuccessResponse(response, "Failed logs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<NotificationLogResponseDto>>.ErrorResponse(ex.Message, 500);
        }
    }
}





//public class NotificationService : INotificationService
//{
//    private readonly ITenantIntegrationRepository _integrationRepo;
//    private readonly INotificationLogRepository _logRepo;
//    private readonly IHttpClientFactory _httpClientFactory;

//    public NotificationService(
//        ITenantIntegrationRepository integrationRepo,
//        INotificationLogRepository logRepo,
//        IHttpClientFactory httpClientFactory)
//    {
//        _integrationRepo = integrationRepo;
//        _logRepo = logRepo;
//        _httpClientFactory = httpClientFactory;
//    }

//    public async Task SendOrderCreatedAsync(Order order)
//    {
//        var integration = await _integrationRepo.GetByTenantAsync(order.TenantId);
//        if (integration == null) return;

//        if (integration.IsEmailEnabled && !string.IsNullOrEmpty(order.CustomerEmail))
//        {
//            var subject = $"✅ Order Confirmed #{order.OrderId}";
//            var body = $@"
//                <div style='font-family:Arial;max-width:600px;margin:auto'>
//                    <h2 style='color:#ea6c2d'>Order Confirmed!</h2>
//                    <p>Dear <strong>{order.CustomerName}</strong>,</p>
//                    <p>Your order has been placed successfully.</p>
//                    <table style='width:100%;border-collapse:collapse'>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Order ID</b></td>
//                            <td style='padding:8px;border:1px solid #ddd'>#{order.OrderId}</td></tr>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Total Amount</b></td>
//                            <td style='padding:8px;border:1px solid #ddd'>Rs. {order.TotalAmount:N0}
//</td></tr>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Status</b></td>
//                            <td style='padding:8px;border:1px solid #ddd'>{order.Status}</td></tr>
//                    </table>
//                    <p style='margin-top:20px'>Thank you for shopping with us!</p>
//                </div>";

//            await SendEmailAsync(integration, order.CustomerEmail, subject, body,
//                order.TenantId, order.OrderId, NotificationEvents.OrderCreated);
//        }

//        if (integration.IsWhatsAppEnabled && !string.IsNullOrEmpty(order.CustomerPhone))
//        {
//            var message = $"✅ *Order Confirmed!*\n\n" +
//                         $"Hello *{order.CustomerName}*,\n" +
//                         $"Your order *#{order.OrderId}* has been placed.\n" +
//                         $"💰 Total: *{order.TotalAmount:C}*\n" +
//                         $"📦 Status: *{order.Status}*\n\n" +
//                         $"Thank you for shopping with us!";

//            await SendWhatsAppAsync(integration, order.CustomerPhone, message,
//                order.TenantId, order.OrderId, NotificationEvents.OrderCreated);
//        }
//    }

//    public async Task SendOrderStatusUpdatedAsync(Order order, string oldStatus)
//    {
//        var integration = await _integrationRepo.GetByTenantAsync(order.TenantId);
//        if (integration == null) return;

//        var emoji = order.Status switch
//        {
//            "Confirmed" => "✅",
//            "Shipped" => "🚚",
//            "Delivered" => "🎉",
//            "Cancelled" => "❌",
//            _ => "📦"
//        };

//        if (integration.IsEmailEnabled && !string.IsNullOrEmpty(order.CustomerEmail))
//        {
//            var subject = $"{emoji} Order #{order.OrderId} Status Updated — {order.Status}";
//            var body = $@"
//                <div style='font-family:Arial;max-width:600px;margin:auto'>
//                    <h2 style='color:#ea6c2d'>{emoji} Order Status Updated</h2>
//                    <p>Dear <strong>{order.CustomerName}</strong>,</p>
//                    <p>Your order status has been updated.</p>
//                    <table style='width:100%;border-collapse:collapse'>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Order ID</b></td>
//                            <td style='padding:8px;border:1px solid #ddd'>#{order.OrderId}</td></tr>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Previous Status</b></td>
//                            <td style='padding:8px;border:1px solid #ddd'>{oldStatus}</td></tr>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>New Status</b></td>
//                            <td style='padding:8px;border:1px solid #ddd;color:green'><b>{order.Status}</b></td></tr>
//                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Total Amount</b></td>
//                            <td style='padding:8px;border:1px solid #ddd'>{order.TotalAmount:C}</td></tr>
//                    </table>
//                    <p style='margin-top:20px'>Thank you for shopping with us!</p>
//                </div>";

//            await SendEmailAsync(integration, order.CustomerEmail, subject, body,
//                order.TenantId, order.OrderId, NotificationEvents.OrderConfirmed);
//        }

//        if (integration.IsWhatsAppEnabled && !string.IsNullOrEmpty(order.CustomerPhone))
//        {
//            var message = $"{emoji} *Order Status Updated!*\n\n" +
//                         $"Hello *{order.CustomerName}*,\n" +
//                         $"Order *#{order.OrderId}* status changed:\n" +
//                         $"📌 Previous: *{oldStatus}*\n" +
//                         $"✅ New: *{order.Status}*\n\n" +
//                         $"Thank you!";

//            await SendWhatsAppAsync(integration, order.CustomerPhone, message,
//                order.TenantId, order.OrderId, NotificationEvents.OrderConfirmed);
//        }
//    }

//    private async Task SendEmailAsync(
//        TenantIntegration integration,
//        string toEmail, string subject, string body,
//        int tenantId, int orderId, string eventType)
//    {
//        var log = new NotificationLog
//        {
//            TenantId = tenantId,
//            OrderId = orderId,
//            Channel = NotificationChannels.Email,
//            EventType = eventType,
//            RecipientContact = toEmail,
//            SentAt = DateTime.Now
//        };

//        try
//        {
//            var client = _httpClientFactory.CreateClient();
//            client.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", integration.EmailApiKey);

//            var payload = new
//            {
//                personalizations = new[] { new { to = new[] { new { email = toEmail } } } },
//                from = new
//                {
//                    email = integration.EmailSenderAddress ?? "noreply@store.com",
//                    name = integration.EmailSenderName ?? "Store"
//                },
//                subject = subject,
//                content = new[] { new { type = "text/html", value = body } }
//            };

//            var json = JsonSerializer.Serialize(payload);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            var response = await client.PostAsync("https://api.sendgrid.com/v3/mail/send", content);

//            log.Status = response.IsSuccessStatusCode
//                ? NotificationStatus.Sent
//                : NotificationStatus.Failed;

//            if (!response.IsSuccessStatusCode)
//                log.ErrorMessage = await response.Content.ReadAsStringAsync();
//        }
//        catch (Exception ex)
//        {
//            log.Status = NotificationStatus.Failed;
//            log.ErrorMessage = ex.Message;
//        }
//        finally
//        {
//            await _logRepo.InsertAsync(log);
//        }
//    }

//    private async Task SendWhatsAppAsync(
//        TenantIntegration integration,
//        string toPhone, string message,
//        int tenantId, int orderId, string eventType)
//    {
//        var log = new NotificationLog
//        {
//            TenantId = tenantId,
//            OrderId = orderId,
//            Channel = NotificationChannels.WhatsApp,
//            EventType = eventType,
//            RecipientContact = toPhone,
//            SentAt = DateTime.Now
//        };

//        try
//        {
//            var client = _httpClientFactory.CreateClient();
//            client.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", integration.WhatsAppToken);

//            var payload = new
//            {
//                messaging_product = "whatsapp",
//                to = toPhone,
//                type = "text",
//                text = new { body = message }
//            };

//            var json = JsonSerializer.Serialize(payload);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            var url = $"https://graph.facebook.com/v18.0/{integration.WhatsAppPhoneNumberId}/messages";
//            var response = await client.PostAsync(url, content);

//            log.Status = response.IsSuccessStatusCode
//                ? NotificationStatus.Sent
//                : NotificationStatus.Failed;

//            if (!response.IsSuccessStatusCode)
//                log.ErrorMessage = await response.Content.ReadAsStringAsync();
//        }
//        catch (Exception ex)
//        {
//            log.Status = NotificationStatus.Failed;
//            log.ErrorMessage = ex.Message;
//        }
//        finally
//        {
//            await _logRepo.InsertAsync(log);
//        }
//    }
//}



public class NotificationService : INotificationService
{
    private readonly ITenantIntegrationRepository _integrationRepo;
    private readonly INotificationLogRepository _logRepo;
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationService(
        ITenantIntegrationRepository integrationRepo,
        INotificationLogRepository logRepo,
        IHttpClientFactory httpClientFactory)
    {
        _integrationRepo = integrationRepo;
        _logRepo = logRepo;
        _httpClientFactory = httpClientFactory;
    }

    public async Task SendOrderCreatedAsync(Order order)
    {
        var integration = await _integrationRepo.GetByTenantAsync(order.TenantId);
        if (integration == null) return;

        if (integration.IsEmailEnabled && !string.IsNullOrEmpty(order.CustomerEmail))
        {
            var subject = $"✅ Order Confirmed #{order.OrderId}";
            var body = $@"
                <div style='font-family:Arial;max-width:600px;margin:auto'>
                    <h2 style='color:#ea6c2d'>Order Confirmed!</h2>
                    <p>Dear <strong>{order.CustomerName}</strong>,</p>
                    <p>Your order has been placed successfully.</p>
                    <table style='width:100%;border-collapse:collapse'>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Order ID</b></td>
                            <td style='padding:8px;border:1px solid #ddd'>#{order.OrderId}</td></tr>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Total Amount</b></td>
                            <td style='padding:8px;border:1px solid #ddd'>Rs. {order.TotalAmount:N0}</td></tr>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Status</b></td>
                            <td style='padding:8px;border:1px solid #ddd'>{order.Status}</td></tr>
                    </table>
                    <p style='margin-top:20px'>Thank you for shopping with us!</p>
                </div>";

            await SendEmailAsync(integration, order.CustomerEmail, subject, body,
                order.TenantId, order.OrderId, NotificationEvents.OrderCreated);
        }

        if (integration.IsWhatsAppEnabled && !string.IsNullOrEmpty(order.CustomerPhone))
        {
            var message = $"✅ *Order Confirmed!*\n\n" +
                         $"Hello *{order.CustomerName}*,\n" +
                         $"Your order *#{order.OrderId}* has been placed.\n" +
                         $"💰 Total: *Rs. {order.TotalAmount:N0}*\n" +
                         $"📦 Status: *{order.Status}*\n\n" +
                         $"Thank you for shopping with us!";

            await SendWhatsAppAsync(integration, order.CustomerPhone, message,
                order.TenantId, order.OrderId, NotificationEvents.OrderCreated);
        }
    }

    public async Task SendOrderStatusUpdatedAsync(Order order, string oldStatus)
    {
        var integration = await _integrationRepo.GetByTenantAsync(order.TenantId);
        if (integration == null) return;

        var emoji = order.Status switch
        {
            "Confirmed" => "✅",
            "Shipped" => "🚚",
            "Delivered" => "🎉",
            "Cancelled" => "❌",
            _ => "📦"
        };

        if (integration.IsEmailEnabled && !string.IsNullOrEmpty(order.CustomerEmail))
        {
            var subject = $"{emoji} Order #{order.OrderId} Status Updated — {order.Status}";
            var body = $@"
                <div style='font-family:Arial;max-width:600px;margin:auto'>
                    <h2 style='color:#ea6c2d'>{emoji} Order Status Updated</h2>
                    <p>Dear <strong>{order.CustomerName}</strong>,</p>
                    <p>Your order status has been updated.</p>
                    <table style='width:100%;border-collapse:collapse'>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Order ID</b></td>
                            <td style='padding:8px;border:1px solid #ddd'>#{order.OrderId}</td></tr>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Previous Status</b></td>
                            <td style='padding:8px;border:1px solid #ddd'>{oldStatus}</td></tr>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>New Status</b></td>
                            <td style='padding:8px;border:1px solid #ddd;color:green'><b>{order.Status}</b></td></tr>
                        <tr><td style='padding:8px;border:1px solid #ddd'><b>Total Amount</b></td>
                            <td style='padding:8px;border:1px solid #ddd'>Rs. {order.TotalAmount:N0}</td></tr>
                    </table>
                    <p style='margin-top:20px'>Thank you for shopping with us!</p>
                </div>";

            await SendEmailAsync(integration, order.CustomerEmail, subject, body,
                order.TenantId, order.OrderId, NotificationEvents.OrderConfirmed);
        }

        if (integration.IsWhatsAppEnabled && !string.IsNullOrEmpty(order.CustomerPhone))
        {
            var message = $"{emoji} *Order Status Updated!*\n\n" +
                         $"Hello *{order.CustomerName}*,\n" +
                         $"Order *#{order.OrderId}* status changed:\n" +
                         $"📌 Previous: *{oldStatus}*\n" +
                         $"✅ New: *{order.Status}*\n\n" +
                         $"Thank you!";

            await SendWhatsAppAsync(integration, order.CustomerPhone, message,
                order.TenantId, order.OrderId, NotificationEvents.OrderConfirmed);
        }
    }

    private async Task SendEmailAsync(
        TenantIntegration integration,
        string toEmail, string subject, string body,
        int tenantId, int orderId, string eventType)
    {
        var log = new NotificationLog
        {
            TenantId = tenantId,
            OrderId = orderId,
            Channel = NotificationChannels.Email,
            EventType = eventType,
            RecipientContact = toEmail,
            SentAt = DateTime.Now
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", integration.EmailApiKey);

            var payload = new
            {
                personalizations = new[] { new { to = new[] { new { email = toEmail } } } },
                from = new
                {
                    email = integration.EmailSenderAddress ?? "noreply@store.com",
                    name = integration.EmailSenderName ?? "Store"
                },
                subject = subject,
                content = new[] { new { type = "text/html", value = body } }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.sendgrid.com/v3/mail/send", content);

            log.Status = response.IsSuccessStatusCode
                ? NotificationStatus.Sent
                : NotificationStatus.Failed;

            if (!response.IsSuccessStatusCode)
                log.ErrorMessage = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            log.Status = NotificationStatus.Failed;
            log.ErrorMessage = ex.Message;
        }
        finally
        {
            await _logRepo.InsertAsync(log);
        }
    }

    private async Task SendWhatsAppAsync(
        TenantIntegration integration,
        string toPhone, string message,
        int tenantId, int orderId, string eventType)
    {
        var log = new NotificationLog
        {
            TenantId = tenantId,
            OrderId = orderId,
            Channel = NotificationChannels.WhatsApp,
            EventType = eventType,
            RecipientContact = toPhone,
            SentAt = DateTime.Now
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", integration.WhatsAppToken);

            var payload = new
            {
                messaging_product = "whatsapp",
                to = toPhone,
                type = "text",
                text = new { body = message }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"https://graph.facebook.com/v18.0/{integration.WhatsAppPhoneNumberId}/messages";
            var response = await client.PostAsync(url, content);

            log.Status = response.IsSuccessStatusCode
                ? NotificationStatus.Sent
                : NotificationStatus.Failed;

            if (!response.IsSuccessStatusCode)
                log.ErrorMessage = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            log.Status = NotificationStatus.Failed;
            log.ErrorMessage = ex.Message;
        }
        finally
        {
            await _logRepo.InsertAsync(log);
        }
    }
}
