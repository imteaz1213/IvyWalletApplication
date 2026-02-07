using IvyBackend.Data;
using IvyBackend.Models.DTO;
using IvyBackend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IvyBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        ApplicationDbContext _context;
        IExpenseRepository _expenseRepository;
        public ExpenseController(ApplicationDbContext context, IExpenseRepository expenseRepository)
        {
            _context = context;
            _expenseRepository = expenseRepository;
        } 

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDTO exp)
        {
            await _expenseRepository.AddExpense(exp);
            return Ok("Expense Added Successfully");
        } 

        [HttpGet("{user_id}/{month}")]
        public async Task<IActionResult> GetExpenseByMonth(int user_id, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month value.");

            var data = await _expenseRepository.GetExpenseByMonth(user_id, month);
            return Ok(data);
        }  

        [HttpGet("{user_id:int}/{month:int}")]
        public async Task<IActionResult> FullExpenseOfAUser(int user_id, int month)
        {
            var data = await _expenseRepository.FullExpenseOfAUser(user_id, month);
            return Ok(data);
        }
    }
}
