using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Repository
{
    public class PurchaseOrderRepository : RepositoryBase<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(po => po.Supplier)
                .Include(po => po.Office)
                .Include(po => po.Warehouse)
                .Include(po => po.CreatedBy)
                .Include(po => po.ApprovedBy)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                        .ThenInclude(d => d.Category)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                        .ThenInclude(d => d.Brand)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid purchaseOrderId, bool trackChanges)
        {
            return await FindByCondition(po => po.Id.Equals(purchaseOrderId), trackChanges)
                .Include(po => po.Supplier)
                .Include(po => po.Office)
                .Include(po => po.Warehouse)
                .Include(po => po.CreatedBy)
                .Include(po => po.ApprovedBy)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                        .ThenInclude(d => d.Category)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                        .ThenInclude(d => d.Brand)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersBySupplierAsync(Guid supplierId, bool trackChanges)
        {
            return await FindByCondition(po => po.SupplierId.Equals(supplierId), trackChanges)
                .Include(po => po.Supplier)
                .Include(po => po.Office)
                .Include(po => po.Warehouse)
                .Include(po => po.CreatedBy)
                .Include(po => po.ApprovedBy)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByStatusAsync(OrderStatus status, bool trackChanges)
        {
            return await FindByCondition(po => po.Status.Equals(status), trackChanges)
                .Include(po => po.Supplier)
                .Include(po => po.Office)
                .Include(po => po.Warehouse)
                .Include(po => po.CreatedBy)
                .Include(po => po.ApprovedBy)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPendingApprovalOrdersAsync(bool trackChanges)
        {
            return await FindByCondition(po => po.Status.Equals(OrderStatus.Draft) || po.Status.Equals(OrderStatus.Sent), trackChanges)
                .Include(po => po.Supplier)
                .Include(po => po.Office)
                .Include(po => po.Warehouse)
                .Include(po => po.CreatedBy)
                .Include(po => po.ApprovedBy)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Device)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();
        }

        public async Task<string?> GetLastOrderNumberAsync(string prefix)
        {
            var lastOrder = await FindByCondition(po => po.OrderNumber.StartsWith(prefix), false)
                .OrderByDescending(po => po.OrderNumber)
                .FirstOrDefaultAsync();

            return lastOrder?.OrderNumber;
        }

        public async Task<decimal> GetTotalPurchaseValueAsync(DateTime startDate, DateTime endDate)
        {
            return await FindByCondition(po => po.OrderDate >= startDate && po.OrderDate <= endDate, false)
                .Where(po => po.Status == OrderStatus.Received || po.Status == OrderStatus.PartiallyReceived)
                .SumAsync(po => po.TotalAmount);
        }

        public async Task<int> GetOrderCountByStatusAsync(OrderStatus status)
        {
            return await FindByCondition(po => po.Status.Equals(status), false)
                .CountAsync();
        }

        public void CreatePurchaseOrder(PurchaseOrder purchaseOrder) => Create(purchaseOrder);

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder) => Update(purchaseOrder);

        public void DeletePurchaseOrder(PurchaseOrder purchaseOrder) => Delete(purchaseOrder);
    }
}