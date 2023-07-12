using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinModifierApp.Data.Interfaces;
using VinModifierApp.Models;
using VinModifierApp.NHTSAApi;

namespace VinModifierApp.Services.Data;

public class DataService : IDataService
{
    public ILogger<DataService> Logger { get; }
    public IVehicleRepository VehicleRepository { get; }
    public NHTSA NHTSAApi { get; }

    public DataService(ILogger<DataService> logger, IVehicleRepository vehicleRepository, NHTSA nhtasaApi)
    {
        Logger = logger;
        VehicleRepository = vehicleRepository;
        NHTSAApi = nhtasaApi;
    }

    public async Task<VehicleModel> GetByVin(string vin)
    {
        try
        {
            var vehicle = await VehicleRepository.GetVehicle(vin);

            // vehicle was found and has not had extra data pulled
            if (!string.IsNullOrWhiteSpace(vehicle.Vin) && !vehicle.VinDataPulled)
            {
                var nhtsaVehicle = await NHTSAApi.GetVehicle(vin);
                if (nhtsaVehicle != null)
                {
                    vehicle.Merge(nhtsaVehicle);
                    await VehicleRepository.UpdateVehicle(vehicle);
                }
                else
                {
                    Logger.LogError($"An error occurred getting vehicle from NHSTA. Vin: {vin}");
                }
            }
            return vehicle;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return new VehicleModel();
        }
    }

    public async Task<IEnumerable<ImportVehicleModel>> GetAll(
        int start = 0,
        int limit = 25,
        int? dealerId = null,
        DateOnly? afterModifiedDate = null)
    {
        try
        {
            var vehicles = await VehicleRepository.GetVehicles(start, limit, dealerId, afterModifiedDate);

            if (vehicles.Count() > 0)
            {
                var nhtsaVehicles = await NHTSAApi.GetVehicles(vehicles.Select(x => x.Vin).ToArray());
                foreach (var vehicle in vehicles)
                {
                    if (!string.IsNullOrWhiteSpace(vehicle.Vin) && !vehicle.VinDataPulled)
                    {
                        var nhtsaVehicle = nhtsaVehicles?.FirstOrDefault(x => x.Vin == vehicle.Vin);
                        if (nhtsaVehicle != null)
                        {
                            var newWehicle = new VehicleModel();
                            newWehicle.Merge(nhtsaVehicle);
                            await VehicleRepository.UpdateVehicle(newWehicle);
                        }
                        else
                        {
                            Logger.LogError($"An error occurred getting vehicle from NHSTA. Vin: {vehicle.Vin}");
                        }
                    }
                }
            }
            return vehicles;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return Enumerable.Empty<VehicleModel>();
        }
    }
}
