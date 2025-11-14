using System;

namespace Entities.Models
{
    public class InventoryAlert
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AlertType AlertType { get; set; }
        public AlertSeverity Severity { get; set; }
        public Guid? DeviceId { get; set; }
        public Device? Device { get; set; }
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public Guid? OfficeId { get; set; }
        public Office? Office { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcknowledgedAt { get; set; }
        public Guid? AcknowledgedById { get; set; }
        public User? AcknowledgedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid? ResolvedById { get; set; }
        public User? ResolvedBy { get; set; }
        public string? ResolutionNotes { get; set; }
        public AlertTrigger Trigger { get; set; }
        public string? TriggerValue { get; set; }
        public int ThresholdValue { get; set; }
    }

    public enum AlertType
    {
        LowStock = 1,
        Overstock = 2,
        MaintenanceDue = 3,
        WarrantyExpiry = 4,
        DeviceFailure = 5,
        UnauthorizedAccess = 6,
        BudgetExceeded = 7,
        AuditRequired = 8,
        DisposalReminder = 9
    }

    public enum AlertSeverity
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum AlertTrigger
    {
        QuantityThreshold = 1,
        DateThreshold = 2,
        StatusChange = 3,
        ValueThreshold = 4,
        CustomRule = 5
    }
}