namespace Sheety.Services.Store;

public interface IScanResultStore
{
	Task<ScanResult> Get();
}