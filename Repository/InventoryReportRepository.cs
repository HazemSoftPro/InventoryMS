using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Repository
{
    public class InventoryReportRepository : RepositoryBase<InventoryReport>, IInventoryReportRepository
    {
        public InventoryReportRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<InventoryReport>> GetAllReportsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(r => r.GeneratedByUser)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<InventoryReport?> GetReportByIdAsync(Guid reportId, bool trackChanges)
        {
            return await FindByCondition(r => r.Id.Equals(reportId), trackChanges)
                .Include(r => r.GeneratedByUser)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<InventoryReport>> GetReportsByTypeAsync(ReportType reportType, bool trackChanges)
        {
            return await FindByCondition(r => r.ReportType.Equals(reportType), trackChanges)
                .Include(r => r.GeneratedByUser)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryReport>> GetReportsByStatusAsync(ReportStatus status, bool trackChanges)
        {
            return await FindByCondition(r => r.Status.Equals(status), trackChanges)
                .Include(r => r.GeneratedByUser)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryReport>> GetScheduledReportsAsync(bool trackChanges)
        {
            return await FindByCondition(r => r.IsScheduled && r.IsActive, trackChanges)
                .Include(r => r.GeneratedByUser)
                .OrderBy(r => r.NextRunAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryReport>> GetReportsByUserAsync(Guid userId, bool trackChanges)
        {
            return await FindByCondition(r => r.GeneratedById.Equals(userId), trackChanges)
                .Include(r => r.GeneratedByUser)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryReport>> GetRecentReportsAsync(int count, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(r => r.GeneratedByUser)
                .OrderByDescending(r => r.GeneratedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<long> GetTotalFileSizeAsync()
        {
            return await FindAll(false)
                .Where(r => r.Status == ReportStatus.Generated)
                .SumAsync(r => r.FileSize);
        }

        public async Task<int> GetReportCountByStatusAsync(ReportStatus status)
        {
            return await FindByCondition(r => r.Status.Equals(status), false)
                .CountAsync();
        }

        public void CreateReport(InventoryReport report) => Create(report);

        public void UpdateReport(InventoryReport report) => Update(report);

        public void DeleteReport(InventoryReport report) => Delete(report);
    }
}