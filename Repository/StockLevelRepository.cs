using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.DTO.Warehouse;

namespace Repository
{
    public class StockLevelRepository : RepositoryBase<StockLevel>, IStockLevelRepository
    {
        public StockLevelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<StockLevel>> GetAllStockLevelsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .OrderBy(s => s.Warehouse.Name)
                .ThenBy(s => s.Device.Name)
                .ToListAsync();
        }

        public async Task<StockLevel?> GetStockLevelByIdAsync(Guid stockLevelId, bool trackChanges)
        {
            return await FindByCondition(s => s.Id.Equals(stockLevelId), trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<StockLevel>> GetStockLevelsByDeviceAsync(Guid deviceId, bool trackChanges)
        {
            return await FindByCondition(s => s.DeviceId.Equals(deviceId), trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .OrderBy(s => s.Warehouse.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockLevel>> GetStockLevelsByWarehouseAsync(Guid warehouseId, bool trackChanges)
        {
            return await FindByCondition(s => s.WarehouseId.Equals(warehouseId), trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .OrderBy(s => s.Device.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockLevel>> GetLowStockItemsAsync(bool trackChanges)
        {
            return await FindByCondition(s => s.QuantityAvailable <= s.ReorderLevel && s.IsActive, trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .OrderBy(s => s.Warehouse.Name)
                .ThenBy(s => s.Device.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockLevel>> GetOverstockItemsAsync(bool trackChanges)
        {
            return await FindByCondition(s => s.QuantityOnHand > s.MaxStockLevel && s.IsActive, trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .OrderBy(s => s.Warehouse.Name)
                .ThenBy(s => s.Device.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockLevel>> GetCriticalStockItemsAsync(bool trackChanges)
        {
            return await FindByCondition(s => s.QuantityAvailable <= s.MinStockLevel && s.IsActive, trackChanges)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Category)
                .Include(s => s.Device)
                    .ThenInclude(d => d.Brand)
                .Include(s => s.Warehouse)
                .OrderBy(s => s.QuantityAvailable)
                .ThenBy(s => s.Warehouse.Name)
                .ThenBy(s => s.Device.Name)
                .ToListAsync();
        }

        public async Task<StockLevel?> GetStockLevelByDeviceAndWarehouseAsync(Guid deviceId, Guid warehouseId, bool trackChanges)
        {
            return await FindByCondition(s => s.DeviceId.Equals(deviceId) && s.WarehouseId.Equals(warehouseId), trackChanges)
                .Include(s => s.Device)
                .Include(s => s.Warehouse)
                .SingleOrDefaultAsync();
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            return await FindAll(false)
                .Where(s => s.IsActive)
                .SumAsync(s => s.TotalValue);
        }

        public async Task<int> GetTotalItemCountAsync()
        {
            return await FindAll(false)
                .Where(s => s.IsActive)
                .SumAsync(s => s.QuantityOnHand);
        }

        public async Task<int> GetLowStockItemCountAsync()
        {
            return await FindByCondition(s => s.QuantityAvailable <= s.ReorderLevel && s.IsActive, false)
                .CountAsync();
        }

        public async Task<int> GetOverstockItemCountAsync()
        {
            return await FindByCondition(s => s.QuantityOnHand > s.MaxStockLevel && s.IsActive, false)
                .CountAsync();
        }

        public void CreateStockLevel(StockLevel stockLevel) => Create(stockLevel);

        public void UpdateStockLevel(StockLevel stockLevel) => Update(stockLevel);

        public void DeleteStockLevel(StockLevel stockLevel) => Delete(stockLevel);
    }
}