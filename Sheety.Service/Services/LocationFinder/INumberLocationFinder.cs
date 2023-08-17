using Sheety.Models;

namespace Sheety.Services.LocationFinder;

public interface INumberLocationFinder
{
	Location? GetLocation(string serialNumber);
}