using System.IO.Ports;
using Microsoft.Extensions.Hosting;
using Sheety.Services.Store;

namespace Sheety.Services.Background.ScanReader;

public class ScannerReader : BackgroundService
{
	private readonly IScanResultDispatcher _dispatcher;

	public ScannerReader(IScanResultDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var port = new SerialPort();
		while (!stoppingToken.IsCancellationRequested)
		{
			var line = port.ReadLine();
			await _dispatcher.Add(GetResult(line));
		}
	}

	private static ScanResult GetResult(string line)
	{
		//ToDo: Add your logic

		return new ScanResult(line);
	}
}