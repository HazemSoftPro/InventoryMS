using Entities.Models;
using Shared.DTO;

namespace Repository
{
    public interface IInventoryAlertRepository
    {
        Task<IEnumerable<InventoryAlert>> GetAllAlertsAsync(bool trackChanges);
        Task<InventoryAlert?> GetAlertByIdAsync(Guid alertId, bool trackChanges);
        Task<IEnumerable<InventoryAlert>> GetActiveAlertsAsync(bool trackChanges);
        Task<IEnumerable<InventoryAlert>> GetUnreadAlertsAsync(bool trackChanges);
        Task<IEnumerable<InventoryAlert>> GetAlertsByTypeAsync(AlertType alertType, bool trackChanges);
        Task<IEnumerable<InventoryAlert>> GetAlertsBySeverityAsync(AlertSeverity severity, bool trackChanges);
        Task<IEnumerable<InventoryAlert>> GetAlertsByDeviceAsync(Guid deviceId, bool trackChanges);
        Task<IEnumerable<InventoryAlert>> GetAlertsByWarehouseAsync(Guid warehouseId, bool trackChanges);
        void CreateAlert(InventoryAlert alert);
        void UpdateAlert(InventoryAlert alert);
        void DeleteAlert(InventoryAlert alert);
        Task<int> GetActiveAlertCountAsync();
        Task<int> GetCriticalAlertCountAsync();
        Task<IEnumerable<InventoryAlert>> GetAlertsNeedingAttentionAsync(bool trackChanges);
    }
}