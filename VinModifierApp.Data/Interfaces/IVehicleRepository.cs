using System;
using VinModifierApp.Models;

namespace VinModifierApp.Data.Interfaces;

public interface IVehicleRepository
{
    public Task AddVehicle(VehicleModel vehicle);
    public Task AddVehicles(IEnumerable<VehicleModel> vehicle);
    public Task<IEnumerable<VehicleModel>> GetVehicles(int start, int limit);
    public Task<IEnumerable<VehicleModel>> GetVehicles(string[] vins);
    public Task<VehicleModel> GetVehicle(string vin);
    public Task UpdateVehicle(VehicleModel vehicle);
}
