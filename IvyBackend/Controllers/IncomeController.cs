using IvyBackend.Models.DTO;
using IvyBackend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvyBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        IIncomeRepository _incomeRepository;
        public IncomeController(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }


      
        [HttpPost]
        public async Task<IActionResult> AddIncome([FromBody] IncomeDTO inc)
        {
            await _incomeRepository.AddIncome(inc);
            return Ok("Income Added Successfully");
        }


        [HttpGet("{user_id}/{month}")]
        public async Task<IActionResult> GetIncomeByMonth(int user_id, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month value.");

            var data = await _incomeRepository.GetIncomeByMonth(user_id, month);
            return Ok(data);
        }


        [HttpGet("{user_id:int}/{month:int}")]
        public async Task<IActionResult> FullIncomeOfAUser(int user_id, int month)
        {
            var data = await _incomeRepository.FullIncomeOfAUser(user_id, month);
            return Ok(data);
        }

       
        [HttpPut]
        public async Task<IActionResult> UpdateIncome([FromBody] IncomeDTO inc)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            await _incomeRepository.UpdateIncome(inc);
            return Ok("Income Updated Successfully");
        }

     
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            await _incomeRepository.DeleteIncome(id);
            return Ok("Income Deleted Successfully");
        }
    }
}

