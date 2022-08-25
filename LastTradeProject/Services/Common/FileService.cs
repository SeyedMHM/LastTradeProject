using System.Text.Json;

namespace LastTradeProject.Services.Common
{
    public class FileService : IFileService
    {
        public async Task SaveObjectToJsonFile<TModel>(TModel model, string fileName)
        {
            string SerializeObject = JsonSerializer.Serialize(model);

            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}.json";

            await File.WriteAllTextAsync(filePath, SerializeObject);
        }
    }
}
