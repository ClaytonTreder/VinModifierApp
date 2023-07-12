using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinModifierApp.Data.Interfaces;
using VinModifierApp.Models;
using VinModifierApp.NHTSAApi;

namespace VinModifierApp.Services.Augment;

public class AugmentService : IAugmentService
{

    public ILogger<AugmentService> Logger { get; }
    public IVehicleRepository VehicleRepository { get; }
    public NHTSA NHTSA { get; }

    public AugmentService(
        ILogger<AugmentService> logger,
        IVehicleRepository vehicleRepository,
        NHTSA nHTSA)
    {
        Logger = logger;
        VehicleRepository = vehicleRepository;
        NHTSA = nHTSA;
    }

    public async Task<bool> AugmentAll()
    {
        try
        {
            var vehicles = await VehicleRepository.GetUnAugmentedVehicles();
            var groupedVehicles = new List<IEnumerable<VehicleModel>>();

            for (var i = 0; i < vehicles.Count(); i += 50)
            {
                groupedVehicles.Add(vehicles.Skip(i).Take(50));
            }

            foreach (var group in groupedVehicles)
            {
                var nhtsaVehicles = await NHTSA.GetVehicles(group.Select(x => x.Vin).ToArray());
                foreach (var vehicle in vehicles)
                {
                    if (!string.IsNullOrWhiteSpace(vehicle.Vin) && !vehicle.VinDataPulled)
                    {
                        var nhtsaVehicle = nhtsaVehicles?.FirstOrDefault(x => x.Vin == vehicle.Vin);
                        if (nhtsaVehicle != null)
                        {
                            vehicle.Merge(nhtsaVehicle);
                            await VehicleRepository.UpdateVehicle(vehicle);
                        }
                        else
                        {
                            Logger.LogError($"An error occurred getting vehicle from NHSTA. Vin: {vehicle.Vin}");
                        }
                    }
                }
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
