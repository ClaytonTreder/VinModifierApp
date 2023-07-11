using System;
using VinModifierApp.Models;

namespace VinModifierApp.Data.Interfaces;

public interface IVehicleRepository
{
    public Task<IEnumerable<VehicleModel> >GetVehicles();
    public Task<VehicleModel> GetVehicle(int id);
    public Task UpdateVehicle(VehicleModel vehicle);
}
