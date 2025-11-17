using IvyBackend.Data;
using IvyBackend.Models;
using IvyBackend.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace IvyBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrency()
        {
            var data = await _context.Currencies.ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var data = await _context.Accounts.ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategory()
        {
            var data = await _context.Categories.ToListAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddIncome([FromBody]IncomeDTO inc)
        {
            var income = new Income
            {
                Title = inc.Title,
                Description = inc.Description,
                Date = inc.Date,
                Amount = inc.Amount,
                CategoryId = inc.CategoryId,
                UserId = inc.UserId,
                AccountId = inc.AccountId
            };
            _context.Incomes.Add(income);
            await  _context.SaveChangesAsync();
            return Ok("Income Added Successfully");
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDTO exp)
        {
            var data = new Expense
            {
                Title = exp.Title,
                Description = exp.Description,
                Date = exp.Date,
                Amount = exp.Amount,
                CategoryId = exp.CategoryId,
                UserId = exp.UserId,
                AccountId = exp.AccountId
            };
            _context.Expenses.Add(data);
            await _context.SaveChangesAsync();
            return Ok("Expense Added Successfully");
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required");
            }

            var data = await (
                    from c in _context.Currencies
                    where c.Name == name || c.Name.ToLower().StartsWith(name.ToLower())
                    select new CurrencyDTO
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Name = c.Name
                    }
                ).ToListAsync();
            return Ok(data);
        }


        [HttpGet("{user_id:int}")]
        public async Task<IActionResult> GetUserCurrency(int user_id)
        {
            var userCurrency = await (
                    from u in _context.Users 
                    join c in _context.Currencies on u.CurrencyId equals c.Id
                    where u.Id == user_id
                    select new
                    {
                        Currency = c.Title
                    }
                ).ToListAsync();
            return Ok(userCurrency);
        }



        [HttpGet("{user_id}/{month}")]
        public async Task<IActionResult> GetBalance(int user_id, int month)
        {
            var incResult = await FullIncomeOfAUser(user_id, month) as OkObjectResult;
            var expResult = await FullExpenseOfAUser(user_id, month) as OkObjectResult;

            if (incResult == null || expResult == null)
                return BadRequest("Could not retrieve income or expense.");

            decimal income = incResult.Value is decimal dInc ? dInc : Convert.ToDecimal(incResult.Value);
            decimal expense = expResult.Value is decimal dExp ? dExp : Convert.ToDecimal(expResult.Value);

            var total = income - expense;
            return Ok(total);
        }



        [HttpGet("{user_id}/{month}")]
        public async Task<IActionResult> GetIncomeByMonth(int user_id, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month value.");

            var data = await (
               from u in _context.Users
               join i in _context.Incomes
               on u.Id equals i.UserId
               join c in _context.Categories
               on i.CategoryId equals c.Id
               join a in _context.Accounts
               on i.AccountId equals a.Id
               join curr in _context.Currencies
               on u.CurrencyId equals curr.Id
               where u.Id == user_id && i.Date.Month == month
               select new
               {
                   UserId = u.Id,
                   C_Name = c.Name,
                   C_Image = c.Image,
                   C_Color = c.Color,
                   A_Name = a.Name,
                   Cr_Currency = curr.Title,
                   I_Amount = i.Amount
               }
               ).ToListAsync();
            return Ok(data);
        }
        [HttpGet("{user_id}/{month}")]
        public async Task<IActionResult> GetExpenseByMonth(int user_id, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month value.");

            var data = await (
               from u in _context.Users
               join e in _context.Expenses
               on u.Id equals e.UserId
               join c in _context.Categories
               on e.CategoryId equals c.Id
               join a in _context.Accounts
               on e.AccountId equals a.Id
               join curr in _context.Currencies
               on u.CurrencyId equals curr.Id
               where u.Id == user_id && e.Date.Month == month
               select new
               {
                   UserId = u.Id,
                   C_Name = c.Name,
                   C_Image = c.Image,
                   C_Color = c.Color,
                   A_Name = a.Name,
                   Cr_Currency = curr.Title,
                   E_Amount = e.Amount
               }
               ).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{user_id:int}/{month:int}")]
        public async Task<IActionResult> FullIncomeOfAUser(int user_id, int month)
        {
            var data = await (
                from u in _context.Users
                join i in _context.Incomes
                on u.Id equals i.UserId
                where u.Id == user_id && i.Date.Month == month  
                select i.Amount
            ).SumAsync();

            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> AddBudget([FromBody] BudgetDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categories = await _context.Categories
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync();

            var budget = new Budget
            {
                Title = dto.Title,
                Amount = dto.Amount,
                Date = dto.Date,
                UserId = dto.UserId,
                Categories = categories
            };
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return Ok("Budget Added Successfully");
        }


        [HttpGet("{user_id:int}/{month:int}")]
        public async Task<IActionResult> FullExpenseOfAUser(int user_id,int month)
        {
            var data = await (
                            from u in _context.Users
                            join e in _context.Expenses
                            on u.Id equals e.UserId
                            where u.Id == user_id && e.Date.Month == month  
                            select e.Amount
                        ).SumAsync();

            return Ok(data);
        }


        [HttpGet("{user_id:int}/{month:int}")]
        public async Task<IActionResult> GetBudgetById(int user_id, int month)
        {
            var budgcat = await (
                from u in _context.Users
                join b in _context.Budgets on u.Id equals b.UserId
                join bc in _context.Set<Dictionary<string, object>>("BudgetCategory")
                    on b.Id equals (int)bc["BudgetsId"]
                join c in _context.Categories on (int)bc["CategoriesId"] equals c.Id
                where u.Id == user_id && b.Date.Month == month
                select new
                {
                    User = u.Name,
                    BudgetId = b.Id,
                    BudgetTitle = b.Title,
                    BudgetAmount = b.Amount,
                    BudgetDate = b.Date,
                    CategoryId = c.Id,
                    CategoryColor = c.Color
                }
            ).ToListAsync();

            var expenseSummary = await (
                from e in _context.Expenses
                where e.UserId == user_id && e.Date.Month == month
                group e by e.CategoryId into g
                select new
                {
                    CategoryId = g.Key,
                    TotalExpense = g.Sum(x => x.Amount)
                }
            ).ToListAsync();

            var result = (
                from b in budgcat
                join e in expenseSummary on b.CategoryId equals e.CategoryId into gj
                from e in gj.DefaultIfEmpty()
                select new
                {
                    BudgetName = b.BudgetTitle,
                    Total = b.BudgetAmount,
                    Spent = e != null ? e.TotalExpense : 0,
                    Color = b.CategoryColor,
                    ExpenseAmount = e != null ? e.TotalExpense : 0
                }
            ).ToList();
            return Ok(result);
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
            if (!string.IsNullOrEmpty(type) && string.Equals(type, "Income", StringComparison.OrdinalIgnoreCase))
            {
                var query = from i in _context.Incomes
                            join a in _context.Accounts on i.AccountId equals a.Id
                            join c in _context.Categories on i.CategoryId equals c.Id
                            where i.UserId == user && i.Date.Month == month
                            select new
                            {
                                Type = "Income",
                                i.Id,
                                i.Amount,
                                i.Date,
                                i.AccountId,
                                i.CategoryId,
                                AccountName = a.Name,
                                CategoryName = c.Name
                            };

                if (accountId.HasValue)
                    query = query.Where(i => i.AccountId == accountId.Value);

                if (categoryId.HasValue)
                    query = query.Where(i => i.CategoryId == categoryId.Value);

                if (amount.HasValue && amount > 0)
                    query = query.Where(i => i.Amount <= amount.Value);

                var result = await query.ToListAsync();
                return Ok(result);
            }
            else if (!string.IsNullOrEmpty(type) && string.Equals(type, "Expense", StringComparison.OrdinalIgnoreCase))
            {
                var query = from e in _context.Expenses
                            join a in _context.Accounts on e.AccountId equals a.Id
                            join c in _context.Categories on e.CategoryId equals c.Id
                            where e.UserId == user && e.Date.Month == month
                            select new
                            {
                                Type = "Expense",
                                e.Id,
                                e.Amount,
                                e.Date,
                                e.AccountId,
                                e.CategoryId,
                                AccountName = a.Name,
                                CategoryName = c.Name
                            };

                if (accountId.HasValue)
                    query = query.Where(e => e.AccountId == accountId.Value);

                if (categoryId.HasValue)
                    query = query.Where(e => e.CategoryId == categoryId.Value);

                if (amount.HasValue && amount > 0)
                    query = query.Where(e => e.Amount <= amount.Value);

                var result = await query.ToListAsync();
                return Ok(result);
            }
            return BadRequest("Invalid type. Must be 'Income' or 'Expense'.");
        }
    }
} 


