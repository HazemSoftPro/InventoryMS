# Database Indexes

This document describes all indexes in the Inventory Management System database, their purposes, and performance considerations.

## Index Overview

Indexes are crucial for database performance, especially in queries involving joins, filtering, and sorting. The system uses a combination of primary key indexes, foreign key indexes, and unique constraints to optimize query performance.

## Primary Key Indexes

All tables have clustered primary key indexes automatically created by SQL Server:

### Business Entity Tables

| Table | Primary Key | Index Type | Description |
|-------|-------------|------------|-------------|
| Categories | Id | Clustered | Unique identifier for categories |
| Brands | Id | Clustered | Unique identifier for brands |
| Suppliers | Id | Clustered | Unique identifier for suppliers |
| Devices | Id | Clustered | Unique identifier for devices |
| Employees | Id | Clustered | Unique identifier for employees |
| Offices | Id | Clustered | Unique identifier for offices |
| DeviceAssignments | Id | Clustered | Unique identifier for assignments |
| MaintenanceSchedules | Id | Clustered | Unique identifier for schedules |
| ServiceHistories | Id | Clustered | Unique identifier for service records |
| Reports | Id | Clustered | Unique identifier for reports |

### ASP.NET Core Identity Tables

| Table | Primary Key | Index Type | Description |
|-------|-------------|------------|-------------|
| AspNetUsers | Id | Clustered | User identifier (string GUID) |
| AspNetRoles | Id | Clustered | Role identifier (string GUID) |
| AspNetUserClaims | Id | Clustered | Claim identifier (identity integer) |
| AspNetUserLogins | LoginProvider, ProviderKey | Clustered | Composite primary key |
| AspNetUserTokens | UserId, LoginProvider, Name | Clustered | Composite primary key |
| AspNetRoleClaims | Id | Clustered | Role claim identifier (identity integer) |

## Foreign Key Indexes

Foreign key indexes are essential for join performance and are automatically created by Entity Framework:

### Device Foreign Keys

| Table | Foreign Key | Referenced Table | Index Purpose |
|-------|-------------|------------------|---------------|
| Devices | CategoryId | Categories | Optimize category filtering and joins |
| Devices | BrandId | Brands | Optimize brand filtering and joins |
| Devices | SupplierId | Suppliers | Optimize supplier filtering and joins |

### Assignment Foreign Keys

| Table | Foreign Key | Referenced Table | Index Purpose |
|-------|-------------|------------------|---------------|
| DeviceAssignments | DeviceId | Devices | Optimize device assignment lookup |
| DeviceAssignments | EmployeeId | Employees | Optimize employee device history |
| DeviceAssignments | OfficeId | Offices | Optimize office device inventory |

### Maintenance and Service Foreign Keys

| Table | Foreign Key | Referenced Table | Index Purpose |
|-------|-------------|------------------|---------------|
| MaintenanceSchedules | DeviceId | Devices | Optimize maintenance schedule lookup |
| ServiceHistories | DeviceId | Devices | Optimize service history lookup |

### Identity Foreign Keys

| Table | Foreign Key | Referenced Table | Index Purpose |
|-------|-------------|------------------|---------------|
| AspNetUserRoles | UserId | AspNetUsers | Optimize user role lookup |
| AspNetUserRoles | RoleId | AspNetRoles | Optimize role member lookup |
| AspNetUserClaims | UserId | AspNetUsers | Optimize user claims lookup |
| AspNetUserLogins | UserId | AspNetUsers | Optimize external login lookup |
| AspNetUserTokens | UserId | AspNetUsers | Optimize token lookup |
| AspNetRoleClaims | RoleId | AspNetRoles | Optimize role claims lookup |

## Unique Indexes and Constraints

### Business Constraints

| Table | Index | Type | Purpose |
|-------|-------|------|---------|
| DeviceAssignments | DeviceId | Unique | Ensures one active assignment per device |
| AspNetUsers | NormalizedUserName | Unique | Prevents duplicate usernames |
| AspNetUsers | NormalizedEmail | Unique | Prevents duplicate emails |
| AspNetRoles | NormalizedName | Unique | Prevents duplicate role names |

### Identity Indexes

| Table | Index | Type | Purpose |
|-------|-------|------|---------|
| AspNetUsers | EmailIndex | Non-unique | Email lookup optimization |
| AspNetUsers | UserNameIndex | Unique | Username lookup optimization |
| AspNetRoles | RoleNameIndex | Unique | Role name lookup optimization |

## Index Creation SQL

