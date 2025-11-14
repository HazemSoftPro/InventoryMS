using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.InventoryTransaction;

namespace InventrySystem.Controllers
{
    [Route("api/inventory-transactions")]
    [ApiController]
    [Authorize]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public InventoryTransactionController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _repository.InventoryTransaction.GetAllTransactionsAsync(trackChanges: false);
                _logger.LogInfo("Returned all inventory transactions from database.");

                var transactionsResult = _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
                return Ok(transactionsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllTransactions action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "TransactionById")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            try
            {
                var transaction = await _repository.InventoryTransaction.GetTransactionByIdAsync(id, trackChanges: false);
                if (transaction == null)
                {
                    _logger.LogError($"Transaction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned transaction with id: {id}");

                var transactionResult = _mapper.Map<InventoryTransactionDto>(transaction);
                return Ok(transactionResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTransactionById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetTransactionsByDevice(Guid deviceId)
        {
            try
            {
                var transactions = await _repository.InventoryTransaction.GetTransactionsByDeviceAsync(deviceId, trackChanges: false);
                _logger.LogInfo($"Returned transactions for device with id: {deviceId}");

                var transactionsResult = _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
                return Ok(transactionsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTransactionsByDevice action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUser(Guid userId)
        {
            try
            {
                var transactions = await _repository.InventoryTransaction.GetTransactionsByUserAsync(userId, trackChanges: false);
                _logger.LogInfo($"Returned transactions for user with id: {userId}");

                var transactionsResult = _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
                return Ok(transactionsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTransactionsByUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("type/{transactionType}")]
        public async Task<IActionResult> GetTransactionsByType(TransactionType transactionType)
        {
            try
            {
                var transactions = await _repository.InventoryTransaction.GetTransactionsByTypeAsync(transactionType, trackChanges: false);
                _logger.LogInfo($"Returned transactions of type: {transactionType}");

                var transactionsResult = _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
                return Ok(transactionsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTransactionsByType action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTransactionsByStatus(TransactionStatus status)
        {
            try
            {
                var transactions = await _repository.InventoryTransaction.GetTransactionsByStatusAsync(status, trackChanges: false);
                _logger.LogInfo($"Returned transactions with status: {status}");

                var transactionsResult = _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
                return Ok(transactionsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTransactionsByStatus action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] InventoryTransactionForCreationDto transaction)
        {
            try
            {
                if (transaction == null)
                {
                    _logger.LogError("Transaction object sent from client is null.");
                    return BadRequest("Transaction object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid transaction object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var transactionEntity = _mapper.Map<InventoryTransaction>(transaction);
                transactionEntity.CreatedAt = DateTime.UtcNow;
                transactionEntity.UpdatedAt = DateTime.UtcNow;

                _repository.InventoryTransaction.CreateTransaction(transactionEntity);
                await _repository.SaveAsync();

                var createdTransaction = _mapper.Map<InventoryTransactionDto>(transactionEntity);

                return CreatedAtRoute("TransactionById", new { id = createdTransaction.Id }, createdTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateTransaction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] InventoryTransactionForUpdateDto transaction)
        {
            try
            {
                if (transaction == null)
                {
                    _logger.LogError("Transaction object sent from client is null.");
                    return BadRequest("Transaction object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid transaction object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var transactionEntity = await _repository.InventoryTransaction.GetTransactionByIdAsync(id, trackChanges: true);
                if (transactionEntity == null)
                {
                    _logger.LogError($"Transaction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(transaction, transactionEntity);
                transactionEntity.UpdatedAt = DateTime.UtcNow;

                _repository.InventoryTransaction.UpdateTransaction(transactionEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateTransaction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveTransaction(Guid id)
        {
            try
            {
                var transactionEntity = await _repository.InventoryTransaction.GetTransactionByIdAsync(id, trackChanges: true);
                if (transactionEntity == null)
                {
                    _logger.LogError($"Transaction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                transactionEntity.Status = TransactionStatus.Approved;
                transactionEntity.ApprovedDate = DateTime.UtcNow;
                transactionEntity.UpdatedAt = DateTime.UtcNow;

                _repository.InventoryTransaction.UpdateTransaction(transactionEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ApproveTransaction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            try
            {
                var transaction = await _repository.InventoryTransaction.GetTransactionByIdAsync(id, trackChanges: false);
                if (transaction == null)
                {
                    _logger.LogError($"Transaction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.InventoryTransaction.DeleteTransaction(transaction);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteTransaction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}