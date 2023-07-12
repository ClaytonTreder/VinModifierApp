using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinModifierApp.Models;

namespace VinModifierApp.Services.Data;

public interface IDataService
{
    public Task<VehicleModel> GetByVin(string vin);
    public Task<IEnumerable<VehicleModel>> GetAll(int start = 0, int limit = 25);
}
