-- E-Commerce Admin Panel Stored Procedures
-- CRUD Operations for all entities

USE db47144;
GO

-- =============================================
-- TENANT STORED PROCEDURES
-- =============================================

-- Create Tenant
CREATE OR ALTER PROCEDURE sp_Tenant_Create
    @Name NVARCHAR(100),
    @Domain NVARCHAR(255),
    @Logo NVARCHAR(255) = NULL,
    @ThemeColor NVARCHAR(50) = NULL
AS
BEGIN
    INSERT INTO Tenants (Name, Domain, Logo, ThemeColor, CreatedDate, Status)
    VALUES (@Name, @Domain, @Logo, @ThemeColor, GETDATE(), 1);
    
    SELECT SCOPE_IDENTITY() AS TenantId;
END;
GO

-- Read Tenant by ID
CREATE OR ALTER PROCEDURE sp_Tenant_GetById
    @TenantId INT
AS
BEGIN
    SELECT TenantId, Name, Domain, Logo, ThemeColor, CreatedDate, Status
    FROM Tenants
    WHERE TenantId = @TenantId;
END;
GO

-- Read All Tenants
CREATE OR ALTER PROCEDURE sp_Tenant_GetAll
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT TenantId, Name, Domain, Logo, ThemeColor, CreatedDate, Status
    FROM Tenants
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update Tenant
CREATE OR ALTER PROCEDURE sp_Tenant_Update
    @TenantId INT,
    @Name NVARCHAR(100),
    @Domain NVARCHAR(255),
    @Logo NVARCHAR(255) = NULL,
    @ThemeColor NVARCHAR(50) = NULL,
    @Status BIT
AS
BEGIN
    UPDATE Tenants
    SET Name = @Name, Domain = @Domain, Logo = @Logo, ThemeColor = @ThemeColor, Status = @Status
    WHERE TenantId = @TenantId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Tenant
CREATE OR ALTER PROCEDURE sp_Tenant_Delete
    @TenantId INT
AS
BEGIN
    DELETE FROM Tenants
    WHERE TenantId = @TenantId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- USER STORED PROCEDURES
-- =============================================

-- Create User
CREATE OR ALTER PROCEDURE sp_User_Create
    @TenantId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(50) = 'User'
AS
BEGIN
    INSERT INTO Users (TenantId, Name, Email, PasswordHash, Role, CreatedDate, Status)
    VALUES (@TenantId, @Name, @Email, @PasswordHash, @Role, GETDATE(), 1);
    
    SELECT SCOPE_IDENTITY() AS UserId;
END;
GO

-- Read User by ID
CREATE OR ALTER PROCEDURE sp_User_GetById
    @UserId INT
AS
BEGIN
    SELECT UserId, TenantId, Name, Email, PasswordHash, Role, CreatedDate, Status
    FROM Users
    WHERE UserId = @UserId;
END;
GO

-- Read Users by Tenant
CREATE OR ALTER PROCEDURE sp_User_GetByTenant
    @TenantId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT UserId, TenantId, Name, Email, PasswordHash, Role, CreatedDate, Status
    FROM Users
    WHERE TenantId = @TenantId
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update User
CREATE OR ALTER PROCEDURE sp_User_Update
    @UserId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @Role NVARCHAR(50),
    @Status BIT
AS
BEGIN
    UPDATE Users
    SET Name = @Name, Email = @Email, Role = @Role, Status = @Status
    WHERE UserId = @UserId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete User
CREATE OR ALTER PROCEDURE sp_User_Delete
    @UserId INT
AS
BEGIN
    DELETE FROM Users
    WHERE UserId = @UserId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- CATEGORY STORED PROCEDURES
-- =============================================

-- Create Category
CREATE OR ALTER PROCEDURE sp_Category_Create
    @TenantId INT,
    @Name NVARCHAR(100),
    @ParentCategoryId INT = NULL
AS
BEGIN
    INSERT INTO Categories (TenantId, Name, ParentCategoryId, Status)
    VALUES (@TenantId, @Name, @ParentCategoryId, 1);
    
    SELECT SCOPE_IDENTITY() AS CategoryId;
END;
GO