### Primary Key Indexes
```sql
-- Example: Devices table primary key
CREATE TABLE [dbo].[Devices] (
    [Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

### Foreign Key Indexes
```sql
-- Example: Devices table foreign key indexes
CREATE NONCLUSTERED INDEX [IX_Devices_BrandId] ON [dbo].[Devices] ([BrandId]);
CREATE NONCLUSTERED INDEX [IX_Devices_CategoryId] ON [dbo].[Devices] ([CategoryId]);
CREATE NONCLUSTERED INDEX [IX_Devices_SupplierId] ON [dbo].[Devices] ([SupplierId]);
```

### Unique Indexes
```sql
-- Example: DeviceAssignments unique constraint
CREATE UNIQUE NONCLUSTERED INDEX [IX_DeviceAssignments_DeviceId] 
ON [dbo].[DeviceAssignments] ([DeviceId]);
```

## Performance Analysis

### Query Patterns and Index Usage

#### 1. Device Lookup Queries
```sql
-- Query: Find devices by category
SELECT * FROM Devices WHERE CategoryId = @CategoryId;
-- Uses: IX_Devices_CategoryId

-- Query: Find devices by brand
SELECT * FROM Devices WHERE BrandId = @BrandId;
-- Uses: IX_Devices_BrandId

-- Query: Find devices by supplier
SELECT * FROM Devices WHERE SupplierId = @SupplierId;
-- Uses: IX_Devices_SupplierId
```

#### 2. Assignment Tracking Queries
```sql
-- Query: Find current assignment for a device
SELECT * FROM DeviceAssignments WHERE DeviceId = @DeviceId AND IsActive = 1;
-- Uses: IX_DeviceAssignments_DeviceId (unique)

-- Query: Find devices assigned to employee
SELECT da.* FROM DeviceAssignments da 
JOIN Devices d ON da.DeviceId = d.Id 
WHERE da.EmployeeId = @EmployeeId AND da.IsActive = 1;
-- Uses: IX_DeviceAssignments_EmployeeId, PK_Devices

-- Query: Find devices in office
SELECT da.* FROM DeviceAssignments da 
WHERE da.OfficeId = @OfficeId AND da.IsActive = 1;
-- Uses: IX_DeviceAssignments_OfficeId
```

#### 3. Maintenance and Service Queries
```sql
-- Query: Find upcoming maintenance for device
SELECT * FROM MaintenanceSchedules 
WHERE DeviceId = @DeviceId AND IsCompleted = 0 
ORDER BY ScheduledDate;
-- Uses: IX_MaintenanceSchedules_DeviceId

-- Query: Find service history for device
SELECT * FROM ServiceHistories 
WHERE DeviceId = @DeviceId 
ORDER BY ServiceDate DESC;
-- Uses: IX_ServiceHistories_DeviceId
```

#### 4. User Authentication Queries
```sql
-- Query: Find user by email
SELECT * FROM AspNetUsers WHERE NormalizedEmail = @NormalizedEmail;
-- Uses: EmailIndex

-- Query: Find user by username
SELECT * FROM AspNetUsers WHERE NormalizedUserName = @NormalizedUserName;
-- Uses: UserNameIndex (unique)

-- Query: Find user roles
SELECT r.* FROM AspNetRoles r
JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
WHERE ur.UserId = @UserId;
-- Uses: IX_AspNetUserRoles_UserId, RoleNameIndex
```

## Index Optimization Recommendations

### Current Index Assessment

#### Well-Optimized Areas
1. **Primary Keys**: All tables have proper clustered primary keys
2. **Foreign Keys**: All foreign keys are indexed for join performance
3. **Unique Constraints**: Business rules are enforced through unique indexes
4. **Identity Tables**: ASP.NET Core Identity indexes are properly configured

#### Potential Improvements

#### 1. Composite Indexes for Complex Queries

```sql
-- Index for device availability filtering
CREATE NONCLUSTERED INDEX [IX_Devices_Availability_Category] 
ON [dbo].[Devices] ([IsAvailable], [CategoryId])
INCLUDE ([Name], [SerialNumber], [BrandId]);

-- Index for active assignments by employee and date
CREATE NONCLUSTERED INDEX [IX_DeviceAssignments_Employee_Active_Date] 
ON [dbo].[DeviceAssignments] ([EmployeeId], [IsActive], [AssignedDate])
INCLUDE ([DeviceId], [OfficeId], [ReturnedDate]);

-- Index for maintenance scheduling
CREATE NONCLUSTERED INDEX [IX_MaintenanceSchedules_Date_Device] 
ON [dbo].[MaintenanceSchedules] ([ScheduledDate], [IsCompleted])
INCLUDE ([DeviceId], [Description]);
```

#### 2. Covering Indexes for Common Queries

```sql
-- Index for device inventory reports
CREATE NONCLUSTERED INDEX [IX_Devices_Inventory_Report] 
ON [dbo].[Devices] ([CategoryId], [BrandId], [IsAvailable])
INCLUDE ([Name], [SerialNumber], [SupplierId], [IsFaulty]);

