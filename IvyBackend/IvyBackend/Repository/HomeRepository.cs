using IvyBackend.Data;
using IvyBackend.Models;
using IvyBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;

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
            var data = await _context.Currencies
                    .FromSqlInterpolated($@"
                        SELECT Id, Title, Name 
                        FROM Currency
                        WHERE Name = {name} 
                           OR LOWER(Name) LIKE {name.ToLower()} + '%'
                    ")
                    .AsNoTracking()
                    .ToListAsync(); 
            return data;
        }

        public async Task<IEnumerable<Account>> GetAccount()
        {
            var data = await _context.Accounts
                             .FromSqlInterpolated($"SELECT Id,Name FROM Account")
                             .ToListAsync(); 
            return data;
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            var data = await _context.Categories
                            .FromSqlInterpolated($"SELECT Id, Name, Image, Color FROM Category")
                            .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<Currency>> GetCurrency()
        {
            var data = await _context.Currencies
                .FromSqlInterpolated($"SELECT Id, Title, Name FROM Currency")
                .ToListAsync(); 
            return data;
        }

        public async Task<UserCurrencyDTO> GetUserCurrency(int user_id)
        {
            var userCurrency = await _context.Set<UserCurrencyDTO>().FromSqlInterpolated($@"
                                SELECT c.Title as Currency 
                                FROM [User] u    
                                JOIN [Currency] c 
                                ON u.CurrencyId = c.Id 
                                WHERE u.Id = {user_id}
                              ").FirstOrDefaultAsync();
            return userCurrency;
        }
    }
}
