using System.IO.Ports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sheety.Services.Store;

namespace Sheety.Services.Background.ScanReader;

public class ScannerReader : BackgroundService
{
	private readonly IScanResultDispatcher _dispatcher;
	
	private string SerialPortName { get; set; }

	public ScannerReader(IScanResultDispatcher dispatcher, IOptions<ScannerReaderOptions> options)
	{
		_dispatcher = dispatcher;
		SerialPortName = options.Value.SerialPortName;

	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Task.Run(async () =>
		{
			var port = new SerialPort(SerialPortName);
			port.Open();
			while (!stoppingToken.IsCancellationRequested)
			{
				var line = port.ReadLine();
				await _dispatcher.Add(GetResult(line)).ConfigureAwait(true);
			}
		}, stoppingToken);
		return Task.CompletedTask;
	}

	private static ScanResult GetResult(string line)
	{
		return new ScanResult(line.Trim());
	}
}