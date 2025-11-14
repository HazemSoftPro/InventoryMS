using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.StockLevel;

namespace InventrySystem.Controllers
{
    [Route("api/stock-levels")]
    [ApiController]
    [Authorize]
    public class StockLevelController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public StockLevelController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStockLevels()
        {
            try
            {
                var stockLevels = await _repository.StockLevel.GetAllStockLevelsAsync(trackChanges: false);
                _logger.LogInfo("Returned all stock levels from database.");

                var stockLevelsResult = _mapper.Map<IEnumerable<StockLevelDto>>(stockLevels);
                return Ok(stockLevelsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllStockLevels action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "StockLevelById")]
        public async Task<IActionResult> GetStockLevelById(Guid id)
        {
            try
            {
                var stockLevel = await _repository.StockLevel.GetStockLevelByIdAsync(id, trackChanges: false);
                if (stockLevel == null)
                {
                    _logger.LogError($"Stock level with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned stock level with id: {id}");

                var stockLevelResult = _mapper.Map<StockLevelDto>(stockLevel);
                return Ok(stockLevelResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetStockLevelById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetStockLevelsByDevice(Guid deviceId)
        {
            try
            {
                var stockLevels = await _repository.StockLevel.GetStockLevelsByDeviceAsync(deviceId, trackChanges: false);
                _logger.LogInfo($"Returned stock levels for device with id: {deviceId}");

                var stockLevelsResult = _mapper.Map<IEnumerable<StockLevelDto>>(stockLevels);
                return Ok(stockLevelsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetStockLevelsByDevice action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetStockLevelsByWarehouse(Guid warehouseId)
        {
            try
            {
                var stockLevels = await _repository.StockLevel.GetStockLevelsByWarehouseAsync(warehouseId, trackChanges: false);
                _logger.LogInfo($"Returned stock levels for warehouse with id: {warehouseId}");

                var stockLevelsResult = _mapper.Map<IEnumerable<StockLevelDto>>(stockLevels);
                return Ok(stockLevelsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetStockLevelsByWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockItems()
        {
            try
            {
                var stockLevels = await _repository.StockLevel.GetLowStockItemsAsync(trackChanges: false);
                _logger.LogInfo("Returned low stock items from database.");

                var stockLevelsResult = _mapper.Map<IEnumerable<StockLevelDto>>(stockLevels);
                return Ok(stockLevelsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetLowStockItems action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("overstock")]
        public async Task<IActionResult> GetOverstockItems()
        {
            try
            {
                var stockLevels = await _repository.StockLevel.GetOverstockItemsAsync(trackChanges: false);
                _logger.LogInfo("Returned overstock items from database.");

                var stockLevelsResult = _mapper.Map<IEnumerable<StockLevelDto>>(stockLevels);
                return Ok(stockLevelsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOverstockItems action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockLevel([FromBody] StockLevelForCreationDto stockLevel)
        {
            try
            {
                if (stockLevel == null)
                {
                    _logger.LogError("Stock level object sent from client is null.");
                    return BadRequest("Stock level object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid stock level object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var stockLevelEntity = _mapper.Map<StockLevel>(stockLevel);
                stockLevelEntity.QuantityAvailable = stockLevelEntity.QuantityOnHand - stockLevelEntity.QuantityReserved;
                stockLevelEntity.TotalValue = stockLevelEntity.QuantityOnHand * stockLevelEntity.UnitCost;
                stockLevelEntity.NextCountDate = DateTime.UtcNow.AddDays(stockLevelEntity.CountFrequency);
                stockLevelEntity.CreatedAt = DateTime.UtcNow;
                stockLevelEntity.UpdatedAt = DateTime.UtcNow;

                _repository.StockLevel.CreateStockLevel(stockLevelEntity);
                await _repository.SaveAsync();

                var createdStockLevel = _mapper.Map<StockLevelDto>(stockLevelEntity);

                return CreatedAtRoute("StockLevelById", new { id = createdStockLevel.Id }, createdStockLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateStockLevel action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStockLevel(Guid id, [FromBody] StockLevelForUpdateDto stockLevel)
        {
            try
            {
                if (stockLevel == null)
                {
                    _logger.LogError("Stock level object sent from client is null.");
                    return BadRequest("Stock level object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid stock level object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var stockLevelEntity = await _repository.StockLevel.GetStockLevelByIdAsync(id, trackChanges: true);
                if (stockLevelEntity == null)
                {
                    _logger.LogError($"Stock level with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(stockLevel, stockLevelEntity);
                stockLevelEntity.QuantityAvailable = stockLevelEntity.QuantityOnHand - stockLevelEntity.QuantityReserved;
                stockLevelEntity.TotalValue = stockLevelEntity.QuantityOnHand * stockLevelEntity.UnitCost;
                stockLevelEntity.UpdatedAt = DateTime.UtcNow;

                _repository.StockLevel.UpdateStockLevel(stockLevelEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateStockLevel action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("adjust")]
        public async Task<IActionResult> AdjustStockLevel([FromBody] StockAdjustmentDto adjustment)
        {
            try
            {
                if (adjustment == null)
                {
                    _logger.LogError("Stock adjustment object sent from client is null.");
                    return BadRequest("Stock adjustment object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid stock adjustment object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var stockLevelEntity = await _repository.StockLevel.GetStockLevelByIdAsync(adjustment.StockLevelId, trackChanges: true);
                if (stockLevelEntity == null)
                {
                    _logger.LogError($"Stock level with id: {adjustment.StockLevelId}, hasn't been found in db.");
                    return NotFound();
                }

                // Apply adjustment
                var oldQuantity = stockLevelEntity.QuantityOnHand;
                switch (adjustment.AdjustmentType)
                {
                    case AdjustmentType.Increase:
                        stockLevelEntity.QuantityOnHand += adjustment.AdjustmentQuantity;
                        break;
                    case AdjustmentType.Decrease:
                        stockLevelEntity.QuantityOnHand -= adjustment.AdjustmentQuantity;
                        break;
                    case AdjustmentType.Set:
                        stockLevelEntity.QuantityOnHand = adjustment.AdjustmentQuantity;
                        break;
                }

                stockLevelEntity.QuantityAvailable = stockLevelEntity.QuantityOnHand - stockLevelEntity.QuantityReserved;
                stockLevelEntity.TotalValue = stockLevelEntity.QuantityOnHand * stockLevelEntity.UnitCost;
                stockLevelEntity.UpdatedAt = DateTime.UtcNow;
                stockLevelEntity.LastCountDate = DateTime.UtcNow;

                _repository.StockLevel.UpdateStockLevel(stockLevelEntity);

                // Create transaction record for the adjustment
                var transaction = new InventoryTransaction
                {
                    DeviceId = stockLevelEntity.DeviceId,
                    TransactionType = TransactionType.Adjustment,
                    Description = $"Stock adjustment: {adjustment.Reason}",
                    UnitCost = stockLevelEntity.UnitCost,
                    TotalValue = Math.Abs(adjustment.AdjustmentQuantity) * stockLevelEntity.UnitCost,
                    Status = TransactionStatus.Completed,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _repository.InventoryTransaction.CreateTransaction(transaction);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AdjustStockLevel action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockLevel(Guid id)
        {
            try
            {
                var stockLevel = await _repository.StockLevel.GetStockLevelByIdAsync(id, trackChanges: false);
                if (stockLevel == null)
                {
                    _logger.LogError($"Stock level with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.StockLevel.DeleteStockLevel(stockLevel);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteStockLevel action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}