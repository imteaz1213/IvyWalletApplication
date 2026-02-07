using IvyBackend.Data;
using IvyBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IvyBackend.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddExpense(ExpenseDTO exp)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO Expense (Title, Description, Date, Amount, CategoryId, UserId, AccountId)  
                VALUES ({exp.Title}, {exp.Description}, {exp.Date}, {exp.Amount}, {exp.CategoryId}, {exp.UserId}, {exp.AccountId})");
        }

        public async Task<decimal> FullExpenseOfAUser(int user_id, int month)
        {
            var data = await(
                            from u in _context.Users
                            join e in _context.Expenses
                            on u.Id equals e.UserId
                            where u.Id == user_id && e.Date.Month == month
                            select e.Amount
                        ).SumAsync(); 
            return data;
        }

        public async Task<IEnumerable<ExpMonthDTO>> GetExpenseByMonth(int user_id, int month)
        {
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
               select new ExpMonthDTO
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
            return data;
        }
    }
}
