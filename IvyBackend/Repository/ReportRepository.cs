
using IvyBackend.Data;
using IvyBackend.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            int user,
            int startMonth,
            int endMonth,
            int? accountId,
            int? categoryId,
            string? type,
            decimal? amount)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                    return new List<TransactionFilterDTO>();

                var userParam = new SqlParameter("@UserId", user);
                var startMonthParam = new SqlParameter("@StartMonth", startMonth);
                var endMonthParam = new SqlParameter("@EndMonth", endMonth);
                var accountParam = new SqlParameter("@AccountId", (object?)accountId ?? DBNull.Value);
                var categoryParam = new SqlParameter("@CategoryId", (object?)categoryId ?? DBNull.Value);
                var typeParam = new SqlParameter("@Type", type);
                var amountParam = new SqlParameter("@Amount", (object?)amount ?? DBNull.Value);

                var transactions = await _context.Set<TransactionFilterDTO>()
                    .FromSqlRaw(@"
                        EXEC ApplyTransactionFilter 
                            @UserId, 
                            @StartMonth, 
                            @EndMonth, 
                            @AccountId, 
                            @CategoryId, 
                            @Type, 
                            @Amount",
                        userParam, startMonthParam, endMonthParam, accountParam, categoryParam, typeParam, amountParam)
                    .AsNoTracking()
                    .ToListAsync();

                return transactions;
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in ApplyFilterAsync: {sqlEx.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in ApplyFilterAsync: {ex.Message}");
                throw;
            }
        }
    }
}
