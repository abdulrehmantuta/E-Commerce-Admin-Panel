namespace ECommerceAdminPanel.Repositories.Repository;

using Dapper;
using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.Helper;
using ECommerceAdminPanel.Models;
using ECommerceAdminPanel.Repositories.IRepository;

/// <summary>
/// Tenant Repository Implementation
/// </summary>
public class TenantRepository : ITenantRepository
{
    private readonly DapperHelper _dapperHelper;

    public TenantRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(Tenant entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Domain", entity.Domain);
        parameters.Add("@Logo", entity.Logo);
        parameters.Add("@ThemeColor", entity.ThemeColor);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Tenant_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<Tenant?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Tenant>("sp_Tenant_GetById", parameters);
    }

    public async Task<List<Tenant>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Tenant>("sp_Tenant_GetAll", parameters);
    }

    public async Task<int> UpdateAsync(int id, Tenant entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", id);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Domain", entity.Domain);
        parameters.Add("@Logo", entity.Logo);
        parameters.Add("@ThemeColor", entity.ThemeColor);
        parameters.Add("@Status", entity.Status);
        return await _dapperHelper.ExecuteAsync("sp_Tenant_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", id);
        return await _dapperHelper.ExecuteAsync("sp_Tenant_Delete", parameters);
    }
    //new

    public async Task<Tenant?> GetByDomainAsync(string domain)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Domain", domain);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Tenant>("sp_Tenant_GetByDomain", parameters);
    }
}

/// <summary>
/// User Repository Implementation
/// </summary>
// public class UserRepository : IUserRepository
// {
//     private readonly DapperHelper _dapperHelper;

//     public UserRepository(DapperHelper dapperHelper)
//     {
//         _dapperHelper = dapperHelper;
//     }

//     public async Task<int> CreateAsync(User entity)
//     {
//         var parameters = new DynamicParameters();
//         parameters.Add("@TenantId", entity.TenantId);
//         parameters.Add("@Name", entity.Name);
//         parameters.Add("@Email", entity.Email);
//         parameters.Add("@PasswordHash", entity.PasswordHash);
//         parameters.Add("@Role", entity.Role);

//         var result = await _dapperHelper.ExecuteScalarAsync("sp_User_Create", parameters);
//         return result != null ? Convert.ToInt32(result) : 0;
//     }

//     public async Task<User?> GetByIdAsync(int id)
//     {
//         var parameters = new DynamicParameters();
//         parameters.Add("@UserId", id);
//         return await _dapperHelper.QuerySingleOrDefaultAsync<User>("sp_User_GetById", parameters);
//     }

//     public async Task<List<User>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
//     {
//         var parameters = new DynamicParameters();
//         parameters.Add("@PageNumber", pageNumber);
//         parameters.Add("@PageSize", pageSize);
//         return await _dapperHelper.QueryAsync<User>("sp_User_GetAll", parameters);
//     }

//     public async Task<List<User>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
//     {
//         var parameters = new DynamicParameters();
//         parameters.Add("@TenantId", tenantId);
//         parameters.Add("@PageNumber", pageNumber);
//         parameters.Add("@PageSize", pageSize);
//         return await _dapperHelper.QueryAsync<User>("sp_User_GetByTenant", parameters);
//     }

//     public async Task<int> UpdateAsync(int id, User entity)
//     {
//         var parameters = new DynamicParameters();
//         parameters.Add("@UserId", id);
//         parameters.Add("@Name", entity.Name);
//         parameters.Add("@Email", entity.Email);
//         parameters.Add("@Role", entity.Role);
//         parameters.Add("@Status", entity.Status);
//         return await _dapperHelper.ExecuteAsync("sp_User_Update", parameters);
//     }

//     public async Task<int> DeleteAsync(int id)
//     {
//         var parameters = new DynamicParameters();
//         parameters.Add("@UserId", id);
//         return await _dapperHelper.ExecuteAsync("sp_User_Delete", parameters);
//     }
// }


public class UserRepository : IUserRepository
{
    private readonly DapperHelper _dapperHelper;

    public UserRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(User entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Email", entity.Email);
        parameters.Add("@PasswordHash", entity.PasswordHash);
        parameters.Add("@Role", entity.Role);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_User_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<User>("sp_User_GetById", parameters);
    }

