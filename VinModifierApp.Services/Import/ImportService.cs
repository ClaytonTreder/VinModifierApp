using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinModifierApp.Models;

namespace VinModifierApp.Services.Import;

public class ImportService : IImportService
{

    public ILogger Logger { get; set; }
    public ImportService(ILogger<ImportService> logger)
    {
        Logger = logger;
    }

    public IEnumerable<ImportVehicleModel> GetImportVehicles()
    {
        Logger.LogDebug("ImportVehicle running");

        return new List<ImportVehicleModel>();
    }
}