-- Read Category by ID
CREATE OR ALTER PROCEDURE sp_Category_GetById
    @CategoryId INT
AS
BEGIN
    SELECT CategoryId, TenantId, Name, ParentCategoryId, Status
    FROM Categories
    WHERE CategoryId = @CategoryId;
END;
GO

-- Read Categories by Tenant
CREATE OR ALTER PROCEDURE sp_Category_GetByTenant
    @TenantId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT CategoryId, TenantId, Name, ParentCategoryId, Status
    FROM Categories
    WHERE TenantId = @TenantId
    ORDER BY Name
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update Category
CREATE OR ALTER PROCEDURE sp_Category_Update
    @CategoryId INT,
    @Name NVARCHAR(100),
    @ParentCategoryId INT = NULL,
    @Status BIT
AS
BEGIN
    UPDATE Categories
    SET Name = @Name, ParentCategoryId = @ParentCategoryId, Status = @Status
    WHERE CategoryId = @CategoryId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Category
CREATE OR ALTER PROCEDURE sp_Category_Delete
    @CategoryId INT
AS
BEGIN
    DELETE FROM Categories
    WHERE CategoryId = @CategoryId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- PRODUCT STORED PROCEDURES
-- =============================================

-- Create Product
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
GO

-- Read Product by ID
CREATE OR ALTER PROCEDURE sp_Product_GetById
    @ProductId INT
AS
BEGIN
    SELECT ProductId, TenantId, Name, Description, Price, ImageUrl, CategoryId, StockQty, Status, CreatedDate
    FROM Products
    WHERE ProductId = @ProductId;
END;
GO

-- Read Products by Tenant
CREATE OR ALTER PROCEDURE sp_Product_GetByTenant
    @TenantId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT ProductId, TenantId, Name, Description, Price, ImageUrl, CategoryId, StockQty, Status, CreatedDate
    FROM Products
    WHERE TenantId = @TenantId AND Status = 1
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Read Products by Category
CREATE OR ALTER PROCEDURE sp_Product_GetByCategory
    @CategoryId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT ProductId, TenantId, Name, Description, Price, ImageUrl, CategoryId, StockQty, Status, CreatedDate
    FROM Products
    WHERE CategoryId = @CategoryId AND Status = 1
    ORDER BY Name
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update Product
CREATE OR ALTER PROCEDURE sp_Product_Update
    @ProductId INT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(MAX) = NULL,
    @Price DECIMAL(18,2),
    @ImageUrl NVARCHAR(255) = NULL,
    @CategoryId INT = NULL,
    @StockQty INT,
    @Status BIT
AS
BEGIN
    UPDATE Products
    SET Name = @Name, Description = @Description, Price = @Price, ImageUrl = @ImageUrl, 
        CategoryId = @CategoryId, StockQty = @StockQty, Status = @Status
    WHERE ProductId = @ProductId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Product
CREATE OR ALTER PROCEDURE sp_Product_Delete
    @ProductId INT
AS
BEGIN
    DELETE FROM Products
    WHERE ProductId = @ProductId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- ORDER STORED PROCEDURES
-- =============================================

-- Create Order
CREATE OR ALTER PROCEDURE sp_Order_Create
    @TenantId INT,
    @CustomerName NVARCHAR(100),
    @CustomerEmail NVARCHAR(150) = NULL,
    @CustomerPhone NVARCHAR(50) = NULL,
    @TotalAmount DECIMAL(18,2),
    @Status NVARCHAR(50) = 'Pending'
AS
BEGIN
    INSERT INTO Orders (TenantId, CustomerName, CustomerEmail, CustomerPhone, TotalAmount, Status, CreatedDate)
    VALUES (@TenantId, @CustomerName, @CustomerEmail, @CustomerPhone, @TotalAmount, @Status, GETDATE());
    
    SELECT SCOPE_IDENTITY() AS OrderId;
END;
GO

-- Read Order by ID
CREATE OR ALTER PROCEDURE sp_Order_GetById
    @OrderId INT
AS
BEGIN
    SELECT OrderId, TenantId, CustomerName, CustomerEmail, CustomerPhone, TotalAmount, Status, CreatedDate
    FROM Orders
    WHERE OrderId = @OrderId;
