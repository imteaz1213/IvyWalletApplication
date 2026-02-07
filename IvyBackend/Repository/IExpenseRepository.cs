using IvyBackend.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IvyBackend.Repository
{
    public interface IExpenseRepository
    {
        Task AddExpense(ExpenseDTO exp);
        Task<IEnumerable<ExpMonthDTO>> GetExpenseByMonth(int user_id, int month);
        Task<decimal> FullExpenseOfAUser(int user_id, int month);
        Task UpdateExpense(ExpenseDTO exp);
        Task DeleteExpense(int expenseId);
    }
}
