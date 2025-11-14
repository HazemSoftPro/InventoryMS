using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.PurchaseOrder;

namespace InventrySystem.Controllers
{
    [Route("api/purchase-orders")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public PurchaseOrderController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            try
            {
                var purchaseOrders = await _repository.PurchaseOrder.GetAllPurchaseOrdersAsync(trackChanges: false);
                _logger.LogInfo("Returned all purchase orders from database.");

                var purchaseOrdersResult = _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
                return Ok(purchaseOrdersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllPurchaseOrders action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "PurchaseOrderById")]
        public async Task<IActionResult> GetPurchaseOrderById(Guid id)
        {
            try
            {
                var purchaseOrder = await _repository.PurchaseOrder.GetPurchaseOrderByIdAsync(id, trackChanges: false);
                if (purchaseOrder == null)
                {
                    _logger.LogError($"Purchase order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned purchase order with id: {id}");

                var purchaseOrderResult = _mapper.Map<PurchaseOrderDto>(purchaseOrder);
                return Ok(purchaseOrderResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPurchaseOrderById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetPurchaseOrdersBySupplier(Guid supplierId)
        {
            try
            {
                var purchaseOrders = await _repository.PurchaseOrder.GetPurchaseOrdersBySupplierAsync(supplierId, trackChanges: false);
                _logger.LogInfo($"Returned purchase orders for supplier with id: {supplierId}");

                var purchaseOrdersResult = _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
                return Ok(purchaseOrdersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPurchaseOrdersBySupplier action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPurchaseOrdersByStatus(OrderStatus status)
        {
            try
            {
                var purchaseOrders = await _repository.PurchaseOrder.GetPurchaseOrdersByStatusAsync(status, trackChanges: false);
                _logger.LogInfo($"Returned purchase orders with status: {status}");

                var purchaseOrdersResult = _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
                return Ok(purchaseOrdersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPurchaseOrdersByStatus action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pending-approval")]
        public async Task<IActionResult> GetPendingApprovalOrders()
        {
            try
            {
                var purchaseOrders = await _repository.PurchaseOrder.GetPendingApprovalOrdersAsync(trackChanges: false);
                _logger.LogInfo("Returned pending approval purchase orders from database.");

                var purchaseOrdersResult = _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
                return Ok(purchaseOrdersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPendingApprovalOrders action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderForCreationDto purchaseOrder)
        {
            try
            {
                if (purchaseOrder == null)
                {
                    _logger.LogError("Purchase order object sent from client is null.");
                    return BadRequest("Purchase order object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid purchase order object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var purchaseOrderEntity = _mapper.Map<PurchaseOrder>(purchaseOrder);
                purchaseOrderEntity.OrderNumber = await GenerateOrderNumber();
                purchaseOrderEntity.OrderDate = DateTime.UtcNow;
                purchaseOrderEntity.Status = OrderStatus.Draft;
                purchaseOrderEntity.CreatedAt = DateTime.UtcNow;
                purchaseOrderEntity.UpdatedAt = DateTime.UtcNow;

                // Calculate totals
                CalculateOrderTotals(purchaseOrderEntity);

                _repository.PurchaseOrder.CreatePurchaseOrder(purchaseOrderEntity);
                await _repository.SaveAsync();

                var createdPurchaseOrder = _mapper.Map<PurchaseOrderDto>(purchaseOrderEntity);

                return CreatedAtRoute("PurchaseOrderById", new { id = createdPurchaseOrder.Id }, createdPurchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreatePurchaseOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(Guid id, [FromBody] PurchaseOrderForUpdateDto purchaseOrder)
        {
            try
            {
                if (purchaseOrder == null)
                {
                    _logger.LogError("Purchase order object sent from client is null.");
                    return BadRequest("Purchase order object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid purchase order object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var purchaseOrderEntity = await _repository.PurchaseOrder.GetPurchaseOrderByIdAsync(id, trackChanges: true);
                if (purchaseOrderEntity == null)
                {
                    _logger.LogError($"Purchase order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(purchaseOrder, purchaseOrderEntity);
                purchaseOrderEntity.UpdatedAt = DateTime.UtcNow;

                // Recalculate totals
                CalculateOrderTotals(purchaseOrderEntity);

                _repository.PurchaseOrder.UpdatePurchaseOrder(purchaseOrderEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdatePurchaseOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApprovePurchaseOrder(Guid id)
        {
            try
            {
                var purchaseOrderEntity = await _repository.PurchaseOrder.GetPurchaseOrderByIdAsync(id, trackChanges: true);
                if (purchaseOrderEntity == null)
                {
                    _logger.LogError($"Purchase order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                purchaseOrderEntity.Status = OrderStatus.Approved;
                purchaseOrderEntity.ApprovedDate = DateTime.UtcNow;
                purchaseOrderEntity.UpdatedAt = DateTime.UtcNow;

                _repository.PurchaseOrder.UpdatePurchaseOrder(purchaseOrderEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ApprovePurchaseOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/receive")]
        public async Task<IActionResult> ReceivePurchaseOrder(Guid id, [FromBody] List<PurchaseOrderItemReceiveDto> items)
        {
            try
            {
                var purchaseOrderEntity = await _repository.PurchaseOrder.GetPurchaseOrderByIdAsync(id, trackChanges: true);
                if (purchaseOrderEntity == null)
                {
                    _logger.LogError($"Purchase order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                foreach (var itemReceive in items)
                {
                    var item = purchaseOrderEntity.Items?.FirstOrDefault(i => i.Id == itemReceive.ItemId);
                    if (item != null)
                    {
                        item.QuantityReceived += itemReceive.QuantityReceived;
                        
                        // Create inventory transaction for received items
                        var transaction = new InventoryTransaction
                        {
                            DeviceId = item.DeviceId,
                            TransactionType = TransactionType.Purchase,
                            Description = $"Received from PO: {purchaseOrderEntity.OrderNumber}",
                            UnitCost = item.UnitPrice,
                            TotalValue = itemReceive.QuantityReceived * item.UnitPrice,
                            Status = TransactionStatus.Completed,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _repository.InventoryTransaction.CreateTransaction(transaction);
                    }
                }

                // Update order status if all items are received
                var allItemsReceived = purchaseOrderEntity.Items?.All(i => i.IsFullyReceived) == true;
                if (allItemsReceived)
                {
                    purchaseOrderEntity.Status = OrderStatus.Received;
                    purchaseOrderEntity.ActualDeliveryDate = DateTime.UtcNow;
                }
                else if (purchaseOrderEntity.Items?.Any(i => i.IsPartiallyReceived) == true)
                {
                    purchaseOrderEntity.Status = OrderStatus.PartiallyReceived;
                }

                purchaseOrderEntity.UpdatedAt = DateTime.UtcNow;
                _repository.PurchaseOrder.UpdatePurchaseOrder(purchaseOrderEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReceivePurchaseOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(Guid id)
        {
            try
            {
                var purchaseOrder = await _repository.PurchaseOrder.GetPurchaseOrderByIdAsync(id, trackChanges: false);
                if (purchaseOrder == null)
                {
                    _logger.LogError($"Purchase order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                // Check if purchase order is approved or received
                if (purchaseOrder.Status == OrderStatus.Approved || purchaseOrder.Status == OrderStatus.PartiallyReceived || purchaseOrder.Status == OrderStatus.Received)
                {
                    _logger.LogError($"Purchase order with id: {id} cannot be deleted as it is approved or received.");
                    return BadRequest("Purchase order cannot be deleted as it is approved or received");
                }

                _repository.PurchaseOrder.DeletePurchaseOrder(purchaseOrder);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeletePurchaseOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<string> GenerateOrderNumber()
        {
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var prefix = $"PO{year}{month:D2}";
            
            // Get the last order number for this month
            var lastOrder = await _repository.PurchaseOrder.GetLastOrderNumberAsync(prefix);
            var sequence = lastOrder != null ? int.Parse(lastOrder.Substring(prefix.Length)) + 1 : 1;
            
            return $"{prefix}{sequence:D4}";
        }

        private void CalculateOrderTotals(PurchaseOrder purchaseOrder)
        {
            decimal subtotal = 0;
            decimal taxAmount = 0;

            if (purchaseOrder.Items != null)
            {
                foreach (var item in purchaseOrder.Items)
                {
                    item.DiscountAmount = item.UnitPrice * item.Quantity * (item.DiscountPercentage / 100);
                    item.TaxAmount = (item.UnitPrice * item.Quantity - item.DiscountAmount) * (item.TaxPercentage / 100);
                    item.TotalPrice = item.UnitPrice * item.Quantity - item.DiscountAmount + item.TaxAmount;

                    subtotal += item.UnitPrice * item.Quantity - item.DiscountAmount;
                    taxAmount += item.TaxAmount;
                }
            }

            purchaseOrder.Subtotal = subtotal;
            purchaseOrder.TaxAmount = taxAmount;
            purchaseOrder.TotalAmount = subtotal + taxAmount + purchaseOrder.ShippingAmount;
        }
    }

    public class PurchaseOrderItemReceiveDto
    {
        public Guid ItemId { get; set; }
        public int QuantityReceived { get; set; }
    }
}