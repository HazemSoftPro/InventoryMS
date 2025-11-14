# Database Quick Reference Guide

This document provides a quick reference for common database operations and key information for the Inventory Management System.

## Essential Commands

### Database Connection
```sql
-- Connect to database
USE [InventorySystemDB];
GO

-- Check current database
SELECT DB_NAME() AS CurrentDatabase;
```

### Table Information
```sql
-- List all tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';

-- Table structure
EXEC sp_help 'Devices';

-- Row counts
SELECT 
    OBJECT_NAME(object_id) AS TableName,
    SUM(rows) AS RowCount
FROM sys.partitions
WHERE index_id IN (0, 1)
GROUP BY object_id
ORDER BY SUM(rows) DESC;
```

### Common Queries

#### Device Operations
```sql
-- Find available devices
SELECT d.Name, d.SerialNumber, c.Name AS Category, b.Name AS Brand
FROM Devices d
JOIN Categories c ON d.CategoryId = c.Id
JOIN Brands b ON d.BrandId = b.Id
WHERE d.IsAvailable = 1 AND d.IsFaulty = 0;

-- Find assigned devices
SELECT d.Name, e.FirstName + ' ' + e.LastName AS Employee, da.AssignedDate
FROM Devices d
JOIN DeviceAssignments da ON d.Id = da.DeviceId
JOIN Employees e ON da.EmployeeId = e.Id
WHERE da.IsActive = 1;

-- Find faulty devices
SELECT d.Name, d.SerialNumber, c.Name AS Category
FROM Devices d
JOIN Categories c ON d.CategoryId = c.Id
WHERE d.IsFaulty = 1;
```

#### Assignment Operations
```sql
-- Current assignments by employee
SELECT e.FirstName + ' ' + e.LastName AS Employee, 
       COUNT(da.DeviceId) AS DeviceCount,
       STRING_AGG(d.Name, ', ') AS Devices
FROM Employees e
JOIN DeviceAssignments da ON e.Id = da.EmployeeId
JOIN Devices d ON da.DeviceId = d.Id
WHERE da.IsActive = 1
GROUP BY e.Id, e.FirstName, e.LastName
ORDER BY DeviceCount DESC;

-- Assignment history for device
SELECT e.FirstName + ' ' + e.LastName AS Employee,
       da.AssignedDate,
       da.ReturnedDate,
       DATEDIFF(DAY, da.AssignedDate, ISNULL(da.ReturnedDate, GETDATE())) AS Days
FROM DeviceAssignments da
JOIN Employees e ON da.EmployeeId = e.Id
WHERE da.DeviceId = 'device-id-here'
ORDER BY da.AssignedDate DESC;
```

#### Maintenance Operations
```sql
-- Upcoming maintenance (next 30 days)
SELECT d.Name, d.SerialNumber, ms.ScheduledDate, ms.Description
FROM MaintenanceSchedules ms
JOIN Devices d ON ms.DeviceId = d.Id
WHERE ms.ScheduledDate BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE())
  AND ms.IsCompleted = 0
ORDER BY ms.ScheduledDate;

-- Overdue maintenance
SELECT d.Name, d.SerialNumber, 
       DATEDIFF(DAY, ms.ScheduledDate, GETDATE()) AS DaysOverdue,
       ms.Description
FROM MaintenanceSchedules ms
JOIN Devices d ON ms.DeviceId = d.Id
WHERE ms.ScheduledDate < GETDATE() AND ms.IsCompleted = 0;
```

## Table Reference

### Core Tables

| Table | Purpose | Key Fields |
|-------|---------|------------|
| **Devices** | Main inventory table | Id, Name, SerialNumber, CategoryId, BrandId, SupplierId, IsAvailable, IsFaulty |
| **Categories** | Device categorization | Id, Name |
| **Brands** | Device manufacturers | Id, Name |
| **Suppliers** | Vendor information | Id, Name, ContactInfo |
| **Employees** | Staff records | Id, FirstName, LastName, EmployeeNumber, Position, Email |
| **Offices** | Location data | Id, Name, Location |
| **DeviceAssignments** | Assignment tracking | Id, DeviceId (UNIQUE), EmployeeId, OfficeId, AssignedDate, ReturnedDate, IsActive |

