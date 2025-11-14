using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repoContext;
        private IDeviceRepository _device;
        private ICategoryRepository _category;
        private IBrandRepository _brand;
        private ISupplierRepository _supplier;
        private IEmployeeRepository _employee;
        private IOfficeRepository _office;
        private IDeviceAssignmentRepository _deviceAssignment;
        private IMaintenanceScheduleRepository _maintenanceSchedule;
        private IServiceHistoryRepository _serviceHistory;
        private IInventoryTransactionRepository _inventoryTransaction;
        private IStockLevelRepository _stockLevel;
        private IWarehouseRepository _warehouse;
        private IPurchaseOrderRepository _purchaseOrder;
        private IInventoryAlertRepository _inventoryAlert;
        private IAuditLogRepository _auditLog;
        private IInventoryReportRepository _inventoryReport;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IDeviceRepository Device
        {
            get
            {
                if (_device == null)
                {
                    _device = new DeviceRepository(_repoContext);
                }

                return _device;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_repoContext);
                }

                return _category;
            }
        }

        public IBrandRepository Brand
        {
            get
            {
                if (_brand == null)
                {
                    _brand = new BrandRepository(_repoContext);
                }

                return _brand;
            }
        }

        public ISupplierRepository Supplier
        {
            get
            {
                if (_supplier == null)
                {
                    _supplier = new SupplierRepository(_repoContext);
                }

                return _supplier;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(_repoContext);
                }

                return _employee;
            }
        }

        public IOfficeRepository Office
        {
            get
            {
                if (_office == null)
                {
                    _office = new OfficeRepository(_repoContext);
                }

                return _office;
            }
        }

        public IDeviceAssignmentRepository DeviceAssignment
        {
            get
            {
                if (_deviceAssignment == null)
                {
                    _deviceAssignment = new DeviceAssignmentRepository(_repoContext);
                }

                return _deviceAssignment;
            }
        }

        public IMaintenanceScheduleRepository MaintenanceSchedule
        {
            get
            {
                if (_maintenanceSchedule == null)
                {
                    _maintenanceSchedule = new MaintenanceScheduleRepository(_repoContext);
                }

                return _maintenanceSchedule;
            }
        }

        public IServiceHistoryRepository ServiceHistory
        {
            get
            {
                if (_serviceHistory == null)
                {
                    _serviceHistory = new ServiceHistoryRepository(_repoContext);
                }

                return _serviceHistory;
            }
        }

        public IInventoryTransactionRepository InventoryTransaction
        {
            get
            {
                if (_inventoryTransaction == null)
                {
                    _inventoryTransaction = new InventoryTransactionRepository(_repoContext);
                }

                return _inventoryTransaction;
            }
        }

        public IStockLevelRepository StockLevel
        {
            get
            {
                if (_stockLevel == null)
                {
                    _stockLevel = new StockLevelRepository(_repoContext);
                }

                return _stockLevel;
            }
        }

        public IWarehouseRepository Warehouse
        {
            get
            {
                if (_warehouse == null)
                {
                    _warehouse = new WarehouseRepository(_repoContext);
                }

                return _warehouse;
            }
        }

        public IPurchaseOrderRepository PurchaseOrder
        {
            get
            {
                if (_purchaseOrder == null)
                {
                    _purchaseOrder = new PurchaseOrderRepository(_repoContext);
                }

                return _purchaseOrder;
            }
        }

        public IInventoryAlertRepository InventoryAlert
        {
            get
            {
                if (_inventoryAlert == null)
                {
                    _inventoryAlert = new InventoryAlertRepository(_repoContext);
                }

                return _inventoryAlert;
            }
        }

        public IAuditLogRepository AuditLog
        {
            get
            {
                if (_auditLog == null)
                {
                    _auditLog = new AuditLogRepository(_repoContext);
                }

                return _auditLog;
            }
        }

        public IInventoryReportRepository InventoryReport
        {
            get
            {
                if (_inventoryReport == null)
                {
                    _inventoryReport = new InventoryReportRepository(_repoContext);
                }

                return _inventoryReport;
            }
        }

        public async Task SaveAsync() => await _repoContext.SaveChangesAsync();
    }
}
