using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinModifierApp.Models;

namespace VinModifierApp.Services.Import;

public interface IImportService
{

    public IEnumerable<ImportVehicleModel> GetImportVehicles();
}
