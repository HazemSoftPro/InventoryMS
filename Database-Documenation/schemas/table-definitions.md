# Table Definitions

This document provides detailed definitions for all tables in the Inventory Management System database.

## Core Business Tables

### Categories

Stores device categories for classification purposes.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the category |
| Name | nvarchar(max) | NULL | Category name (e.g., Laptops, Desktops, Printers) |

**Indexes:**
- Primary key on `Id`

**Sample Data:**
- Laptops
- Desktops  
- Printers
- Mobile Phone

### Brands

Stores device manufacturer information.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the brand |
| Name | nvarchar(max) | NULL | Brand name (e.g., Apple, Samsung, Dell) |

**Indexes:**
- Primary key on `Id`

**Sample Data:**
- Apple
- Samsung
- Dell
- Lenovo
- HP

### Suppliers

Stores vendor/supplier information.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the supplier |
| Name | nvarchar(max) | NULL | Supplier company name |
| ContactInfo | nvarchar(max) | NULL | Contact details for the supplier |

**Indexes:**
- Primary key on `Id`

**Sample Data:**
- CompuParts Eswatini
- DataNet Eswatini
- Omega IT Eswatini

### Devices

Central table storing all inventory items.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the device |
| Name | nvarchar(max) | NULL | Device name/description |
| SerialNumber | nvarchar(max) | NULL | Manufacturer serial number |
| CategoryId | uniqueidentifier | FOREIGN KEY, NOT NULL | Reference to Categories table |
| BrandId | uniqueidentifier | FOREIGN KEY, NOT NULL | Reference to Brands table |
| SupplierId | uniqueidentifier | FOREIGN KEY, NOT NULL | Reference to Suppliers table |
| IsFaulty | bit | NOT NULL | Device fault status (0=Good, 1=Faulty) |
| IsAvailable | bit | NOT NULL, DEFAULT 1 | Device availability (0=Assigned, 1=Available) |

**Indexes:**
- Primary key on `Id`
- Foreign key index on `CategoryId`
- Foreign key index on `BrandId` 
- Foreign key index on `SupplierId`

**Constraints:**
- FOREIGN KEY `CategoryId` → `Categories(Id)` ON DELETE CASCADE
- FOREIGN KEY `BrandId` → `Brands(Id)` ON DELETE CASCADE
- FOREIGN KEY `SupplierId` → `Suppliers(Id)` ON DELETE CASCADE

### Employees

Stores employee information for device assignments.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the employee |
| FirstName | nvarchar(max) | NULL | Employee first name |
| LastName | nvarchar(max) | NULL | Employee last name |
| EmployeeNumber | nvarchar(max) | NULL | Unique employee identifier |
| Position | nvarchar(max) | NULL | Job position/title |
| Email | nvarchar(max) | NULL | Employee email address |

**Indexes:**
- Primary key on `Id`

### Offices

Stores office/location information.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the office |
| Name | nvarchar(max) | NULL | Office name/identifier |
| Location | nvarchar(max) | NULL | Physical location description |

**Indexes:**
- Primary key on `Id`

**Sample Data:**
- Office A - Building 1, Floor 2
- Office B - Building 2, Floor 1
- Office C - Building 3, Floor 3

### DeviceAssignments

Tracks device assignments to employees and offices.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the assignment |
| DeviceId | uniqueidentifier | FOREIGN KEY, NOT NULL, UNIQUE | Reference to Devices table |
| EmployeeId | uniqueidentifier | FOREIGN KEY, NULL | Reference to Employees table |
| OfficeId | uniqueidentifier | FOREIGN KEY, NULL | Reference to Offices table |
| AssignedDate | datetime2 | NOT NULL | Date when device was assigned |
| ReturnedDate | datetime2 | NULL | Date when device was returned |
| IsActive | bit | NOT NULL | Assignment status (0=Inactive, 1=Active) |

**Indexes:**
- Primary key on `Id`
- Unique index on `DeviceId` (ensures one active assignment per device)
- Foreign key index on `EmployeeId`
- Foreign key index on `OfficeId`

**Constraints:**
- FOREIGN KEY `DeviceId` → `Devices(Id)` ON DELETE CASCADE
- FOREIGN KEY `EmployeeId` → `Employees(Id)` ON DELETE RESTRICT
- FOREIGN KEY `OfficeId` → `Offices(Id)` ON DELETE RESTRICT
- UNIQUE constraint on `DeviceId` (one-to-one relationship with Device)

### MaintenanceSchedules

Tracks preventive maintenance schedules for devices.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the schedule |
| DeviceId | uniqueidentifier | FOREIGN KEY, NOT NULL | Reference to Devices table |
| ScheduledDate | datetime2 | NOT NULL | Planned maintenance date |
| Description | nvarchar(max) | NULL | Maintenance description/details |
| IsCompleted | bit | NOT NULL | Completion status (0=Pending, 1=Completed) |

**Indexes:**
- Primary key on `Id`
- Foreign key index on `DeviceId`

**Constraints:**
- FOREIGN KEY `DeviceId` → `Devices(Id)` ON DELETE CASCADE

### ServiceHistories

Maintains service and repair history for devices.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the service record |
| DeviceId | uniqueidentifier | FOREIGN KEY, NOT NULL | Reference to Devices table |
| ServiceDate | datetime2 | NOT NULL | Date when service was performed |
| Description | nvarchar(max) | NULL | Service description/details |
| PerformedBy | nvarchar(max) | NULL | Person/organization who performed service |

**Indexes:**
- Primary key on `Id`
- Foreign key index on `DeviceId`

**Constraints:**
- FOREIGN KEY `DeviceId` → `Devices(Id)` ON DELETE CASCADE

