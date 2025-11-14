# Data Dictionary

This document provides a comprehensive dictionary of all fields in the Inventory Management System database, including detailed descriptions, data types, constraints, and usage examples.

## Field Classification

### Identifiers
Fields used to uniquely identify records and establish relationships.

### Attributes
Fields storing business data and descriptive information.

### Status Fields
Fields indicating state, condition, or availability.

### Audit Fields
Fields tracking dates, times, and modification history.

---

## Core Business Entities

### Categories Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for category records | GUID | `9aa0f4cd-de28-4d3c-b38b-586819845ba3` |
| Name | nvarchar(max) | Attribute | Display name of the device category | Text string (max 2GB) | "Laptops", "Desktops", "Printers" |

**Usage Notes:**
- Categories provide hierarchical organization of devices
- Used in dropdown lists and filtering
- Should represent broad device classifications
- Deleting a category cascades to all devices in that category

### Brands Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for brand records | GUID | `f10323d3-da72-44e7-ae7d-0379da31b329` |
| Name | nvarchar(max) | Attribute | Manufacturer or brand name | Text string (max 2GB) | "Apple", "Samsung", "Dell", "HP" |

**Usage Notes:**
- Brands represent device manufacturers
- Used for filtering and reporting
- Should match official brand names
- Deleting a brand cascades to all devices from that brand

### Suppliers Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for supplier records | GUID | `3fff2d50-83f4-4128-a5dd-bb74f0d754e8` |
| Name | nvarchar(max) | Attribute | Supplier company name | Text string (max 2GB) | "CompuParts Eswatini", "DataNet Eswatini" |
| ContactInfo | nvarchar(max) | Attribute | Supplier contact details | Text string (max 2GB) | "Phone: +268 1234 5678, Email: info@compuparts.sz" |

**Usage Notes:**
- Suppliers track vendor information for procurement
- Contact info should include phone, email, address
- Used for warranty and support information
- Deleting a supplier cascades to all devices from that supplier

### Devices Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for device records | GUID | `b2719efd-4bea-4cc2-84c3-4400e838a545` |
| Name | nvarchar(max) | Attribute | Device name or description | Text string (max 2GB) | "Laptop X1", "Desktop Y1", "Printer Z1" |
| SerialNumber | nvarchar(max) | Attribute | Manufacturer serial number | Text string (max 2GB) | "SN123456", "SN789012", "SN345678" |
| CategoryId | uniqueidentifier | Identifier | Foreign key to Categories | Valid Category GUID | `9aa0f4cd-de28-4d3c-b38b-586819845ba3` |
| BrandId | uniqueidentifier | Identifier | Foreign key to Brands | Valid Brand GUID | `f10323d3-da72-44e7-ae7d-0379da31b329` |
| SupplierId | uniqueidentifier | Identifier | Foreign key to Suppliers | Valid Supplier GUID | `029e2d94-fd9d-41bd-9b4a-58b2f738c662` |
| IsFaulty | bit | Status | Device fault condition | 0 (Good) or 1 (Faulty) | `0`, `1` |
| IsAvailable | bit | Status | Device availability for assignment | 0 (Assigned) or 1 (Available) | `0`, `1` |

**Usage Notes:**
- Central table in the inventory system
- Serial numbers should be unique within the system
- `IsAvailable` should automatically update based on assignments
- `IsFaulty` devices should not be assignable
- All foreign key relationships are required (NOT NULL)

### Employees Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for employee records | GUID | `7f8ba2f9-3462-4d1e-9dbc-eacab5de0bef` |
| FirstName | nvarchar(max) | Attribute | Employee first name | Text string (max 2GB) | "John", "Jane", "Alice" |
| LastName | nvarchar(max) | Attribute | Employee last name | Text string (max 2GB) | "Doe", "Smith", "Johnson" |
| EmployeeNumber | nvarchar(max) | Attribute | Unique employee identifier | Text string (max 2GB) | "EMP001", "EMP002", "EMP003" |
| Position | nvarchar(max) | Attribute | Job title or position | Text string (max 2GB) | "Software Engineer", "Project Manager" |
| Email | nvarchar(max) | Attribute | Employee email address | Valid email format | "john.doe@example.com" |

**Usage Notes:**
- EmployeeNumber should be unique across the organization
- Email should be validated for format
- Used for device assignment tracking
- Cannot be deleted if has active device assignments

### Offices Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for office records | GUID | `1fe78363-8333-4168-8dd7-532dcb58de42` |
| Name | nvarchar(max) | Attribute | Office name or identifier | Text string (max 2GB) | "Office A", "Office B", "Office C" |
| Location | nvarchar(max) | Attribute | Physical location description | Text string (max 2GB) | "Building 1, Floor 2", "Building 2, Floor 1" |

**Usage Notes:**
- Offices represent physical locations or departments
- Used for device location tracking
- Cannot be deleted if has active device assignments
- Location should be descriptive for easy identification

