# Sample Database Queries

This document provides common SQL queries for the Inventory Management System, covering typical operations and reporting scenarios.

## Table of Contents

- [Basic CRUD Operations](#basic-crud-operations)
- [Device Management Queries](#device-management-queries)
- [Assignment Tracking Queries](#assignment-tracking-queries)
- [Maintenance and Service Queries](#maintenance-and-service-queries)
- [Reporting and Analytics Queries](#reporting-and-analytics-queries)
- [User Management Queries](#user-management-queries)
- [Performance Monitoring Queries](#performance-monitoring-queries)

---

## Basic CRUD Operations

### Insert Operations

#### Add New Device
```sql
-- Insert a new device
INSERT INTO Devices (Id, Name, SerialNumber, CategoryId, BrandId, SupplierId, IsFaulty, IsAvailable)
VALUES (
    NEWID(), 
    'Laptop Pro X1', 
    'SN2024001', 
    '9aa0f4cd-de28-4d3c-b38b-586819845ba3',  -- Laptops category
    'f10323d3-da72-44e7-ae7d-0379da31b329',  -- Apple brand
    '029e2d94-fd9d-41bd-9b4a-58b2f738c662',  -- Omega IT supplier
    0,  -- Not faulty
    1   -- Available
);
```

#### Add New Employee
```sql
-- Insert a new employee
INSERT INTO Employees (Id, FirstName, LastName, EmployeeNumber, Position, Email)
VALUES (
    NEWID(),
    'Michael',
    'Brown',
    'EMP004',
    'Senior Developer',
    'michael.brown@example.com'
);
```

#### Assign Device to Employee
```sql
-- Assign a device to an employee
INSERT INTO DeviceAssignments (Id, DeviceId, EmployeeId, OfficeId, AssignedDate, IsActive)
VALUES (
    NEWID(),
    'b2719efd-4bea-4cc2-84c3-4400e838a545',  -- Device ID
    '7f8ba2f9-3462-4d1e-9dbc-eacab5de0bef',  -- Employee ID
    '1fe78363-8333-4168-8dd7-532dcb58de42',  -- Office ID
    GETDATE(),
    1  -- Active assignment
);

-- Update device availability
UPDATE Devices 
SET IsAvailable = 0 
WHERE Id = 'b2719efd-4bea-4cc2-84c3-4400e838a545';
```

### Update Operations

#### Update Device Status
```sql
-- Mark device as faulty
UPDATE Devices 
SET IsFaulty = 1, IsAvailable = 0
WHERE Id = 'b2719efd-4bea-4cc2-84c3-4400e838a545';
```

#### Update Employee Information
```sql
-- Update employee position and email
UPDATE Employees 
SET Position = 'Lead Developer', 
    Email = 'john.doe.lead@example.com'
WHERE Id = '7f8ba2f9-3462-4d1e-9dbc-eacab5de0bef';
```

#### Return Device Assignment
```sql
-- Deactivate assignment and set return date
UPDATE DeviceAssignments 
SET IsActive = 0, 
    ReturnedDate = GETDATE()
WHERE DeviceId = 'b2719efd-4bea-4cc2-84c3-4400e838a545' 
  AND IsActive = 1;

-- Update device availability
UPDATE Devices 
SET IsAvailable = 1 
WHERE Id = 'b2719efd-4bea-4cc2-84c3-4400e838a545';
```

### Delete Operations

#### Delete Maintenance Schedule
```sql
-- Delete completed maintenance schedule
DELETE FROM MaintenanceSchedules 
WHERE Id = 'schedule-id-here' 
  AND IsCompleted = 1;
```

#### Delete Service History
```sql
-- Delete old service history (older than 5 years)
DELETE FROM ServiceHistories 
WHERE ServiceDate < DATEADD(YEAR, -5, GETDATE());
```

---

## Device Management Queries

### Device Inventory

#### Get All Available Devices
```sql
-- List all available devices with details
SELECT 
    d.Id,
    d.Name,
    d.SerialNumber,
    c.Name AS CategoryName,
    b.Name AS BrandName,
    s.Name AS SupplierName,
    d.IsFaulty
FROM Devices d
JOIN Categories c ON d.CategoryId = c.Id
JOIN Brands b ON d.BrandId = b.Id
JOIN Suppliers s ON d.SupplierId = s.Id
WHERE d.IsAvailable = 1
  AND d.IsFaulty = 0
ORDER BY c.Name, b.Name, d.Name;
```

#### Get Devices by Category
```sql
-- Get all devices in a specific category
SELECT 
    d.Id,
    d.Name,
    d.SerialNumber,
    b.Name AS BrandName,
    d.IsAvailable,
    d.IsFaulty,
    CASE 
        WHEN d.IsAvailable = 1 AND d.IsFaulty = 0 THEN 'Available'
        WHEN d.IsAvailable = 0 AND d.IsFaulty = 0 THEN 'Assigned'
        ELSE 'Faulty'
    END AS Status
FROM Devices d
JOIN Brands b ON d.BrandId = b.Id
WHERE d.CategoryId = '9aa0f4cd-de28-4d3c-b38b-586819845ba3'  -- Laptops
ORDER BY d.Name;
```

#### Search Devices by Name or Serial Number
```sql
-- Search devices by text match
SELECT 
    d.Id,
    d.Name,
    d.SerialNumber,
    c.Name AS CategoryName,
    b.Name AS BrandName,
    d.IsAvailable,
    d.IsFaulty
FROM Devices d
JOIN Categories c ON d.CategoryId = c.Id
JOIN Brands b ON d.BrandId = b.Id
WHERE d.Name LIKE '%Laptop%' 
   OR d.SerialNumber LIKE '%SN123%'
ORDER BY d.Name;
```

#### Device Count by Category
```sql
-- Count devices by category and status
SELECT 
    c.Name AS CategoryName,
    COUNT(*) AS TotalDevices,
    SUM(CASE WHEN d.IsAvailable = 1 AND d.IsFaulty = 0 THEN 1 ELSE 0 END) AS Available,
    SUM(CASE WHEN d.IsAvailable = 0 AND d.IsFaulty = 0 THEN 1 ELSE 0 END) AS Assigned,
    SUM(CASE WHEN d.IsFaulty = 1 THEN 1 ELSE 0 END) AS Faulty
FROM Categories c
LEFT JOIN Devices d ON c.Id = d.CategoryId
GROUP BY c.Id, c.Name
ORDER BY c.Name;
```

---

## Assignment Tracking Queries

### Current Assignments

#### Get Active Device Assignments
```sql
-- List all currently assigned devices
SELECT 
    da.Id AS AssignmentId,
    d.Name AS DeviceName,
    d.SerialNumber,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    e.EmployeeNumber,
    o.Name AS OfficeName,
    o.Location AS OfficeLocation,
    da.AssignedDate,
    DATEDIFF(DAY, da.AssignedDate, GETDATE()) AS DaysAssigned
FROM DeviceAssignments da
JOIN Devices d ON da.DeviceId = d.Id
JOIN Employees e ON da.EmployeeId = e.Id
JOIN Offices o ON da.OfficeId = o.Id
WHERE da.IsActive = 1
ORDER BY da.AssignedDate DESC;
```

#### Get Employee's Current Devices
```sql
-- List all devices currently assigned to a specific employee
SELECT 
    d.Id AS DeviceId,
    d.Name AS DeviceName,
    d.SerialNumber,
    c.Name AS CategoryName,
    b.Name AS BrandName,
    da.AssignedDate,
    DATEDIFF(DAY, da.AssignedDate, GETDATE()) AS DaysAssigned
FROM DeviceAssignments da
JOIN Devices d ON da.DeviceId = d.Id
JOIN Categories c ON d.CategoryId = c.Id
JOIN Brands b ON d.BrandId = b.Id
WHERE da.EmployeeId = '7f8ba2f9-3462-4d1e-9dbc-eacab5de0bef'  -- Employee ID
  AND da.IsActive = 1
ORDER BY da.AssignedDate;
```

#### Get Devices in Office
```sql
-- List all devices currently in a specific office
SELECT 
    d.Id AS DeviceId,
    d.Name AS DeviceName,
    d.SerialNumber,
    c.Name AS CategoryName,
    e.FirstName + ' ' + e.LastName AS AssignedTo,
    da.AssignedDate
FROM DeviceAssignments da
JOIN Devices d ON da.DeviceId = d.Id
JOIN Categories c ON d.CategoryId = c.Id
LEFT JOIN Employees e ON da.EmployeeId = e.Id
WHERE da.OfficeId = '1fe78363-8333-4168-8dd7-532dcb58de42'  -- Office ID
  AND da.IsActive = 1
ORDER BY e.FirstName, e.LastName, d.Name;
```

### Assignment History

#### Get Device Assignment History
```sql
-- Complete assignment history for a device
SELECT 
    da.Id AS AssignmentId,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    e.EmployeeNumber,
    o.Name AS OfficeName,
    da.AssignedDate,
    da.ReturnedDate,
    DATEDIFF(DAY, da.AssignedDate, ISNULL(da.ReturnedDate, GETDATE())) AS DurationDays,
    da.IsActive
FROM DeviceAssignments da
LEFT JOIN Employees e ON da.EmployeeId = e.Id
LEFT JOIN Offices o ON da.OfficeId = o.Id
WHERE da.DeviceId = 'b2719efd-4bea-4cc2-84c3-4400e838a545'  -- Device ID
ORDER BY da.AssignedDate DESC;
```

#### Get Employee Assignment History
```sql
-- Assignment history for an employee
SELECT 
    da.Id AS AssignmentId,
    d.Name AS DeviceName,
    d.SerialNumber,
    c.Name AS CategoryName,
    o.Name AS OfficeName,
    da.AssignedDate,
    da.ReturnedDate,
    DATEDIFF(DAY, da.AssignedDate, ISNULL(da.ReturnedDate, GETDATE())) AS DurationDays,
    da.IsActive
FROM DeviceAssignments da
JOIN Devices d ON da.DeviceId = d.Id
JOIN Categories c ON d.CategoryId = c.Id
LEFT JOIN Offices o ON da.OfficeId = o.Id
WHERE da.EmployeeId = '7f8ba2f9-3462-4d1e-9dbc-eacab5de0bef'  -- Employee ID
ORDER BY da.AssignedDate DESC;
```

---

## Maintenance and Service Queries

### Maintenance Schedules

#### Get Upcoming Maintenance
```sql
-- List scheduled maintenance for next 30 days
SELECT 
    ms.Id AS ScheduleId,
    d.Name AS DeviceName,
    d.SerialNumber,
    e.FirstName + ' ' + e.LastName AS AssignedTo,
    ms.ScheduledDate,
    DATEDIFF(DAY, GETDATE(), ms.ScheduledDate) AS DaysUntilMaintenance,
    ms.Description,
    ms.IsCompleted
FROM MaintenanceSchedules ms
JOIN Devices d ON ms.DeviceId = d.Id
LEFT JOIN DeviceAssignments da ON d.Id = da.DeviceId AND da.IsActive = 1
LEFT JOIN Employees e ON da.EmployeeId = e.Id
WHERE ms.ScheduledDate BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE())
  AND ms.IsCompleted = 0
ORDER BY ms.ScheduledDate;
```

#### Get Overdue Maintenance
```sql
-- Find overdue maintenance schedules
SELECT 
    ms.Id AS ScheduleId,
    d.Name AS DeviceName,
    d.SerialNumber,
    e.FirstName + ' ' + e.LastName AS AssignedTo,
    ms.ScheduledDate,
    DATEDIFF(DAY, ms.ScheduledDate, GETDATE()) AS DaysOverdue,
    ms.Description
FROM MaintenanceSchedules ms
JOIN Devices d ON ms.DeviceId = d.Id
LEFT JOIN DeviceAssignments da ON d.Id = da.DeviceId AND da.IsActive = 1
LEFT JOIN Employees e ON da.EmployeeId = e.Id
WHERE ms.ScheduledDate < GETDATE()
  AND ms.IsCompleted = 0
ORDER BY ms.ScheduledDate;
```

### Service History

#### Get Device Service History
```sql
-- Complete service history for a device
SELECT 
    sh.Id AS ServiceId,
    sh.ServiceDate,
    sh.Description,
    sh.PerformedBy,
    DATEDIFF(DAY, sh.ServiceDate, GETDATE()) AS DaysAgo
FROM ServiceHistories sh
WHERE sh.DeviceId = 'b2719efd-4bea-4cc2-84c3-4400e838a545'  -- Device ID
ORDER BY sh.ServiceDate DESC;
```

#### Get Recent Service Activities
```sql
-- Service activities in the last 30 days
SELECT 
    sh.Id AS ServiceId,
    d.Name AS DeviceName,
    d.SerialNumber,
    c.Name AS CategoryName,
    sh.ServiceDate,
    sh.Description,
    sh.PerformedBy
FROM ServiceHistories sh
JOIN Devices d ON sh.DeviceId = d.Id
JOIN Categories c ON d.CategoryId = c.Id
WHERE sh.ServiceDate >= DATEADD(DAY, -30, GETDATE())
ORDER BY sh.ServiceDate DESC;
```

---

## Reporting and Analytics Queries

### Inventory Reports

#### Monthly Inventory Summary
```sql
-- Current inventory status summary
SELECT 
    'Total Devices' AS Metric,
    COUNT(*) AS Value
FROM Devices
UNION ALL
SELECT 
    'Available Devices',
    COUNT(*)
FROM Devices
WHERE IsAvailable = 1 AND IsFaulty = 0
UNION ALL
SELECT 
    'Assigned Devices',
    COUNT(*)
FROM Devices
WHERE IsAvailable = 0 AND IsFaulty = 0
UNION ALL
SELECT 
    'Faulty Devices',
    COUNT(*)
FROM Devices
WHERE IsFaulty = 1;
```

#### Device Utilization by Category
```sql
-- Device utilization rate by category
SELECT 
    c.Name AS CategoryName,
    COUNT(*) AS TotalDevices,
    SUM(CASE WHEN d.IsAvailable = 0 AND d.IsFaulty = 0 THEN 1 ELSE 0 END) AS AssignedDevices,
    CAST(
        SUM(CASE WHEN d.IsAvailable = 0 AND d.IsFaulty = 0 THEN 1 ELSE 0 END) * 100.0 / 
        NULLIF(COUNT(*), 0) 
        AS DECIMAL(10,2)
    ) AS UtilizationPercentage
FROM Categories c
LEFT JOIN Devices d ON c.Id = d.CategoryId
GROUP BY c.Id, c.Name
ORDER BY UtilizationPercentage DESC;
```

#### Brand Performance Report
```sql
-- Device distribution and issues by brand
SELECT 
    b.Name AS BrandName,
    COUNT(*) AS TotalDevices,
    SUM(CASE WHEN d.IsFaulty = 1 THEN 1 ELSE 0 END) AS FaultyDevices,
    CAST(
        SUM(CASE WHEN d.IsFaulty = 1 THEN 1 ELSE 0 END) * 100.0 / 
        NULLIF(COUNT(*), 0) 
        AS DECIMAL(10,2)
    ) AS FaultPercentage,
    AVG(CASE WHEN EXISTS(
        SELECT 1 FROM ServiceHistories sh 
        WHERE sh.DeviceId = d.Id 
        AND sh.ServiceDate >= DATEADD(YEAR, -1, GETDATE())
    ) THEN 1 ELSE 0 END) * 100 AS ServiceRatePercentage
FROM Brands b
LEFT JOIN Devices d ON b.Id = d.BrandId
GROUP BY b.Id, b.Name
ORDER BY TotalDevices DESC;
```

### Employee Reports

#### Employee Device Allocation
```sql
-- Current device allocation per employee
SELECT 
    e.Id AS EmployeeId,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    e.EmployeeNumber,
    e.Position,
    COUNT(da.DeviceId) AS AssignedDevices,
    STRING_AGG(c.Name, ', ') AS DeviceTypes
FROM Employees e
LEFT JOIN DeviceAssignments da ON e.Id = da.EmployeeId AND da.IsActive = 1
LEFT JOIN Devices d ON da.DeviceId = d.Id
LEFT JOIN Categories c ON d.CategoryId = c.Id
GROUP BY e.Id, e.FirstName, e.LastName, e.EmployeeNumber, e.Position
HAVING COUNT(da.DeviceId) > 0
ORDER BY COUNT(da.DeviceId) DESC;
```

#### Assignment Duration Analysis
```sql
-- Average assignment duration by category
SELECT 
    c.Name AS CategoryName,
    COUNT(*) AS TotalAssignments,
    AVG(DATEDIFF(DAY, da.AssignedDate, ISNULL(da.ReturnedDate, GETDATE()))) AS AvgAssignmentDays,
    MIN(DATEDIFF(DAY, da.AssignedDate, ISNULL(da.ReturnedDate, GETDATE()))) AS MinDays,
    MAX(DATEDIFF(DAY, da.AssignedDate, ISNULL(da.ReturnedDate, GETDATE()))) AS MaxDays
FROM DeviceAssignments da
JOIN Devices d ON da.DeviceId = d.Id
JOIN Categories c ON d.CategoryId = c.Id
WHERE da.ReturnedDate IS NOT NULL  -- Only completed assignments
GROUP BY c.Id, c.Name
ORDER BY AvgAssignmentDays DESC;
```

---

## User Management Queries

### Authentication and Authorization

#### User Information with Roles
```sql
-- Get user details with their roles
SELECT 
    u.Id,
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    u.EmailConfirmed,
    u.LockoutEnabled,
    STRING_AGG(r.Name, ', ') AS Roles
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
GROUP BY u.Id, u.UserName, u.Email, u.FirstName, u.LastName, u.EmailConfirmed, u.LockoutEnabled
ORDER BY u.UserName;
```

#### Users in Specific Role
```sql
-- Get all users in Admin role
SELECT 
    u.Id,
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    u.EmailConfirmed
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Admin'
ORDER BY u.UserName;
```

#### User Activity Summary
```sql
-- User activity and security status
SELECT 
    u.UserName,
    u.Email,
    u.FirstName + ' ' + u.LastName AS FullName,
    u.EmailConfirmed,
    u.AccessFailedCount,
    CASE 
        WHEN u.LockoutEnd > GETDATE() THEN 'Locked'
        WHEN u.LockoutEnabled = 1 THEN 'Enabled'
        ELSE 'Disabled'
    END AS LockStatus,
    u.TwoFactorEnabled,
    STRING_AGG(r.Name, ', ') AS Roles
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
GROUP BY u.Id, u.UserName, u.Email, u.FirstName, u.LastName, 
         u.EmailConfirmed, u.AccessFailedCount, u.LockoutEnd, 
         u.LockoutEnabled, u.TwoFactorEnabled
ORDER BY u.UserName;
```

---

## Performance Monitoring Queries

### Database Health

#### Table Row Counts
```sql
-- Get row counts for all tables
SELECT 
    t.NAME AS TableName,
    p.rows AS RowCount
FROM 
    sys.tables t
INNER JOIN     
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
WHERE 
    t.NAME NOT LIKE 'sys%' 
    AND i.OBJECT_ID > 255
GROUP BY 
    t.Name, p.Rows
ORDER BY 
    p.Rows DESC;
```

#### Index Usage Statistics
```sql
-- Monitor index usage for performance
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates,
    s.last_user_seek,
    s.last_user_scan
FROM 
    sys.indexes i
LEFT JOIN 
    sys.dm_db_index_usage_stats s ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE 
    OBJECT_NAME(i.object_id) LIKE '%Device%' 
    OR OBJECT_NAME(i.object_id) LIKE '%Assignment%'
ORDER BY 
    s.user_seeks + s.user_scans + s.user_lookups DESC;
```

#### Query Performance Analysis
```sql
-- Find slow queries related to inventory
SELECT TOP 10
    qs.total_elapsed_time / 1000000.0 AS total_elapsed_time_seconds,
    qs.total_worker_time / 1000000.0 AS total_cpu_time_seconds,
    qs.total_logical_reads,
    qs.execution_count,
    qs.total_elapsed_time / qs.execution_count / 1000000.0 AS avg_elapsed_time_seconds,
    SUBSTRING(qt.text, (qs.statement_start_offset/2)+1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(qt.text)
            ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) + 1) AS query_text
FROM 
    sys.dm_exec_query_stats qs
CROSS APPLY 
    sys.dm_exec_sql_text(qs.sql_handle) qt
WHERE 
    qt.text LIKE '%Device%' OR qt.text LIKE '%Assignment%'
ORDER BY 
    qs.total_elapsed_time DESC;
```

---

## Stored Procedure Examples

### Common Operations

#### Create Device Assignment Procedure
```sql
CREATE PROCEDURE sp_AssignDevice
    @DeviceId UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER,
    @OfficeId UNIQUEIDENTIFIER,
    @AssignmentId UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if device is available
        IF NOT EXISTS (SELECT 1 FROM Devices WHERE Id = @DeviceId AND IsAvailable = 1 AND IsFaulty = 0)
        BEGIN
            RAISERROR('Device is not available for assignment', 16, 1);
            RETURN;
        END
        
        -- Create assignment
        SET @AssignmentId = NEWID();
        INSERT INTO DeviceAssignments (Id, DeviceId, EmployeeId, OfficeId, AssignedDate, IsActive)
        VALUES (@AssignmentId, @DeviceId, @EmployeeId, @OfficeId, GETDATE(), 1);
        
        -- Update device availability
        UPDATE Devices SET IsAvailable = 0 WHERE Id = @DeviceId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        RAISERROR('Error assigning device: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
```

#### Return Device Procedure
```sql
CREATE PROCEDURE sp_ReturnDevice
    @DeviceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Deactivate current assignment
        UPDATE DeviceAssignments 
        SET IsActive = 0, ReturnedDate = GETDATE()
        WHERE DeviceId = @DeviceId AND IsActive = 1;
        
        -- Update device availability
        UPDATE Devices SET IsAvailable = 1 WHERE Id = @DeviceId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        RAISERROR('Error returning device: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
```

These queries cover the most common operations and reporting needs for the Inventory Management System. They can be used as templates for building application-specific queries and stored procedures.