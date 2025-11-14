using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Repository
{
    public class InventoryAlertRepository : RepositoryBase<InventoryAlert>, IInventoryAlertRepository
    {
        public InventoryAlertRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<InventoryAlert>> GetAllAlertsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<InventoryAlert?> GetAlertByIdAsync(Guid alertId, bool trackChanges)
        {
            return await FindByCondition(a => a.Id.Equals(alertId), trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetActiveAlertsAsync(bool trackChanges)
        {
            return await FindByCondition(a => a.IsActive && !a.ResolvedAt.HasValue, trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetUnreadAlertsAsync(bool trackChanges)
        {
            return await FindByCondition(a => a.IsActive && !a.IsRead, trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetAlertsByTypeAsync(AlertType alertType, bool trackChanges)
        {
            return await FindByCondition(a => a.AlertType.Equals(alertType), trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetAlertsBySeverityAsync(AlertSeverity severity, bool trackChanges)
        {
            return await FindByCondition(a => a.Severity.Equals(severity), trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetAlertsByDeviceAsync(Guid deviceId, bool trackChanges)
        {
            return await FindByCondition(a => a.DeviceId.Equals(deviceId), trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetAlertsByWarehouseAsync(Guid warehouseId, bool trackChanges)
        {
            return await FindByCondition(a => a.OfficeId.Equals(warehouseId), trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetActiveAlertCountAsync()
        {
            return await FindByCondition(a => a.IsActive && !a.ResolvedAt.HasValue, false)
                .CountAsync();
        }

        public async Task<int> GetCriticalAlertCountAsync()
        {
            return await FindByCondition(a => a.IsActive && a.Severity.Equals(AlertSeverity.Critical) && !a.ResolvedAt.HasValue, false)
                .CountAsync();
        }

        public async Task<IEnumerable<InventoryAlert>> GetAlertsNeedingAttentionAsync(bool trackChanges)
        {
            return await FindByCondition(a => a.IsActive && 
                                           (a.Severity == AlertSeverity.High || a.Severity == AlertSeverity.Critical) && 
                                           !a.ResolvedAt.HasValue, trackChanges)
                .Include(a => a.Device)
                    .ThenInclude(d => d.Category)
                .Include(a => a.Category)
                .Include(a => a.Office)
                .Include(a => a.AcknowledgedBy)
                .Include(a => a.ResolvedBy)
                .OrderBy(a => a.Severity)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public void CreateAlert(InventoryAlert alert) => Create(alert);

        public void UpdateAlert(InventoryAlert alert) => Update(alert);

        public void DeleteAlert(InventoryAlert alert) => Delete(alert);
    }
}