using IvyBackend.Data;
using IvyBackend.Models;
using IvyBackend.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IvyBackend.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> FindByName(string name)
        {
            try
            {
                return await _context.Currencies
                    .FromSqlInterpolated($"EXEC FindCurrencyByName @Name={name}")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in FindByName: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Account>> GetAccount()
        {
            try
            {
                return await _context.Accounts
                    .FromSqlInterpolated($"EXEC GetAllAccounts")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in GetAccount: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            try
            {
                return await _context.Categories
                    .FromSqlInterpolated($"EXEC GetAllCategories")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in GetCategory: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Currency>> GetCurrency()
        {
            try
            {
                return await _context.Currencies
                    .FromSqlInterpolated($"EXEC GetAllCurrencies")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in GetCurrency: {ex.Message}");
                throw;
            }
        }

        public async Task<UserCurrencyDTO> GetUserCurrency(int user_id)
        {
            try
            {
                using var command = _context.Database.GetDbConnection().CreateCommand();

                command.CommandText = "GetUserCurrency";
                command.CommandType = CommandType.StoredProcedure;

                var param = command.CreateParameter();
                param.ParameterName = "@UserId";
                param.Value = user_id;
                command.Parameters.Add(param);

                await _context.Database.OpenConnectionAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new UserCurrencyDTO
                    {
                        Currency = reader["Currency"]?.ToString() ?? "N/A"
                    };
                }

                return new UserCurrencyDTO { Currency = "N/A" };
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine($"SQL Error in GetUserCurrency: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"General Error in GetUserCurrency: {ex.Message}");
                throw;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

    }
}
