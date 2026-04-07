# API Reference - E-Commerce Admin Panel

## Base URL
```
https://localhost:7xxx/api
```

## Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {},
  "statusCode": 200,
  "timestamp": "2024-12-19T10:30:00Z"
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "statusCode": 400,
  "timestamp": "2024-12-19T10:30:00Z"
}
```

## Endpoints

### PRODUCTS API

#### Create Product
```
POST /api/product/create
Content-Type: application/json

{
  "tenantId": 1,
  "name": "Product Name",
  "description": "Product description",
  "price": 99.99,
  "imageUrl": "http://example.com/image.jpg",
  "categoryId": 1,
  "stockQty": 50
}

Response: 201 Created
```

#### Get Product by ID
```
GET /api/product/{productId}

Response: 200 OK
```

#### Get Products by Tenant
```
GET /api/product/tenant/{tenantId}?pageNumber=1&pageSize=10

Response: 200 OK with pagination
```

#### Get Products by Category
```
GET /api/product/category/{categoryId}?pageNumber=1&pageSize=10

Response: 200 OK with pagination
```

#### Update Product
```
PUT /api/product/{productId}
Content-Type: application/json

{
  "name": "Updated Name",
  "description": "Updated description",
  "price": 129.99,
  "imageUrl": "http://example.com/new-image.jpg",
  "categoryId": 2,
  "stockQty": 45,
  "status": true
}

Response: 200 OK
```

#### Delete Product
```
DELETE /api/product/{productId}

Response: 200 OK
```

---

### CATEGORIES API

#### Create Category
```
POST /api/category/create

{
  "tenantId": 1,
  "name": "Electronics",
  "parentCategoryId": null,
  "status": true
}
```

#### Get Category by ID
```
GET /api/category/{categoryId}
```

#### Get Categories by Tenant
```
GET /api/category/tenant/{tenantId}?pageNumber=1&pageSize=10
```

#### Update Category
```
PUT /api/category/{categoryId}

{
  "tenantId": 1,
  "name": "Updated Category",
  "parentCategoryId": null,
  "status": true
}
```

#### Delete Category
```
DELETE /api/category/{categoryId}
```

---

### TENANTS API

#### Create Tenant
```
POST /api/tenant/create

{
  "name": "Company Name",
  "domain": "company.domain.com",
  "logo": "http://example.com/logo.png",
  "themeColor": "#FF5733"
}
```

#### Get Tenant by ID
```
GET /api/tenant/{tenantId}
```

#### Get All Tenants
```
GET /api/tenant?pageNumber=1&pageSize=10
```

#### Update Tenant
```
PUT /api/tenant/{tenantId}

{
  "name": "Updated Name",
  "domain": "updated.domain.com",
  "logo": "http://example.com/new-logo.png",
  "themeColor": "#FFFFFF"
}
```

#### Delete Tenant
```
DELETE /api/tenant/{tenantId}
```

---

### USERS API

#### Create User
```
POST /api/user/create

{
  "tenantId": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "passwordHash": "hashed_password_here",
  "role": "Admin"
}
```

#### Get User by ID
```
GET /api/user/{userId}
```

#### Get Users by Tenant
```
GET /api/user/tenant/{tenantId}?pageNumber=1&pageSize=10
```

#### Update User
```
PUT /api/user/{userId}

{
  "name": "Jane Doe",
  "email": "jane@example.com",
  "role": "Manager",
  "status": true
}
```

#### Delete User
```
DELETE /api/user/{userId}
```

---

### ORDERS API

#### Create Order
```
POST /api/order/create

{
  "tenantId": 1,
  "customerName": "Customer Name",
  "customerEmail": "customer@example.com",
  "customerPhone": "+1234567890",
  "totalAmount": 299.99,
  "status": "Pending"
}

Response: 201 Created with OrderId
```

#### Get Order by ID
```
GET /api/order/{orderId}

Response: Includes OrderDetails array
```

#### Get Orders by Tenant
```
GET /api/order/tenant/{tenantId}?pageNumber=1&pageSize=10
```

#### Update Order
```
PUT /api/order/{orderId}

