using System;

namespace Shared.DTO.InventoryTransaction
{
    public class InventoryTransactionDto
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceSerialNumber { get; set; }
        public Guid? FromUserId { get; set; }
        public string? FromUserName { get; set; }
        public Guid? ToUserId { get; set; }
        public string? ToUserName { get; set; }
        public Guid? FromOfficeId { get; set; }
        public string? FromOfficeName { get; set; }
        public Guid? ToOfficeId { get; set; }
        public string? ToOfficeName { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionTypeDisplay { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalValue { get; set; }
        public string? ReferenceNumber { get; set; }
        public TransactionStatus Status { get; set; }
        public string StatusDisplay { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public Guid? ApprovedById { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class InventoryTransactionForCreationDto
    {
        public Guid DeviceId { get; set; }
        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }
        public Guid? FromOfficeId { get; set; }
        public Guid? ToOfficeId { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Description { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalValue { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
    }

    public class InventoryTransactionForUpdateDto
    {
        public TransactionType TransactionType { get; set; }
        public string? Description { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalValue { get; set; }
        public string? ReferenceNumber { get; set; }
        public TransactionStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}