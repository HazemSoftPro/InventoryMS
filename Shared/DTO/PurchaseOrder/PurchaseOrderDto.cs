using System;
using System.Collections.Generic;

namespace Shared.DTO.PurchaseOrder
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid? OfficeId { get; set; }
        public string? OfficeName { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusDisplay { get; set; }
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? CreatedByName { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PurchaseOrderItemDto>? Items { get; set; }
    }

    public class PurchaseOrderForCreationDto
    {
        public Guid SupplierId { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? WarehouseId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public List<PurchaseOrderItemForCreationDto>? Items { get; set; }
    }

    public class PurchaseOrderForUpdateDto
    {
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceSerialNumber { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Description { get; set; }
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public int QuantityReceived { get; set; }
        public bool IsFullyReceived { get; set; }
        public bool IsPartiallyReceived { get; set; }
    }

    public class PurchaseOrderItemForCreationDto
    {
        public Guid DeviceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxPercentage { get; set; }
        public string? Description { get; set; }
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
    }
}