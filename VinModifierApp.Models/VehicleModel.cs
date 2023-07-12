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

}