END;
GO

-- Read Orders by Tenant
CREATE OR ALTER PROCEDURE sp_Order_GetByTenant
    @TenantId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT OrderId, TenantId, CustomerName, CustomerEmail, CustomerPhone, TotalAmount, Status, CreatedDate
    FROM Orders
    WHERE TenantId = @TenantId
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update Order
CREATE OR ALTER PROCEDURE sp_Order_Update
    @OrderId INT,
    @CustomerName NVARCHAR(100),
    @CustomerEmail NVARCHAR(150) = NULL,
    @CustomerPhone NVARCHAR(50) = NULL,
    @TotalAmount DECIMAL(18,2),
    @Status NVARCHAR(50)
AS
BEGIN
    UPDATE Orders
    SET CustomerName = @CustomerName, CustomerEmail = @CustomerEmail, CustomerPhone = @CustomerPhone,
        TotalAmount = @TotalAmount, Status = @Status
    WHERE OrderId = @OrderId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Order
CREATE OR ALTER PROCEDURE sp_Order_Delete
    @OrderId INT
AS
BEGIN
    DELETE FROM Orders
    WHERE OrderId = @OrderId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- ORDER DETAIL STORED PROCEDURES
-- =============================================

-- Create Order Detail
CREATE OR ALTER PROCEDURE sp_OrderDetail_Create
    @OrderId INT,
    @ProductId INT,
    @Quantity INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)
    VALUES (@OrderId, @ProductId, @Quantity, @Price);
    
    SELECT SCOPE_IDENTITY() AS OrderDetailId;
END;
GO

-- Read Order Details by Order
CREATE OR ALTER PROCEDURE sp_OrderDetail_GetByOrder
    @OrderId INT
AS
BEGIN
    SELECT OrderDetailId, OrderId, ProductId, Quantity, Price
    FROM OrderDetails
    WHERE OrderId = @OrderId;
END;
GO

-- Update Order Detail
CREATE OR ALTER PROCEDURE sp_OrderDetail_Update
    @OrderDetailId INT,
    @Quantity INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    UPDATE OrderDetails
    SET Quantity = @Quantity, Price = @Price
    WHERE OrderDetailId = @OrderDetailId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Order Detail
CREATE OR ALTER PROCEDURE sp_OrderDetail_Delete
    @OrderDetailId INT
AS
BEGIN
    DELETE FROM OrderDetails
    WHERE OrderDetailId = @OrderDetailId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- PAGE STORED PROCEDURES
-- =============================================

-- Create Page
CREATE OR ALTER PROCEDURE sp_Page_Create
    @TenantId INT,
    @Title NVARCHAR(100),
    @Slug NVARCHAR(100)
AS
BEGIN
    INSERT INTO Pages (TenantId, Title, Slug, CreatedDate, Status)
    VALUES (@TenantId, @Title, @Slug, GETDATE(), 1);
    
    SELECT SCOPE_IDENTITY() AS PageId;
END;
GO

-- Read Page by ID
CREATE OR ALTER PROCEDURE sp_Page_GetById
    @PageId INT
AS
BEGIN
    SELECT PageId, TenantId, Title, Slug, Status, CreatedDate
    FROM Pages
    WHERE PageId = @PageId;
END;
GO

-- Read Pages by Tenant
CREATE OR ALTER PROCEDURE sp_Page_GetByTenant
    @TenantId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT PageId, TenantId, Title, Slug, Status, CreatedDate
    FROM Pages
    WHERE TenantId = @TenantId
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update Page
CREATE OR ALTER PROCEDURE sp_Page_Update
    @PageId INT,
    @Title NVARCHAR(100),
    @Slug NVARCHAR(100),
    @Status BIT
AS
BEGIN
    UPDATE Pages
    SET Title = @Title, Slug = @Slug, Status = @Status
    WHERE PageId = @PageId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Page
CREATE OR ALTER PROCEDURE sp_Page_Delete
    @PageId INT
AS
BEGIN
    DELETE FROM Pages
    WHERE PageId = @PageId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- SECTION STORED PROCEDURES
