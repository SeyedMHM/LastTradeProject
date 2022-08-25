namespace LastTradeProject.Services.Common
{
    public class PrintService : IPrintService
    {
        public void PrintOnConsole<TType>(IEnumerable<TType> models)
        {
            foreach (var model in models)
            {
                List<string> itemsWithValue = new List<string>();
                foreach (var item in model.GetType().GetProperties())
                {
                    var name = item.Name;
                    var value = item.GetValue(model, null);

                    itemsWithValue.Add($"{name}:{value}");
                }

                Console.WriteLine(string.Join("\t", itemsWithValue));
            }
        }
    }
}
