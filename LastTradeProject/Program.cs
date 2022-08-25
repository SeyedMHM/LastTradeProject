using LastTradeProject.Services.Common;
using LastTradeProject.Services.LastTradeServices;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Fetch data...");

var serviceProvider = GetServiceProvider().BuildServiceProvider();

ILastTradeService _lastTradeService = serviceProvider.GetService<ILastTradeService>();
DateTime? startDate = DateTime.Parse("2020-01-04 00:00:00.000");

//در صورت سوال گفته شده متد 
//GetAsync
//صدا زده شود اما متد مورد نظر یک متد عمومی است که بهتر است صرفا عملیات مربوط به دریافت داده را انجام دهد
//بهمین خاطر یک متد دیگر با اسم زیر ساخته شد تا در کنار دریافت داده، عملیات مربوط به نمایش اطلاعات
//در کنسول و ذخیره آن در فایل جسون را بر عهده بگیرد
await _lastTradeService.LastTradeOperations(startDate, CreateCancellationTokenObject());

Console.WriteLine("Fetching data is complete!");
Console.ReadLine();


IServiceCollection GetServiceProvider()
{
    //Register services to DI-Container
    var serviceProvider = new ServiceCollection()
        .AddScoped<ISqlQueryService, SqlQueryService>()
        .AddScoped<ILastTradeService, LastTradeService>()
        .AddScoped<IFileService, FileService>()
        .AddScoped<IPrintService, PrintService>()
        ;

    return serviceProvider;
}


/// <summary>
/// Create cancellationToken object
/// Cancel operation when press Ctrl + C
/// </summary>
CancellationToken CreateCancellationTokenObject()
{
    var cancellationTokenSource = new CancellationTokenSource();
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Canceling...");
        cancellationTokenSource.Cancel();
        e.Cancel = true;
    };

    return cancellationTokenSource.Token;
}