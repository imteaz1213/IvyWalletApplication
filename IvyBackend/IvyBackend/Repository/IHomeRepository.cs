using IvyBackend.Models;
using IvyBackend.Models.DTO;

namespace IvyBackend.Repository
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Currency>> GetCurrency();
        Task<IEnumerable<Account>> GetAccount();
        Task<IEnumerable<Category>> GetCategory();
        Task<IEnumerable<Currency>> FindByName(string name);
        Task<UserCurrencyDTO> GetUserCurrency(int user_id);
    }
}