    public async Task<List<User>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<User>("sp_User_GetAll", parameters);
    }

    public async Task<List<User>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<User>("sp_User_GetByTenant", parameters);
    }

    public async Task<int> UpdateAsync(int id, User entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", id);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@Email", entity.Email);
        parameters.Add("@Role", entity.Role);
        parameters.Add("@Status", entity.Status);
        return await _dapperHelper.ExecuteAsync("sp_User_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", id);
        return await _dapperHelper.ExecuteAsync("sp_User_Delete", parameters);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Email", email);
        return await _dapperHelper.QuerySingleOrDefaultAsync<User>("sp_User_GetByEmail", parameters);
    }
}

/// <summary>
/// Category Repository Implementation
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly DapperHelper _dapperHelper;

    public CategoryRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(Category entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@Name", entity.Name);
        parameters.Add("@ParentCategoryId", entity.ParentCategoryId);
        parameters.Add("@ImageUrl", entity.ImageUrl);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Category_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CategoryId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Category>("sp_Category_GetById", parameters);
    }

    public async Task<List<Category>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Category>("sp_Category_GetAll", parameters);
    }

    public async Task<List<Category>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Category>("sp_Category_GetByTenant", parameters);
    }

    public async Task<int> UpdateAsync(int id, Category entity)
{
    var parameters = new DynamicParameters();
    parameters.Add("@CategoryId", id);
    parameters.Add("@Name", entity.Name);
    parameters.Add("@ParentCategoryId", entity.ParentCategoryId);

    // ✅ SAFE handling
    parameters.Add("@ImageUrl", entity.ImageUrl ?? (object)DBNull.Value);

    parameters.Add("@Status", entity.Status);

    return await _dapperHelper.ExecuteAsync("sp_Category_Update", parameters);
}

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CategoryId", id);
        return await _dapperHelper.ExecuteAsync("sp_Category_Delete", parameters);
    }
}

/// <summary>
/// Order Repository Implementation
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly DapperHelper _dapperHelper;

    public OrderRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(Order entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@UserId", entity.UserId);      // ✅ add karo
        parameters.Add("@CustomerName", entity.CustomerName);
        parameters.Add("@CustomerEmail", entity.CustomerEmail);
        parameters.Add("@CustomerPhone", entity.CustomerPhone);
        parameters.Add("@TotalAmount", entity.TotalAmount);
        parameters.Add("@Status", entity.Status);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Order_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Order>("sp_Order_GetById", parameters);
    }

    //new

    public async Task<List<Order>> GetByUserAsync(int tenantId, int customerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@UserId", customerId);
        return await _dapperHelper.QueryAsync<Order>("sp_Order_GetByUser", parameters);
    }

    public async Task<List<Order>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Order>("sp_Order_GetAll", parameters);
    }

    public async Task<List<Order>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Order>("sp_Order_GetByTenant", parameters);
    }

    public async Task<int> UpdateAsync(int id, Order entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", id);
        parameters.Add("@CustomerName", entity.CustomerName);
        parameters.Add("@CustomerEmail", entity.CustomerEmail);
        parameters.Add("@CustomerPhone", entity.CustomerPhone);
        parameters.Add("@TotalAmount", entity.TotalAmount);
        parameters.Add("@Status", entity.Status);
        return await _dapperHelper.ExecuteAsync("sp_Order_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", id);
        return await _dapperHelper.ExecuteAsync("sp_Order_Delete", parameters);
    }
}

/// <summary>
/// OrderDetail Repository Implementation
/// </summary>
public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly DapperHelper _dapperHelper;

    public OrderDetailRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(OrderDetail entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", entity.OrderId);
        parameters.Add("@ProductId", entity.ProductId);
        parameters.Add("@Quantity", entity.Quantity);
        parameters.Add("@Price", entity.Price);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_OrderDetail_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<OrderDetail?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderDetailId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<OrderDetail>("sp_OrderDetail_GetById", parameters);
    }

    public async Task<List<OrderDetail>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<OrderDetail>("sp_OrderDetail_GetAll", parameters);
    }

    public async Task<List<OrderDetail>> GetByOrderAsync(int orderId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", orderId);
        return await _dapperHelper.QueryAsync<OrderDetail>("sp_OrderDetail_GetByOrder", parameters);
    }

    public async Task<int> UpdateAsync(int id, OrderDetail entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderDetailId", id);
        parameters.Add("@Quantity", entity.Quantity);
        parameters.Add("@Price", entity.Price);
        return await _dapperHelper.ExecuteAsync("sp_OrderDetail_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@OrderDetailId", id);
        return await _dapperHelper.ExecuteAsync("sp_OrderDetail_Delete", parameters);
    }
}

