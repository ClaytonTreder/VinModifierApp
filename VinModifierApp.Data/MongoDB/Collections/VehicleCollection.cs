using System;
using VinModifierApp.Data.Interfaces;
using VinModifierApp.Models;

namespace VinModifierApp.Data.MongoDB.Collections;

public class VehicleCollection : IVehicleRepository
{
    public VehicleCollection()
    {
    }

    public Task<VehicleModel> GetVehicle(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VehicleModel>> GetVehicles()
    {
        throw new NotImplementedException();
    }

    public Task UpdateVehicle(VehicleModel vehicle)
    {
        throw new NotImplementedException();
    }
}
