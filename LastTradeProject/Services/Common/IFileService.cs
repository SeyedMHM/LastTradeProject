namespace LastTradeProject.Services.Common
{
    public interface IFileService
    {
        Task SaveObjectToJsonFile<TModel>(TModel model, string fileName);
    }
}