### Supporting Tables

| Table | Purpose | Key Fields |
|-------|---------|------------|
| **MaintenanceSchedules** | Preventive maintenance | Id, DeviceId, ScheduledDate, Description, IsCompleted |
| **ServiceHistories** | Service/repair records | Id, DeviceId, ServiceDate, Description, PerformedBy |
| **Reports** | Generated reports | Id, Name, GeneratedDate, Content |

### Identity Tables

| Table | Purpose | Key Fields |
|-------|---------|------------|
| **AspNetUsers** | User accounts | Id, UserName, Email, FirstName, LastName, PasswordHash |
| **AspNetRoles** | Role definitions | Id, Name, NormalizedName |
| **AspNetUserRoles** | User-role mapping | UserId, RoleId |

## Common Operations

### Add New Device
```sql
-- Insert device
INSERT INTO Devices (Id, Name, SerialNumber, CategoryId, BrandId, SupplierId, IsFaulty, IsAvailable)
VALUES (NEWID(), 'Laptop Model X', 'SN123456', 
        'category-guid', 'brand-guid', 'supplier-guid', 0, 1);
```

### Assign Device
```sql
-- Create assignment
INSERT INTO DeviceAssignments (Id, DeviceId, EmployeeId, OfficeId, AssignedDate, IsActive)
VALUES (NEWID(), 'device-guid', 'employee-guid', 'office-guid', GETDATE(), 1);

-- Update device availability
UPDATE Devices SET IsAvailable = 0 WHERE Id = 'device-guid';
```

### Return Device
```sql
-- Update assignment
UPDATE DeviceAssignments 
SET IsActive = 0, ReturnedDate = GETDATE()
WHERE DeviceId = 'device-guid' AND IsActive = 1;

-- Update device availability
UPDATE Devices SET IsAvailable = 1 WHERE Id = 'device-guid';
```

### Schedule Maintenance
```sql
INSERT INTO MaintenanceSchedules (Id, DeviceId, ScheduledDate, Description, IsCompleted)
VALUES (NEWID(), 'device-guid', DATEADD(DAY, 30, GETDATE()), 'Quarterly maintenance', 0);
```

### Record Service
```sql
INSERT INTO ServiceHistories (Id, DeviceId, ServiceDate, Description, PerformedBy)
VALUES (NEWID(), 'device-guid', GETDATE(), 'Screen replacement', 'IT Department');
```

## Troubleshooting Commands

### Check Data Integrity
```sql
-- Find orphaned records
SELECT 'Devices without Category' AS Issue, COUNT(*) AS Count
FROM Devices d
LEFT JOIN Categories c ON d.CategoryId = c.Id
WHERE c.Id IS NULL

UNION ALL

SELECT 'Devices without Brand', COUNT(*)
FROM Devices d
LEFT JOIN Brands b ON d.BrandId = b.Id
WHERE b.Id IS NULL

UNION ALL

SELECT 'Assignments without Device', COUNT(*)
FROM DeviceAssignments da
LEFT JOIN Devices d ON da.DeviceId = d.Id
WHERE d.Id IS NULL;
```

### Performance Checks
```sql
-- Long running queries
SELECT TOP 10
    total_elapsed_time / 1000000.0 AS duration_seconds,
    SUBSTRING(qt.text, (qs.statement_start_offset/2)+1, 100) AS query
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
WHERE total_elapsed_time > 5000000  -- 5 seconds
ORDER BY total_elapsed_time DESC;

-- Index fragmentation
SELECT 
    OBJECT_NAME(ind.OBJECT_ID) AS TableName,
    ind.name AS IndexName,
    indexstats.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) indexstats
INNER JOIN sys.indexes ind ON ind.object_id = indexstats.object_id AND ind.index_id = indexstats.index_id
WHERE indexstats.avg_fragmentation_in_percent > 10
ORDER BY indexstats.avg_fragmentation_in_percent DESC;
```

