
using IvyBackend.Data;
using IvyBackend.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            try
            {
                var categoryTable = new DataTable();
                categoryTable.Columns.Add("Id", typeof(int));

                foreach (var id in dto.CategoryIds)
                    categoryTable.Rows.Add(id);

                var categoryParam = new SqlParameter("@CategoryIds", categoryTable)
                {
                    TypeName = "CategoryIdTableType",
                    SqlDbType = SqlDbType.Structured
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddBudget @Title={0}, @Amount={1}, @Date={2}, @UserId={3}, @CategoryIds=@CategoryIds",
                    dto.Title, dto.Amount, dto.Date, dto.UserId, categoryParam
                );
            }
            catch (SqlException sqlEx)
            { 
                Console.Error.WriteLine($"SQL Error in AddBudget: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
       
                Console.Error.WriteLine($"Error in AddBudget: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<BudgetIdDTO>> GetBudgetById(int userId, int month)
        {
            try
            {
                var budgets = await _context.Set<BudgetIdDTO>()
                    .FromSqlRaw("EXEC GetBudgetByUserMonth @UserId={0}, @Month={1}", userId, month)
                    .ToListAsync();

                return budgets;
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in GetBudgetById: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetBudgetById: {ex.Message}");
                throw;
            }
        }


        public async Task UpdateBudget(BudgetDTO dto)
        {
            try
            {
                var categoryTable = new DataTable();
                categoryTable.Columns.Add("Id", typeof(int));

                foreach (var id in dto.CategoryIds)
                    categoryTable.Rows.Add(id);

                var categoryParam = new SqlParameter("@CategoryIds", categoryTable)
                {
                    TypeName = "CategoryIdTableType",
                    SqlDbType = SqlDbType.Structured
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateBudget @BudgetId={0}, @Title={1}, @Amount={2}, @Date={3}, @CategoryIds=@CategoryIds",
                    dto.Id, dto.Title, dto.Amount, dto.Date, categoryParam
                );
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in UpdateBudget: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in UpdateBudget: {ex.Message}");
                throw;
            }
        }


        public async Task DeleteBudget(int budgetId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteBudget @BudgetId={0}", budgetId
                );
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"SQL Error in DeleteBudget: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in DeleteBudget: {ex.Message}");
                throw;
            }
        } 
    }
}
