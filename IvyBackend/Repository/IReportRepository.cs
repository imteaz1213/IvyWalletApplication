using IvyBackend.Models.DTO;

namespace IvyBackend.Repository
{
    public interface IReportRepository
    {
        Task<IEnumerable<TransactionFilterDTO>> ApplyFilterAsync(
            int userId,
            int startMonth,
            int endMonth,
            int? accountId,
            int? categoryId,
            string? type,
            decimal? amount);
    }
}