-- Index for service history reports
CREATE NONCLUSTERED INDEX [IX_ServiceHistories_Report] 
ON [dbo].[ServiceHistories] ([DeviceId], [ServiceDate])
INCLUDE ([Description], [PerformedBy]);
```

#### 3. Filtered Indexes for Specific Scenarios

```sql
-- Index for only active assignments
CREATE NONCLUSTERED INDEX [IX_DeviceAssignments_Active_Only] 
ON [dbo].[DeviceAssignments] ([DeviceId], [EmployeeId])
WHERE [IsActive] = 1;

-- Index for pending maintenance
CREATE NONCLUSTERED INDEX [IX_MaintenanceSchedules_Pending] 
ON [dbo].[MaintenanceSchedules] ([ScheduledDate])
WHERE [IsCompleted] = 0;
```

## Index Maintenance

### Fragmentation Monitoring
```sql
-- Check index fragmentation
SELECT 
    OBJECT_NAME(ind.OBJECT_ID) AS TableName,
    ind.name AS IndexName,
    indexstats.avg_fragmentation_in_percent
FROM 
    sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) indexstats
INNER JOIN 
    sys.indexes ind ON ind.object_id = indexstats.object_id AND ind.index_id = indexstats.index_id
WHERE 
    indexstats.avg_fragmentation_in_percent > 10
ORDER BY 
    indexstats.avg_fragmentation_in_percent DESC;
```

### Index Rebuilding Strategy
```sql
-- Rebuild highly fragmented indexes (>30%)
ALTER INDEX [IX_Devices_CategoryId] ON [dbo].[Devices] REBUILD;

-- Reorganize moderately fragmented indexes (10-30%)
ALTER INDEX [IX_Devices_BrandId] ON [dbo].[Devices] REORGANIZE;
```

## Performance Metrics

### Expected Query Performance

| Query Type | Expected Time (ms) | Indexes Used |
|------------|-------------------|--------------|
| Device lookup by ID | < 5 | PK_Devices |
| Device list by category | < 10 | IX_Devices_CategoryId |
| Current assignment lookup | < 5 | IX_DeviceAssignments_DeviceId |
| Employee device history | < 15 | IX_DeviceAssignments_EmployeeId |
| Upcoming maintenance list | < 20 | IX_MaintenanceSchedules_DeviceId |
| User authentication | < 5 | UserNameIndex |
| Role lookup | < 5 | RoleNameIndex |

### Monitoring Queries

```sql
-- Find slow queries related to inventory
SELECT 
    qs.total_elapsed_time/1000.0 as total_elapsed_time_ms,
    qs.total_logical_reads,
    qs.execution_count,
    SUBSTRING(qt.text, (qs.statement_start_offset/2)+1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(qt.text)
            ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) + 1) as query_text
FROM 
    sys.dm_exec_query_stats qs
CROSS APPLY 
    sys.dm_exec_sql_text(qs.sql_handle) qt
WHERE 
    qt.text LIKE '%Device%' OR qt.text LIKE '%Assignment%'
ORDER BY 
    qs.total_elapsed_time DESC;
```

## Index Best Practices

### Do's
1. **Index Foreign Keys**: Always index foreign key columns for join performance
2. **Consider Selectivity**: Index columns with high selectivity (many unique values)
3. **Use Covering Indexes**: Include frequently accessed columns to avoid table lookups
4. **Monitor Fragmentation**: Regularly check and maintain index fragmentation
5. **Test Query Plans**: Always verify that indexes are being used by the query optimizer

### Don'ts
1. **Over-Index**: Avoid indexing every column - it slows down inserts/updates
2. **Index Low-Selectivity Columns**: Avoid indexing columns with few unique values
3. **Ignore Update Performance**: Consider the impact of indexes on write operations
4. **Forget Statistics**: Ensure statistics are up-to-date for optimal query plans
5. **Neglect Maintenance**: Regular maintenance is essential for optimal performance

## Index Summary

The current index structure provides solid foundation for most common operations:

1. **Primary Keys**: Properly indexed with clustered indexes
2. **Foreign Keys**: All relationships are indexed for join performance
3. **Business Rules**: Unique constraints enforce data integrity
4. **Identity Integration**: ASP.NET Core Identity indexes are optimal

The recommended additional indexes would further improve performance for:
- Complex reporting queries
- Dashboard and analytics operations
- High-volume assignment tracking
- Maintenance scheduling workflows

Regular monitoring and maintenance of these indexes will ensure optimal database performance as the system grows.