/// <summary>
/// Page Repository Implementation
/// </summary>
public class PageRepository : IPageRepository
{
    private readonly DapperHelper _dapperHelper;

    public PageRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(Page entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@Title", entity.Title);
        parameters.Add("@Slug", entity.Slug);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Page_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<Page?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Page>("sp_Page_GetById", parameters);
    }

    public async Task<List<Page>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Page>("sp_Page_GetAll", parameters);
    }

    public async Task<List<Page>> GetByTenantAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Page>("sp_Page_GetByTenant", parameters);
    }

    public async Task<int> UpdateAsync(int id, Page entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageId", id);
        parameters.Add("@Title", entity.Title);
        parameters.Add("@Slug", entity.Slug);
        parameters.Add("@Status", entity.Status);
        return await _dapperHelper.ExecuteAsync("sp_Page_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageId", id);
        return await _dapperHelper.ExecuteAsync("sp_Page_Delete", parameters);
    }
}

/// <summary>
/// Section Repository Implementation
/// </summary>
public class SectionRepository : ISectionRepository
{
    private readonly DapperHelper _dapperHelper;

    public SectionRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(Section entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageId", entity.PageId);
        parameters.Add("@Type", entity.Type);
        parameters.Add("@OrderNo", entity.OrderNo);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_Section_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<Section?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SectionId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<Section>("sp_Section_GetById", parameters);
    }

    public async Task<List<Section>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<Section>("sp_Section_GetAll", parameters);
    }

    public async Task<List<Section>> GetByPageAsync(int pageId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageId", pageId);
        return await _dapperHelper.QueryAsync<Section>("sp_Section_GetByPage", parameters);
    }

    public async Task<int> UpdateAsync(int id, Section entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SectionId", id);
        parameters.Add("@Type", entity.Type);
        parameters.Add("@OrderNo", entity.OrderNo);
        parameters.Add("@Status", entity.Status);
        return await _dapperHelper.ExecuteAsync("sp_Section_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SectionId", id);
        return await _dapperHelper.ExecuteAsync("sp_Section_Delete", parameters);
    }
}

/// <summary>
/// SectionData Repository Implementation
/// </summary>
public class SectionDataRepository : ISectionDataRepository
{
    private readonly DapperHelper _dapperHelper;

    public SectionDataRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> CreateAsync(SectionData entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SectionId", entity.SectionId);
        parameters.Add("@Key", entity.Key);
        parameters.Add("@Value", entity.Value);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_SectionData_Create", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<SectionData?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@DataId", id);
        return await _dapperHelper.QuerySingleOrDefaultAsync<SectionData>("sp_SectionData_GetById", parameters);
    }

    public async Task<List<SectionData>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<SectionData>("sp_SectionData_GetAll", parameters);
    }

    public async Task<List<SectionData>> GetBySectionAsync(int sectionId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SectionId", sectionId);
        return await _dapperHelper.QueryAsync<SectionData>("sp_SectionData_GetBySection", parameters);
    }

    public async Task<int> UpdateAsync(int id, SectionData entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@DataId", id);
        parameters.Add("@Key", entity.Key);
        parameters.Add("@Value", entity.Value);
        return await _dapperHelper.ExecuteAsync("sp_SectionData_Update", parameters);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@DataId", id);
        return await _dapperHelper.ExecuteAsync("sp_SectionData_Delete", parameters);
    }
}











/// <summary>
/// TenantSettingsRepository Implementation
/// </summary>
public class TenantSettingsRepository : ITenantSettingsRepository
{
    private readonly DapperHelper _dapperHelper;

