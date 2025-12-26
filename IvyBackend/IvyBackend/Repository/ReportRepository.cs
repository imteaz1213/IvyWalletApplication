using IvyBackend.Data;
using IvyBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IvyBackend.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionFilterDTO>> ApplyFilterAsync(
            int user, int month, int? accountId, int? categoryId, string? type, decimal? amount)
        {
            if (string.IsNullOrEmpty(type))
                return new List<TransactionFilterDTO>();

            if (type.Equals("Income", StringComparison.OrdinalIgnoreCase))
            {
                var query = from i in _context.Incomes
                            join a in _context.Accounts on i.AccountId equals a.Id
                            join c in _context.Categories on i.CategoryId equals c.Id
                            where i.UserId == user && i.Date.Month == month
                            select new TransactionFilterDTO
                            {
                                Type = "Income",
                                Id = i.Id,
                                Amount = i.Amount,
                                Date = i.Date,
                                AccountId = i.AccountId,
                                CategoryId = i.CategoryId,
                                AccountName = a.Name,
                                CategoryName = c.Name
                            };

                if (accountId.HasValue)
                    query = query.Where(x => x.AccountId == accountId.Value);

                if (categoryId.HasValue)
                    query = query.Where(x => x.CategoryId == categoryId.Value);

                if (amount.HasValue && amount > 0)
                    query = query.Where(x => x.Amount <= amount.Value);

                return await query.ToListAsync();
            }
            else if (type.Equals("Expense", StringComparison.OrdinalIgnoreCase))
            {
                var query = from e in _context.Expenses
                            join a in _context.Accounts on e.AccountId equals a.Id
                            join c in _context.Categories on e.CategoryId equals c.Id
                            where e.UserId == user && e.Date.Month == month
                            select new TransactionFilterDTO
                            {
                                Type = "Expense",
                                Id = e.Id,
                                Amount = e.Amount,
                                Date = e.Date,
                                AccountId = e.AccountId,
                                CategoryId = e.CategoryId,
                                AccountName = a.Name,
                                CategoryName = c.Name
                            };

                if (accountId.HasValue)
                    query = query.Where(x => x.AccountId == accountId.Value);

                if (categoryId.HasValue)
                    query = query.Where(x => x.CategoryId == categoryId.Value);

                if (amount.HasValue && amount > 0)
                    query = query.Where(x => x.Amount <= amount.Value);

                return await query.ToListAsync();
            }

            // Return empty list if type is invalid
            return new List<TransactionFilterDTO>();
        }
    }
}
