
using IvyBackend.Data;
using IvyBackend.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC AddExpense 
                        @Title = {exp.Title}, 
                        @Description = {exp.Description}, 
                        @Date = {exp.Date}, 
                        @Amount = {exp.Amount}, 
                        @CategoryId = {exp.CategoryId}, 
                        @UserId = {exp.UserId}, 
                        @AccountId = {exp.AccountId}");
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in AddExpense: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AddExpense: {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> FullExpenseOfAUser(int userId, int month)
        {
            try
            {
                await using var conn = _context.Database.GetDbConnection();
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "FullExpenseOfAUser";
                cmd.CommandType = CommandType.StoredProcedure;

             
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@UserId";
                p1.DbType = DbType.Int32;
                p1.Value = userId;
                cmd.Parameters.Add(p1);
          
                var p2 = cmd.CreateParameter();
                p2.ParameterName = "@Month";
                p2.DbType = DbType.Int32;
                p2.Value = month;
                cmd.Parameters.Add(p2);

                var result = await cmd.ExecuteScalarAsync();

                return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FullExpenseOfAUser: " + ex.Message);
                throw;
            }
        }


        public async Task<IEnumerable<ExpMonthDTO>> GetExpenseByMonth(int user_id, int month)
        {
            try
            {
                var expenses = await _context.Set<ExpMonthDTO>()
                    .FromSqlInterpolated($@"
                        EXEC GetExpenseByMonth 
                            @UserId = {user_id}, 
                            @Month = {month}")
                    .AsNoTracking()
                    .ToListAsync();

                return expenses;
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in GetExpenseByMonth: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetExpenseByMonth: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateExpense(ExpenseDTO exp)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC UpdateExpense 
                @ExpenseId = {exp.Id}, 
                @Title = {exp.Title}, 
                @Description = {exp.Description}, 
                @Date = {exp.Date}, 
                @Amount = {exp.Amount}, 
                @CategoryId = {exp.CategoryId}, 
                @AccountId = {exp.AccountId}");
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in UpdateExpense: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in UpdateExpense: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteExpense(int expenseId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC DeleteExpense 
                @ExpenseId = {expenseId}");
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in DeleteExpense: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in DeleteExpense: {ex.Message}");
                throw;
            }
        }

    }
}
