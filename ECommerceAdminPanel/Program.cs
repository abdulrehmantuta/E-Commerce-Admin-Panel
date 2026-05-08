using ECommerceAdminPanel.Config;
using System.Data;
using Microsoft.Data.SqlClient;
using ECommerceAdminPanel.Helper;
using ECommerceAdminPanel.Repositories.IRepository;
using ECommerceAdminPanel.Repositories.Repository;
using ECommerceAdminPanel.Services.IServices;
using ECommerceAdminPanel.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddLogging();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var jwtKey = builder.Configuration["Jwt:Key"] ?? string.Empty;
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString!));
builder.Services.AddScoped(_ => new DapperHelper(connectionString!));

// Repositories

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISectionDataRepository, SectionDataRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITenantSettingsRepository, TenantSettingsRepository>();
builder.Services.AddScoped<ITenantSliderRepository, TenantSliderRepository>();


// Services

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISectionDataService, SectionDataService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITenantSettingsService, TenantSettingsService>();
builder.Services.AddScoped<ITenantSliderService, TenantSliderService>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-Commerce Admin Panel API",
        Version = "v1",
        Description = "Complete E-Commerce Admin Panel API with Dapper and .NET 9"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
    options.RoutePrefix = string.Empty;
});

app.UseCors("AllowAll");

// Static files
app.UseStaticFiles();

// Uploads folder
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

// Angular Admin Panel routing
app.MapGet("/admin/{**path}", async context =>
{
    var requestPath = context.Request.Path.Value ?? "";
    var adminPath = Path.Combine(app.Environment.WebRootPath, "admin");

    // File physically exist karti hai to directly serve karo
    var physicalPath = Path.Combine(app.Environment.WebRootPath,
        requestPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

    if (System.IO.File.Exists(physicalPath))
    {
        var ext = Path.GetExtension(physicalPath);
        context.Response.ContentType = ext switch
        {
            ".js" => "application/javascript",
            ".css" => "text/css",
            ".ico" => "image/x-icon",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            _ => "application/octet-stream"
        };
        await context.Response.SendFileAsync(physicalPath);
        return;
    }

    // Angular route hai to index.html serve karo
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync(
        Path.Combine(adminPath, "index.html"));
});



// Angular Website routing
app.MapGet("/website/{**path}", async context =>
{
    var requestPath = context.Request.Path.Value ?? "";
    var websitePath = Path.Combine(app.Environment.WebRootPath, "website");

    var physicalPath = Path.Combine(app.Environment.WebRootPath,
        requestPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

    if (System.IO.File.Exists(physicalPath))
    {
        var ext = Path.GetExtension(physicalPath);
        context.Response.ContentType = ext switch
        {
            ".js" => "application/javascript",
            ".css" => "text/css",
            ".ico" => "image/x-icon",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            _ => "application/octet-stream"
        };
        await context.Response.SendFileAsync(physicalPath);
        return;
    }

    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync(
        Path.Combine(websitePath, "index.html"));
});


app.MapControllers();

app.Run();