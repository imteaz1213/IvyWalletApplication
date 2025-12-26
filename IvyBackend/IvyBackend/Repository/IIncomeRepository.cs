using IvyBackend.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IvyBackend.Repository
{
    public interface IIncomeRepository
    {
        Task AddIncome(IncomeDTO inc); 
        Task<IEnumerable<IncMonthDTO>> GetIncomeByMonth(int user_id, int month);
        Task<decimal> FullIncomeOfAUser(int user_id, int month);
    }
}
