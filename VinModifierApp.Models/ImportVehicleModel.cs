using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace VinModifierApp.Models;

[BsonIgnoreExtraElements]
public class ImportVehicleModel
{
    public int DealerId { get; set; }
    public string Vin { get; set; } = string.Empty;
    public DateOnly ModifiedDate { get; set; }
    [JsonIgnore]
    public bool VinDataPulled { get; set; } = false;
}