-- =============================================

-- Create Section
CREATE OR ALTER PROCEDURE sp_Section_Create
    @PageId INT,
    @Type NVARCHAR(50),
    @OrderNo INT = 0
AS
BEGIN
    INSERT INTO Sections (PageId, Type, OrderNo, Status)
    VALUES (@PageId, @Type, @OrderNo, 1);
    
    SELECT SCOPE_IDENTITY() AS SectionId;
END;
GO

-- Read Section by ID
CREATE OR ALTER PROCEDURE sp_Section_GetById
    @SectionId INT
AS
BEGIN
    SELECT SectionId, PageId, Type, OrderNo, Status
    FROM Sections
    WHERE SectionId = @SectionId;
END;
GO

-- Read Sections by Page
CREATE OR ALTER PROCEDURE sp_Section_GetByPage
    @PageId INT
AS
BEGIN
    SELECT SectionId, PageId, Type, OrderNo, Status
    FROM Sections
    WHERE PageId = @PageId
    ORDER BY OrderNo;
END;
GO

-- Update Section
CREATE OR ALTER PROCEDURE sp_Section_Update
    @SectionId INT,
    @Type NVARCHAR(50),
    @OrderNo INT,
    @Status BIT
AS
BEGIN
    UPDATE Sections
    SET Type = @Type, OrderNo = @OrderNo, Status = @Status
    WHERE SectionId = @SectionId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Section
CREATE OR ALTER PROCEDURE sp_Section_Delete
    @SectionId INT
AS
BEGIN
    DELETE FROM Sections
    WHERE SectionId = @SectionId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- =============================================
-- SECTION DATA STORED PROCEDURES
-- =============================================

-- Create Section Data
CREATE OR ALTER PROCEDURE sp_SectionData_Create
    @SectionId INT,
    @Key NVARCHAR(100),
    @Value NVARCHAR(MAX) = NULL
AS
BEGIN
    INSERT INTO SectionData (SectionId, [Key], [Value])
    VALUES (@SectionId, @Key, @Value);
    
    SELECT SCOPE_IDENTITY() AS DataId;
END;
GO

-- Read Section Data by Section
CREATE OR ALTER PROCEDURE sp_SectionData_GetBySection
    @SectionId INT
AS
BEGIN
    SELECT DataId, SectionId, [Key], [Value]
    FROM SectionData
    WHERE SectionId = @SectionId;
END;
GO

-- Update Section Data
CREATE OR ALTER PROCEDURE sp_SectionData_Update
    @DataId INT,
    @Key NVARCHAR(100),
    @Value NVARCHAR(MAX) = NULL
AS
BEGIN
    UPDATE SectionData
    SET [Key] = @Key, [Value] = @Value
    WHERE DataId = @DataId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Delete Section Data
CREATE OR ALTER PROCEDURE sp_SectionData_Delete
    @DataId INT
AS
BEGIN
    DELETE FROM SectionData
    WHERE DataId = @DataId;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO


-- =============================================
-- CUSTOMER STORED PROCEDURES
-- =============================================

-- Customers Table (if not exists)
IF OBJECT_ID('dbo.Customers', 'U') IS NULL
BEGIN
    CREATE TABLE Customers (
        CustomerId INT IDENTITY(1,1) PRIMARY KEY,
        TenantId INT NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NULL,
        Password NVARCHAR(255) NOT NULL,
        Status BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Customers_Tenant 
            FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
    );
    PRINT 'Customers table created';
END
ELSE
    PRINT 'Customers table already exists';
GO

-- Orders mein UserId
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Orders' AND COLUMN_NAME = 'UserId'
)
BEGIN
    ALTER TABLE Orders ADD UserId INT NULL;
    ALTER TABLE Orders ADD CONSTRAINT FK_Orders_Customer
        FOREIGN KEY (UserId) REFERENCES Customers(CustomerId);
    PRINT 'UserId added to Orders';
END
ELSE
    PRINT 'UserId already exists';
GO

CREATE OR ALTER PROCEDURE sp_Customer_Create
    @TenantId INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(150) = NULL,
    @Password NVARCHAR(255),
    @Status BIT = 1
