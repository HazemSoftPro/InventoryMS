using System;

namespace Entities.Models
{
    public class StockLevel
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public Device? Device { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; } = 0;
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public int MinStockLevel { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string? Location { get; set; }
        public string? BinNumber { get; set; }
        public string? ShelfNumber { get; set; }
        public DateTime LastCountDate { get; set; } = DateTime.UtcNow;
        public DateTime NextCountDate { get; set; }
        public int CountFrequency { get; set; } = 30; // days
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Computed properties
        public bool NeedsReorder => QuantityAvailable <= ReorderLevel;
        public bool IsOverstocked => QuantityOnHand > MaxStockLevel;
        public bool IsBelowMinimum => QuantityOnHand < MinStockLevel;
        public decimal ReorderQuantity => MaxStockLevel - QuantityAvailable;
    }
}