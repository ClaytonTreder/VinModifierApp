using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VinModifierApp.Data.Interfaces;
using VinModifierApp.FileStorage;
using VinModifierApp.FileStorage.S3;
using VinModifierApp.Models;

namespace VinModifierApp.Services.VinService;

public class VinService : IVinService
{
    public ILogger<VinService> Logger { get; }
    public IFileStorage S3FileStorage { get; }
    public IVehicleRepository VehicleRepository { get; }

    public VinService(
        ILogger<VinService> logger,
        IFileStorage s3FileStorage,
        IVehicleRepository vehicleRepository)
    {
        Logger = logger;
        S3FileStorage = s3FileStorage;
        VehicleRepository = vehicleRepository;
    }

    public async Task<VinServiceResponse> SaveVinInfo(IFormFile[] files)
    {
        var response = new VinServiceResponse();

        foreach (var file in files)
        {
            var dt = ConvertCsvToDataTable(file);
            if (dt == null || !ValidateCSV(dt))
            {
                response.FailedFiles.Add(file.FileName);
                continue;
            }

            // save file to s3
            await S3FileStorage.SaveFile(file);

            var vehicles = ConvertDataTableToArray(dt);
            // save vehicles to db
            await VehicleRepository.AddVehicles(vehicles);

        }

        return response;
    }

    public DataTable? ConvertCsvToDataTable(IFormFile file)
    {
        try
        {
            var dt = new DataTable();
            using (var stream = file.OpenReadStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    string[]? headers = streamReader.ReadLine()?.Split(",") ?? Array.Empty<string>();
                    foreach (var header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!streamReader.EndOfStream)
                    {
                        string[]? row = streamReader.ReadLine()?.Split(",");
                        var dataRow = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            if (row?.Count() > 0)
                                dataRow[i] = row[i];
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return null;
        }
    }

    public IEnumerable<VehicleModel> ConvertDataTableToArray(DataTable dataTable)
    {
        return dataTable.AsEnumerable().Select(dr => new VehicleModel()
        {
            DealerId = int.Parse(dr["dealerId"] as string),
            Vin = dr["vin"] as string,
            ModifiedDate = DateOnly.Parse(dr["modifiedDate"] as string)
        }
        );
    }

    /// <summary>
    /// ToDo: Handle failings more gracefully
    /// </summary>
    public bool ValidateCSV(DataTable? dataTable)
    {
        try
        {
            // to many columns in table
            if (dataTable?.Columns.Count > 3)
                return false;

            // values do not parse
            foreach (var row in dataTable?.AsEnumerable() ?? Enumerable.Empty<DataRow>())
            {
                var id = row["dealerId"] as string;
                if (!int.TryParse(id, out var _))
                    return false;

                var vin = row["vin"] as string;
                if (vin?.Length != 17)
                    return false;

                var modDate = row["modifiedDate"] as string;
                if (!DateTime.TryParse(modDate, out var _))
                    return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return false;
        }
    }
}
