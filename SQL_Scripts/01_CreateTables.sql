-- E-Commerce Admin Panel Database Schema
-- Run this script against your database to create tables

USE db47144;

-- =============================================
-- TENANTS TABLE
-- =============================================
IF OBJECT_ID('dbo.Tenants', 'U') IS NOT NULL
    DROP TABLE dbo.Tenants;

CREATE TABLE Tenants (
    TenantId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Domain NVARCHAR(255) NOT NULL,
    Logo NVARCHAR(255) NULL,
    ThemeColor NVARCHAR(50) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    Status BIT DEFAULT 1
);

-- =============================================
-- USERS TABLE
-- =============================================
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) DEFAULT 'User',
    CreatedDate DATETIME DEFAULT GETDATE(),
    Status BIT DEFAULT 1,
    CONSTRAINT FK_Users_Tenant FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
);

-- =============================================
-- CATEGORIES TABLE
-- =============================================
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL
    DROP TABLE dbo.Categories;

CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    ParentCategoryId INT NULL,
    Status BIT DEFAULT 1,
    CONSTRAINT FK_Categories_Tenant FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE,
    CONSTRAINT FK_Categories_Parent FOREIGN KEY (ParentCategoryId) REFERENCES Categories(CategoryId)
);

-- =============================================
-- PRODUCTS TABLE
-- =============================================
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL
    DROP TABLE dbo.Products;

CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL,
    ImageUrl NVARCHAR(255) NULL,
    CategoryId INT NULL,
    StockQty INT DEFAULT 0,
    Status BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Products_Tenant FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE,
    CONSTRAINT FK_Products_Category FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE SET NULL
);

-- =============================================
-- ORDERS TABLE
-- =============================================
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL
    DROP TABLE dbo.Orders;

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    CustomerName NVARCHAR(100) NOT NULL,
    CustomerEmail NVARCHAR(150) NULL,
    CustomerPhone NVARCHAR(50) NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending',
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Orders_Tenant FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
);

-- =============================================
-- ORDER DETAILS TABLE
-- =============================================
IF OBJECT_ID('dbo.OrderDetails', 'U') IS NOT NULL
    DROP TABLE dbo.OrderDetails;

CREATE TABLE OrderDetails (
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderDetails_Order FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Product FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- =============================================
-- PAGES TABLE
-- =============================================
IF OBJECT_ID('dbo.Pages', 'U') IS NOT NULL
    DROP TABLE dbo.Pages;

CREATE TABLE Pages (
    PageId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Slug NVARCHAR(100) NOT NULL,
    Status BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Pages_Tenant FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
);

-- =============================================
-- SECTIONS TABLE
-- =============================================
IF OBJECT_ID('dbo.Sections', 'U') IS NOT NULL
    DROP TABLE dbo.Sections;

CREATE TABLE Sections (
    SectionId INT IDENTITY(1,1) PRIMARY KEY,
    PageId INT NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    OrderNo INT DEFAULT 0,
    Status BIT DEFAULT 1,
    CONSTRAINT FK_Sections_Page FOREIGN KEY (PageId) REFERENCES Pages(PageId) ON DELETE CASCADE
);

-- =============================================
-- SECTION DATA TABLE
-- =============================================
IF OBJECT_ID('dbo.SectionData', 'U') IS NOT NULL
    DROP TABLE dbo.SectionData;

CREATE TABLE SectionData (
    DataId INT IDENTITY(1,1) PRIMARY KEY,
    SectionId INT NOT NULL,
    [Key] NVARCHAR(100) NOT NULL,
    [Value] NVARCHAR(MAX) NULL,
    CONSTRAINT FK_SectionData_Section FOREIGN KEY (SectionId) REFERENCES Sections(SectionId) ON DELETE CASCADE
);

-- Create indexes for better query performance
CREATE INDEX IX_Users_TenantId ON Users(TenantId);
CREATE INDEX IX_Categories_TenantId ON Categories(TenantId);
CREATE INDEX IX_Products_TenantId ON Products(TenantId);
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Orders_TenantId ON Orders(TenantId);
CREATE INDEX IX_Pages_TenantId ON Pages(TenantId);

PRINT 'Database schema created successfully!';
GO
