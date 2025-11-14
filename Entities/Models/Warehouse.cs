using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
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
        public decimal CurrentUtilization { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsTemperatureControlled { get; set; } = false;
        public bool IsSecureStorage { get; set; } = false;
        public string? OperatingHours { get; set; }
        public string? SpecialInstructions { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public Guid? UpdatedById { get; set; }
        public User? UpdatedBy { get; set; }
        
        // Navigation properties
        public List<Device>? Devices { get; set; }
        public List<InventoryTransaction>? Transactions { get; set; }
        public List<StockLevel>? StockLevels { get; set; }
    }
}