using Entities.Models;
using Shared.DTO;

namespace Repository
{
    public interface IStockLevelRepository
    {
        Task<IEnumerable<StockLevel>> GetAllStockLevelsAsync(bool trackChanges);
        Task<StockLevel?> GetStockLevelByIdAsync(Guid stockLevelId, bool trackChanges);
        Task<IEnumerable<StockLevel>> GetStockLevelsByDeviceAsync(Guid deviceId, bool trackChanges);
        Task<IEnumerable<StockLevel>> GetStockLevelsByWarehouseAsync(Guid warehouseId, bool trackChanges);
        Task<IEnumerable<StockLevel>> GetLowStockItemsAsync(bool trackChanges);
        Task<IEnumerable<StockLevel>> GetOverstockItemsAsync(bool trackChanges);
        Task<IEnumerable<StockLevel>> GetCriticalStockItemsAsync(bool trackChanges);
        void CreateStockLevel(StockLevel stockLevel);
        void UpdateStockLevel(StockLevel stockLevel);
        void DeleteStockLevel(StockLevel stockLevel);
        Task<StockLevel?> GetStockLevelByDeviceAndWarehouseAsync(Guid deviceId, Guid warehouseId, bool trackChanges);
        Task<decimal> GetTotalInventoryValueAsync();
        Task<int> GetTotalItemCountAsync();
        Task<int> GetLowStockItemCountAsync();
        Task<int> GetOverstockItemCountAsync();
    }
}