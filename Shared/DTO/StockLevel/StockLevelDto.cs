using System;

namespace Shared.DTO.StockLevel
{
    public class StockLevelDto
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceSerialNumber { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public int MinStockLevel { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string? Location { get; set; }
        public string? BinNumber { get; set; }
        public string? ShelfNumber { get; set; }
        public DateTime LastCountDate { get; set; }
        public DateTime NextCountDate { get; set; }
        public int CountFrequency { get; set; }
        public bool IsActive { get; set; }
        public bool NeedsReorder { get; set; }
        public bool IsOverstocked { get; set; }
        public bool IsBelowMinimum { get; set; }
        public decimal ReorderQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class StockLevelForCreationDto
    {
        public Guid DeviceId { get; set; }
        public Guid WarehouseId { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int ReorderLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public int MinStockLevel { get; set; }
        public decimal UnitCost { get; set; }
        public string? Location { get; set; }
        public string? BinNumber { get; set; }
        public string? ShelfNumber { get; set; }
        public int CountFrequency { get; set; }
    }

    public class StockLevelForUpdateDto
    {
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int ReorderLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public int MinStockLevel { get; set; }
        public decimal UnitCost { get; set; }
        public string? Location { get; set; }
        public string? BinNumber { get; set; }
        public string? ShelfNumber { get; set; }
        public int CountFrequency { get; set; }
        public bool IsActive { get; set; }
    }

    public class StockAdjustmentDto
    {
        public Guid StockLevelId { get; set; }
        public int AdjustmentQuantity { get; set; }
        public string? Reason { get; set; }
        public AdjustmentType AdjustmentType { get; set; }
    }

    public enum AdjustmentType
    {
        Increase = 1,
        Decrease = 2,
        Set = 3
    }
}