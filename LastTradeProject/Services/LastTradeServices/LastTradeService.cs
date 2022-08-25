using LastTradeProject.Models;
using LastTradeProject.Services.Common;

namespace LastTradeProject.Services.LastTradeServices
{
    public class LastTradeService : ILastTradeService
    {
        private readonly ISqlQueryService _sqlQueryService;
        private readonly IFileService _fileService;
        private readonly IPrintService _printService;

        public LastTradeService(ISqlQueryService sqlQueryService, IFileService fileService, IPrintService printService)
        {
            _sqlQueryService = sqlQueryService;
            _fileService = fileService;
            _printService = printService;
        }


        public async Task<IEnumerable<LastTrade>> GetAsync(DateTime? startDate, CancellationToken cancellationToken)
        {
            string command = CreateSelectCommand(startDate);

            IEnumerable <LastTrade> lastTrades = await _sqlQueryService.GetRowsAsync<LastTrade>(command, cancellationToken);

            return lastTrades;
        }


        public async Task LastTradeOperations(DateTime? startDate, CancellationToken cancellationToken)
        {
            IEnumerable<LastTrade> lastTrades = await GetAsync(startDate, cancellationToken);

            await _fileService.SaveObjectToJsonFile(lastTrades, "lastTrades");

            _printService.PrintOnConsole(lastTrades);
        }


        private string CreateSelectCommand(DateTime? startDate)
        {
            string command = "SELECT Id, InstrumentId, ShortName, DateTimeEn, [Open], [High], [Low], [Close] FROM LastTrade";

            if (startDate == null || startDate == DateTime.MinValue)
            {
                return command;
            }

            command += $" Where DateTimeEn >= '{startDate}'";

            return command;
        }

    }
}