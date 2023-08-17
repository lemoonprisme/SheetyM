// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Sheety;
using Sheety.Services.Background.Displayer;
using Sheety.Services.Background.ScanReader;
using Sheety.Services.LocationFinder;
using Sheety.Services.Store;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
Console.Write("Write the file name(sheet): "); 
var filename = Console.ReadLine();
if (String.IsNullOrEmpty(filename)) 
	filename = "sheet";
var folderName = $@"ExelSheet\\{filename}.xlsx";

Console.Write("Write the list name(First List): ");
var sheetName = Console.ReadLine();

var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

var builder = Host.CreateApplicationBuilder();

builder.Services.AddLogging(c =>
{
	c.AddConsole();
});

var scanDispatcher = new ScanDispatcher();
builder.Services.AddSingleton<IScanResultDispatcher>(scanDispatcher);
builder.Services.AddSingleton<IScanResultStore>(scanDispatcher);

Console.Write("Write the number of outer stickers(6): ");
var outerStickersNumberString = Console.ReadLine();

Console.Write("Write the name of serial port(COM1): ");
var serialPortName = Console.ReadLine();
if (String.IsNullOrEmpty(serialPortName))
{
	serialPortName = "COM1";
}
builder.Services.AddOptions<NumberLocationFinderOptions>().Configure(o =>
{
	if (Int32.TryParse(outerStickersNumberString, out var outerStickersNumber))
		o.OuterStickersNumber = outerStickersNumber;
	o.FolderName = folderName;
	o.SheetName = sheetName;

});
builder.Services.AddOptions<ScannerReaderOptions>().Configure(o =>
{
	o.SerialPortName = serialPortName;

});
//FileProvider for real file system
builder.Services.AddScoped<IFileProvider>(_ => new PhysicalFileProvider( /*Root directory*/desktopPath));

//Service than find location from excel file
builder.Services.AddScoped<INumberLocationFinder, ExcelNumberLocationFinder>();


//Background service to read from serial port
builder.Services.AddHostedService<ScannerReader>();

//Background service to read fake data
// builder.Services.AddHostedService<FakeScannerReader>();

//Background service to display location
builder.Services.AddHostedService<SerialNumberLocationDisplayer>();

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press 'Q' to exit");
while (Console.ReadKey() == new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false))
{
	
}
await host.StopAsync();