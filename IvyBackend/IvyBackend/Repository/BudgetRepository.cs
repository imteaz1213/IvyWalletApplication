using IvyBackend.Data;
using IvyBackend.Models;
using IvyBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IvyBackend.Repository
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;
        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBudget(BudgetDTO dto)
        {
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
        }

        public async Task<IEnumerable<BudgetIdDTO>> GetBudgetById(int user_id, int month)
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
                select new BudgetIdDTO
                {
                    BudgetName = b.BudgetTitle,
                    Total = b.BudgetAmount,
                    Spent = e != null ? e.TotalExpense : 0,
                    Color = b.CategoryColor,
                    ExpenseAmount = e != null ? e.TotalExpense : 0
                }
            ).ToList();
            return result;
        }
    }
}
