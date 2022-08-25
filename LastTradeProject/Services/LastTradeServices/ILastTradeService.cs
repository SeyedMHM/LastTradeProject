using LastTradeProject.Models;

namespace LastTradeProject.Services.LastTradeServices
{
    public interface ILastTradeService
    {
        Task<IEnumerable<LastTrade>> GetAsync(DateTime? startDate, CancellationToken cancellationToken);
        Task LastTradeOperations(DateTime? startDate, CancellationToken cancellationToken);
    }
}