### Backup and Restore
```sql
-- Backup database
BACKUP DATABASE [InventorySystemDB]
TO DISK = 'C:\Backup\InventorySystemDB.bak'
WITH FORMAT, INIT;

-- Restore database
RESTORE DATABASE [InventorySystemDB_Test]
FROM DISK = 'C:\Backup\InventorySystemDB.bak'
WITH REPLACE, MOVE 'InventorySystemDB' TO 'C:\Data\InventorySystemDB_Test.mdf',
MOVE 'InventorySystemDB_log' TO 'C:\Data\InventorySystemDB_Test_log.ldf';
```

## Utility Scripts

### Reset Test Data
```sql
-- Clear test assignments
UPDATE DeviceAssignments SET IsActive = 0, ReturnedDate = GETDATE() WHERE IsActive = 1;
UPDATE Devices SET IsAvailable = 1 WHERE IsAvailable = 0;

-- Clear test maintenance schedules
DELETE FROM MaintenanceSchedules WHERE IsCompleted = 0;
```

### Generate Report Data
```sql
-- Monthly inventory summary
SELECT 
    'Total Devices' AS Metric,
    COUNT(*) AS Value,
    GETDATE() AS ReportDate
FROM Devices
UNION ALL
SELECT 'Available', COUNT(*), GETDATE()
FROM Devices WHERE IsAvailable = 1 AND IsFaulty = 0
UNION ALL
SELECT 'Assigned', COUNT(*), GETDATE()
FROM Devices WHERE IsAvailable = 0 AND IsFaulty = 0
UNION ALL
SELECT 'Faulty', COUNT(*), GETDATE()
FROM Devices WHERE IsFaulty = 1;
```

## Common Issues & Solutions

### Issue: Device shows as assigned but no active assignment exists
```sql
-- Find affected devices
SELECT * FROM Devices WHERE IsAvailable = 0 
AND Id NOT IN (SELECT DeviceId FROM DeviceAssignments WHERE IsActive = 1);

-- Fix: Update availability
UPDATE Devices SET IsAvailable = 1 
WHERE IsAvailable = 0 
AND Id NOT IN (SELECT DeviceId FROM DeviceAssignments WHERE IsActive = 1);
```

### Issue: Duplicate assignments for same device
```sql
-- Find duplicates
SELECT DeviceId, COUNT(*) AS AssignmentCount
FROM DeviceAssignments
WHERE IsActive = 1
GROUP BY DeviceId
HAVING COUNT(*) > 1;

-- Fix: Deactivate all but the most recent
WITH CTE AS (
    SELECT *, 
           ROW_NUMBER() OVER (PARTITION BY DeviceId ORDER BY AssignedDate DESC) AS RN
    FROM DeviceAssignments
    WHERE IsActive = 1
)
UPDATE DeviceAssignments
SET IsActive = 0, ReturnedDate = GETDATE()
WHERE Id IN (SELECT Id FROM CTE WHERE RN > 1);
```

## Key Performance Indicators

### Database Health
```sql
-- Database size and growth
SELECT 
    DB_NAME(database_id) AS DatabaseName,
    CAST(SUM(size) * 8.0 / 1024 AS DECIMAL(10,2)) AS SizeMB
FROM sys.master_files
WHERE database_id = DB_ID()
GROUP BY database_id;

-- Connection count
SELECT COUNT(*) AS ActiveConnections
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID()
  AND is_user_process = 1;
```

### Business Metrics
```sql
-- Device utilization
SELECT 
    CAST(SUM(CASE WHEN IsAvailable = 0 AND IsFaulty = 0 THEN 1 ELSE 0 END) * 100.0 / 
         NULLIF(COUNT(*), 0) AS DECIMAL(10,2)) AS UtilizationPercent,
    COUNT(*) AS TotalDevices
FROM Devices;

-- Average assignment duration
SELECT AVG(DATEDIFF(DAY, AssignedDate, ISNULL(ReturnedDate, GETDATE()))) AS AvgDays
FROM DeviceAssignments
WHERE ReturnedDate IS NOT NULL;
```

This quick reference guide provides the most commonly used commands and patterns for working with the Inventory Management System database.