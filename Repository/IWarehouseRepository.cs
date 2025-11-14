using Entities.Models;
using Shared.DTO;

namespace Repository
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges);
        Task<Warehouse?> GetWarehouseByIdAsync(Guid warehouseId, bool trackChanges);
        Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(bool trackChanges);
        Task<IEnumerable<Warehouse>> GetWarehousesWithAvailableCapacityAsync(bool trackChanges);
        void CreateWarehouse(Warehouse warehouse);
        void UpdateWarehouse(Warehouse warehouse);
        void DeleteWarehouse(Warehouse warehouse);
        Task<WarehouseStatisticsDto?> GetWarehouseStatisticsAsync(Guid warehouseId);
        Task<decimal> GetTotalCapacityAsync();
        Task<decimal> GetTotalUtilizationAsync();
        Task<int> GetActiveWarehouseCountAsync();
    }
}