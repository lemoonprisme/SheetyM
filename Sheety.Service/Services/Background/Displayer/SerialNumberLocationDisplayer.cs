using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sheety.Services.LocationFinder;
using Sheety.Services.Store;

namespace Sheety.Services.Background.Displayer;

public class SerialNumberLocationDisplayer : BackgroundService
{
	private readonly IScanResultStore _store;
	private readonly INumberLocationFinder _locationFinder;
	private readonly ILogger<SerialNumberLocationDisplayer> _logger;


	public SerialNumberLocationDisplayer(IScanResultStore store, INumberLocationFinder locationFinder, ILogger<SerialNumberLocationDisplayer> logger)
	{
		_store = store;
		_locationFinder = locationFinder;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var scanResult = await _store.Get();
			var location = _locationFinder.GetLocation(scanResult.SerialNumber);
			if (location == null)
			{
				_logger.LogInformation("There is no the serial number");
			}
			else
			{
				_logger.LogInformation("{ScanResultSerialNumber} - Outer Page: {LocationPage}. Inner Page: {LocationIndex}",
					scanResult.SerialNumber, location.OuterPage, location.InnerPage);
			}
		}
	}
	
}