AS
BEGIN
    INSERT INTO Customers 
    (TenantId, FirstName, LastName, Email, Password, Status, CreatedDate)
    VALUES 
    (@TenantId, @FirstName, @LastName, @Email, @Password, @Status, GETDATE());
    SELECT SCOPE_IDENTITY() AS CustomerId;
END;
GO

CREATE OR ALTER PROCEDURE sp_Customer_GetById
    @CustomerId INT
AS
BEGIN
    SELECT * FROM Customers WHERE CustomerId = @CustomerId;
END;
GO

CREATE OR ALTER PROCEDURE sp_Customer_GetByTenant
    @TenantId INT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT * FROM Customers
    WHERE TenantId = @TenantId
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

CREATE OR ALTER PROCEDURE sp_Order_GetByUser
    @TenantId INT,
    @UserId INT
AS
BEGIN
    SELECT OrderId, TenantId, CustomerName, CustomerEmail,
           CustomerPhone, TotalAmount, Status, CreatedDate
    FROM Orders
    WHERE TenantId = @TenantId AND UserId = @UserId
    ORDER BY CreatedDate DESC;
END;
GO

-- sp_Order_Create updated with UserId
CREATE OR ALTER PROCEDURE sp_Order_Create
    @TenantId INT,
    @UserId INT = NULL,
    @CustomerName NVARCHAR(100),
    @CustomerEmail NVARCHAR(150) = NULL,
    @CustomerPhone NVARCHAR(50) = NULL,
    @TotalAmount DECIMAL(18,2),
    @Status NVARCHAR(50) = 'Pending'
AS
BEGIN
    INSERT INTO Orders 
    (TenantId, UserId, CustomerName, CustomerEmail, CustomerPhone, TotalAmount, Status, CreatedDate)
    VALUES 
    (@TenantId, @UserId, @CustomerName, @CustomerEmail, @CustomerPhone, @TotalAmount, @Status, GETDATE());
    SELECT SCOPE_IDENTITY() AS OrderId;
END;
GO










-- =============================================
-- STEP 1: TENANT SETTINGS TABLE
-- =============================================
IF OBJECT_ID('dbo.TenantSettings', 'U') IS NOT NULL
    DROP TABLE dbo.TenantSettings;

CREATE TABLE TenantSettings (
    SettingId         INT IDENTITY(1,1) PRIMARY KEY,
    TenantId          INT NOT NULL UNIQUE,

    -- Branding
    StoreName         NVARCHAR(200)  NULL,
    LogoUrl           NVARCHAR(500)  NULL,
    FaviconUrl        NVARCHAR(500)  NULL,

    -- Colors
    PrimaryColor      NVARCHAR(20)   DEFAULT '#ea6c2d',
    SecondaryColor    NVARCHAR(20)   DEFAULT '#1a1a2e',
    AccentColor       NVARCHAR(20)   DEFAULT '#ffffff',
    BackgroundColor   NVARCHAR(20)   DEFAULT '#ffffff',
    TextColor         NVARCHAR(20)   DEFAULT '#1a1a1a',
    NavbarBgColor     NVARCHAR(20)   DEFAULT '#ffffff',
    NavbarTextColor   NVARCHAR(20)   DEFAULT '#1a1a1a',
    FooterBgColor     NVARCHAR(20)   DEFAULT '#0f172a',
    FooterTextColor   NVARCHAR(20)   DEFAULT '#ffffff',
    ButtonColor       NVARCHAR(20)   DEFAULT '#ea6c2d',
    ButtonTextColor   NVARCHAR(20)   DEFAULT '#ffffff',

    -- Typography
    FontFamily        NVARCHAR(100)  DEFAULT 'Poppins',

    -- Social Links
    FacebookUrl       NVARCHAR(300)  NULL,
    InstagramUrl      NVARCHAR(300)  NULL,
    WhatsappNumber    NVARCHAR(50)   NULL,

    -- Footer
    FooterTagline     NVARCHAR(500)  NULL,

    UpdatedDate       DATETIME       DEFAULT GETDATE(),

    CONSTRAINT FK_TenantSettings_Tenant
        FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
);

