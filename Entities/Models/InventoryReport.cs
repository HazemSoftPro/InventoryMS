using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class InventoryReport
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ReportType ReportType { get; set; }
        public string? Parameters { get; set; }
        public string? Filters { get; set; }
        public string? GroupBy { get; set; }
        public string? SortBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastRunAt { get; set; }
        public string? GeneratedBy { get; set; }
        public Guid? GeneratedById { get; set; }
        public User? GeneratedByUser { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public FileFormat FileFormat { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Generated;
        public bool IsScheduled { get; set; } = false;
        public string? ScheduleExpression { get; set; }
        public List<string>? Recipients { get; set; }
        public bool IsActive { get; set; } = true;
        public int RunCount { get; set; } = 0;
        public DateTime? NextRunAt { get; set; }
        public string? ErrorMessage { get; set; }
        public long FileSize { get; set; }
        public int RecordCount { get; set; }
        public TimeSpan GenerationTime { get; set; }
        public string? Summary { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public enum ReportType
    {
        InventorySummary = 1,
        StockLevelReport = 2,
        TransactionHistory = 3,
        AssetValuation = 4,
        DepreciationReport = 5,
        MaintenanceReport = 6,
        PurchaseOrderReport = 7,
        SupplierPerformance = 8,
        WarehouseUtilization = 9,
        UserActivityReport = 10,
        AuditReport = 11,
        CustomReport = 99
    }

    public enum FileFormat
    {
        PDF = 1,
        Excel = 2,
        CSV = 3,
        JSON = 4,
        XML = 5,
        HTML = 6
    }

    public enum ReportStatus
    {
        Pending = 1,
        Generating = 2,
        Generated = 3,
        Failed = 4,
        Scheduled = 5,
        Cancelled = 6
    }
}