# Database Setup and Installation Guide

This guide provides comprehensive instructions for setting up and configuring the Inventory Management System database.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Database Setup Options](#database-setup-options)
- [Manual Database Setup](#manual-database-setup)
- [Entity Framework Migrations](#entity-framework-migrations)
- [Configuration](#configuration)
- [Data Seeding](#data-seeding)
- [Testing the Installation](#testing-the-installation)
- [Troubleshooting](#troubleshooting)
- [Maintenance](#maintenance)

---

## Prerequisites

### Software Requirements

#### Database Server
- **SQL Server 2019 or later** (Express, Standard, or Enterprise edition)
- **SQL Server Management Studio (SSMS)** for database administration
- **SQL Server Data Tools (SSDT)** for development (optional)

#### Development Environment
- **.NET 8.0 SDK** or later
- **Visual Studio 2022** or **Visual Studio Code**
- **Entity Framework Core 8.0** tools

#### Permissions
- **SQL Server Sysadmin** or **Database Creator** permissions
- **DBO (Database Owner)** permissions on the target database
- **ALTER**, **CREATE**, **DROP** permissions for schema management

### Hardware Requirements

#### Minimum Requirements
- **CPU**: 2 cores
- **RAM**: 4 GB
- **Storage**: 10 GB available space
- **Network**: 100 Mbps

#### Recommended Requirements
- **CPU**: 4+ cores
- **RAM**: 16+ GB
- **Storage**: 50+ GB SSD
- **Network**: 1 Gbps

---

## Database Setup Options

### Option 1: Entity Framework Migrations (Recommended)

This is the preferred method for development and production deployments.

**Pros:**
- Automated version control
- Consistent deployments
- Rollback capabilities
- Integration with application code

**Cons:**
- Requires .NET development environment
- Learning curve for EF Core migrations

### Option 2: Manual SQL Script Setup

This method uses generated SQL scripts for manual deployment.

**Pros:**
- No .NET dependencies
- Full control over execution
- Suitable for DBA teams
- Can be integrated into CI/CD pipelines

**Cons:**
- Manual version management
- No automatic rollback
- More prone to human error

### Option 3: Database Project (Visual Studio)

Using a SQL Server Database Project for enterprise deployments.

**Pros:**
- Source control integration
- Automated testing
- CI/CD pipeline integration
- Schema comparison tools

**Cons:**
- Requires Visual Studio
- Steeper learning curve
- Additional project maintenance

---

## Manual Database Setup

### Step 1: Create Database

#### Using SQL Server Management Studio

```sql
-- Create the inventory database
CREATE DATABASE [InventorySystemDB]
ON PRIMARY (
    NAME = N'InventorySystemDB',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InventorySystemDB.mdf',
    SIZE = 50MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 10MB
)
LOG ON (
    NAME = N'InventorySystemDB_log',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InventorySystemDB_log.ldf',
    SIZE = 10MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 10%
);

-- Set database options
ALTER DATABASE [InventorySystemDB] SET COMPATIBILITY_LEVEL = 150;
ALTER DATABASE [InventorySystemDB] SET ANSI_NULL_DEFAULT ON;
ALTER DATABASE [InventorySystemDB] SET ANSI_NULLS ON;
ALTER DATABASE [InventorySystemDB] SET ANSI_PADDING ON;
ALTER DATABASE [InventorySystemDB] SET ANSI_WARNINGS ON;
ALTER DATABASE [InventorySystemDB] SET ARITHABORT ON;
ALTER DATABASE [InventorySystemDB] SET AUTO_CLOSE OFF;
ALTER DATABASE [InventorySystemDB] SET AUTO_SHRINK OFF;
ALTER DATABASE [InventorySystemDB] SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE [InventorySystemDB] SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE [InventorySystemDB] SET CURSOR_CLOSE_ON_COMMIT OFF;
ALTER DATABASE [InventorySystemDB] SET CURSOR_DEFAULT GLOBAL;
ALTER DATABASE [InventorySystemDB] SET CONCAT_NULL_YIELDS_NULL ON;
ALTER DATABASE [InventorySystemDB] SET NUMERIC_ROUNDABORT OFF;
ALTER DATABASE [InventorySystemDB] SET QUOTED_IDENTIFIER ON;
ALTER DATABASE [InventorySystemDB] SET RECURSIVE_TRIGGERS OFF;
ALTER DATABASE [InventorySystemDB] SET ENABLE_BROKER;
ALTER DATABASE [InventorySystemDB] SET AUTO_UPDATE_STATISTICS_ASYNC ON;
ALTER DATABASE [InventorySystemDB] SET DATE_CORRELATION_OPTIMIZATION OFF;
ALTER DATABASE [InventorySystemDB] SET TRUSTWORTHY OFF;
ALTER DATABASE [InventorySystemDB] SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE [InventorySystemDB] SET PARAMETERIZATION SIMPLE;
ALTER DATABASE [InventorySystemDB] SET READ_COMMITTED_SNAPSHOT ON;
ALTER DATABASE [InventorySystemDB] SET HONOR_BROKER_PRIORITY OFF;
ALTER DATABASE [InventorySystemDB] SET RECOVERY FULL;
ALTER DATABASE [InventorySystemDB] SET PAGE_VERIFY CHECKSUM;
ALTER DATABASE [InventorySystemDB] SET DB_CHAINING OFF;
ALTER DATABASE [InventorySystemDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF );
ALTER DATABASE [InventorySystemDB] SET XTP_BATCH_DELAY_ON = OFF;
ALTER DATABASE [InventorySystemDB] SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = ON;
ALTER DATABASE [InventorySystemDB] SET DELAYED_DURABILITY = DISABLED;
ALTER DATABASE [InventorySystemDB] SET QUERY_STORE = OFF;
ALTER DATABASE [InventorySystemDB] SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
ALTER DATABASE [InventorySystemDB] SCOPED CONFIGURATION SET MAXDOP = 0;
ALTER DATABASE [InventorySystemDB] SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
ALTER DATABASE [InventorySystemDB] SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
```

#### Using PowerShell

```powershell
# Create database using PowerShell
Invoke-Sqlcmd -ServerInstance "localhost" -Query "
CREATE DATABASE [InventorySystemDB];
ALTER DATABASE [InventorySystemDB] SET RECOVERY FULL;
"
```

### Step 2: Create Database Schema

#### Core Tables Script

```sql
USE [InventorySystemDB];
GO

-- Create Categories table
CREATE TABLE [dbo].[Categories] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Create Brands table
CREATE TABLE [dbo].[Brands] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NULL,
    CONSTRAINT [PK_Brands] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Create Suppliers table
CREATE TABLE [dbo].[Suppliers] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NULL,
    [ContactInfo] [nvarchar](max) NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Create Devices table
CREATE TABLE [dbo].[Devices] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NULL,
    [SerialNumber] [nvarchar](max) NULL,
    [CategoryId] [uniqueidentifier] NOT NULL,
    [BrandId] [uniqueidentifier] NOT NULL,
    [SupplierId] [uniqueidentifier] NOT NULL,
    [IsFaulty] [bit] NOT NULL,
    [IsAvailable] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Devices_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Devices_Brands] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brands] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Devices_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([Id]) ON DELETE CASCADE
);
GO

-- Create indexes for Devices table
CREATE NONCLUSTERED INDEX [IX_Devices_CategoryId] ON [dbo].[Devices] ([CategoryId]);
CREATE NONCLUSTERED INDEX [IX_Devices_BrandId] ON [dbo].[Devices] ([BrandId]);
CREATE NONCLUSTERED INDEX [IX_Devices_SupplierId] ON [dbo].[Devices] ([SupplierId]);
GO

-- Create Employees table
CREATE TABLE [dbo].[Employees] (
    [Id] [uniqueidentifier] NOT NULL,
    [FirstName] [nvarchar](max) NULL,
    [LastName] [nvarchar](max) NULL,
    [EmployeeNumber] [nvarchar](max) NULL,
    [Position] [nvarchar](max) NULL,
    [Email] [nvarchar](max) NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Create Offices table
CREATE TABLE [dbo].[Offices] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NULL,
    [Location] [nvarchar](max) NULL,
    CONSTRAINT [PK_Offices] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Create DeviceAssignments table
CREATE TABLE [dbo].[DeviceAssignments] (
    [Id] [uniqueidentifier] NOT NULL,
    [DeviceId] [uniqueidentifier] NOT NULL,
    [EmployeeId] [uniqueidentifier] NULL,
    [OfficeId] [uniqueidentifier] NULL,
    [AssignedDate] [datetime2](7) NOT NULL,
    [ReturnedDate] [datetime2](7) NULL,
    [IsActive] [bit] NOT NULL,
    CONSTRAINT [PK_DeviceAssignments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_DeviceAssignments_DeviceId] UNIQUE NONCLUSTERED ([DeviceId]),
    CONSTRAINT [FK_DeviceAssignments_Devices] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Devices] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DeviceAssignments_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]) ON DELETE RESTRICT,
    CONSTRAINT [FK_DeviceAssignments_Offices] FOREIGN KEY ([OfficeId]) REFERENCES [dbo].[Offices] ([Id]) ON DELETE RESTRICT
);
GO

-- Create indexes for DeviceAssignments table
CREATE NONCLUSTERED INDEX [IX_DeviceAssignments_EmployeeId] ON [dbo].[DeviceAssignments] ([EmployeeId]);
CREATE NONCLUSTERED INDEX [IX_DeviceAssignments_OfficeId] ON [dbo].[DeviceAssignments] ([OfficeId]);
GO

-- Create MaintenanceSchedules table
CREATE TABLE [dbo].[MaintenanceSchedules] (
    [Id] [uniqueidentifier] NOT NULL,
    [DeviceId] [uniqueidentifier] NOT NULL,
    [ScheduledDate] [datetime2](7) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [IsCompleted] [bit] NOT NULL,
    CONSTRAINT [PK_MaintenanceSchedules] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MaintenanceSchedules_Devices] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Devices] ([Id]) ON DELETE CASCADE
);
GO

-- Create index for MaintenanceSchedules table
CREATE NONCLUSTERED INDEX [IX_MaintenanceSchedules_DeviceId] ON [dbo].[MaintenanceSchedules] ([DeviceId]);
GO

-- Create ServiceHistories table
CREATE TABLE [dbo].[ServiceHistories] (
    [Id] [uniqueidentifier] NOT NULL,
    [DeviceId] [uniqueidentifier] NOT NULL,
    [ServiceDate] [datetime2](7) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [PerformedBy] [nvarchar](max) NULL,
    CONSTRAINT [PK_ServiceHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceHistories_Devices] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Devices] ([Id]) ON DELETE CASCADE
);
GO

-- Create index for ServiceHistories table
CREATE NONCLUSTERED INDEX [IX_ServiceHistories_DeviceId] ON [dbo].[ServiceHistories] ([DeviceId]);
GO

-- Create Reports table
CREATE TABLE [dbo].[Reports] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NULL,
    [GeneratedDate] [datetime2](7) NOT NULL,
    [Content] [nvarchar](max) NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
```

#### ASP.NET Core Identity Tables

```sql
-- Create ASP.NET Core Identity tables
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](256) NULL,
    [NormalizedName] [nvarchar](256) NULL,
    [ConcurrencyStamp] [nvarchar](max) NULL,
    [DateCreated] [datetime2](7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE TABLE [dbo].[AspNetUsers] (
    [Id] [nvarchar](450) NOT NULL,
    [UserName] [nvarchar](256) NULL,
    [NormalizedUserName] [nvarchar](256) NULL,
    [Email] [nvarchar](256) NULL,
    [NormalizedEmail] [nvarchar](256) NULL,
    [EmailConfirmed] [bit] NOT NULL DEFAULT 0,
    [PasswordHash] [nvarchar](max) NULL,
    [SecurityStamp] [nvarchar](max) NULL,
    [ConcurrencyStamp] [nvarchar](max) NULL,
    [PhoneNumber] [nvarchar](max) NULL,
    [PhoneNumberConfirmed] [bit] NOT NULL DEFAULT 0,
    [TwoFactorEnabled] [bit] NOT NULL DEFAULT 0,
    [LockoutEnd] [datetimeoffset](7) NULL,
    [LockoutEnabled] [bit] NOT NULL DEFAULT 1,
    [AccessFailedCount] [int] NOT NULL DEFAULT 0,
    [FirstName] [nvarchar](max) NULL,
    [LastName] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([NormalizedEmail]);
GO

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] [nvarchar](450) NOT NULL,
    [RoleId] [nvarchar](450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles] ([RoleId]);
GO

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar](450) NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims] ([UserId]);
GO

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] [nvarchar](450) NOT NULL,
    [ProviderKey] [nvarchar](450) NOT NULL,
    [ProviderDisplayName] [nvarchar](max) NULL,
    [UserId] [nvarchar](450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins] ([UserId]);
GO

CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] [nvarchar](450) NOT NULL,
    [LoginProvider] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](450) NOT NULL,
    [Value] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleId] [nvarchar](450) NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims] ([RoleId]);
GO
```

---

## Entity Framework Migrations

### Step 1: Install EF Core Tools

```bash
# Install global EF Core tools
dotnet tool install --global dotnet-ef

# Update existing tools
dotnet tool update --global dotnet-ef
```

### Step 2: Configure Database Context

Ensure your `RepositoryContext` is properly configured in `Program.cs`:

```csharp
// In Program.cs
builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("InventrySystem")
    ));

// Configure Identity
builder.Services.AddIdentity<User, UserRole>(options =>
{
    // Identity options
})
    .AddEntityFrameworkStores<RepositoryContext>()
    .AddDefaultTokenProviders();
```

### Step 3: Update Connection String

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server-name;Database=InventorySystemDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;"
  }
}
```

### Step 4: Apply Migrations

#### Development Environment

```bash
# Navigate to the project directory
cd InventorySystem/InventrySystem

# Add new migration (if needed)
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

#### Production Environment

```bash
# Generate SQL script for review
dotnet ef migrations script

# Apply to specific migration
dotnet ef database update 20240724112030_InitialData
```

### Step 5: Verify Migration History

```sql
-- Check migration history
SELECT [MigrationId], [ProductVersion]
FROM [dbo].[__EFMigrationsHistory]
ORDER BY [MigrationId];
```

---

## Configuration

### Connection String Configuration

#### Development (appsettings.Development.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InventorySystemDB_Dev;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Debug"
    }
  }
}
```

#### Production (appsettings.Production.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=InventorySystemDB;User Id=app_user;Password=strong_password;MultipleActiveResultSets=true;TrustServerCertificate=true;Connection Timeout=30;Command Timeout=300;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.EntityFrameworkCore": "Error"
    }
  }
}
```

### Entity Framework Configuration

```csharp
// In RepositoryContext.cs
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions
                .EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)
                .CommandTimeout(300));
    }
}
```

---

## Data Seeding

### Method 1: EF Core Data Seeding

In your `DbContext` or `ModelConfiguration` classes:

```csharp
// In RoleConfiguration.cs
modelBuilder.Entity<UserRole>().HasData(
    new UserRole
    {
        Id = "cfa9978f-2afd-4786-9cf9-97b4493f4d34",
        Name = "Admin",
        NormalizedName = "ADMIN",
        DateCreated = new DateTime(2015, 10, 13)
    },
    new UserRole
    {
        Id = "6a670f0d-a08f-4bba-b1fd-9b6df6e42d70",
        Name = "User",
        NormalizedName = "USER",
        DateCreated = new DateTime(2015, 10, 13)
    }
);

// In CategoryConfiguration.cs
modelBuilder.Entity<Category>().HasData(
    new Category
    {
        Id = new Guid("9aa0f4cd-de28-4d3c-b38b-586819845ba3"),
        Name = "Laptops"
    },
    new Category
    {
        Id = new Guid("afc1bef3-e71d-4bd8-9bb2-c838c40e9ee0"),
        Name = "Desktops"
    }
    // ... more seed data
);
```

### Method 2: SQL Script Seeding

```sql
-- Insert seed data
INSERT INTO Categories (Id, Name) VALUES
('9aa0f4cd-de28-4d3c-b38b-586819845ba3', 'Laptops'),
('afc1bef3-e71d-4bd8-9bb2-c838c40e9ee0', 'Desktops'),
('42a2b158-1964-47da-8c4e-31a249aa1b3a', 'Printers'),
('f8f32941-7bad-471e-9d15-07b0ed660516', 'Mobile Phone');

INSERT INTO Brands (Id, Name) VALUES
('f10323d3-da72-44e7-ae7d-0379da31b329', 'Apple'),
('302a431a-2f54-4768-8a34-b6414f3909df', 'Samsung'),
('14c1b3fb-57d0-48f5-aa4a-130a1ab629c0', 'Dell'),
('89491906-e1e3-4d90-b8da-7363d1d92518', 'Lenovo'),
('ffb0451c-5f0b-457d-a513-e308e9b87326', 'HP');

-- Insert default admin user
INSERT INTO AspNetRoles (Id, Name, NormalizedName, DateCreated) VALUES
('cfa9978f-2afd-4786-9cf9-97b4493f4d34', 'Admin', 'ADMIN', GETDATE()),
('6a670f0d-a08f-4bba-b1fd-9b6df6e42d70', 'User', 'USER', GETDATE());

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, FirstName, LastName, SecurityStamp) VALUES
('a2bd32c0-d75e-4966-8274-758e273da3fb', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEPJVW/qjZZqjKrbSEDEQ3D9qvD+HCiPTlVu7PfZpJ6JUrQ8KHmAyNZuNZ5bdkIGNfQ==', 'System', 'Administrator', '');

INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES
('a2bd32c0-d75e-4966-8274-758e273da3fb', 'cfa9978f-2afd-4786-9cf9-97b4493f4d34');
```

---

## Testing the Installation

### 1. Basic Connectivity Test

```sql
-- Test database connectivity
SELECT 
    DB_NAME() AS DatabaseName,
    USER_NAME() AS CurrentUser,
    SERVERPROPERTY('ProductVersion') AS SQLServerVersion,
    GETDATE() AS ServerTime;
```

### 2. Table Verification

```sql
-- Verify all tables exist
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;
```

### 3. Data Integrity Test

```sql
-- Test foreign key relationships
SELECT 
    COUNT(*) AS TotalDevices,
    COUNT(DISTINCT d.CategoryId) AS CategoriesUsed,
    COUNT(DISTINCT d.BrandId) AS BrandsUsed,
    COUNT(DISTINCT d.SupplierId) AS SuppliersUsed
FROM Devices d
WHERE d.CategoryId IN (SELECT Id FROM Categories)
  AND d.BrandId IN (SELECT Id FROM Brands)
  AND d.SupplierId IN (SELECT Id FROM Suppliers);
```

### 4. Application Connection Test

Create a simple console application to test Entity Framework connection:

```csharp
using Microsoft.EntityFrameworkCore;
using Repository;

var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
optionsBuilder.UseSqlServer("your-connection-string");

using var context = new RepositoryContext(optionsBuilder.Options);

try
{
    var canConnect = context.Database.CanConnect();
    Console.WriteLine($"Database connection: {(canConnect ? "Success" : "Failed")}");
    
    var migrations = context.Database.GetAppliedMigrations();
    Console.WriteLine($"Applied migrations: {string.Join(", ", migrations)}");
    
    var deviceCount = context.Devices.Count();
    Console.WriteLine($"Device count: {deviceCount}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

---

## Troubleshooting

### Common Issues

#### 1. Connection String Problems

**Error:** `A network-related or instance-specific error occurred`

**Solution:**
- Verify server name is correct
- Check SQL Server is running
- Ensure firewall allows SQL Server connections
- Test connection in SSMS first

#### 2. Login Failures

**Error:** `Login failed for user`

**Solution:**
- Create database user if using SQL authentication:
```sql
CREATE USER [app_user] FOR LOGIN [app_user];
ALTER ROLE [db_owner] ADD MEMBER [app_user];
```

- For Windows authentication, ensure IIS/AppPool has permissions

#### 3. Migration Conflicts

**Error:** `There is already an object named 'TableName' in the database`

**Solution:**
- Check `__EFMigrationsHistory` table for applied migrations
- Use `dotnet ef database update` to sync
- For manual fixes, use `dotnet ef migrations script`

#### 4. Permission Issues

**Error:** `The EXECUTE permission was denied on the object`

**Solution:**
```sql
-- Grant necessary permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO [app_user];
GRANT EXECUTE ON SCHEMA::dbo TO [app_user];
```

### Diagnostic Queries

```sql
-- Check database state
SELECT 
    name,
    state_desc,
    recovery_model_desc,
    compatibility_level
FROM sys.databases
WHERE name = 'InventorySystemDB';

-- Check table sizes
SELECT 
    t.NAME AS TableName,
    s.Name AS SchemaName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS TotalSpaceKB,
    SUM(a.used_pages) * 8 AS UsedSpaceKB,
    (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
INNER JOIN sys.partitions p ON i.object_id = p.object_id AND i.index_id = p.index_id
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
LEFT OUTER JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE t.NAME NOT LIKE 'dt%'
    AND t.is_ms_shipped = 0
    AND i.object_id > 255
GROUP BY t.Name, s.Name, p.Rows
ORDER BY TotalSpaceKB DESC;

-- Check recent errors
SELECT TOP 10 
    log_date,
    processinfo,
    error_number,
    severity,
    state,
    text
FROM sys.fn_get_audit_file('C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Log\*.sqlaudit',default,default)
WHERE text LIKE '%InventorySystemDB%'
ORDER BY log_date DESC;
```

---

## Maintenance

### Regular Maintenance Tasks

#### 1. Index Maintenance

```sql
-- Rebuild fragmented indexes
DECLARE @TableName NVARCHAR(255);
DECLARE @IndexName NVARCHAR(255);
DECLARE @SQL NVARCHAR(MAX);

DECLARE Cursor_Index CURSOR FOR
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ps
JOIN sys.indexes i ON ps.object_id = i.object_id AND ps.index_id = i.index_id
WHERE ps.avg_fragmentation_in_percent > 30
  AND i.name IS NOT NULL;

OPEN Cursor_Index;
FETCH NEXT FROM Cursor_Index INTO @TableName, @IndexName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'ALTER INDEX [' + @IndexName + '] ON [' + @TableName + '] REBUILD;';
    EXEC sp_executesql @SQL;
    
    FETCH NEXT FROM Cursor_Index INTO @TableName, @IndexName;
END

CLOSE Cursor_Index;
DEALLOCATE Cursor_Index;
```

#### 2. Statistics Update

```sql
-- Update all statistics
EXEC sp_updatestats;

-- Update specific table statistics
UPDATE STATISTICS Devices;
UPDATE STATISTICS DeviceAssignments;
UPDATE STATISTICS AspNetUsers;
```

#### 3. Backup Strategy

```sql
-- Full backup
BACKUP DATABASE [InventorySystemDB]
TO DISK = 'C:\Backups\InventorySystemDB_Full.bak'
WITH FORMAT, INIT, NAME = 'InventorySystemDB-Full Database Backup', STATS = 10;

-- Differential backup
BACKUP DATABASE [InventorySystemDB]
TO DISK = 'C:\Backups\InventorySystemDB_Diff.bak'
WITH DIFFERENTIAL, STATS = 10;

-- Transaction log backup
BACKUP LOG [InventorySystemDB]
TO DISK = 'C:\Backups\InventorySystemDB_Log.trn'
WITH STATS = 10;
```

#### 4. Monitoring Script

```sql
-- Database health check
SELECT 
    'Database Size' AS Metric,
    CAST(SUM(size) * 8.0 / 1024 AS DECIMAL(10,2)) AS ValueMB
FROM sys.master_files
WHERE database_id = DB_ID('InventorySystemDB')

UNION ALL

SELECT 
    'Active Connections',
    COUNT(*)
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('InventorySystemDB')
  AND is_user_process = 1

UNION ALL

SELECT 
    'Blocked Processes',
    COUNT(*)
FROM sys.dm_exec_requests
WHERE blocking_session_id <> 0

UNION ALL

SELECT 
    'Long Running Queries',
    COUNT(*)
FROM sys.dm_exec_requests
WHERE total_elapsed_time > 30000000; -- 30 seconds
```

### Automated Maintenance

Create a SQL Server Agent Job for regular maintenance:

```sql
USE msdb;
GO

BEGIN TRANSACTION;
DECLARE @ReturnCode INT;

-- Create job
EXEC @ReturnCode = msdb.dbo.sp_add_job 
    @job_name = N'InventorySystemDB Maintenance', 
    @owner_login_name = N'sa', 
    @enabled = 1, 
    @description = N'Regular maintenance for Inventory System Database';

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

-- Add job step for index maintenance
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep 
    @job_name = N'InventorySystemDB Maintenance', 
    @step_name = N'Rebuild Indexes', 
    @subsystem = N'TSQL', 
    @command = N'USE InventorySystemDB;
EXEC sp_msforeachtable ''ALTER INDEX ALL ON ? REBUILD;'', 
    @database_name = N'InventorySystemDB', 
    @on_success_action = 1;

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

-- Schedule job to run weekly
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule 
    @job_name = N'InventorySystemDB Maintenance', 
    @name = N'Weekly Schedule', 
    @freq_type = 8, -- Weekly
    @freq_interval = 1, -- Sunday
    @freq_subday_type = 1, -- Once
    @active_start_time = 200000; -- 2:00 AM

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback;

COMMIT TRANSACTION;
GOTO EndSave;

QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION;

EndSave:
GO
```

This comprehensive setup guide ensures proper installation and maintenance of the Inventory Management System database. Regular monitoring and maintenance will keep the system running optimally.