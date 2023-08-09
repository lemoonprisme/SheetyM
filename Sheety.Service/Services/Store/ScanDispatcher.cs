using System.Threading.Channels;

namespace Sheety.Services.Store;

internal class ScanDispatcher : IScanResultStore, IScanResultDispatcher
{
	private readonly Channel<ScanResult> _channel = Channel.CreateUnbounded<ScanResult>();

	public async Task<ScanResult> Get()
	{
		return await _channel.Reader.ReadAsync();
	}

	public async Task Add(ScanResult result)
	{
		await _channel.Writer.WriteAsync(result);
	}
}