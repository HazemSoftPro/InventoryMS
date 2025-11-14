using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AuditLogRepository : RepositoryBase<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<AuditLog?> GetAuditLogByIdAsync(Guid auditLogId, bool trackChanges)
        {
            return await FindByCondition(a => a.Id.Equals(auditLogId), trackChanges)
                .Include(a => a.User)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByEntityAsync(string entityName, Guid entityId, bool trackChanges)
        {
            return await FindByCondition(a => a.EntityName.Equals(entityName) && a.EntityId.Equals(entityId), trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(Guid userId, bool trackChanges)
        {
            return await FindByCondition(a => a.UserId.Equals(userId), trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges)
        {
            return await FindByCondition(a => a.Timestamp >= startDate && a.Timestamp <= endDate, trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(string action, bool trackChanges)
        {
            return await FindByCondition(a => a.Action.Equals(action), trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByTypeAsync(AuditType auditType, bool trackChanges)
        {
            return await FindByCondition(a => a.AuditType.Equals(auditType), trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<int> GetAuditLogCountAsync(DateTime startDate, DateTime endDate)
        {
            return await FindByCondition(a => a.Timestamp >= startDate && a.Timestamp <= endDate, false)
                .CountAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetFailedOperationsAsync(bool trackChanges)
        {
            return await FindByCondition(a => !a.IsSuccess, trackChanges)
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public void CreateAuditLog(AuditLog auditLog) => Create(auditLog);
    }
}