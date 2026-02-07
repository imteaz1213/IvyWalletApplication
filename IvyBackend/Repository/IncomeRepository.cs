
using IvyBackend.Data;
using IvyBackend.Models;
using IvyBackend.Models.DTO;
using IvyBackend.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class IncomeRepository : IIncomeRepository
{
    private readonly ApplicationDbContext _context;

    public IncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task AddIncome(IncomeDTO inc)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC AddIncome 
                    @Title={inc.Title},
                    @Description={inc.Description},
                    @Date={inc.Date},
                    @Amount={inc.Amount},
                    @CategoryId={inc.CategoryId},
                    @UserId={inc.UserId},
                    @AccountId={inc.AccountId}");
        }
        catch (SqlException sqlEx)
        {
            Console.Error.WriteLine($"SQL Error in AddIncome: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in AddIncome: {ex.Message}");
            throw;
        }
    }

   
    public async Task UpdateIncome(IncomeDTO inc)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC UpdateIncome
                    @Id={inc.Id},
                    @Title={inc.Title},
                    @Description={inc.Description},
                    @Date={inc.Date},
                    @Amount={inc.Amount},
                    @CategoryId={inc.CategoryId},
                    @AccountId={inc.AccountId}");
        }
        catch (SqlException sqlEx)
        {
            Console.Error.WriteLine($"SQL Error in UpdateIncome: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in UpdateIncome: {ex.Message}");
            throw;
        }
    }

  
    public async Task DeleteIncome(int id)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC DeleteIncome @Id={id}");
        }
        catch (SqlException sqlEx)
        {
            Console.Error.WriteLine($"SQL Error in DeleteIncome: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in DeleteIncome: {ex.Message}");
            throw;
        }
    }
     
 

    public async Task<IEnumerable<IncMonthDTO>> GetIncomeByMonth(int user_id, int month)
    {
        try
        {
            return await _context.Set<IncMonthDTO>()
                .FromSqlInterpolated($@"
                    EXEC GetIncomeByMonth 
                        @UserId = {user_id}, 
                        @Month = {month}")
                .AsNoTracking()
                .ToListAsync();
        }
        catch (SqlException sqlEx)
        {
            Console.Error.WriteLine($"SQL Error in GetIncomeByMonth: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in GetIncomeByMonth: {ex.Message}");
            throw;
        }
    }


    public async Task<decimal> FullIncomeOfAUser(int user_id, int month)
    {
        try
        {
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "FullIncomeOfAUser";
            command.CommandType = CommandType.StoredProcedure;

            var paramUser = command.CreateParameter();
            paramUser.ParameterName = "@UserId";
            paramUser.Value = user_id;
            command.Parameters.Add(paramUser);

            var paramMonth = command.CreateParameter();
            paramMonth.ParameterName = "@Month";
            paramMonth.Value = month;
            command.Parameters.Add(paramMonth);

            var result = await command.ExecuteScalarAsync();

            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }
        catch (SqlException sqlEx)
        {
            Console.Error.WriteLine($"SQL Error in FullIncomeOfAUser: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in FullIncomeOfAUser: {ex.Message}");
            throw;
        }
    }

}
