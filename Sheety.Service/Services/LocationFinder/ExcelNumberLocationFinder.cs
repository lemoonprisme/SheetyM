using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using Sheety.Models;

namespace Sheety.Services.LocationFinder;

public class ExcelNumberLocationFinder : INumberLocationFinder
{
	private readonly IFileProvider _fileProvider;
	
	private string Filename { get; }
	public string SheetName { get; }

	public ExcelNumberLocationFinder(IFileProvider fileProvider, string filename, string sheetName = "list1")
	{
		Filename = filename;
		SheetName = sheetName;
		_fileProvider = fileProvider;
	}

	public Location GetLocation(string serialNumber)
	{
		using var package = new ExcelPackage(_fileProvider.GetFileInfo(Filename).CreateReadStream());
		var worksheet = package.Workbook.Worksheets[SheetName];
		//Logic here

		return new Location(0, 0);
	}
}