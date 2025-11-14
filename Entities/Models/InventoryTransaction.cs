using System;

namespace Entities.Models
{
    public class InventoryTransaction
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public Device? Device { get; set; }
        public Guid? FromUserId { get; set; }
        public User? FromUser { get; set; }
        public Guid? ToUserId { get; set; }
        public User? ToUser { get; set; }
        public Guid? FromOfficeId { get; set; }
        public Office? FromOffice { get; set; }
        public Guid? ToOfficeId { get; set; }
        public Office? ToOffice { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public decimal? UnitCost { get; set; }
        public decimal? TotalValue { get; set; }
        public string? ReferenceNumber { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
        public string? Notes { get; set; }
        public Guid? ApprovedById { get; set; }
        public User? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum TransactionType
    {
        Purchase = 1,
        Transfer = 2,
        Assignment = 3,
        Return = 4,
        Disposal = 5,
        Repair = 6,
        Maintenance = 7,
        Audit = 8,
        Adjustment = 9
    }

    public enum TransactionStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Completed = 4,
        Cancelled = 5
    }
}