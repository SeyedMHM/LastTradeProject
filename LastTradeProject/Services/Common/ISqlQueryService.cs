namespace LastTradeProject.Services.Common
{
    public interface ISqlQueryService
    {
        Task<List<TResult>> GetRowsAsync<TResult>(string command, CancellationToken cancellationToken);
    }
}