### DeviceAssignments Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for assignment records | GUID | `a1b2c3d4-e5f6-7890-abcd-ef1234567890` |
| DeviceId | uniqueidentifier | Identifier | Foreign key to Devices (Unique) | Valid Device GUID | `b2719efd-4bea-4cc2-84c3-4400e838a545` |
| EmployeeId | uniqueidentifier | Identifier | Foreign key to Employees (Nullable) | Valid Employee GUID or NULL | `7f8ba2f9-3462-4d1e-9dbc-eacab5de0bef` |
| OfficeId | uniqueidentifier | Identifier | Foreign key to Offices (Nullable) | Valid Office GUID or NULL | `1fe78363-8333-4168-8dd7-532dcb58de42` |
| AssignedDate | datetime2 | Audit | Date when device was assigned | Valid datetime | `2024-01-15 09:30:00` |
| ReturnedDate | datetime2 | Audit | Date when device was returned | Valid datetime or NULL | `2024-03-20 14:45:00` |
| IsActive | bit | Status | Current assignment status | 0 (Inactive) or 1 (Active) | `1` |

**Usage Notes:**
- DeviceId is unique - one assignment per device
- EmployeeId and OfficeId can be NULL (unassigned storage)
- AssignedDate defaults to current time on creation
- ReturnedDate is set when IsActive becomes false
- IsActive should be mutually exclusive with device availability

### MaintenanceSchedules Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for maintenance records | GUID | `m1n2o3p4-q5r6-7890-stuv-wxyz12345678` |
| DeviceId | uniqueidentifier | Identifier | Foreign key to Devices | Valid Device GUID | `b2719efd-4bea-4cc2-84c3-4400e838a545` |
| ScheduledDate | datetime2 | Audit | Planned maintenance date | Valid datetime (future) | `2024-06-15 10:00:00` |
| Description | nvarchar(max) | Attribute | Maintenance task description | Text string (max 2GB) | "Annual hardware inspection", "Software update" |
| IsCompleted | bit | Status | Maintenance completion status | 0 (Pending) or 1 (Completed) | `0` |

**Usage Notes:**
- Used for preventive maintenance planning
- ScheduledDate should be in the future for new records
- IsCompleted should be updated when maintenance is performed
- Multiple schedules can exist per device
- Completed schedules can be used for service history

### ServiceHistories Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for service records | GUID | `s1t2u3v4-w5x6-7890-yzab-cdef12345678` |
| DeviceId | uniqueidentifier | Identifier | Foreign key to Devices | Valid Device GUID | `b2719efd-4bea-4cc2-84c3-4400e838a545` |
| ServiceDate | datetime2 | Audit | Date when service was performed | Valid datetime (past) | `2024-02-10 15:30:00` |
| Description | nvarchar(max) | Attribute | Service performed description | Text string (max 2GB) | "Screen replacement", "Battery replacement" |
| PerformedBy | nvarchar(max) | Attribute | Person/organization who performed service | Text string (max 2GB) | "In-house IT team", "Authorized service center" |

**Usage Notes:**
- Maintains complete service and repair history
- ServiceDate should be in the past
- Description should be detailed for warranty tracking
- PerformedBy helps track service provider performance
- Used for device reliability analysis

### Reports Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | uniqueidentifier | Identifier | Primary key for report records | GUID | `r1e2p3o4-r5t6-7890-s123-456789abcdef` |
| Name | nvarchar(max) | Attribute | Report name or title | Text string (max 2GB) | "Monthly Inventory Report", "Device Status Summary" |
| GeneratedDate | datetime2 | Audit | Report generation timestamp | Valid datetime | `2024-03-31 17:00:00` |
| Content | nvarchar(max) | Attribute | Report content or data | Text string (max 2GB) | JSON, XML, or formatted text data |

**Usage Notes:**
- Stores generated reports for historical reference
- Content can store data in various formats
- GeneratedDate defaults to current time
- Used for audit trail and trend analysis

---

## ASP.NET Core Identity Tables

