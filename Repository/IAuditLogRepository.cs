using Entities.Models;

namespace Repository
{
    public interface IAuditLogRepository
    {
        Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync(bool trackChanges);
        Task<AuditLog?> GetAuditLogByIdAsync(Guid auditLogId, bool trackChanges);
        Task<IEnumerable<AuditLog>> GetAuditLogsByEntityAsync(string entityName, Guid entityId, bool trackChanges);
        Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(Guid userId, bool trackChanges);
        Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges);
        Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(string action, bool trackChanges);
        Task<IEnumerable<AuditLog>> GetAuditLogsByTypeAsync(AuditType auditType, bool trackChanges);
        void CreateAuditLog(AuditLog auditLog);
        Task<int> GetAuditLogCountAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> GetFailedOperationsAsync(bool trackChanges);
    }
}