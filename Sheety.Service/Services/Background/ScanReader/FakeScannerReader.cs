using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sheety.Services.Store;

namespace Sheety.Services.Background.ScanReader;

public class FakeScannerReader : BackgroundService
{
	private readonly IScanResultDispatcher _dispatcher;
	private readonly ILogger<FakeScannerReader> _logger;

	public FakeScannerReader(IScanResultDispatcher dispatcher, ILogger<FakeScannerReader> logger)
	{
		_dispatcher = dispatcher;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Creating fake data");

		while (!stoppingToken.IsCancellationRequested)
		{
			await _dispatcher.Add(new ScanResult(Guid.NewGuid().ToString()));
			await Task.Delay(2500, stoppingToken);
		}
	}
}