### AspNetUsers Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | nvarchar(450) | Identifier | User primary key (string GUID) | String GUID | `"a2bd32c0-d75e-4966-8274-758e273da3fb"` |
| UserName | nvarchar(256) | Attribute | User login name | Unique string | `"user@example.com"` |
| NormalizedUserName | nvarchar(256) | Attribute | Uppercase username for comparison | Unique uppercase string | `"USER@EXAMPLE.COM"` |
| Email | nvarchar(256) | Attribute | User email address | Valid email format | `"user@example.com"` |
| NormalizedEmail | nvarchar(256) | Attribute | Uppercase email for comparison | Unique uppercase string | `"USER@EXAMPLE.COM"` |
| EmailConfirmed | bit | Status | Email verification status | 0 (Unconfirmed) or 1 (Confirmed) | `1` |
| PasswordHash | nvarchar(max) | Security | Hashed password string | Hash string | `"AQAAAAIAAYagAAAAEPJVW..."` |
| FirstName | nvarchar(max) | Attribute | User first name | Text string (max 2GB) | `"John"` |
| LastName | nvarchar(max) | Attribute | User last name | Text string (max 2GB) | `"Doe"` |
| AccessFailedCount | int | Security | Failed login attempt counter | Non-negative integer | `0`, `1`, `2`, `3` |
| LockoutEnabled | bit | Security | Account lockout feature status | 0 (Disabled) or 1 (Enabled) | `1` |
| LockoutEnd | datetimeoffset | Security | Lockout end time | Valid datetime or NULL | `"2024-01-15T10:30:00Z"` |
| PhoneNumber | nvarchar(max) | Attribute | User phone number | Phone number format | `"+26812345678"` |
| PhoneNumberConfirmed | bit | Status | Phone verification status | 0 (Unconfirmed) or 1 (Confirmed) | `0` |
| SecurityStamp | nvarchar(max) | Security | Session invalidation token | Random string | `"aadea458-77e8-4a1d-b158-85bb1da80d71"` |
| TwoFactorEnabled | bit | Security | Two-factor authentication status | 0 (Disabled) or 1 (Enabled) | `0` |
| ConcurrencyStamp | nvarchar(max) | Security | Concurrency control token | Random string | `"concurrency-token-value"` |

**Usage Notes:**
- Inherits from ASP.NET Core Identity User
- UserName typically equals Email for simplicity
- SecurityStamp changes on security-sensitive operations
- LockoutEnd is set after multiple failed attempts
- PasswordHash never stores plain text passwords

### AspNetRoles Table

| Field | Type | Classification | Description | Valid Values | Example |
|-------|------|----------------|-------------|--------------|---------|
| Id | nvarchar(450) | Identifier | Role primary key (string GUID) | String GUID | `"cfa9978f-2afd-4786-9cf9-97b4493f4d34"` |
| Name | nvarchar(256) | Attribute | Role display name | Unique string | `"Admin"`, `"User"` |
| NormalizedName | nvarchar(256) | Attribute | Uppercase role name for comparison | Unique uppercase string | `"ADMIN"`, `"USER"` |
| DateCreated | datetime2 | Audit | Role creation timestamp | Valid datetime | `"2015-10-13 00:00:00"` |
| ConcurrencyStamp | nvarchar(max) | Security | Concurrency control token | Random string | `"role-concurrency-token"` |

**Usage Notes:**
- Used for role-based authorization
- Pre-populated with Admin and User roles
- DateCreated shows when role was first created
- NormalizedName ensures case-insensitive role matching

---

## Data Type Specifications

### GUID Fields (uniqueidentifier)
- **Format**: 32 hexadecimal characters with hyphens
- **Example**: `f10323d3-da72-44e7-ae7d-0379da31b329`
- **Usage**: Primary keys and foreign keys for business entities
- **Generation**: Automatically generated by Entity Framework

### String Fields (nvarchar)
- **Max Length**: `nvarchar(max)` supports up to 2GB
- **Fixed Length**: `nvarchar(n)` supports up to n characters
- **Unicode**: Supports all Unicode characters including emojis
- **Case Sensitivity**: Database collation dependent

### Boolean Fields (bit)
- **Values**: 0 (False/No) or 1 (True/Yes)
- **Storage**: 1 bit per field
- **Default**: Often set to 0 for false
- **Usage**: Status flags and switches

### Date/Time Fields (datetime2)
- **Precision**: Fractional seconds up to 7 decimal places
- **Range**: January 1, 0001 to December 31, 9999
- **Format**: `YYYY-MM-DD HH:MM:SS.fffffff`
- **Timezone**: Local server time (not UTC)

### Date/Time Offset (datetimeoffset)
- **Precision**: Fractional seconds up to 7 decimal places
- **Range**: January 1, 0001 to December 31, 9999
- **Format**: `YYYY-MM-DD HH:MM:SS.fffffff ±HH:mm`
- **Timezone**: Includes timezone offset from UTC

### Integer Fields (int)
- **Range**: -2,147,483,648 to 2,147,483,647
- **Usage**: Identity table primary keys and counters
- **Identity**: Auto-incrementing for Id fields in some tables

---

## Business Rules Summary

### Required Fields
- All primary key fields are required
- Device foreign keys (Category, Brand, Supplier) are required
- Assignment dates and status are required
- User authentication fields are required

### Optional Fields
- Employee and Office in DeviceAssignments (can be NULL)
- ReturnedDate in DeviceAssignments (NULL until returned)
- Contact information fields
- Description fields for maintenance and service

### Unique Constraints
- DeviceAssignments.DeviceId (one assignment per device)
- AspNetUsers.NormalizedUserName
- AspNetRoles.NormalizedName
- SerialNumber should be unique (application-level validation)

### Default Values
- Device.IsAvailable defaults to true
- Assignment.AssignedDate defaults to current time
- Report.GeneratedDate defaults to current time
- User AccessFailedCount defaults to 0

This data dictionary serves as the authoritative reference for all database fields and their proper usage in the Inventory Management System.