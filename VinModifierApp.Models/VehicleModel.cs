using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinModifierApp.Models;

[BsonIgnoreExtraElements]
public class VehicleModel : ImportVehicleModel
{
    public string? Make { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? ModelYear { get; set; }
    public string? VehicleType { get; set; }

    public void Merge(VehicleModel vehicle)
    {
        Make = vehicle.Make;
        Manufacturer = vehicle.Manufacturer;
        Model = vehicle.Model;
        ModelYear = vehicle.ModelYear;
        VehicleType = vehicle.VehicleType;
        VinDataPulled = true;
    }
}
