using Entities.Models;
using Shared.DTO;

namespace Repository
{
    public interface IInventoryTransactionRepository
    {
        Task<IEnumerable<InventoryTransaction>> GetAllTransactionsAsync(bool trackChanges);
        Task<InventoryTransaction?> GetTransactionByIdAsync(Guid transactionId, bool trackChanges);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByDeviceAsync(Guid deviceId, bool trackChanges);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByUserAsync(Guid userId, bool trackChanges);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByTypeAsync(TransactionType transactionType, bool trackChanges);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByStatusAsync(TransactionStatus status, bool trackChanges);
        void CreateTransaction(InventoryTransaction transaction);
        void UpdateTransaction(InventoryTransaction transaction);
        void DeleteTransaction(InventoryTransaction transaction);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges);
        Task<decimal> GetTotalValueByTypeAsync(TransactionType transactionType);
        Task<int> GetTransactionCountByStatusAsync(TransactionStatus status);
    }
}