PRINT 'TenantSettings table created!';
GO

-- =============================================
-- STEP 2: TENANT SLIDERS TABLE
-- =============================================
IF OBJECT_ID('dbo.TenantSliders', 'U') IS NOT NULL
    DROP TABLE dbo.TenantSliders;

CREATE TABLE TenantSliders (
    SliderId      INT IDENTITY(1,1) PRIMARY KEY,
    TenantId      INT NOT NULL,

    -- Slide Content
    ImageUrl      NVARCHAR(500)  NOT NULL,
    Title         NVARCHAR(200)  NULL,
    Subtitle      NVARCHAR(500)  NULL,
    ButtonText    NVARCHAR(100)  NULL,
    ButtonLink    NVARCHAR(300)  NULL,

    -- Control
    OrderNo       INT            DEFAULT 1,
    IsActive      BIT            DEFAULT 1,
    CreatedDate   DATETIME       DEFAULT GETDATE(),

    CONSTRAINT FK_TenantSliders_Tenant
        FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
);

CREATE INDEX IX_TenantSliders_TenantId ON TenantSliders(TenantId);

PRINT 'TenantSliders table created!';
GO

-- =============================================
-- STEP 3: STORED PROCEDURES — TenantSettings
-- =============================================

-- GET Settings by TenantId
CREATE OR ALTER PROCEDURE sp_TenantSettings_Get
    @TenantId INT
AS
BEGIN
    SELECT * FROM TenantSettings
    WHERE TenantId = @TenantId;
END;
GO

-- UPSERT Settings (Insert if not exists, Update if exists)
CREATE OR ALTER PROCEDURE sp_TenantSettings_Upsert
    @TenantId         INT,
    @StoreName        NVARCHAR(200)  = NULL,
    @LogoUrl          NVARCHAR(500)  = NULL,
    @FaviconUrl       NVARCHAR(500)  = NULL,
    @PrimaryColor     NVARCHAR(20)   = '#ea6c2d',
    @SecondaryColor   NVARCHAR(20)   = '#1a1a2e',
    @AccentColor      NVARCHAR(20)   = '#ffffff',
    @BackgroundColor  NVARCHAR(20)   = '#ffffff',
    @TextColor        NVARCHAR(20)   = '#1a1a1a',
    @NavbarBgColor    NVARCHAR(20)   = '#ffffff',
    @NavbarTextColor  NVARCHAR(20)   = '#1a1a1a',
    @FooterBgColor    NVARCHAR(20)   = '#0f172a',
    @FooterTextColor  NVARCHAR(20)   = '#ffffff',
    @ButtonColor      NVARCHAR(20)   = '#ea6c2d',
    @ButtonTextColor  NVARCHAR(20)   = '#ffffff',
    @FontFamily       NVARCHAR(100)  = 'Poppins',
    @FacebookUrl      NVARCHAR(300)  = NULL,
    @InstagramUrl     NVARCHAR(300)  = NULL,
    @WhatsappNumber   NVARCHAR(50)   = NULL,
    @FooterTagline    NVARCHAR(500)  = NULL
