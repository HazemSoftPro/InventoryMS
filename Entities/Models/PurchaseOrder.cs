using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public Guid? OfficeId { get; set; }
        public Office? Office { get; set; }
        public Guid? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; } = 0;
        public decimal ShippingAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; } = "USD";
        public OrderStatus Status { get; set; } = OrderStatus.Draft;
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public Guid? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public Guid? ApprovedById { get; set; }
        public User? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public List<PurchaseOrderItem>? Items { get; set; }
        public List<InventoryTransaction>? Transactions { get; set; }
    }

    public class PurchaseOrderItem
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public Guid DeviceId { get; set; }
        public Device? Device { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TaxPercentage { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public decimal TotalPrice { get; set; }
        public string? Description { get; set; }
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public int QuantityReceived { get; set; } = 0;
        public bool IsFullyReceived => QuantityReceived >= Quantity;
        public bool IsPartiallyReceived => QuantityReceived > 0 && QuantityReceived < Quantity;
    }

    public enum OrderStatus
    {
        Draft = 1,
        Sent = 2,
        Approved = 3,
        Rejected = 4,
        PartiallyReceived = 5,
        Received = 6,
        Cancelled = 7,
        Closed = 8
    }
}