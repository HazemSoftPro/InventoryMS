using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Repository
{
    public class InventoryTransactionRepository : RepositoryBase<InventoryTransaction>, IInventoryTransactionRepository
    {
        public InventoryTransactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<InventoryTransaction>> GetAllTransactionsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Category)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Brand)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<InventoryTransaction?> GetTransactionByIdAsync(Guid transactionId, bool trackChanges)
        {
            return await FindByCondition(t => t.Id.Equals(transactionId), trackChanges)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Category)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Brand)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByDeviceAsync(Guid deviceId, bool trackChanges)
        {
            return await FindByCondition(t => t.DeviceId.Equals(deviceId), trackChanges)
                .Include(t => t.Device)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByUserAsync(Guid userId, bool trackChanges)
        {
            return await FindByCondition(t => t.FromUserId.Equals(userId) || t.ToUserId.Equals(userId), trackChanges)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Category)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Brand)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByTypeAsync(TransactionType transactionType, bool trackChanges)
        {
            return await FindByCondition(t => t.TransactionType.Equals(transactionType), trackChanges)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Category)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Brand)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByStatusAsync(TransactionStatus status, bool trackChanges)
        {
            return await FindByCondition(t => t.Status.Equals(status), trackChanges)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Category)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Brand)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges)
        {
            return await FindByCondition(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate, trackChanges)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Category)
                .Include(t => t.Device)
                    .ThenInclude(d => d.Brand)
                .Include(t => t.FromUser)
                .Include(t => t.ToUser)
                .Include(t => t.FromOffice)
                .Include(t => t.ToOffice)
                .Include(t => t.ApprovedBy)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalValueByTypeAsync(TransactionType transactionType)
        {
            return await FindByCondition(t => t.TransactionType.Equals(transactionType), false)
                .Where(t => t.TotalValue.HasValue)
                .SumAsync(t => t.TotalValue.Value);
        }

        public async Task<int> GetTransactionCountByStatusAsync(TransactionStatus status)
        {
            return await FindByCondition(t => t.Status.Equals(status), false)
                .CountAsync();
        }

        public void CreateTransaction(InventoryTransaction transaction) => Create(transaction);

        public void UpdateTransaction(InventoryTransaction transaction) => Update(transaction);

        public void DeleteTransaction(InventoryTransaction transaction) => Delete(transaction);
    }
}