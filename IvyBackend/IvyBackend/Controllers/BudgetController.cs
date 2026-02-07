using IvyBackend.Models;
using IvyBackend.Models.DTO;
using IvyBackend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IvyBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        IBudgetRepository _budgetRepository;
        public BudgetController(IBudgetRepository budgetRepository)
        {
             _budgetRepository = budgetRepository;
        } 

        [HttpGet("{user_id:int}/{month:int}")]
        public async Task<IActionResult> GetBudgetById(int user_id, int month)
        {
            var result = await _budgetRepository.GetBudgetById(user_id, month);   
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBudget([FromBody] BudgetDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _budgetRepository.AddBudget(dto); 

            return Ok("Budget Added Successfully");
        }
    }
}
