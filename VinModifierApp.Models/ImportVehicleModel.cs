using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinModifierApp.Models;

public class ImportVehicleModel
{
    public int DealerId { get; set; }
    public string Vin { get; set; } = string.Empty;
    public DateTime ModifiedDate { get; set; }
}
