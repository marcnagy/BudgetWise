using BudgetWise.API.Constants;
using BudgetWise.Application.DTOs;
using BudgetWise.Application.Interfaces;
using BudgetWise.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController(IExpenseService expenseService, ILogger<ExpenseController> logger) : ControllerBase
    {
        private readonly IExpenseService _expenseService = expenseService;
        private readonly ILogger<ExpenseController> _logger = logger;

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    _logger.LogError(Messages.InvalidToken);
                    return Unauthorized(new { Message = Messages.InvalidToken });
                }

                var expenses = await _expenseService.GetAllExpensesAsync(userId.Value);
                _logger.LogInformation("User {UserId} retrieved all expenses successfully.", userId);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.InternalServerError);
                return StatusCode(500, new { Message = Messages.InternalServerError, Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(int id)
        {
            if (id <= 0)
            {
                _logger.LogError(Messages.InvalidExpenseId);
                return BadRequest(new { Message = Messages.InvalidExpenseId });
            }

            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    _logger.LogError(Messages.InvalidToken);
                    return Unauthorized(new { Message = Messages.InvalidToken });
                }

                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null || expense.UserId != userId)
                {
                    _logger.LogError(Messages.ExpenseNotFound);
                    return NotFound(new { Message = Messages.ExpenseNotFound });
                }

                _logger.LogInformation("User {UserId} retrieved expense {ExpenseId} successfully.", userId, id);
                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.InternalServerError);
                return StatusCode(500, new { Message = Messages.InternalServerError, Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> CreateExpense([FromBody] ExpenseDTO expenseDto)
        {
            if (expenseDto == null)
            {
                _logger.LogError(Messages.ExpenseDataRequired);
                return BadRequest(new { Message = Messages.ExpenseDataRequired });
            }

            if (expenseDto.Amount <= 0 || string.IsNullOrEmpty(expenseDto.Name))
            {
                _logger.LogError(Messages.InvalidExpenseData);
                return BadRequest(new { Message = Messages.InvalidExpenseData });
            }

            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    _logger.LogError(Messages.InvalidToken);
                    return Unauthorized(new { Message = Messages.InvalidToken });
                }

                var expense = new Expense
                {
                    Name = expenseDto.Name,
                    Amount = expenseDto.Amount,
                    Date = expenseDto.Date,
                    Description = expenseDto.Description,
                    UserId = userId.Value
                };

                await _expenseService.AddExpenseAsync(expense);
                _logger.LogInformation("User {UserId} created a new expense {ExpenseId}.", userId, expense.Id);
                return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.InternalServerError);
                return StatusCode(500, new { Message = Messages.InternalServerError, Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDTO expenseDto)
        {
            if (id <= 0 || expenseDto == null)
            {
                _logger.LogError(Messages.InvalidExpenseData);
                return BadRequest(new { Message = Messages.InvalidExpenseData });
            }

            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    _logger.LogError(Messages.InvalidToken);
                    return Unauthorized(new { Message = Messages.InvalidToken });
                }

                var existingExpense = await _expenseService.GetExpenseByIdAsync(id);
                if (existingExpense == null || existingExpense.UserId != userId)
                {
                    _logger.LogError(Messages.ExpenseNotFound);
                    return NotFound(new { Message = Messages.ExpenseNotFound });
                }

                existingExpense.Name = expenseDto.Name;
                existingExpense.Amount = expenseDto.Amount;
                existingExpense.Date = expenseDto.Date;
                existingExpense.Description = expenseDto.Description;

                await _expenseService.UpdateExpenseAsync(existingExpense);
                _logger.LogInformation("User {UserId} updated expense {ExpenseId}.", userId, id);
                return Ok(new { Message = Messages.ExpenseUpdated });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.InternalServerError);
                return StatusCode(500, new { Message = Messages.InternalServerError, Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (id <= 0)
            {
                _logger.LogError(Messages.InvalidExpenseId);
                return BadRequest(new { Message = Messages.InvalidExpenseId });
            }

            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    _logger.LogError(Messages.InvalidToken);
                    return Unauthorized(new { Message = Messages.InvalidToken });
                }

                var existingExpense = await _expenseService.GetExpenseByIdAsync(id);
                if (existingExpense == null || existingExpense.UserId != userId)
                {
                    _logger.LogError(Messages.ExpenseNotFound);
                    return NotFound(new { Message = Messages.ExpenseNotFound });
                }

                var deleted = await _expenseService.DeleteExpenseAsync(id);
                if (!deleted)
                {
                    _logger.LogError(Messages.ExpenseDeleteFailed);
                    return StatusCode(500, new { Message = Messages.ExpenseDeleteFailed });
                }

                _logger.LogInformation("User {UserId} deleted expense {ExpenseId}.", userId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.InternalServerError);
                return StatusCode(500, new { Message = Messages.InternalServerError, Error = ex.Message });
            }
        }
    }
}