{
  "customerName": "Updated Name",
  "customerEmail": "updated@example.com",
  "customerPhone": "+9876543210",
  "totalAmount": 349.99,
  "status": "Processing"
}
```

#### Delete Order
```
DELETE /api/order/{orderId}
```

#### Add Order Detail (Line Item)
```
POST /api/order/detail/add

{
  "orderId": 1,
  "productId": 5,
  "quantity": 2,
  "price": 99.99
}

Response: 201 Created with OrderDetailId
```

#### Get Order Details
```
GET /api/order/{orderId}/details

Response: Array of OrderDetail items
```

#### Delete Order Detail
```
DELETE /api/order/detail/{orderDetailId}
```

---

### PAGES API

#### Create Page
```
POST /api/page/create

{
  "tenantId": 1,
  "title": "About Us",
  "slug": "about-us",
  "status": true
}
```

#### Get Page by ID
```
GET /api/page/{pageId}
```

#### Get Pages by Tenant
```
GET /api/page/tenant/{tenantId}?pageNumber=1&pageSize=10
```

#### Update Page
```
PUT /api/page/{pageId}

{
  "tenantId": 1,
  "title": "Our Company",
  "slug": "our-company",
  "status": true
}
```

#### Delete Page
```
DELETE /api/page/{pageId}
```

---

### SECTIONS API

#### Create Section
```
POST /api/section/create

{
  "pageId": 1,
  "type": "Hero",
  "orderNo": 1,
  "status": true
}
```

#### Get Section by ID
```
GET /api/section/{sectionId}
```

#### Get Sections by Page
```
GET /api/section/page/{pageId}

Response: Array of sections ordered by OrderNo
```

#### Update Section
```
PUT /api/section/{sectionId}

{
  "pageId": 1,
  "type": "Features",
  "orderNo": 2,
  "status": true
}
```

#### Delete Section
```
DELETE /api/section/{sectionId}
```

---

### SECTION DATA API

#### Create Section Data
```
POST /api/sectiondata/create

{
  "sectionId": 1,
  "key": "title",
  "value": "Section Title"
}
```

#### Get Section Data by ID
```
GET /api/sectiondata/{dataId}
```

#### Get Section Data by Section
```
GET /api/sectiondata/section/{sectionId}

Response: Array of key-value pairs
```

#### Update Section Data
```
PUT /api/sectiondata/{dataId}

{
  "sectionId": 1,
  "key": "title",
  "value": "Updated Title"
}
```

#### Delete Section Data
```
DELETE /api/sectiondata/{dataId}
```

---

## Common Query Parameters

### Pagination
- `pageNumber`: Page number (default: 1)
- `pageSize`: Items per page (default: 10)

### Response Example with Pagination
```json
{
  "items": [],
  "totalCount": 100,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 10,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

## Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request |
| 404 | Not Found |
| 500 | Internal Server Error |

## Enums

### UserRole
- `User` - Regular user (0)
- `Admin` - Administrator (1)
- `Manager` - Manager (2)
- `Superadmin` - Super administrator (3)

### OrderStatus
- `Pending` - Order pending
- `Processing` - Order processing
- `Shipped` - Order shipped
- `Delivered` - Order delivered
- `Cancelled` - Order cancelled
- `Returned` - Order returned
- `Failed` - Order failed

### EntityStatus
- `Inactive` (0)
- `Active` (1)

## Authentication Note

Currently, no authentication is required. Add JWT bearer token authentication in future versions:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ... });
```

## CORS

Default CORS policy allows all origins. Update in `Program.cs` for production:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", builder =>
    {
        builder.WithOrigins("https://yourdomain.com")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

## Rate Limiting

Consider adding rate limiting for production:

```csharp
builder.Services.AddRateLimiter(options => { ... });
```

## Validation

Add validation with FluentValidation:

```csharp
builder.Services.AddFluentValidation();
```

Create validators for each request DTO:

```csharp
public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequestDto>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```
