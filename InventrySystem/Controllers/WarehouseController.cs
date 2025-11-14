using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Warehouse;

namespace InventrySystem.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public WarehouseController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses()
        {
            try
            {
                var warehouses = await _repository.Warehouse.GetAllWarehousesAsync(trackChanges: false);
                _logger.LogInfo("Returned all warehouses from database.");

                var warehousesResult = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);
                return Ok(warehousesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllWarehouses action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "WarehouseById")]
        public async Task<IActionResult> GetWarehouseById(Guid id)
        {
            try
            {
                var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: false);
                if (warehouse == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned warehouse with id: {id}");

                var warehouseResult = _mapper.Map<WarehouseDto>(warehouse);
                return Ok(warehouseResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetWarehouseById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> GetWarehouseStatistics(Guid id)
        {
            try
            {
                var statistics = await _repository.Warehouse.GetWarehouseStatisticsAsync(id);
                if (statistics == null)
                {
                    _logger.LogError($"Statistics for warehouse with id: {id}, couldn't be generated.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned statistics for warehouse with id: {id}");
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetWarehouseStatistics action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveWarehouses()
        {
            try
            {
                var warehouses = await _repository.Warehouse.GetActiveWarehousesAsync(trackChanges: false);
                _logger.LogInfo("Returned all active warehouses from database.");

                var warehousesResult = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);
                return Ok(warehousesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetActiveWarehouses action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("available-capacity")]
        public async Task<IActionResult> GetWarehousesWithAvailableCapacity()
        {
            try
            {
                var warehouses = await _repository.Warehouse.GetWarehousesWithAvailableCapacityAsync(trackChanges: false);
                _logger.LogInfo("Returned warehouses with available capacity from database.");

                var warehousesResult = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);
                return Ok(warehousesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetWarehousesWithAvailableCapacity action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseForCreationDto warehouse)
        {
            try
            {
                if (warehouse == null)
                {
                    _logger.LogError("Warehouse object sent from client is null.");
                    return BadRequest("Warehouse object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid warehouse object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var warehouseEntity = _mapper.Map<Warehouse>(warehouse);
                warehouseEntity.CreatedAt = DateTime.UtcNow;
                warehouseEntity.UpdatedAt = DateTime.UtcNow;

                _repository.Warehouse.CreateWarehouse(warehouseEntity);
                await _repository.SaveAsync();

                var createdWarehouse = _mapper.Map<WarehouseDto>(warehouseEntity);

                return CreatedAtRoute("WarehouseById", new { id = createdWarehouse.Id }, createdWarehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] WarehouseForUpdateDto warehouse)
        {
            try
            {
                if (warehouse == null)
                {
                    _logger.LogError("Warehouse object sent from client is null.");
                    return BadRequest("Warehouse object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid warehouse object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var warehouseEntity = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: true);
                if (warehouseEntity == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(warehouse, warehouseEntity);
                warehouseEntity.UpdatedAt = DateTime.UtcNow;

                _repository.Warehouse.UpdateWarehouse(warehouseEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(Guid id)
        {
            try
            {
                var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: false);
                if (warehouse == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                // Check if warehouse has any devices or stock levels
                if (warehouse.Devices?.Any() == true || warehouse.StockLevels?.Any() == true)
                {
                    _logger.LogError($"Warehouse with id: {id} cannot be deleted as it contains devices or stock levels.");
                    return BadRequest("Warehouse cannot be deleted as it contains devices or stock levels");
                }

                _repository.Warehouse.DeleteWarehouse(warehouse);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/utilization")]
        public async Task<IActionResult> UpdateWarehouseUtilization(Guid id, [FromBody] decimal utilization)
        {
            try
            {
                var warehouseEntity = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: true);
                if (warehouseEntity == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                warehouseEntity.CurrentUtilization = utilization;
                warehouseEntity.UpdatedAt = DateTime.UtcNow;

                _repository.Warehouse.UpdateWarehouse(warehouseEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateWarehouseUtilization action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}