using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Sheety.Models;

namespace Sheety.Services.LocationFinder;

public class ExcelNumberLocationFinder : INumberLocationFinder
{
	private readonly IFileProvider _fileProvider;
	private readonly IOptions<NumberLocationFinderOptions> _options;

	private string FolderName { get; }
	private string? SheetName { get; }

	private int OuterStickersNumber { get; set; }
	

	public ExcelNumberLocationFinder(IFileProvider fileProvider,IOptions<NumberLocationFinderOptions> options)
	{
		_options = options;
		_fileProvider = fileProvider;
		FolderName = _options.Value.FolderName;
		SheetName = _options.Value.SheetName;
		OuterStickersNumber = _options.Value.OuterStickersNumber;
	}

	public Location? GetLocation(string serialNumber)
	{
		using var package = new ExcelPackage(_fileProvider.GetFileInfo(FolderName).CreateReadStream());
		ExcelWorksheet worksheet;
		if (String.IsNullOrEmpty(SheetName))
		{
			worksheet = package.Workbook.Worksheets[0];
		}
		else
		{
			worksheet = package.Workbook.Worksheets[SheetName];
		}

		//Logic here
		var query = worksheet.Cells["a:a"]
			.FirstOrDefault(cell => cell.Value?.ToString() == serialNumber);
		if (query == null)
			return null;
		var row = query.FirstOrDefault()!.EntireRow.StartRow;
		return new Location((row-1)/OuterStickersNumber+1, (row-1)/48+1);
	}
	
}