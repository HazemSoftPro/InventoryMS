using System;
using System.Collections.Generic;

namespace Shared.DTO.Warehouse
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerContact { get; set; }
        public decimal Capacity { get; set; }
        public decimal CurrentUtilization { get; set; }
        public decimal UtilizationPercentage { get; set; }
        public bool IsActive { get; set; }
        public bool IsTemperatureControlled { get; set; }
        public bool IsSecureStorage { get; set; }
        public string? OperatingHours { get; set; }
        public string? SpecialInstructions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public int DeviceCount { get; set; }
        public int StockLevelCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }

    public class WarehouseForCreationDto
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerContact { get; set; }
        public decimal Capacity { get; set; }
        public bool IsTemperatureControlled { get; set; }
        public bool IsSecureStorage { get; set; }
        public string? OperatingHours { get; set; }
        public string? SpecialInstructions { get; set; }
    }

    public class WarehouseForUpdateDto
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerContact { get; set; }
        public decimal Capacity { get; set; }
        public bool IsActive { get; set; }
        public bool IsTemperatureControlled { get; set; }
        public bool IsSecureStorage { get; set; }
        public string? OperatingHours { get; set; }
        public string? SpecialInstructions { get; set; }
    }

    public class WarehouseStatisticsDto
    {
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int TotalDevices { get; set; }
        public int AvailableDevices { get; set; }
        public int FaultyDevices { get; set; }
        public int AssignedDevices { get; set; }
        public int MaintenanceDevices { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UtilizationPercentage { get; set; }
        public int LowStockItems { get; set; }
        public int OverstockItems { get; set; }
        public int PendingTransactions { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}