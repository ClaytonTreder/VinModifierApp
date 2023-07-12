using System;
using MongoDB.Driver;
using VinModifierApp.Data.Interfaces;
using VinModifierApp.Models;

namespace VinModifierApp.Data.MongoDB.Collections;

public class VehicleCollection : IVehicleRepository
{
    private readonly IMongoCollection<VehicleModel> Collection;
    private readonly IMongoCollection<ImportVehicleModel> SimpleCollection;

    public VehicleCollection(IConnect connect)
    {
        Collection = connect.GetDatabase().GetCollection<VehicleModel>("Vehicles");
        SimpleCollection = connect.GetDatabase().GetCollection<ImportVehicleModel>("Vehicles");
    }

    public async Task AddVehicle(VehicleModel vehicle)
    {
        await Collection.InsertOneAsync(vehicle);
    }

    public async Task AddVehicles(IEnumerable<VehicleModel> vehicle)
    {
        await Collection.InsertManyAsync(vehicle);
    }

    public async Task<IEnumerable<VehicleModel>> GetUnAugmentedVehicles()
    {
        return await Collection
            .Find(x => x.VinDataPulled == false)
            .ToListAsync();
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

    public async Task<IEnumerable<ImportVehicleModel>> GetVehicles(
        int start = 0,
        int limit = 25,
        int? dealerId = null,
        DateOnly? afterModifiedDate = null)
    {

        FilterDefinition<ImportVehicleModel> filter = Builders<ImportVehicleModel>.Filter.Exists(x => x.Vin); 

        if (dealerId != null)
        {
            filter = Builders<ImportVehicleModel>.Filter.Eq(x => x.DealerId, dealerId);
        }

        if (afterModifiedDate != null)
        {
            filter = filter & Builders<ImportVehicleModel>.Filter.Gte(x => x.ModifiedDate, afterModifiedDate);
        }

        return await SimpleCollection
            .Find(filter)
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
