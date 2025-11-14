namespace Contracts
{
    public interface IRepositoryManager
    {
        IDeviceRepository Device { get; }
        ICategoryRepository Category { get; }
        IBrandRepository Brand { get; }
        ISupplierRepository Supplier { get; }
        IEmployeeRepository Employee { get; }
        IOfficeRepository Office { get; }
        IDeviceAssignmentRepository DeviceAssignment { get; }
        IMaintenanceScheduleRepository MaintenanceSchedule { get; }
        IServiceHistoryRepository ServiceHistory { get; }
        IInventoryTransactionRepository InventoryTransaction { get; }
        IStockLevelRepository StockLevel { get; }
        IWarehouseRepository Warehouse { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }
        IInventoryAlertRepository InventoryAlert { get; }
        IAuditLogRepository AuditLog { get; }
        IInventoryReportRepository InventoryReport { get; }
        Task SaveAsync();
    }
}
