using System;
using VinModifierApp.Models;

namespace VinModifierApp.Data.Interfaces;

public interface IVehicleRepository
{
    public Task AddVehicle(VehicleModel vehicle);
    public Task AddVehicles(IEnumerable<VehicleModel> vehicle);
    public Task<IEnumerable<ImportVehicleModel>> GetVehicles(
        int start = 0,
        int limit = 25,
        int? dealerId = null,
        DateOnly? afterModifiedDate = null);
    public Task<IEnumerable<VehicleModel>> GetVehicles(string[] vins);
    public Task<IEnumerable<VehicleModel>> GetUnAugmentedVehicles();
    public Task<VehicleModel> GetVehicle(string vin);
    public Task UpdateVehicle(VehicleModel vehicle);
}