    public TenantSettingsRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<TenantSettings?> GetByTenantAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        return await _dapperHelper.QuerySingleOrDefaultAsync<TenantSettings>(
            "sp_TenantSettings_Get", parameters);
    }

    public async Task<TenantSettings?> UpsertAsync(TenantSettings entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@StoreName", entity.StoreName);
        parameters.Add("@LogoUrl", entity.LogoUrl);
        parameters.Add("@FaviconUrl", entity.FaviconUrl);
        parameters.Add("@PrimaryColor", entity.PrimaryColor);
        parameters.Add("@SecondaryColor", entity.SecondaryColor);
        parameters.Add("@AccentColor", entity.AccentColor);
        parameters.Add("@BackgroundColor", entity.BackgroundColor);
        parameters.Add("@TextColor", entity.TextColor);
        parameters.Add("@NavbarBgColor", entity.NavbarBgColor);
        parameters.Add("@NavbarTextColor", entity.NavbarTextColor);
        parameters.Add("@FooterBgColor", entity.FooterBgColor);
        parameters.Add("@FooterTextColor", entity.FooterTextColor);
        parameters.Add("@ButtonColor", entity.ButtonColor);
        parameters.Add("@ButtonTextColor", entity.ButtonTextColor);
        parameters.Add("@FontFamily", entity.FontFamily);
        parameters.Add("@FacebookUrl", entity.FacebookUrl);
        parameters.Add("@InstagramUrl", entity.InstagramUrl);
        parameters.Add("@WhatsappNumber", entity.WhatsappNumber);
        parameters.Add("@FooterTagline", entity.FooterTagline);
        parameters.Add("@HeroBgColor", entity.HeroBgColor); // ✅ naya — baaki sab same raho

        parameters.Add("@PromoBannerBg", entity.PromoBannerBg);
        parameters.Add("@PromoBannerText", entity.PromoBannerText);
        parameters.Add("@CardBg", entity.CardBg);
        parameters.Add("@CardText", entity.CardText);
        // ✅ Naye
        parameters.Add("@CardStyle", entity.CardStyle);
        parameters.Add("@CardRadius", entity.CardRadius);
        parameters.Add("@FontHeading", entity.FontHeading);
        parameters.Add("@FontBody", entity.FontBody);
        parameters.Add("@ButtonRadius", entity.ButtonRadius);
        parameters.Add("@ImageAspectRatio", entity.ImageAspectRatio);
        return await _dapperHelper.QuerySingleOrDefaultAsync<TenantSettings>(
            "sp_TenantSettings_Upsert", parameters);
    }
}




/// <summary>
/// TenantSliderRepository Implementation
/// </summary>
/// 

public class TenantSliderRepository : ITenantSliderRepository
{
    private readonly DapperHelper _dapperHelper;

    public TenantSliderRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<List<TenantSlider>> GetActiveByTenantAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        return await _dapperHelper.QueryAsync<TenantSlider>(
            "sp_TenantSliders_GetByTenant", parameters);
    }

    public async Task<List<TenantSlider>> GetAllByTenantAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        return await _dapperHelper.QueryAsync<TenantSlider>(
            "sp_TenantSliders_GetAll", parameters);
    }

    public async Task<int> AddAsync(TenantSlider entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", entity.TenantId);
        parameters.Add("@ImageUrl", entity.ImageUrl);
        parameters.Add("@Title", entity.Title);
        parameters.Add("@Subtitle", entity.Subtitle);
        parameters.Add("@ButtonText", entity.ButtonText);
        parameters.Add("@ButtonLink", entity.ButtonLink);
        parameters.Add("@OrderNo", entity.OrderNo);
        parameters.Add("@IsActive", entity.IsActive);
        parameters.Add("@LayoutType", entity.LayoutType);      // NEW
        parameters.Add("@BgColor", entity.BgColor);         // NEW
        parameters.Add("@TextColor", entity.TextColor);       // NEW
        parameters.Add("@OverlayOpacity", entity.OverlayOpacity);  // NEW
        parameters.Add("@IsPresetImage", entity.IsPresetImage);  // ← NEW


        var result = await _dapperHelper.ExecuteScalarAsync(
            "sp_TenantSliders_Add", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task UpdateAsync(int sliderId, TenantSlider entity)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SliderId", sliderId);
        parameters.Add("@ImageUrl", entity.ImageUrl);
        parameters.Add("@Title", entity.Title);
        parameters.Add("@Subtitle", entity.Subtitle);
        parameters.Add("@ButtonText", entity.ButtonText);
        parameters.Add("@ButtonLink", entity.ButtonLink);
        parameters.Add("@OrderNo", entity.OrderNo);
        parameters.Add("@IsActive", entity.IsActive);
        parameters.Add("@LayoutType", entity.LayoutType);      // NEW
        parameters.Add("@BgColor", entity.BgColor);         // NEW
        parameters.Add("@TextColor", entity.TextColor);       // NEW
        parameters.Add("@OverlayOpacity", entity.OverlayOpacity);  // NEW
        parameters.Add("@IsPresetImage", entity.IsPresetImage);  // ← NEW


        await _dapperHelper.ExecuteAsync("sp_TenantSliders_Update", parameters);
    }

    public async Task<List<TenantSlider>> GetPresetImagesAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        return await _dapperHelper.QueryAsync<TenantSlider>(
            "sp_TenantSliders_GetPresetImages", parameters);
    }
    public async Task DeleteAsync(int sliderId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SliderId", sliderId);
        await _dapperHelper.ExecuteAsync("sp_TenantSliders_Delete", parameters);
    }



}






