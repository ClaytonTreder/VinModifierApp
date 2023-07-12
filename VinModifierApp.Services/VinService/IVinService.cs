using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using VinModifierApp.Models;

namespace VinModifierApp.Services.VinService;

public interface IVinService
{
    public Task<VinServiceResponse> SaveVinInfo(IFormFile[] files);

    public DataTable? ConvertCsvToDataTable(IFormFile file);

    public bool ValidateCSV(DataTable? dataTable);

    public IEnumerable<VehicleModel> ConvertDataTableToArray(DataTable dataTable);

}
