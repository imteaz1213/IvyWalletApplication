using IvyBackend.Data;
using IvyBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IvyBackend.Repository
{ 
    public class IncomeRepository : IIncomeRepository
    {
        private readonly ApplicationDbContext _context;
        public IncomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddIncome(IncomeDTO inc)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                 INSERT INTO Income (Title, Description, Date, Amount, CategoryId, UserId, AccountId)
                 VALUES ({inc.Title}, {inc.Description}, {inc.Date}, {inc.Amount}, {inc.CategoryId},
                 {inc.UserId}, {inc.AccountId})");
        }

        public async Task<decimal> FullIncomeOfAUser(int user_id, int month)
        {
            var data = await(
                from u in _context.Users
                join i in _context.Incomes
                on u.Id equals i.UserId
                where u.Id == user_id && i.Date.Month == month
                select i.Amount
            ).SumAsync();
            return data;
        }

        public async Task<IEnumerable<IncMonthDTO>> GetIncomeByMonth(int user_id, int month)
        { 
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
               select new IncMonthDTO
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

            return data;
        }
    }
}
