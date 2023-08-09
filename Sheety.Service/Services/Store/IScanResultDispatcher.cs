namespace Sheety.Services.Store;

public interface IScanResultDispatcher
{
	public Task Add(ScanResult result);
}