### Reports

Stores generated reports.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | uniqueidentifier | PRIMARY KEY, NOT NULL | Unique identifier for the report |
| Name | nvarchar(max) | NULL | Report name/title |
| GeneratedDate | datetime2 | NOT NULL | Report generation timestamp |
| Content | nvarchar(max) | NULL | Report content/data |

**Indexes:**
- Primary key on `Id`

## Identity Management Tables

### AspNetUsers

ASP.NET Core Identity user accounts table.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | nvarchar(450) | PRIMARY KEY, NOT NULL | User identifier |
| UserName | nvarchar(256) | NOT NULL | User login name |
| NormalizedUserName | nvarchar(256) | NOT NULL | Normalized username for case-insensitive comparison |
| Email | nvarchar(256) | NOT NULL | User email address |
| NormalizedEmail | nvarchar(256) | NOT NULL | Normalized email for case-insensitive comparison |
| EmailConfirmed | bit | NOT NULL | Email verification status |
| PasswordHash | nvarchar(max) | NULL | Hashed password |
| FirstName | nvarchar(max) | NULL | User first name |
| LastName | nvarchar(max) | NULL | User last name |
| AccessFailedCount | int | NOT NULL | Failed login attempt count |
| LockoutEnabled | bit | NOT NULL | Account lockout status |
| LockoutEnd | datetimeoffset | NULL | Lockout end time |
| PhoneNumber | nvarchar(max) | NULL | Phone number |
| PhoneNumberConfirmed | bit | NOT NULL | Phone verification status |
| SecurityStamp | nvarchar(max) | NULL | Security stamp for session invalidation |
| TwoFactorEnabled | bit | NOT NULL | Two-factor authentication status |
| ConcurrencyStamp | nvarchar(max) | NULL | Concurrency token |

**Indexes:**
- Primary key on `Id`
- Index on `NormalizedEmail` (named `EmailIndex`)
- Unique index on `NormalizedUserName` (named `UserNameIndex`)

### AspNetRoles

ASP.NET Core Identity role definitions table.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | nvarchar(450) | PRIMARY KEY, NOT NULL | Role identifier |
| Name | nvarchar(256) | NOT NULL | Role name |
| NormalizedName | nvarchar(256) | NOT NULL | Normalized role name |
| DateCreated | datetime2 | NOT NULL | Role creation date |
| ConcurrencyStamp | nvarchar(max) | NULL | Concurrency token |

**Indexes:**
- Primary key on `Id`
- Unique index on `NormalizedName` (named `RoleNameIndex`)

### AspNetUserRoles

Many-to-many relationship between users and roles.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| UserId | nvarchar(450) | PRIMARY KEY, FOREIGN KEY, NOT NULL | Reference to AspNetUsers |
| RoleId | nvarchar(450) | PRIMARY KEY, FOREIGN KEY, NOT NULL | Reference to AspNetRoles |

**Indexes:**
- Composite primary key on `(UserId, RoleId)`
- Foreign key index on `RoleId`

**Constraints:**
- FOREIGN KEY `UserId` → `AspNetUsers(Id)` ON DELETE CASCADE
- FOREIGN KEY `RoleId` → `AspNetRoles(Id)` ON DELETE CASCADE

### AspNetUserClaims

Stores claims associated with users.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY, NOT NULL | Claim identifier |
| UserId | nvarchar(450) | FOREIGN KEY, NOT NULL | Reference to AspNetUsers |
| ClaimType | nvarchar(max) | NULL | Claim type |
| ClaimValue | nvarchar(max) | NULL | Claim value |

**Indexes:**
- Primary key on `Id`
- Foreign key index on `UserId`

### AspNetUserLogins

Stores external login information for users.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| LoginProvider | nvarchar(450) | PRIMARY KEY, NOT NULL | External provider name |
| ProviderKey | nvarchar(450) | PRIMARY KEY, NOT NULL | Provider-specific key |
| ProviderDisplayName | nvarchar(max) | NULL | Provider display name |
| UserId | nvarchar(450) | FOREIGN KEY, NOT NULL | Reference to AspNetUsers |

**Indexes:**
- Composite primary key on `(LoginProvider, ProviderKey)`
- Foreign key index on `UserId`

### AspNetUserTokens

Stores authentication tokens for users.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| UserId | nvarchar(450) | PRIMARY KEY, FOREIGN KEY, NOT NULL | Reference to AspNetUsers |
| LoginProvider | nvarchar(450) | PRIMARY KEY, NOT NULL | Login provider |
| Name | nvarchar(450) | PRIMARY KEY, NOT NULL | Token name |
| Value | nvarchar(max) | NULL | Token value |

**Indexes:**
- Composite primary key on `(UserId, LoginProvider, Name)`

### AspNetRoleClaims

Stores claims associated with roles.

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY, NOT NULL | Claim identifier |
| RoleId | nvarchar(450) | FOREIGN KEY, NOT NULL | Reference to AspNetRoles |
| ClaimType | nvarchar(max) | NULL | Claim type |
| ClaimValue | nvarchar(max) | NULL | Claim value |

**Indexes:**
- Primary key on `Id`
- Foreign key index on `RoleId`

## Data Types Summary

- **uniqueidentifier**: GUID primary keys and foreign keys
- **nvarchar(max)**: Variable-length Unicode strings (max 2GB)
- **nvarchar(n)**: Fixed-length Unicode strings with specified max length
- **bit**: Boolean values (0 or 1)
- **datetime2**: Date and time values with fractional seconds precision
- **datetimeoffset**: Date and time with timezone offset
- **int**: 32-bit integers (used for Identity tables)