using Entities.Models;
using Shared.DTO;

namespace Repository
{
    public interface IInventoryReportRepository
    {
        Task<IEnumerable<InventoryReport>> GetAllReportsAsync(bool trackChanges);
        Task<InventoryReport?> GetReportByIdAsync(Guid reportId, bool trackChanges);
        Task<IEnumerable<InventoryReport>> GetReportsByTypeAsync(ReportType reportType, bool trackChanges);
        Task<IEnumerable<InventoryReport>> GetReportsByStatusAsync(ReportStatus status, bool trackChanges);
        Task<IEnumerable<InventoryReport>> GetScheduledReportsAsync(bool trackChanges);
        Task<IEnumerable<InventoryReport>> GetReportsByUserAsync(Guid userId, bool trackChanges);
        void CreateReport(InventoryReport report);
        void UpdateReport(InventoryReport report);
        void DeleteReport(InventoryReport report);
        Task<IEnumerable<InventoryReport>> GetRecentReportsAsync(int count, bool trackChanges);
        Task<long> GetTotalFileSizeAsync();
        Task<int> GetReportCountByStatusAsync(ReportStatus status);
    }
}