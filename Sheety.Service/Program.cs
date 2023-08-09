// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sheety.Services.Background.Displayer;
using Sheety.Services.Background.ScanReader;
using Sheety.Services.LocationFinder;
using Sheety.Services.Store;

//ToDo: Get filname from console
var filename = "sheet.xlsx";
var sheetName = "";
var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

var builder = Host.CreateApplicationBuilder();

builder.Services.AddLogging(c =>
{
	c.AddConsole();
});

var scanDispatcher = new ScanDispatcher();
builder.Services.AddSingleton<IScanResultDispatcher>(scanDispatcher);
builder.Services.AddSingleton<IScanResultStore>(scanDispatcher);

//FileProvider for real file system
builder.Services.AddScoped<IFileProvider>(_ => new PhysicalFileProvider( /*Root directory*/desktopPath));

//Service than find location from excel file
builder.Services.AddScoped<INumberLocationFinder, ExcelNumberLocationFinder>(sp => new ExcelNumberLocationFinder(sp.GetRequiredService<IFileProvider>(), filename, sheetName));


//Background service to read from serial port
//builder.Services.AddHostedService<ScannerReader>();

//Background service to read fake data
builder.Services.AddHostedService<FakeScannerReader>();

//Background service to display location
builder.Services.AddHostedService<SerialNumberLocationDisplayer>();

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press 'Q' to exit");
while (Console.ReadKey() == new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false))
{
	
}

await host.StopAsync();