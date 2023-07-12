using System;
using MongoDB.Driver;
using VinModifierApp.Data.Interfaces;
using VinModifierApp.Models;

namespace VinModifierApp.Data.MongoDB.Collections;

public class VehicleCollection : IVehicleRepository
{
    private readonly IMongoCollection<VehicleModel> Collection;

    public VehicleCollection(IConnect connect)
    {
        Collection = connect.GetDatabase().GetCollection<VehicleModel>("Vehicles");
    }

    public async Task AddVehicle(VehicleModel vehicle)
    {
        await Collection.InsertOneAsync(vehicle);
    }

    public async Task AddVehicles(IEnumerable<VehicleModel> vehicle)
    {
        await Collection.InsertManyAsync(vehicle);
    }

    public async Task<VehicleModel> GetVehicle(string vin)
    {
        return await Collection
            .Find(x => x.Vin == vin)
            .Sort(Builders<VehicleModel>.Sort.Descending(x => x.ModifiedDate))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<VehicleModel>> GetVehicles(string[] vins)
    {
        var filter = Builders<VehicleModel>.Filter.In<string>(x => x.Vin, vins);
        return await Collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<VehicleModel>> GetVehicles(int start, int limit)
    {
        return await Collection
            .Find(x => x != null)
            .Skip(start)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task UpdateVehicle(VehicleModel vehicle)
    {
        var filter = Builders<VehicleModel>.Filter.Eq<string>(x => x.Vin, vehicle.Vin);
        await Collection.FindOneAndReplaceAsync<VehicleModel>(filter, vehicle);
    }
}