AS
BEGIN
    IF EXISTS (SELECT 1 FROM TenantSettings WHERE TenantId = @TenantId)
    BEGIN
        UPDATE TenantSettings SET
            StoreName        = @StoreName,
            LogoUrl          = @LogoUrl,
            FaviconUrl       = @FaviconUrl,
            PrimaryColor     = @PrimaryColor,
            SecondaryColor   = @SecondaryColor,
            AccentColor      = @AccentColor,
            BackgroundColor  = @BackgroundColor,
            TextColor        = @TextColor,
            NavbarBgColor    = @NavbarBgColor,
            NavbarTextColor  = @NavbarTextColor,
            FooterBgColor    = @FooterBgColor,
            FooterTextColor  = @FooterTextColor,
            ButtonColor      = @ButtonColor,
            ButtonTextColor  = @ButtonTextColor,
            FontFamily       = @FontFamily,
            FacebookUrl      = @FacebookUrl,
            InstagramUrl     = @InstagramUrl,
            WhatsappNumber   = @WhatsappNumber,
            FooterTagline    = @FooterTagline,
            UpdatedDate      = GETDATE()
        WHERE TenantId = @TenantId;
    END
    ELSE
    BEGIN
        INSERT INTO TenantSettings (
            TenantId, StoreName, LogoUrl, FaviconUrl,
            PrimaryColor, SecondaryColor, AccentColor,
            BackgroundColor, TextColor, NavbarBgColor, NavbarTextColor,
            FooterBgColor, FooterTextColor, ButtonColor, ButtonTextColor,
            FontFamily, FacebookUrl, InstagramUrl, WhatsappNumber,
            FooterTagline, UpdatedDate
        ) VALUES (
            @TenantId, @StoreName, @LogoUrl, @FaviconUrl,
            @PrimaryColor, @SecondaryColor, @AccentColor,
            @BackgroundColor, @TextColor, @NavbarBgColor, @NavbarTextColor,
            @FooterBgColor, @FooterTextColor, @ButtonColor, @ButtonTextColor,
            @FontFamily, @FacebookUrl, @InstagramUrl, @WhatsappNumber,
            @FooterTagline, GETDATE()
        );
    END

    SELECT * FROM TenantSettings WHERE TenantId = @TenantId;
END;
GO

PRINT 'TenantSettings stored procedures created!';
GO

-- =============================================
-- STEP 4: STORED PROCEDURES — TenantSliders
-- =============================================

-- GET all active sliders for a tenant
CREATE OR ALTER PROCEDURE sp_TenantSliders_GetByTenant
    @TenantId INT
AS
BEGIN
    SELECT * FROM TenantSliders
    WHERE TenantId = @TenantId AND IsActive = 1
    ORDER BY OrderNo ASC;
END;
GO

-- GET all sliders (admin — including inactive)
CREATE OR ALTER PROCEDURE sp_TenantSliders_GetAll
    @TenantId INT
AS
BEGIN
    SELECT * FROM TenantSliders
    WHERE TenantId = @TenantId
    ORDER BY OrderNo ASC;
END;
GO

-- ADD slider
CREATE OR ALTER PROCEDURE sp_TenantSliders_Add
    @TenantId     INT,
    @ImageUrl     NVARCHAR(500),
    @Title        NVARCHAR(200)  = NULL,
    @Subtitle     NVARCHAR(500)  = NULL,
    @ButtonText   NVARCHAR(100)  = NULL,
    @ButtonLink   NVARCHAR(300)  = NULL,
    @OrderNo      INT            = 1,
    @IsActive     BIT            = 1
AS
BEGIN
    INSERT INTO TenantSliders
        (TenantId, ImageUrl, Title, Subtitle, ButtonText, ButtonLink, OrderNo, IsActive, CreatedDate)
    VALUES
        (@TenantId, @ImageUrl, @Title, @Subtitle, @ButtonText, @ButtonLink, @OrderNo, @IsActive, GETDATE());

    SELECT SCOPE_IDENTITY() AS SliderId;
END;
GO

-- UPDATE slider
CREATE OR ALTER PROCEDURE sp_TenantSliders_Update
    @SliderId     INT,
    @ImageUrl     NVARCHAR(500),
    @Title        NVARCHAR(200)  = NULL,
    @Subtitle     NVARCHAR(500)  = NULL,
    @ButtonText   NVARCHAR(100)  = NULL,
    @ButtonLink   NVARCHAR(300)  = NULL,
    @OrderNo      INT            = 1,
    @IsActive     BIT            = 1
AS
BEGIN
    UPDATE TenantSliders SET
        ImageUrl    = @ImageUrl,
        Title       = @Title,
        Subtitle    = @Subtitle,
        ButtonText  = @ButtonText,
        ButtonLink  = @ButtonLink,
        OrderNo     = @OrderNo,
        IsActive    = @IsActive
    WHERE SliderId = @SliderId;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- DELETE slider
CREATE OR ALTER PROCEDURE sp_TenantSliders_Delete
    @SliderId INT
AS
BEGIN
    DELETE FROM TenantSliders WHERE SliderId = @SliderId;
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO







PRINT 'All stored procedures created successfully!';
GO
