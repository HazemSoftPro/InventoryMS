using Entities.Models;
using Shared.DTO;

namespace Repository
{
    public interface IPurchaseOrderRepository
    {
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync(bool trackChanges);
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid purchaseOrderId, bool trackChanges);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersBySupplierAsync(Guid supplierId, bool trackChanges);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByStatusAsync(OrderStatus status, bool trackChanges);
        Task<IEnumerable<PurchaseOrder>> GetPendingApprovalOrdersAsync(bool trackChanges);
        void CreatePurchaseOrder(PurchaseOrder purchaseOrder);
        void UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        void DeletePurchaseOrder(PurchaseOrder purchaseOrder);
        Task<string?> GetLastOrderNumberAsync(string prefix);
        Task<decimal> GetTotalPurchaseValueAsync(DateTime startDate, DateTime endDate);
        Task<int> GetOrderCountByStatusAsync(OrderStatus status);
    }
}