// =============================================
// ✅ NEW — TenantIntegration Repository
// =============================================

/// <summary>
/// TenantIntegration Repository Implementation
/// </summary>
public class TenantIntegrationRepository : ITenantIntegrationRepository
{
    private readonly DapperHelper _dapperHelper;

    public TenantIntegrationRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<TenantIntegration?> GetByTenantAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        return await _dapperHelper.QuerySingleOrDefaultAsync<TenantIntegration>(
            "sp_TenantIntegrations_Get", parameters);
    }

    public async Task<TenantIntegration?> UpsertAsync(int tenantId, TenantIntegrationRequestDto dto)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@IsEmailEnabled", dto.IsEmailEnabled);
        parameters.Add("@EmailProvider", dto.EmailProvider);
        parameters.Add("@EmailApiKey", dto.EmailApiKey);
        parameters.Add("@EmailSenderAddress", dto.EmailSenderAddress);
        parameters.Add("@EmailSenderName", dto.EmailSenderName);
        parameters.Add("@IsWhatsAppEnabled", dto.IsWhatsAppEnabled);
        parameters.Add("@WhatsAppProvider", dto.WhatsAppProvider);
        parameters.Add("@WhatsAppToken", dto.WhatsAppToken);
        parameters.Add("@WhatsAppPhoneNumberId", dto.WhatsAppPhoneNumberId);
        parameters.Add("@WhatsAppBusinessId", dto.WhatsAppBusinessId);

        return await _dapperHelper.QuerySingleOrDefaultAsync<TenantIntegration>(
            "sp_TenantIntegrations_Upsert", parameters);
    }

    public async Task<bool> DeleteAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        var rows = await _dapperHelper.ExecuteAsync("sp_TenantIntegrations_Delete", parameters);
        return rows > 0;
    }
}

// =============================================
// ✅ NEW — NotificationLog Repository
// =============================================

/// <summary>
/// NotificationLog Repository Implementation
/// </summary>
public class NotificationLogRepository : INotificationLogRepository
{
    private readonly DapperHelper _dapperHelper;

    public NotificationLogRepository(DapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
    }

    public async Task<int> InsertAsync(NotificationLog log)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", log.TenantId);
        parameters.Add("@OrderId", log.OrderId);
        parameters.Add("@Channel", log.Channel);
        parameters.Add("@EventType", log.EventType);
        parameters.Add("@RecipientContact", log.RecipientContact);
        parameters.Add("@Status", log.Status);
        parameters.Add("@ErrorMessage", log.ErrorMessage);

        var result = await _dapperHelper.ExecuteScalarAsync("sp_NotificationLog_Insert", parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<List<NotificationLog>> GetByTenantAsync(int tenantId, int page = 1, int pageSize = 20)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@PageNumber", page);
        parameters.Add("@PageSize", pageSize);
        return await _dapperHelper.QueryAsync<NotificationLog>(
            "sp_NotificationLog_GetByTenant", parameters);
    }

    public async Task<List<NotificationLog>> GetByOrderAsync(int tenantId, int orderId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        parameters.Add("@OrderId", orderId);
        return await _dapperHelper.QueryAsync<NotificationLog>(
            "sp_NotificationLog_GetByOrder", parameters);
    }

    public async Task<List<NotificationLog>> GetFailedAsync(int tenantId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TenantId", tenantId);
        return await _dapperHelper.QueryAsync<NotificationLog>(
            "sp_NotificationLog_GetFailed", parameters);
    }
}
