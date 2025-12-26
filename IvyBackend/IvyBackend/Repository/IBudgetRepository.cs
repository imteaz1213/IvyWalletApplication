using IvyBackend.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IvyBackend.Repository
{
    public interface IBudgetRepository
    {
        Task AddBudget(BudgetDTO dto);
        Task<IEnumerable<BudgetIdDTO>> GetBudgetById(int user_id, int month);
    }
}
