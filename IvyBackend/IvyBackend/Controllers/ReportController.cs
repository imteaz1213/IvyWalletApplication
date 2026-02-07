using IvyBackend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvyBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        IReportRepository _reportRepository;
        public ReportController(IReportRepository reportRepository)
        {
             _reportRepository = reportRepository;
        }
        [HttpGet("ApplyFilter")]
        public async Task<IActionResult> ApplyFilter(
            [FromQuery] int user,
            [FromQuery] int month,
            [FromQuery] int? accountId,
            [FromQuery] int? categoryId,
            [FromQuery] string? type,
            [FromQuery] decimal? amount)
        {
            var result = await _reportRepository.ApplyFilterAsync(user, month, accountId, categoryId, type, amount);

            if (!result.Any())
                return BadRequest("Invalid type or no records found. Must be 'Income' or 'Expense'.");

            return Ok(result);
        }
    }
}
