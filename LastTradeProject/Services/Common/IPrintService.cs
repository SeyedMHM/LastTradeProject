namespace LastTradeProject.Services.Common
{
    public interface IPrintService
    {
        void PrintOnConsole<TType>(IEnumerable<TType> models);
    }
}
