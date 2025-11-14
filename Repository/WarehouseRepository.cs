using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.DTO.Warehouse;

namespace Repository
{
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(w => w.Devices)
                .Include(w => w.StockLevels)
                    .ThenInclude(s => s.Device)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<Warehouse?> GetWarehouseByIdAsync(Guid warehouseId, bool trackChanges)
        {
            return await FindByCondition(w => w.Id.Equals(warehouseId), trackChanges)
                .Include(w => w.Devices)
                    .ThenInclude(d => d.Category)
                .Include(w => w.Devices)
                    .ThenInclude(d => d.Brand)
                .Include(w => w.StockLevels)
                    .ThenInclude(s => s.Device)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(bool trackChanges)
        {
            return await FindByCondition(w => w.IsActive, trackChanges)
                .Include(w => w.Devices)
                .Include(w => w.StockLevels)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetWarehousesWithAvailableCapacityAsync(bool trackChanges)
        {
            return await FindByCondition(w => w.IsActive && w.CurrentUtilization < w.Capacity, trackChanges)
                .Include(w => w.Devices)
                .Include(w => w.StockLevels)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<WarehouseStatisticsDto?> GetWarehouseStatisticsAsync(Guid warehouseId)
        {
            var warehouse = await FindByCondition(w => w.Id.Equals(warehouseId), false)
                .Include(w => w.Devices)
                .Include(w => w.StockLevels)
                    .ThenInclude(s => s.Device)
                .SingleOrDefaultAsync();

            if (warehouse == null) return null;

            var deviceCount = warehouse.Devices?.Count ?? 0;
            var availableDevices = warehouse.Devices?.Count(d => d.IsAvailable) ?? 0;
            var faultyDevices = warehouse.Devices?.Count(d => d.IsFaulty) ?? 0;
            var assignedDevices = warehouse.Devices?.Count(d => d.CurrentAssignment != null) ?? 0;
            var maintenanceDevices = warehouse.Devices?.Count(d => d.MaintenanceSchedules?.Any(ms => ms.IsCompleted == false) == true) ?? 0;

            var totalValue = warehouse.StockLevels?.Sum(s => s.TotalValue) ?? 0;
            var utilizationPercentage = warehouse.Capacity > 0 ? (warehouse.CurrentUtilization / warehouse.Capacity) * 100 : 0;

            var lowStockItems = warehouse.StockLevels?.Count(s => s.NeedsReorder) ?? 0;
            var overstockItems = warehouse.StockLevels?.Count(s => s.IsOverstocked) ?? 0;

            // Get pending transactions count (this would require additional query)
            var pendingTransactions = await RepositoryContext.Set<InventoryTransaction>()
                .Where(t => (t.FromOfficeId == warehouseId || t.ToOfficeId == warehouseId) && 
                           t.Status == TransactionStatus.Pending)
                .CountAsync();

            return new WarehouseStatisticsDto
            {
                WarehouseId = warehouse.Id,
                WarehouseName = warehouse.Name,
                TotalDevices = deviceCount,
                AvailableDevices = availableDevices,
                FaultyDevices = faultyDevices,
                AssignedDevices = assignedDevices,
                MaintenanceDevices = maintenanceDevices,
                TotalValue = totalValue,
                UtilizationPercentage = utilizationPercentage,
                LowStockItems = lowStockItems,
                OverstockItems = overstockItems,
                PendingTransactions = pendingTransactions,
                LastUpdated = DateTime.UtcNow
            };
        }

        public async Task<decimal> GetTotalCapacityAsync()
        {
            return await FindAll(false)
                .Where(w => w.IsActive)
                .SumAsync(w => w.Capacity);
        }

        public async Task<decimal> GetTotalUtilizationAsync()
        {
            return await FindAll(false)
                .Where(w => w.IsActive)
                .SumAsync(w => w.CurrentUtilization);
        }

        public async Task<int> GetActiveWarehouseCountAsync()
        {
            return await FindByCondition(w => w.IsActive, false)
                .CountAsync();
        }

        public void CreateWarehouse(Warehouse warehouse) => Create(warehouse);

        public void UpdateWarehouse(Warehouse warehouse) => Update(warehouse);

        public void DeleteWarehouse(Warehouse warehouse) => Delete(warehouse);
    }
}